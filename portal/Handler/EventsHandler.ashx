<%@ WebHandler Language="C#" Class="EventsHandler" %>

using System;
using System.Web;
using Demo.BLL;
using Demo.DAL.SQLEntity;
using System.Text;
using Demo.Util;
using System.Data;
using System.Collections.Generic;
public class EventsHandler : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        string act = context.Request["queryact"];
        string action = string.Empty;
        string regjson = string.Empty;
        try
        {
            if (!string.IsNullOrEmpty(act) && act == "querystr")
            {
                action = context.Request["act"];
                int eventid = int.Parse(context.Request["eventid"]);
                DataSet ds = null;
                switch (action)
                {
                    case "initEvent":
                        regjson = "{\"success\":1,\"data\":" + GetSingleJson(EventsFacade.GetSingleEventByID(eventid)) + "}";
                        break;
                    case "initTopic":
                        ds = EventsFacade.GetEventTopicByEventID(eventid);
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            regjson = ds.Tables[0].ToJson(true);
                        }
                        break;
                    case "initClue":
                        ds = EventsFacade.GetEventClueByEventID(eventid);
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            //regjson = ds.Tables[0].ToJson();
                            regjson = ds.Tables[0].ToJson(true);
                        }
                        break;
                    case "initImg":
                        ds = EventsFacade.GetEventImgByEventID(eventid);
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            regjson = ds.Tables[0].ToJson(true);
                        }
                        break;
                    case "categoryFlash":
                        string[] info = context.Request["categoryid"].Split(',');
                        int len = info.Length;
                        string fromDate = string.Empty;
                        string toDate = string.Empty;
                        string categoryId = string.Empty;
                        if (len > 0)
                        {
                            categoryId = info[0];
                        }
                        if (len > 1)
                        {
                            fromDate = info[1];
                        }
                        if (len > 2)
                        {
                            toDate = info[2];
                        }
                        regjson = TrendFacade.GetTrendJson(categoryId, fromDate, toDate);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                regjson = "{\"success\":1}";
            }
        }
        catch (Exception ex)
        {
            regjson = "{\"error\":1,\"errorMsg\":\"" + ex.Message + "\"}";
        }
        finally
        {
            context.Response.Write(regjson);
        }

    }
    private string GetSingleJson(EventsEntity entity)
    {
        StringBuilder Eventjson = new StringBuilder();
        Eventjson.Append("{");
        Eventjson.AppendFormat("\"id\":\"{0}\",", entity.EventId.ToString());
        Eventjson.AppendFormat("\"eventname\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.EventName));
        Eventjson.AppendFormat("\"eventtime\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.EventTime.ToString()));
        Eventjson.AppendFormat("\"keywords\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.KeyWords));
        Eventjson.AppendFormat("\"summary\":\"{0}\"", EncodeByEscape.GetEscapeStr(entity.Summary));
        Eventjson.Append("}");
        return Eventjson.ToString();
    }
    private string GetEventTopicJson(List<EventTopicEntity> entity)
    {
        StringBuilder Eventjson = new StringBuilder();
        return Eventjson.ToString();
    }

    public bool IsReusable
    {
        get
        {
            return true;
        }
    }

}