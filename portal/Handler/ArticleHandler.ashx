<%@ WebHandler Language="C#" Class="ArticleHandler" %>

using System;
using System.Web;
using Demo.BLL;
using System.Configuration;
public class ArticleHandler : IHttpHandler {
    public void ProcessRequest (HttpContext context) {
       // context.Response.ContentType = "text/plain";
        //context.Response.Write("Hello World");
      
        string res = string.Empty;
        string act;
        act = context.Request.Form["act"];
        try
        {
            switch (act)
            {
                case "getArticleInfo":
                    string id = context.Request.Form["id"];
                    res = ArticleFacade.GetArticleInfoByColumnId(Convert.ToInt32(id));
                    break;
                case "getColumnByParentId":
                    string ParentId = context.Request.Form["id"];
                    res = ColumnFacade.GetColumnByParentId(ParentId);
                    break;
                case "getListByColumnId":
                    string columnid = context.Request.Form["id"];
                   // res = ArticleFacade(Convert.ToInt32(columnid));
                    break;
                case "initarticleinfo":
                    string articleid = context.Request.Form["articleid"];
                    string where = " ID=" + articleid;
                    res = ArticleFacade.GetPager(where, null, 10, 1);
                    break;
                default:
                    break;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        context.Response.Write(res);
    }
    
    public bool IsReusable {
        get {
            return false;
        }
    }

}