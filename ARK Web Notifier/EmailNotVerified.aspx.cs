using ARKWebNotifier.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ARKWebNotifier
{
    public partial class EmailNotVerified : CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (CustomMasterPage.sessionHandler.EmailVerified)
            {
                CustomMasterPage.Redirect("~/Default.aspx");
            }
        }

        protected void btnSendAgain_Click(object sender, EventArgs e)
        {
            string queEmailQuery = @"
delete from queued_emails where user_id=@userid and email_type='register';
insert into queued_emails (unique_id,user_id,email_type) values(UUID(),@userid,'register');
";
            MySQLHelper.ExecuteQuery(queEmailQuery, new string[] { "userid" }, new object[] { CustomMasterPage.sessionHandler.user.dbID });
        }
    }
}