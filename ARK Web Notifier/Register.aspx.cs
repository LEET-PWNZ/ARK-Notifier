using ARKWebNotifier.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ARKWebNotifier
{
    public partial class Register : CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (CustomMasterPage.sessionHandler.IsLoggedIn)
            {
                Response.Redirect("~/Default.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {

            if (!txtEmailAddress.Text.Equals(txtConfirmEmail.Text))
            {
                spanInfo.InnerText = "Email addresses do not match.";
                txtEmailAddress.Text = "";
                txtConfirmEmail.Text = "";
                return;
            }
            if(Regex.Matches(txtEmailAddress.Text, ".*\\@.*\\..*").Count <= 0)
            {
                spanInfo.InnerText = "Email address is not valid.";
                txtEmailAddress.Text = "";
                txtConfirmEmail.Text = "";
                return;
            }
            if (!EmailAvailable())
            {
                spanInfo.InnerText = "Email address in use.";
                txtEmailAddress.Text = "";
                txtConfirmEmail.Text = "";
                return;
            }
            if (txtUsername.Text.Length < 6)
            {
                spanInfo.InnerText = "Username must be at least 6 characters in length.";
                txtUsername.Text = "";
                return;
            }
            if (!UsernameAvailable())
            {
                spanInfo.InnerText = "Username in use.";
                txtUsername.Text = "";
                return;
            }
            if (!txtPassword.Text.Equals(txtPasswordConfirm.Text))
            {
                spanInfo.InnerText = "Passwords do not match.";
                return;
            }
            if (txtPassword.Text.Length<8)
            {
                spanInfo.InnerText = "Password must be at least 8 characters in length.";
                return;
            }

            string insertUserQuery = @"
insert into users (username,password,email_address) values (@username,@password,@emailAddress);
insert into queued_emails (unique_id,user_id,email_type) values(UUID(),LAST_INSERT_ID(),'register');
";
            MySQLHelper.ExecuteQuery(insertUserQuery, new string[] { "username", "password", "emailAddress" }, new object[] { txtUsername.Text, Globals.Encrypt(txtPassword.Text), txtEmailAddress.Text });
            spanInfo.Attributes["style"] = "color:#00ee00";
            spanInfo.InnerText = "An email will be sent to the supplied address, please follow the link in the email to complete registration.";
            txtUsername.Text = "";
            txtEmailAddress.Text = "";
            txtConfirmEmail.Text = "";
        }

        private bool EmailAvailable()
        {
            bool result = false;
            CustomDataTable tbl = MySQLHelper.ExecuteQueryReader("select * from users where email_address=@email", new string[] { "email" }, new object[] { txtEmailAddress.Text });
            if (!tbl.HasRows)
            {
                result = true;
            }
            return result;
        }

        private bool UsernameAvailable()
        {
            bool result = false;
            CustomDataTable tbl = MySQLHelper.ExecuteQueryReader("select * from users where username=@Username", new string[] { "Username" }, new object[] { txtUsername.Text });
            if (!tbl.HasRows)
            {
                result = true;
            }
            return result;
        }

    }
}