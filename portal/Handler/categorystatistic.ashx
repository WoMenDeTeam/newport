<%@ WebHandler Language="C#" Class="categorystatistic" %>

using System;
using System.Web;
using Demo.BLL;
using System.Text;

public class categorystatistic : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        string fromDate = string.Empty;
        string toDate = string.Empty;
        string categoryId = string.Empty;
        string[] info = context.Request["category_id"].Split(',');
        int len = info.Length;
        if (len > 0) {
            categoryId = info[0];
        }
        if (len > 1)
        {
            fromDate = info[1];
        }
        if (len > 2)
        {
            toDate = info[2];
        }
        string backstr = TrendFacade.GetTrendStr(categoryId, fromDate, toDate);
        context.Response.ContentEncoding = Encoding.GetEncoding("gb2312"); 
        context.Response.Write(backstr);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}