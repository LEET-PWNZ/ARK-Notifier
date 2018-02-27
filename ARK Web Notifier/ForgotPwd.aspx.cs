using ARKWebNotifier.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ARKWebNotifier
{
    public partial class ForgotPwd : CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (CustomMasterPage.sessionHandler.IsLoggedIn)
            {
                Response.Redirect("~/Default.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txtUsernameEmail.Text.Equals(""))
            {
                spanInfo.InnerText = "Please enter a username or email address.";
                return;
            }
            string userid = "";
            if (!EmailExists(ref userid))
            {
                if (!UsernameExists(ref userid))
                {
                    spanInfo.InnerText = "The supplied username or email address has not been registered.";
                    txtUsernameEmail.Text = "";
                    return;
                }
            }
            string queEmailQuery = @"
insert into queued_emails (unique_id,user_id,email_type) values(UUID(),@userid,'forgotpwd');
";
            MySQLHelper.ExecuteQuery(queEmailQuery, new string[] { "userid" }, new object[] { userid});
            spanInfo.Attributes["style"] = "color:#00ee00";
            spanInfo.InnerText = "An email will be sent to the requested account. Follow the link in the email to reset the password.";
            txtUsernameEmail.Text = "";
        }

        private bool EmailExists(ref string userid)
        {
            bool result = false;
            CustomDataTable tbl = MySQLHelper.ExecuteQueryReader("select id from users where email_address=@email", new string[] { "email" }, new object[] { txtUsernameEmail.Text });
            if (tbl.HasRows)
            {
                tbl.Read();
                userid = tbl.GetInt(0).ToString();
                result = true;
            }
            return result;
        }

        private bool UsernameExists(ref string userid)
        {
            bool result = false;
            CustomDataTable tbl = MySQLHelper.ExecuteQueryReader("select id from users where username=@Username", new string[] { "Username" }, new object[] { txtUsernameEmail.Text });
            if (tbl.HasRows)
            {
                tbl.Read();
                userid = tbl.GetInt(0).ToString();
                result = true;
            }
            return result;
        }

    }
}