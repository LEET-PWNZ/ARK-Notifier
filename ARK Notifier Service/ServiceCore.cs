using ARKNotifierService.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ARKNotifierService
{
    public partial class ServiceCore : ServiceBase
    {

        private Thread emailerThread=null;
        private Thread purgeOldSessionsThread = null;
        private int emailThreadLoopInterval,purgeThreadLoopInterval;
        public ServiceCore()
        {
            InitializeComponent();
        }

        public void DebugStart()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            //EmailHelper.SendEmail("test", "message", "ste.coe@gmail.com");
            emailThreadLoopInterval = int.Parse(ConfigurationManager.AppSettings["EmailThreadLoopIntervalSeconds"])*1000;
            purgeThreadLoopInterval = int.Parse(ConfigurationManager.AppSettings["PurgeThreadLoopIntervalSeconds"])*1000;
            emailerThread = new Thread(() =>
              {
                  while (true)
                  {
                      QueNotificationEmails();
                      SendPendingNotifications();
                      SendPendingSystemEmails();
                      try
                      {
                          Thread.Sleep(emailThreadLoopInterval);
                      }
                      catch (Exception) { }
                  }
              });
            emailerThread.Start();
            purgeOldSessionsThread = new Thread(() =>
            {
                while (true)
                {
                    MySQLHelper.ExecuteQuery("delete from login_sessions where access_date <'"+DateTime.Now.AddMonths(-1).ToString()+"'");
                    try
                    {
                        Thread.Sleep(purgeThreadLoopInterval);
                    }
                    catch (Exception) { }
                }
            });
            purgeOldSessionsThread.Start();
        }

        protected override void OnStop()
        {
            if (emailerThread.ThreadState == System.Threading.ThreadState.WaitSleepJoin) emailerThread.Abort();
            if (purgeOldSessionsThread.ThreadState == System.Threading.ThreadState.WaitSleepJoin) purgeOldSessionsThread.Abort();
        }

        private void QueNotificationEmails()
        {
            string sqlGetNotifications = @"
select notif.id,usr.id from notifications notif
left join users usr on usr.steamid=notif.steam_id
where usr.steamid is not null and email_queued=0
";
            CustomDataTable tbl = MySQLHelper.ExecuteQueryReader(sqlGetNotifications);
            if (tbl.HasRows)
            {
                while (tbl.Read())
                {
                    string sqlQuery = @"
insert into queued_emails (unique_id,user_id,email_type,notification_id) values(UUID(),@userid,'notification',@notifID);
update notifications set email_queued=1 where id=@notifID;
";
                    MySQLHelper.ExecuteQuery(sqlQuery,new string[] { "userid", "notifID" },new object[] { tbl.GetInt(1),tbl.GetInt(0)});
                }
            }
        }

        private void SendPendingNotifications()
        {
            string sqlGetPendingEmails = @"
select usr.email_address,notif.note_title,srv.server_name,emails.id from queued_emails emails
left join notifications notif on notif.id=emails.notification_id
left join users usr on usr.steamid=notif.steam_id and usr.id=emails.user_id
left join servers srv on srv.id=notif.server_id
where email_processed=0 and email_type='notification'
";
            CustomDataTable tbl = MySQLHelper.ExecuteQueryReader(sqlGetPendingEmails);
            if (tbl.HasRows)
            {
                while (tbl.Read())
                {
                    EmailHelper.SendEmail("ARK Notification - "+DateTime.Now.ToString(), EmailHelper.GenerateNotificationMessageBody(tbl.GetString(1),tbl.GetString(2)), tbl.GetString(0));
                    MySQLHelper.ExecuteQuery("update queued_emails set email_processed=1 where id=" + tbl.GetInt(3).ToString());
                }
            }
        }

        private void SendPendingSystemEmails()
        {
            string sqlGetPendingEmails = @"
select emails.id,usr.email_address,email_type,unique_id from queued_emails emails
left join users usr on usr.id=emails.user_id
where email_processed=0 and email_type in ('register','forgotpwd')
";
            CustomDataTable tbl = MySQLHelper.ExecuteQueryReader(sqlGetPendingEmails);
            if (tbl.HasRows)
            {
                while (tbl.Read())
                {
                    string emailType = tbl.GetString(2);
                    if (emailType.Equals("register"))
                    {
                        EmailHelper.SendEmail("ARK Notifier Registration", EmailHelper.GenerateRegisterMessageBody(tbl.GetString(3)), tbl.GetString(1));
                    }else
                    {
                        EmailHelper.SendEmail("ARK Notifier Password Reset", EmailHelper.GenerateForgotPwdMessageBody(tbl.GetString(3)), tbl.GetString(1));
                    }
                    MySQLHelper.ExecuteQuery("update queued_emails set email_processed=1 where id="+tbl.GetInt(0).ToString());
                }
            }
        }

    }
}
