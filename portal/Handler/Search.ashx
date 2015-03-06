<%@ WebHandler Language="C#" Class="Search" %>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Threading;
using Demo.BLL;
using Demo.DAL;
using Demo.Util;
using System.Configuration;
using System.Web.SessionState;

public class Search : IHttpHandler, IRequiresSessionState
{
    private static string SessionKey = ConfigurationManager.AppSettings["SessionKey"].ToString();
    public void ProcessRequest(HttpContext context)
    {
        try
        {
            if (context.Request["act"] == "weiboCountent")
            {
                string s = "";
            }

            string columnId = context.Request["columnid"];
            string sitetype = context.Request["sitetype"];
            string isRecordClick = context.Request["isrecordclick"];
            string isRecordKeyword = context.Request["isrecordkeyword"];
            QueryParamEntity queryParamsEntity = new QueryParamEntity();
            if (!string.IsNullOrEmpty(columnId))
            {
                int top = Convert.ToInt32(context.Request["page_size"]);
                IList<ARTICLEEntity> articlelist = ArticleFacade.FindNew(top, Convert.ToInt32(columnId), Convert.ToInt32(sitetype));
                if (articlelist != null && articlelist.Count > 0)
                {
                    queryParamsEntity.MatchReference = ArticleFacade.GetArticleUrlList(articlelist);
                    queryParamsEntity.Text = "*";
                    queryParamsEntity.FieldText = context.Request["fieldtext"];
                    queryParamsEntity.Start = 1;
                    queryParamsEntity.DataBase = ConfigurationManager.AppSettings["ALLDATABASE"];
                    queryParamsEntity.PageSize = top;
                    IdolQuery query = IdolQueryFactory.GetDisStyle("query");
                    query.queryParamsEntity = queryParamsEntity;
                    IList<IdolNewsEntity> newslist = query.GetResultList();
                    context.Response.Write(ArticleFacade.GetArticleJsonStr(articlelist, newslist, Convert.ToInt32(context.Request["page_size"])));
                }
            }
            else
            {
                DoSearch(context, queryParamsEntity);
                if (!string.IsNullOrEmpty(isRecordClick))
                {
                    Record("recordclick", context);
                }
                if (!string.IsNullOrEmpty(isRecordKeyword))
                {
                    Record("recordkeyword", context);
                }
            }
        }
        catch (Exception e)
        {
            context.Response.Write(e.ToString());
        }

    }

    private void Record(string recordtype, HttpContext context)
    {
        string visitorAddr = context.Request.UserHostAddress;
        string AccId = GetAccId(context);
        switch (recordtype)
        {
            case "recordclick":
                string rawref = EncodeByEscape.GetUnEscapeStr(context.Request["matchreference"]);
                string clickref = EncodeByEscape.GetUnEscapeStr(context.Request["clickref"]);
                ClickDetailFacade.add(AccId, rawref, clickref, visitorAddr);
                break;
            case "recordkeyword":
                string keyword = context.Request["text"];
                KeywordDetailFacade.add(AccId, keyword, visitorAddr);
                break;
            default:
                break;
        }
    }

    private string GetAccId(HttpContext context)
    {
        if (context.Request.Cookies[SessionKey] != null)
        {
            string userInfo = context.Request.Cookies[SessionKey].Value;
            return DESEncrypt.Decrypt(userInfo).Split('|')[3];
        }
        else
        {
            return null;
        }
    }

    private void DoSearch(HttpContext context, QueryParamEntity queryParamsEntity)
    {
        string action = context.Request["action"];
        if (!string.IsNullOrEmpty(action))
        {
            action = action.ToLower();
            queryParamsEntity = QueryParamsDao.GetEntity(context);
            if (action == "categoryquery")
            {
                string categoryid = context.Request["category"];
                CATEGORYEntity entity = CategoryFacade.GetCategoryEntity(categoryid);
                if (entity.QUERYTYPE == "commonquery")
                {
                    action = "query";
                    string text = context.Request["text"];
                    if (!string.IsNullOrEmpty(text))
                    {
                        queryParamsEntity.Text = text;
                    }
                    else
                    {
                        queryParamsEntity.Text = entity.KEYWORD;
                    }
                    string minscore = entity.MINSCORE;
                    if (!string.IsNullOrEmpty(minscore))
                    {
                        queryParamsEntity.MinScore = minscore;
                    }
                    else
                    {
                        queryParamsEntity.MinScore = "10";
                    }
                }
                else
                {
                    string minscore = entity.MINSCORE;
                    if (!string.IsNullOrEmpty(minscore))
                    {
                        queryParamsEntity.MinScore = minscore;
                    }
                    else
                    {
                        queryParamsEntity.MinScore = "10";
                    }
                }
            }
            queryParamsEntity.Action = action;
            IdolQuery idolquery = IdolQueryFactory.GetDisStyle(action);
            idolquery.queryParamsEntity = queryParamsEntity;

            string result = "";
            string switch_on = context.Request["act"];
            switch (switch_on)
            {
                case "weiboCountent":
                case "weiboVideoCountent":
                    result = idolquery.GetHtmlStr("IdolHttp_video");
                    break;
                default:
                    result = idolquery.GetHtmlStr();
                    break;
            }


            if (string.IsNullOrEmpty(result))
            {
                result = "{\"success\":0,\"TotalCount\":0,\"totalcount\":0}";
            }
            context.Response.Write(result);
        }
    }
    public bool IsReusable
    {
        get
        {
            return true;
        }
    }

}