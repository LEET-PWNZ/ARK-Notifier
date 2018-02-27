using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ARKWebNotifier
{
    public partial class NotificationControl : System.Web.UI.UserControl
    {
        public string NotificationTitle;
        public string NotificationMessage;
        public string ServerName;
        public DateTime NotificationDate;
        protected void Page_Load(object sender, EventArgs e)
        {
            SpanTitle.InnerText = NotificationTitle;
            SpanDate.InnerText = NotificationDate.ToString();
            SpanMessage.InnerText = NotificationMessage;
            SpanServer.InnerText = ServerName;
        }

    }
}