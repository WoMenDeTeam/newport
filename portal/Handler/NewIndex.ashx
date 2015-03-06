<%@ WebHandler Language="C#"  Class="NewIndex" %>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using Demo.BLL;
using Demo.Util;

public class NewIndex : IHttpHandler
{
    private  readonly IdolClusterEntity.IdolClusterEntityDao clusterDao = new IdolClusterEntity.IdolClusterEntityDao();
    public void ProcessRequest(HttpContext context)
    {
        IList<string> jobList = new List<string>();
        StringBuilder backstr = new StringBuilder();
        backstr.Append("{");
        foreach (string key in context.Request.Form.Keys) { 
            string job = context.Request.Form[key];
            backstr.AppendFormat("\"{0}\":{1},", job, GetClusterJsonStr(job));
        }
        backstr.Append("\"SuccessCode\":1}");        
        context.Response.Write(backstr);
    }

    private string GetClusterJsonStr(string jobname) {
        //string LastJobName = ClusterResultFacade.GetLastJobName(jobname);
        //return ClusterResultFacade.GetClusterJsonStr(LastJobName, 6, 6);
        return ClusterListFacade.GetTopCluster(6, 6);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}

