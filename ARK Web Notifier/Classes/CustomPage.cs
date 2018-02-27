using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace ARKWebNotifier.Classes
{
    public class CustomPage:Page
    {
        public Site CustomMasterPage;
        public CustomPage()
        {
            this.Load += CustomPage_Load;
        }

        private void CustomPage_Load(object sender, EventArgs e)
        {
            CustomMasterPage = (Site)this.Master;
            CustomMasterPage.PageInitialize();
        }
    }
}