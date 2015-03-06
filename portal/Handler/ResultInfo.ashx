<%@ WebHandler Language="C#"  Class="ResultInfo" %>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Demo.BLL;
using Demo.Util;
using Demo.DAL;
using System.Text;
using System.Threading;

public class ResultInfo : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        string action = context.Request["action"];
        if (!string.IsNullOrEmpty(action))
        {
            action = action.ToLower();
            QueryParamEntity queryParamsEntity = QueryParamsDao.GetEntity(context);
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
            string StoreStateID = GetStoreStateID(context, queryParamsEntity, action);
            if (!string.IsNullOrEmpty(StoreStateID))
            {
                StringBuilder backstr = new StringBuilder();
                backstr.Append("{");
                backstr.AppendFormat("\"StoreStateID\":\"{0}\",", EncodeByEscape.GetEscapeStr(StoreStateID));
                backstr.AppendFormat("\"leaderinfo\":{0},", GetKeywordInfo(StoreStateID, 1, queryParamsEntity, action));
                backstr.AppendFormat("\"orginfo\":{0}", GetKeywordInfo(StoreStateID, 3, queryParamsEntity, action));
                backstr.Append("}");
                context.Response.Write(backstr.ToString());
            }
        }        
    }
    
    private string GetKeywordInfo(string StoreStateID, int type, QueryParamEntity queryParamsEntity, string action)
    {
        Dictionary<string, string> dict = RelateInfo.GetInfoByType(type);
        StringBuilder jsonstr = new StringBuilder();
        if (dict.Keys.Count > 0)
        {
            jsonstr.Append("{");
            int count = 1;
            foreach (string key in dict.Keys)
            {
                IdolQuery query = IdolQueryFactory.GetDisStyle("query");
                queryParamsEntity.StateMatchID = StoreStateID;                
                queryParamsEntity.TotalResults = true;                
                queryParamsEntity.Text = dict[key];               
                query.queryParamsEntity = queryParamsEntity;
                string totalcount = query.GetTotalCount();
                jsonstr.AppendFormat("\"entity_{0}\":", count);
                jsonstr.Append("{");
                jsonstr.AppendFormat("\"tag\":\"{0}\",", EncodeByEscape.GetEscapeStr(key));
                jsonstr.AppendFormat("\"queryrule\":\"{0}\",", EncodeByEscape.GetEscapeStr(dict[key]));
                jsonstr.AppendFormat("\"count\":\"{0}\"", totalcount);
                jsonstr.Append("},");
                Thread.Sleep(50);
                count++;
            }
            jsonstr.Append("\"SuccessCode\":1}");
        }
        return jsonstr.ToString();
    }

    private string GetStoreStateID(HttpContext context, QueryParamEntity queryParamsEntity, string action)
    {
        IdolQuery query = IdolQueryFactory.GetDisStyle(action);
        query.queryParamsEntity = queryParamsEntity;
        return query.GetStoreState();
    }
    
    

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}

