<%@ WebHandler Language="C#" Class="ReoptrToExcel" %>

using System;
using System.Web;

public class ReoptrToExcel : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        string result = "{\"success\":\"0\",\"backurl\":\"\",\"errormsg\":\"\"}";
        string switch_on = context.Request["action"];
        try
        {
            switch (switch_on)
            {
                case "getreport":
                    result = ExportExcel(context);
                    break;
                default:
                    break;
            }
        }
        catch (Exception e)
        {
            result = "{\"success\":\"0\",\"backurl\":\"\",\"errormsg\":\"" + e.ToString() + "\"}";
        }
        context.Response.Write(result);
    }
    public string ExportExcel(HttpContext context)
    {
        string result = "";
        string stime = context.Request["stime"];
        string etime = context.Request["etime"];
        string area = context.Request["area"];
        stime = string.IsNullOrEmpty(stime) ? DateTime.Now.ToString("yyyy年MM月dd日") : Convert.ToDateTime(stime).ToString("yyyy年MM月dd日");
        etime = string.IsNullOrEmpty(etime) ? DateTime.Now.ToString("yyyy年MM月dd日") : Convert.ToDateTime(etime).ToString("yyyy年MM月dd日");
        string title = string.Format("{0}-{1}{2}事故调查报告公开明细", stime, etime, area);
        string path = context.Server.MapPath("../reportfile");
        Demo.BLL.facade.AccidentReportFacade.ToExcel(context.Request["ids"], path, title);
        result = "{\"success\":\"1\",\"backurl\":\"" + title + ".xlsx\",\"error\":\"\"}";
        return result;
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}