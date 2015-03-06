<%@ WebHandler Language="C#" Class="data" %>

using System;
using System.Web;
using Demo.BLL;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Demo.Util;

public class data : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        string info = context.Request["act"];
        if (!string.IsNullOrEmpty(info))
        {
            
            QueryParamEntity paramEntity = new QueryParamEntity();             
            paramEntity.DatePeriod = "day";
            paramEntity.Sort = "date";
            paramEntity.FieldName = "AUTN_DATE";
            paramEntity.DataBase = info;
            string mindate = string.Empty;
            string maxdate = string.Empty;
            Dictionary<string, string> dict = IdolStaticFacade.GetTrendStaticInfo(paramEntity, "yyyy-MM-dd", out mindate, out maxdate);
            Dictionary<string, string> backDict = ReplenishTrendData(dict,  mindate, maxdate);
            StringBuilder backstr = new StringBuilder();
            if (backDict.Keys.Count > 0)
            {
                foreach (string key in backDict.Keys)
                {
                    backstr.AppendFormat("{0},{1},{1}", key, backDict[key], backDict[key]);
                    backstr.Append("\r\n");
                }
            }
            context.Response.Write(backstr.ToString());
        }
    }

    private Dictionary<string, string> ReplenishTrendData(Dictionary<string, string> dict,  string mindate, string maxdate)
    {                
        DateTime startTime = Convert.ToDateTime(mindate);              
        DateTime endTime = Convert.ToDateTime(maxdate);
        
        Dictionary<string, string> list = new Dictionary<string, string>();
        while (endTime >= startTime)
        {
            string startTimeStr = endTime.ToString("yyyy-MM-dd");
            if (dict.ContainsKey(startTimeStr))
            {
                string value = dict[startTimeStr];
                list.Add(startTimeStr, value);
            }
            else {
                list.Add(startTimeStr, "0");
            }
            endTime = endTime.AddDays(-1);
        }
        return list;
    }

    

 
    public bool IsReusable {
        get {
            return false;
        }
    }

}