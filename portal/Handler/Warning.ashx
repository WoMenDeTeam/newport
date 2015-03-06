<%@ WebHandler Language="C#" Class="Warning" %>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using System.Xml;
using System.Configuration;
using Demo.BLL;
using Demo.Util;

public class Warning : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        string result = "";
        try
        {
            string type = context.Request["type"];
            switch (type)
            {
                case "sensive":
                    //context.Response.Write(WordWarningFacade.GetWordListWarningJsonStr(50, ""));
                    result = WordWarningFacade.GetWordListWarningJsonStr(50, "");
                    break;
                case "view":
                    //context.Response.Write(GetViewJsonStr());
                    result = GetViewJsonStr();
                    break;
                default:
                    break;
            }
        }
        catch (Exception e)
        {

            result = "{\"errormsg\":\"" + e.ToString() + "\"}";
        }
        context.Response.Write(result);
    }

    private string GetViewJsonStr()
    {
        QueryParamEntity paramsEntity = new QueryParamEntity();
        paramsEntity.Text = Demo.Util.Common.GetFilterKeyWords();
        paramsEntity.Start = 1;
        paramsEntity.PageSize = 8;
        paramsEntity.Sort = "READNUM:numberdecreasing";
        paramsEntity.FieldText = "MATCH{bbs}:C1";
        paramsEntity.PrintFields = "DRETITLE,MYSITENAME,DREDATE,DOMAINSITENAME,READNUM,REPLYNUM";
        paramsEntity.DataBase = ConfigurationManager.AppSettings["DATABASE"];
        paramsEntity.MinDate = DateTime.Now.AddDays(-30).ToString("dd/MM/yyyy");
        IdolQuery query = IdolQueryFactory.GetDisStyle("query");
        query.queryParamsEntity = paramsEntity;
        IList<IdolNewsEntity> doclist = query.GetResultList();
        StringBuilder jsonstr = new StringBuilder();
        if (doclist.Count > 0)
        {
            jsonstr.Append("{");
            int count = 1;
            foreach (IdolNewsEntity entity in doclist)
            {
                jsonstr.AppendFormat("\"entity_{0}\":", count);
                jsonstr.Append("{");
                jsonstr.AppendFormat("\"title\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.Title));
                jsonstr.AppendFormat("\"href\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.Href));
                jsonstr.AppendFormat("\"time\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.TimeStr));
                jsonstr.AppendFormat("\"viewcount\":\"{0}\"", EncodeByEscape.GetEscapeStr(entity.ClickNum));
                jsonstr.Append("},");
                count++;
            }
            jsonstr.Append("\"SuccessCode\":1}");
        }
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
