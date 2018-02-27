using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace ARKWebNotifier.Classes
{
    public class SessionHandler
    {
        private Site mainPage;
        public UserContainer user=null;
        public enum SessionStorageLocations { None,Cookie, Session};
        public SessionStorageLocations SessionStorageLocation=SessionStorageLocations.None;
        
        public SessionHandler(Site _page)
        {
            mainPage = _page;
            if (mainPage.Request.Cookies["ARKNotifierSession"] != null)
            {
                SecurityTokenManager secToken = SecurityTokenManager.DecompileTokenString(mainPage.Request.Cookies["ARKNotifierSession"].Value);
                string[] paramNames = { "UID", "LastAccessDate" };
                object[] paramVals = { secToken.DeviceUID, secToken.LastAccessDate };
                CustomDataTable tbl = MySQLHelper.ExecuteQueryReader("validate_session", paramNames, paramVals,true);
                if (tbl.HasRows)
                {
                    tbl.Read();
                    SessionStorageLocation = SessionStorageLocations.Cookie;
                    secToken.LastAccessDate = tbl.GetDateTime(0);
                    SecurityToken = secToken.GenerateTokenString();
                }
                else
                {
                    LogOut();
                }
            }
            else if (mainPage.Request.Cookies["ARKNotifierSession"] == null && mainPage.Session["ARKNotifierSession"] != null)
            {
                SecurityTokenManager secToken = SecurityTokenManager.DecompileTokenString(mainPage.Session["ARKNotifierSession"].ToString());
                string[] paramNames = { "UID", "LastAccessDate" };
                object[] paramVals = { secToken.DeviceUID, secToken.LastAccessDate };
                CustomDataTable tbl = MySQLHelper.ExecuteQueryReader("validate_session", paramNames, paramVals,true);
                if (tbl.HasRows)
                {
                    tbl.Read();
                    SessionStorageLocation = SessionStorageLocations.Session;
                    secToken.LastAccessDate = tbl.GetDateTime(0);
                    SecurityToken = secToken.GenerateTokenString();
                }
                else
                {
                    LogOut();
                }
            }
            if (SessionStorageLocation != SessionStorageLocations.None)
            {
                SecurityTokenManager secToken = SecurityTokenManager.DecompileTokenString(SecurityToken);
                string[] paramNames = { "UID", "LastAccessDate" };
                object[] paramVals = { secToken.DeviceUID, secToken.LastAccessDate };
                CustomDataTable tbl = MySQLHelper.ExecuteQueryReader("get_user_profile", paramNames, paramVals,true);
                if (tbl.HasRows)
                {
                    user = new UserContainer();
                    tbl.Read();
                    user.dbID = tbl.GetInt(0);
                    user.username = tbl.GetString(1);
                    user.email = tbl.GetString(2);
                    user.steamid = tbl.GetString(3);
                    bool? tmp = int.Parse(tbl[4].ToString()) == 0 ? false : true;
                    user.email_verified = tmp == null || tmp == false ? false : true;
                }
            }
        }

        public string SecurityToken
        {
            get
            {
                string result = "";
                switch (SessionStorageLocation)
                {
                    case SessionStorageLocations.Cookie:
                        result= mainPage.Request.Cookies["ARKNotifierSession"].Value;
                        break;
                    case SessionStorageLocations.Session:
                        result = mainPage.Session["ARKNotifierSession"].ToString();
                        break;
                }
                return result;
            }
            set
            {
                switch (SessionStorageLocation)
                {
                    case SessionStorageLocations.None:
                        mainPage.Session["ARKInfoSession"] = value;
                        SessionStorageLocation = SessionStorageLocations.Session;
                        break;
                    case SessionStorageLocations.Cookie:
                        mainPage.Response.Cookies["ARKNotifierSession"].Value = value;
                        mainPage.Response.Cookies["ARKNotifierSession"].Expires = DateTime.Now.AddDays(30);
                        if (mainPage.Request.Cookies["ARKNotifierSession"] != null)
                        {
                            mainPage.Request.Cookies["ARKNotifierSession"].Value = value;
                            mainPage.Request.Cookies["ARKNotifierSession"].Expires = DateTime.Now.AddDays(30);
                        }
                        break;
                    case SessionStorageLocations.Session:
                        mainPage.Session["ARKNotifierSession"] = value;
                        break;
                }
            }
        }

        public void LogOut(string redirectPath="")
        {
            if (mainPage.Request.Cookies["ARKNotifierSession"] != null)
            {
                mainPage.Response.Cookies["ARKNotifierSession"].Expires = DateTime.Now.AddDays(-1);
            }

            if (mainPage.Request.Cookies["ASP.NET_SessionId"] != null)
            {
                mainPage.Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddDays(-1);
            }

            SessionStorageLocation=SessionStorageLocations.None;
            if (!redirectPath.Equals(""))
            {
                mainPage.Redirect(redirectPath);
            }else
            {
                mainPage.Redirect("~/Default.aspx");
            }
        }

        public bool IsLoggedIn
        {
            get { return user != null; }
        }

        public bool HasSteamID
        {
            get
            {
                if (user != null)
                {
                    return !string.IsNullOrEmpty(user.steamid);
                }
                else
                {
                    return false;
                }
            }
        }

        public bool EmailVerified
        {
            get
            {
                if (user != null)
                {
                    return user.email_verified;
                }
                else
                {
                    return false;
                }
            }
        }

    }
}