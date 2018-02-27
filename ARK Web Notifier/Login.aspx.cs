using ARKWebNotifier.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ARKWebNotifier
{
    public partial class Login : CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (CustomMasterPage.sessionHandler.IsLoggedIn)
            {
                CustomMasterPage.Redirect("~/Default.aspx");
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUsernameEmail.Text.Equals(""))
            {
                spanInfo.InnerText = "Please enter a username or email address.";
                return;
            }
            if (txtPassword.Text.Equals(""))
            {
                spanInfo.InnerText = "Please enter a password.";
                return;
            }
            CustomDataTable tbl = MySQLHelper.ExecuteQueryReader("get_password", new string[] { "UsernameEmail" }, new object[] { txtUsernameEmail.Text }, true);
            bool LoginSucceeded = false;
            if (tbl.HasRows)
            {
                tbl.Read();
                string encPwd = tbl.GetString(0);
                if (Globals.Decrypt(encPwd).Equals(txtPassword.Text))
                {
                    LoginSucceeded = true;
                    SecurityTokenManager token = new SecurityTokenManager();
                    tbl = MySQLHelper.ExecuteQueryReader("authorize_session", new string[] { "UsernameEmail" }, new object[] { txtUsernameEmail.Text }, true);
                    if (tbl.HasRows)
                    {
                        tbl.Read();
                        token.DeviceUID = tbl[0].ToString();
                        token.LastAccessDate = tbl.GetDateTime(1);
                        if (chkRememberMe.Checked)
                        {
                            CustomMasterPage.sessionHandler.SessionStorageLocation = SessionHandler.SessionStorageLocations.Cookie;
                        }
                        else
                        {
                            CustomMasterPage.sessionHandler.SessionStorageLocation = SessionHandler.SessionStorageLocations.Session;
                        }
                        CustomMasterPage.sessionHandler.SecurityToken = token.GenerateTokenString();
                        CustomMasterPage.Redirect("~/Default.aspx");
                    }
                }
            }
            if (!LoginSucceeded)
            {
                spanInfo.InnerText = "Invalid credentials entered.";
                return;
            }
        }

    }
}