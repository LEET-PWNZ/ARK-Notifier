using ARKWebNotifier.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ARKWebNotifier
{
    public partial class CompleteRegistration : CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string uniqueID = Request.QueryString["mailid"];
            CustomDataTable tbl = MySQLHelper.ExecuteQueryReader("select * from queued_emails where email_type='register' and unique_id=@uid and action_executed=0", new string[] { "uid"},new object[] { uniqueID});
            if (tbl.HasRows)
            {
                string sqlQuery = @"
update users set email_verified=1 where id=(select user_id from queued_emails where unique_id=@uid);
update queued_emails set action_executed=1 where unique_id=@uid;
";
                MySQLHelper.ExecuteQuery(sqlQuery, new string[] { "uid" }, new object[] { uniqueID });
                spanInfo.InnerText = "Your email address has been verified, registration completed successfully.";
                spanInfo.InnerHtml += " - <a href=\"Default.aspx\">Home</a>";
            }
            else
            {
                spanInfo.Attributes["style"] = "color:#ee0000;";
                spanInfo.InnerText = "This email address has already been verified, or the request is invalid.";
            }
        }
    }
}