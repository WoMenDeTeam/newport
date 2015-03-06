<%@ WebHandler Language="C#" Class="MicroBlog" %>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IdolACINet;
using System.Configuration;
using System.Xml;
using System.Text;
using Demo.Util;
using System.Text.RegularExpressions;


public class MicroBlog : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        Connection conn = new Connection(ConfigurationManager.AppSettings["IdolHttp"], 9000);

        //context.Response.ContentType = "application/xml";
        string site = context.Request["site"];
        string type = context.Request["type"];
        string latest = context.Request["latest"];//是否去当天，1取当天
        string start = context.Request["start"];
        string pageSize = context.Request["size"];

        string fieldText = "MATCH{t_china}:C1";//+AND+MATCH{" + type + "}:C2";

        if (!string.IsNullOrEmpty(type))
        {
            fieldText += "+AND+MATCH{" + type + "}:C2";
        }
        if (!string.IsNullOrEmpty(site))
        {
            fieldText += "+AND+MATCH{" + site + "}:PARENTSITE";
        }

        string printFields = context.Request["printFields"];
        if (string.IsNullOrEmpty(printFields))
        {
            //MYSITENAME,WEBSITENAME,BBSVIEWNUM,DREDATE,DOMAINSITE,DRECONTENT
            printFields = "BBSREPLYNUM,BBSVIEWNUM,DREDATE,DRETITLE,DRECONTENT,BBSAUTHOR";
        }

        QueryCommand query = new QueryCommand();
        query.SetParam(QueryParams.TotalResults, "FALSE");

        query.MaxResults = 20;
        if (!string.IsNullOrEmpty(start))
        {
            query.SetParam(QueryParams.Start, start);
            query.SetParam(QueryParams.TotalResults, "TRUE");
            if (!string.IsNullOrEmpty(pageSize))
            {
                query.SetParam(QueryParams.MaxResults, int.Parse(start) + int.Parse(pageSize) - 1);
            }
        }
        query.SetParam(QueryParams.DatabaseMatch, ConfigurationManager.AppSettings["DATABASE"]);
        query.SetParam(QueryParams.Print, QueryParamValue.Fields);
        if (string.IsNullOrEmpty(latest))
        {
            query.SetParam(QueryParams.Sort, "Date");
        }
        else
        {
            query.SetParam(QueryParams.MinDate, 730);
            query.SetParam(QueryParams.Sort, "BBSVIEWNUM:numberdecreasing");
        }

        query.SetParam("PrintFields", printFields);

        //query.SetParam(QueryParams.Print, QueryParamValue.All);
        //query.SetParam(QueryParams.Sort, "Date");
        //query.SetParam(QueryParams.Highlight, "summaryterms");
        //query.MinScore = 50;
        query.Parameters.Add("FieldText", fieldText);
        //query.SetParam(QueryParams.Combine, "DRECONTENT");
        XmlDocument xmlDoc = conn.Execute(query).Data;

        string json = xmlDoc.ToJson();
        context.Response.Write(json);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}

public static class ExtentionMethods
{
    public static string GetText(this XmlNode node)
    {
        try
        {
            return node.InnerText;
        }
        catch
        {
            return "";
        }
    }

    public static string ToJson(this XmlDocument xmlDoc)
    {
        XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
        nsmgr.AddNamespace("autn", "http://schemas.autonomy.com/aci/");
        XmlNodeList hits = xmlDoc.SelectNodes("//autn:hit", nsmgr);

        StringBuilder result = new StringBuilder();

        result.Append("{");
        if (null != hits && hits.Count > 1)
        {
            XmlNode total = xmlDoc.SelectSingleNode("autnresponse/responsedata/autn:totalhits", nsmgr);
            result.AppendFormat("\"total\":\"{0}\",", total.GetText());
            result.AppendFormat("\"results\":{{");

            string tempStr = string.Empty;
            StringBuilder temp = new StringBuilder();
            int counter = 0;
            for (int i = 0; counter < 8 && i < hits.Count; i++)
            {
                XmlNode node = hits[i];

                //ChildNodes[6].LastChild.ChildNodes[2].
                XmlNode document = node.SelectSingleNode("autn:content/DOCUMENT", nsmgr);
                string content = document.SelectSingleNode("DRECONTENT").GetText().RemoveHTML().HTMLDecode().Trim();
                if (content.Length > 25)
                {
                    temp.AppendFormat("\"{0}\":{{\"content\":\"{1}\",\"author\":\"{2}\",\"transfer\":\"{3}\",\"time\":\"{4}\"}},", EncodeByEscape.GetEscapeStr(node.SelectSingleNode("autn:reference", nsmgr).GetText()), EncodeByEscape.GetEscapeStr(content), EncodeByEscape.GetEscapeStr(document.SelectSingleNode("BBSAUTHOR").GetText()), document.SelectSingleNode("BBSVIEWNUM").GetText(), TimeHelp.ConvertToDateTimeString(document.SelectSingleNode("DREDATE").GetText()));
                    counter++;
                }
            }

            tempStr = temp.ToString().TrimEnd(',');

            result.AppendFormat("{0}}}", tempStr);
        }

        result.Append("}");

        return result.ToString();
    }

    public static string HTMLDecode(this string str)
    {
        string s = "";
        if (str.Length == 0) return "";
        s = str.Replace("&gt;", "&");
        //s = s.Replace("&lt;", "<");
        //s = s.Replace("&gt;", ">");
        s = s.Replace("&nbsp;", " ");
        s = s.Replace("&#39;", "\'");
        s = s.Replace("&quot;", "\"");
        //s = s.replace(/<br>/g, "\n");
        return s;
    }

    /// <summary>  
    /// 替换HTML源代码  
    /// </summary>  
    /// <param name="HtmlCode">html源代码</param>  
    /// <returns></returns>  
    public static string RemoveHTML(this string HtmlCode)
    {
        string MatchVale = HtmlCode;
        foreach (Match s in Regex.Matches(HtmlCode, "<.+?>"))
        {
            MatchVale = MatchVale.Replace(s.Value, "");
        }
        return MatchVale;
    }
}

