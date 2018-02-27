using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Web;

namespace ARKNotifierService.Classes
{
    public class EmailHelper
    {
        public static void SendEmail(string subject,string messageBody,string toRecipient)
        {
            string emailHost = ConfigurationManager.AppSettings["SMTPHost"];
            int emailPort = int.Parse(ConfigurationManager.AppSettings["SMTPPort"]);
            string emailUser = ConfigurationManager.AppSettings["SMTPUser"];
            string emailUserName = ConfigurationManager.AppSettings["SMTPFromName"];
            string emailPassword = ConfigurationManager.AppSettings["SMTPPassword"];
            SmtpClient client = new SmtpClient(emailHost, emailPort);
            client.Credentials = new NetworkCredential(emailUser, emailPassword);
            MailMessage msg = new MailMessage(new MailAddress(emailUser, emailUserName), new MailAddress(toRecipient));
            msg.Subject = subject;
            msg.Body = messageBody;
            msg.IsBodyHtml = true;
            try
            {
                client.Send(msg);
            }
            catch (Exception){}
        }

        public static string GenerateNotificationMessageBody(string notificationTitle,string serverName)
        {
            string result = File.ReadAllText(AppPath+"EmailBodies\\Notification.html");
            result = result.Replace("@message@", notificationTitle);
            result = result.Replace("@server@", serverName);
            return result;
        }

        public static string GenerateRegisterMessageBody(string uniqueID)
        {
            string result = File.ReadAllText(AppPath + "EmailBodies\\Registration.html");
            result = result.Replace("@webAppUrl@", WebAppHostname+"CompleteRegistration.aspx?mailid="+HttpUtility.UrlEncode(uniqueID));
            return result;
        }

        public static string GenerateForgotPwdMessageBody(string uniqueID)
        {
            string result = File.ReadAllText(AppPath + "EmailBodies\\ForgotPwd.html");
            result = result.Replace("@webAppUrl@", WebAppHostname + "ResetPassword.aspx?mailid=" + HttpUtility.UrlEncode(uniqueID));
            return result;
        }

        private static string AppPath
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }

        private static string WebAppHostname
        {
            get
            {
                string result = ConfigurationManager.AppSettings["WebAppHostname"];
                if (!result.StartsWith("http://")) result = "http://" + result;
                if (!result.EndsWith("/")) result += "/";
                return result;
            }
        }

    }
}
