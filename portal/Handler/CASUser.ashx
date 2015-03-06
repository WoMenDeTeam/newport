<%@ WebHandler Language="C#" Class="CASUser" %>

using System;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using System.Configuration;
using System.Web.SessionState;
using Demo.BLL;
using Demo.DAL;
using log4net;
using System.Xml;
using Demo.Util;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

public class CASUser : IHttpHandler, IRequiresSessionState
{
    private static string SessionKey = ConfigurationManager.AppSettings["SessionKey"].ToString();
    public void ProcessRequest (HttpContext context) {
        string ticket = context.Request.QueryString["ticket"];
        if (!string.IsNullOrEmpty(ticket))
        {
            string userName = GetUserName(ticket); 
            if (!string.IsNullOrEmpty(userName))
            {
                UsersEntity user = UsersFacade.GetUser(userName, "123456");
                if (user != null)
                {
                    string roleid = UsersInRolesFacade.GetUserRoleIdList(user.USERID.Value);
                    if (context.Request.Cookies[SessionKey] != null)
                    {
                        HttpCookie oldCookie = context.Request.Cookies[SessionKey];
                        oldCookie.Expires = DateTime.Now.AddDays(-1);
                        context.Response.SetCookie(oldCookie);
                    }
                    HttpCookie cookie = new HttpCookie(SessionKey);
                    cookie.Value = DESEncrypt.Encrypt(user.USERNAME + "|" + roleid + "|" + user.USERID.ToString() + "|" + user.ACCID);
                    cookie.Expires = DateTime.Now.AddDays(1);
                    context.Response.SetCookie(cookie);
                    if (UsersInRolesFacade.IsLeader(user.USERID.Value))
                    {
                        context.Response.Redirect("../leader.html");
                    }
                    else
                    {
                        context.Response.Redirect("../index.html");
                    }
                    
                }
            }
            
        }
    }

    private string GetUserName(string ticket)
    {
        ILog logger = LogManager.GetLogger("Error");
        try
        {
            
            logger.Error("ticket" + ticket);
            string formaturl = "{2}proxyValidate?ticket={0}&service={1}";
            string url = string.Format(formaturl, ticket, ConfigurationManager.AppSettings["CASService"], ConfigurationManager.AppSettings["CASPostHttp"]);
            logger.Error("url:" + url);
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            string userxml = reader.ReadToEnd();
            logger.Error("userxml" + userxml);
            string username = GetUserNameFromXml(userxml);
            logger.Error("username" + username);
            return username;
        }
        catch (Exception e) {
            logger.Error("Error:" + e.ToString());
            return null;
        }
    }

    public static bool ValidateServerCertificate(object sender,X509Certificate certificate,X509Chain chain,SslPolicyErrors sslPolicyErrors)
    {
        return true;
    }

    private string GetUserNameFromXml(string xmlstr) {
        XmlDocument xmldoc = new XmlDocument();
        xmldoc.LoadXml(xmlstr);
        XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmldoc.NameTable);
        nsmgr.AddNamespace("cas", "http://www.yale.edu/tp/cas");
        XmlNode usernameNode = xmldoc.SelectSingleNode("//cas:user", nsmgr);
        return usernameNode.InnerText;
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }
}