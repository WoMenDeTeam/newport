<%@ WebHandler Language="C#" Class="hotlist" %>

using System;
using System.Web;
using Demo.BLL;
using System.Collections.Generic;
using System.Text;

public class hotlist : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        string action = context.Request["action"];
        string jobName = context.Request["jobname"];
        if (!string.IsNullOrEmpty(action)) {
            string backstr = string.Empty;
            switch (action) {
                case "initjoblist":
                    IList<string> joblist = ClusterResultFacade.GetJobList();
                    if (joblist != null && joblist.Count > 0) {
                        backstr = getjobjsonstr(joblist);
                    }
                    break;
                case "getjobinfo":
                    backstr = ClusterResultFacade.GetClusterJsonStr(jobName);
                    break;
                default:
                    break;
            }
            context.Response.Write(backstr);
        }
    }

    private string getjobjsonstr(IList<string> joblist) {
        StringBuilder jsonstr = new StringBuilder();
        int count = 1;
        jsonstr.Append("{");
        foreach (string job in joblist) {
            jsonstr.AppendFormat("\"job_{0}\":\"{1}\",", count, job);
            count++;
        }
        jsonstr.Append("\"SuccessCode\":1}");
        return jsonstr.ToString();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}