<%@ WebHandler Language="C#" Class="User" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Text;
using Demo.Util;
using Demo.BLL;
using Demo.DAL;
using System.Configuration;
using System.Collections.Generic;
using log4net;

public class User : IHttpHandler, IRequiresSessionState
{
    private static string SessionKey = ConfigurationManager.AppSettings["SessionKey"].ToString();
    public void ProcessRequest(HttpContext context)
    {
        string action = context.Request.Form["action"];
        if (!string.IsNullOrEmpty(action))
        {
            switch (action)
            {
                case "user_login":
                    //context.Response.Write(TestUserLogin(context));
                    context.Response.Write(UserLogin(context));
                    break;
                case "check_user":
                    //context.Response.Write(TestCheckUser(context));
                    context.Response.Write(CheckUser(context));
                    break;
                case "log_visitor":
                    context.Response.Write(RecordVisitor(context));
                    break;
                case "get_user_accid":
                    context.Response.Write(GetUserAccIdJson(context));
                    break;
                default:
                    break;
            }
        }
    }

    private string TestCheckUser(HttpContext context)
    {
        return "{\"SuccessCode\":1,\"userName\":\"管理员\",\"Show\":1,\"path\":\"" + EncodeByEscape.GetEscapeStr("index.html") + "\"}";
    }
    private string CheckUser(HttpContext context)
    {
        StringBuilder jsonStr = new StringBuilder();
        string userName = GetUserName(context);
        if (!string.IsNullOrEmpty(userName))
        {
            string roleid = GetRoleId(context);
            //Dictionary<string, string> dict = GetCategoryList.GetList(roleid);
            string path = "index.html";
            if (roleid == "24" || roleid == "44" || roleid == "64")
            {
                path = "leader.html";
            }
            if (roleid == "1")
            {
                jsonStr.Append("{\"SuccessCode\":1,\"userName\":\"" + userName + "\",\"Show\":1,\"path\":\"" + EncodeByEscape.GetEscapeStr(path) + "\"}");
            }
            else
            {
                jsonStr.Append("{\"SuccessCode\":1,\"userName\":\"" + userName + "\",\"Show\":0,\"path\":\"" + EncodeByEscape.GetEscapeStr(path) + "\"}");
            }
        }
        else
        {
            string loginUrl = ConfigurationManager.AppSettings["CASPostHttp"];// + "login?service=" + ConfigurationManager.AppSettings["CASService"];
            jsonStr.Append("{\"SuccessCode\":0,\"path\":\"").Append(EncodeByEscape.GetEscapeStr(loginUrl)).Append("\"}");
        }
        return jsonStr.ToString();
    }

    private string GetUserName(HttpContext context)
    {
        if (context.Request.Cookies[SessionKey] != null)
        {
            string userInfo = context.Request.Cookies[SessionKey].Value;
            return DESEncrypt.Decrypt(userInfo).Split('|')[0];
        }
        else
        {
            return null;
        }
    }

    private string GetAccId(HttpContext context)
    {
        if (context.Request.Cookies[SessionKey] != null)
        {
            string userInfo = context.Request.Cookies[SessionKey].Value;
            return DESEncrypt.Decrypt(userInfo).Split('|')[3];
        }
        else
        {
            return null;
        }
    }

    private string GetUserAccIdJson(HttpContext context)
    {
        string AccId = GetAccId(context);
        string backStr = string.Empty;
        if (!string.IsNullOrEmpty(AccId))
        {
            backStr = "{\"SuccessCode\":1,\"AccId\":\"" + EncodeByEscape.GetEscapeStr(AccId) + "\"}";
        }
        return backStr;
    }

    private string GetRoleId(HttpContext context)
    {
        if (context.Request.Cookies[SessionKey] != null)
        {
            string userInfo = context.Request.Cookies[SessionKey].Value;
            return DESEncrypt.Decrypt(userInfo).Split('|')[1];
        }
        else
        {
            return null;
        }
    }
    private string TestUserLogin(HttpContext context)
    {
        return "{\"SuccessCode\":1,\"path\":\"index.html\"}";
    }
    private string UserLogin(HttpContext context)
    {
        string userName = context.Request.Form["User_Name"];
        string passWord = context.Request.Form["Pass_Word"];
        StringBuilder jsonStr = new StringBuilder();
        if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(passWord))
        {
            UsersEntity user = UsersFacade.GetUser(userName, passWord);
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
                jsonStr.Append("{\"SuccessCode\":1");
                if (UsersInRolesFacade.IsLeader(user.USERID.Value))
                {
                    jsonStr.Append(",\"path\":\"leader.html\"");
                }
                else
                {
                    jsonStr.Append(",\"path\":\"index.html\"");
                }
                jsonStr.AppendFormat(",\"userName\":\"{0}\"", userName);
                jsonStr.Append("}");

            }
            else
            {
                jsonStr.Append("{\"SuccessCode\":0}");
            }
        }
        else
        {
            jsonStr.Append("{\"SuccessCode\":0}");
        }
        return jsonStr.ToString();
    }

    private string RecordVisitor(HttpContext context)
    {
        try
        {
            string pageUrl = context.Request.Form["page_url"].ToString();
            string visitorAddr = context.Request.UserHostAddress;
            string time_str = DateTime.Now.ToString("yyyy-MM-dd MM:hh:ss");
            string logStr = visitorAddr + "在" + time_str + "访问了页面" + pageUrl;
            ILog logger = LogManager.GetLogger("Visitor");
            logger.Error(logStr);
            return "{\"SuccessCode\":1}";
        }
        catch (Exception e)
        {
            return "{\"Error\":1,\"msg\":\"" + e.ToString() + "\"}";
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}