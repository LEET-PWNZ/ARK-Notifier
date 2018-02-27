using ARKWebNotifier.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ARKWebNotifier
{
    public partial class Site : System.Web.UI.MasterPage
    {
        public SessionHandler sessionHandler=null;
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        public void PageInitialize()
        {
            sessionHandler = new SessionHandler(this);
        }

        public void Redirect(string redirectLocation)
        {
            Response.Redirect(redirectLocation, false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            sessionHandler.LogOut();
        }

    }
}