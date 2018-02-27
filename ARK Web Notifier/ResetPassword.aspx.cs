using ARKWebNotifier.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ARKWebNotifier
{
    public partial class ResetPassword : CustomPage
    {
        private string uniqueID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            uniqueID = Request.QueryString["mailid"];
            CustomDataTable tbl = MySQLHelper.ExecuteQueryReader("select * from queued_emails where email_type='forgotpwd' and unique_id=@uid and action_executed=0", new string[] { "uid" }, new object[] { uniqueID });
            if (!tbl.HasRows)
            {
                tblPasswords.Visible = false;
                spanInfo.InnerText = "This request is invalid or it has been processed already.";
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!txtPassword.Text.Equals(txtConfirmPassword.Text))
            {
                spanInfo.InnerText = "Passwords do not match.";
                return;
            }
            if (txtPassword.Text.Length<8)
            {
                spanInfo.InnerText = "Password must be at least 8 characters in length.";
                return;
            }
            string sqlQuery = @"
update users set password=@pwd where id=(select user_id from queued_emails where unique_id=@uid);
update queued_emails set action_executed=1 where unique_id=@uid;
";
            MySQLHelper.ExecuteQuery(sqlQuery, new string[] {"uid","pwd" }, new object[] {uniqueID,Globals.Encrypt(txtPassword.Text) });
            spanInfo.Attributes["style"] = "color:#00ee00;";
            spanInfo.InnerText = "Your password has been reset successfully, redirecting to login page...";
            Response.AddHeader("REFRESH", "3;URL=Login.aspx");
        }
    }
}