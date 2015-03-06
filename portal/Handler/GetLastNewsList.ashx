<%@ WebHandler Language="C#" Class="GetLastNewsList" %>

using System;
using System.Web;
using System.Xml;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Data.Sql;
using System.Data;
using System.Text;
using Demo.DAL;
using Demo.Util;
using Demo.BLL;
using IdolACINet;

public class GetLastNewsList : IHttpHandler
{
    Connection conn = new Connection(ConfigurationManager.AppSettings["IdolHttp"], 9000);
    IdolNewsEntity.IdolNewsDao newDao = new IdolNewsEntity.IdolNewsDao(); 
    public void ProcessRequest(HttpContext context)
    {
        context.Response.Write("{\"newsList\":" + GetNewsList(conn, newDao) + "}");
    }

    private string GetNewsList(Connection conn, IdolNewsEntity.IdolNewsDao dao)
    {
        StringBuilder jsonstr = new StringBuilder();
        jsonstr.Append("{");
        try
        {
            QueryCommand query = new QueryCommand();
            query.Text = "*";
            query.MaxResults = 50;
            query.SetParam(QueryParams.Sort, "Date");
            query.SetParam(QueryParams.Print, QueryParamValue.Fields);
            query.SetParam(QueryParams.PrintFields, "MYSITENAME,DREDATE,WEBSITENAME");
            query.SetParam(QueryParams.Combine, "DRETITLE");
            query.SetParam(QueryParams.DatabaseMatch, ConfigurationManager.AppSettings["DATABASE"]);
            IList<IdolNewsEntity> list = dao.GetNewsList(conn.Execute(query).Data);
            int count = 1;
            int totalcount = list.Count;
            foreach (IdolNewsEntity entity in list)
            {
                if (count == totalcount)
                {
                    jsonstr.AppendFormat("\"{0}\":\"{1}_{2}_{3}\"", EncodeByEscape.GetEscapeStr(entity.Href), EncodeByEscape.GetEscapeStr(entity.Title).Replace("_", ""), EncodeByEscape.GetEscapeStr(entity.SiteName), EncodeByEscape.GetEscapeStr(entity.TimeStr));
                }
                else
                {
                    jsonstr.AppendFormat("\"{0}\":\"{1}_{2}_{3}\",", EncodeByEscape.GetEscapeStr(entity.Href), EncodeByEscape.GetEscapeStr(entity.Title).Replace("_", ""), EncodeByEscape.GetEscapeStr(entity.SiteName), EncodeByEscape.GetEscapeStr(entity.TimeStr));
                }
                count++;
            }
        }
        catch (Exception ex)
        {
            //LogWriter.WriteErrLog(ex.ToString());
        }
        jsonstr.Append("}");
        return jsonstr.ToString();
    }
    
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}