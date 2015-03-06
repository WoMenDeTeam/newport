<%@ WebHandler Language="C#" Class="GetUserTree" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Configuration;
using Demo.Util;
using Demo.BLL;

public class GetUserTree : IHttpHandler, IRequiresSessionState
{
    private static string SessionKey = ConfigurationManager.AppSettings["SessionKey"].ToString();
    public void ProcessRequest (HttpContext context) {        
        if (context.Request.Cookies[SessionKey] != null)
        {
            string userInfo = context.Request.Cookies[SessionKey].Value;
            string ueserid = DESEncrypt.Decrypt(userInfo).Split('|')[2];
            context.Response.Write(UsersInRolesFacade.GetTreeStr());
        }        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}