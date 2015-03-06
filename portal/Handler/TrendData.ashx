<%@ WebHandler Language="C#" Class="TrendData" %>
using System;
using System.Web;
using System.Xml;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Demo.DAL;
using Demo.Util;

public class TrendData : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        string fromDate = context.Request["from_date"];
        string toDate = context.Request["to_date"];
        string categoryId = context.Request["category_id"];
        StringBuilder strSql = new StringBuilder();
        strSql.Append("SELECT T.* FROM TRENDDATA T,CATEGORY C where T.CATEGORYID = C.CATEGORYID");
        strSql.AppendFormat(" AND C.CATEGORYID = {0} order by T.\"DATE\" DESC", Convert.ToInt64(categoryId));
        SqlHelper helper = new SqlHelper("SentimentConnStr");
        DataTable dt = helper.ExecuteDateSet(strSql.ToString()).Tables[0];
        StringBuilder jsonStr = new StringBuilder();
        jsonStr.Append("{");
        int count = 0;
        for (int i = 0, j = dt.Rows.Count; i < j; i++)
        {
            DateTime time = Convert.ToDateTime(Convert.ToDateTime(dt.Rows[i][2]).ToString("yyyy-MM-dd"));
            string timeStr = TimeHelp.GetMilliSecond(time).ToString();
            jsonStr.Append("\"" + timeStr + "\":\"" + dt.Rows[i][1].ToString() + "\",");
            if (count == j - 1)
                jsonStr.Append("\"" + timeStr + "\":\"" + dt.Rows[i][1].ToString() + "\"");
            count++;
        }
        jsonStr.Append("}");
        context.Response.Write(jsonStr.ToString());
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}
