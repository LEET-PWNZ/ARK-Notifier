using ARKWebNotifier.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace ARKWebNotifier
{
    public partial class web_notif : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Request.Form["key"]!=null&& Request.Form["steamid"] !=null && Request.Form["notetitle"] != null && Request.Form["message"] != null)
            {
                
                string serverKey = Request.Form["key"];
                string requiredServerKey = ConfigurationManager.AppSettings["ArkPostKey"];
                if (serverKey.Equals(requiredServerKey))
                {

                    int serverID = int.Parse(Request.QueryString["server_id"]);
                    string steamid = Request.Form["steamid"];
                    string noteTitle = Request.Form["notetitle"];
                    string message = Request.Form["message"];
                    string[] paramNames = { "steamid", "notetitle", "message","serverid"};
                    object[] paramVals = { steamid, noteTitle, message, serverID };
                    MySQLHelper.ExecuteQuery("insert into notifications (steam_id,note_title,message,server_id) values (@steamid,@notetitle,@message,@serverid);", paramNames, paramVals);
                }
            }
            
        }
    }
}