<%@ WebHandler Language="C#" Class="Static" %>

using System;
using System.Web;
using Demo.BLL;
using System.Text;

public class Static : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        string type = context.Request.QueryString["type"];
        switch (type) { 
            case "getweibostaticbytime":
                string backstr = WEIBOFacade.GetStaticStrByTime();
                context.Response.ContentEncoding = Encoding.GetEncoding("gb2312");
                context.Response.Write(backstr);
                break;
            default:
                break;
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}