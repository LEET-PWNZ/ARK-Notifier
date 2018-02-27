using ARKWebNotifier.Classes;
using DotNetOpenAuth.OpenId.RelyingParty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ARKWebNotifier
{
    public partial class SteamID : CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (CustomMasterPage.sessionHandler.HasSteamID)
            {
                CustomMasterPage.Redirect("~/Default.aspx");
                return;
            }
            OpenIdRelyingParty party = new OpenIdRelyingParty();
            IAuthenticationResponse response = party.GetResponse();
            if (response != null)
            {
                switch (response.Status)
                {
                    case AuthenticationStatus.Authenticated:
                        string steamID = response.ClaimedIdentifier.ToString().Replace("http://steamcommunity.com/openid/id/", "");
                        MySQLHelper.ExecuteQuery("update users set steamid=@steamID where id=@userID",new string[] { "steamID","userID"},new object[] { steamID, CustomMasterPage.sessionHandler.user.dbID});
                        spanInfo.Attributes["style"] = "color:#00ee00";
                        spanInfo.InnerText = "Your Steam ID has been linked successfully, redirecting...";
                        Response.AddHeader("REFRESH", "3;URL=Default.aspx");
                        break;
                    default:
                        spanInfo.Attributes["style"] = "color:#ee0000";
                        spanInfo.InnerText = "Oops! Something went wrong while trying to add your Steam ID, please try again.";
                        break;
                }
            }
        }

        protected void btnSteamSignIn_Click(object sender, EventArgs e)
        {
            using (OpenIdRelyingParty aparty = new OpenIdRelyingParty())
            {
                IAuthenticationRequest request = aparty.CreateRequest("http://steamcommunity.com/openid");
                request.RedirectToProvider();
            }
        }
    }
}