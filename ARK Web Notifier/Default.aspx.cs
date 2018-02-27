using ARKWebNotifier.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ARKWebNotifier
{
    public partial class Default : CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CustomMasterPage.sessionHandler.IsLoggedIn)
            {
                CustomMasterPage.Redirect("~/Login.aspx");
                return;
            }
            if (!CustomMasterPage.sessionHandler.EmailVerified)
            {
                CustomMasterPage.Redirect("~/EmailNotVerified.aspx");
                return;
            }
            if (!CustomMasterPage.sessionHandler.HasSteamID)
            {
                CustomMasterPage.Redirect("~/SteamID.aspx");
                return;
            }
            RenderNotifications();
        }

        private void RenderNotifications()
        {
            string sqlQuery = @"
select note_title,message,srv.server_name,notification_date from notifications notif
left join users usr on usr.steamid=notif.steam_id
left join servers srv on srv.id=notif.server_id
where usr.steamid='"+CustomMasterPage.sessionHandler.user.steamid+@"'
order by notif.notification_date desc
";
            CustomDataTable tbl = MySQLHelper.ExecuteQueryReader(sqlQuery);
            MainContainer.Controls.Clear();
            if (tbl.HasRows)
            {
                while (tbl.Read())
                {
                    NotificationControl cnt = (NotificationControl)LoadControl("~/NotificationControl.ascx");
                    cnt.NotificationTitle = tbl.GetString(0);
                    cnt.NotificationMessage = tbl.GetString(1);
                    cnt.ServerName = tbl.GetString(2);
                    cnt.NotificationDate = (DateTime)tbl.GetDateTime(3);
                    MainContainer.Controls.Add(cnt);
                }
            }else
            {
                MainContainer.Attributes["style"] = "padding:10px;";
                MainContainer.InnerText = "There are no notifications for this Steam ID yet.";
            }
        }

    }
}