<%@ WebHandler Language="C#" Class="LoginOut" %>

using System;
using System.Web;
using System.Configuration;

public class LoginOut : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        string SessionKey = ConfigurationManager.AppSettings["SessionKey"].ToString();
        if (context.Request.Cookies[SessionKey] != null)
        {
            HttpCookie oldCookie = context.Request.Cookies[SessionKey];
            oldCookie.Expires = DateTime.Now.AddDays(-1);
            context.Response.SetCookie(oldCookie);
        }      
        string loginUrl = "../login.html";
        context.Response.Redirect(loginUrl);  
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}