<%@ WebHandler Language="C#" Class="setting" %>

using System;
using System.Web;
using Demo.Util;

public class setting : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        string databasestr = context.Request.QueryString["act"];
        string dataurl = "Handler/data.ashx?act=" + databasestr;
        string setfilepath = context.Server.MapPath("~/settings.xml");
        string fileStr = FileManage.ReadStr(setfilepath);
        string backstr = fileStr.Replace("{$.#datafilepath}", dataurl);
        context.Response.Write(backstr);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}