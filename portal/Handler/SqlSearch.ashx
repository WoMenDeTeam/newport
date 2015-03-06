<%@ WebHandler Language="C#" Class="SqlSearch" %>

using System;
using System.Web;
using System.Collections.Generic;
using Demo.DAL;
using Demo.BLL;
using Demo.Util;
using System.Text;
using System.Configuration;
using System.Web.SessionState;

public class SqlSearch : IHttpHandler, IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        string action = context.Request["action"];
        int start = Convert.ToInt32(context.Request["start"]);
        int pagesize = Convert.ToInt32(context.Request["page_size"]);
        string strwhere = context.Request["strwhere"];
        string strorder = context.Request["strorder"];
        string category = context.Request["category"];
        if (!string.IsNullOrEmpty(action))
        {
            try
            {
                string SessionKey = ConfigurationManager.AppSettings["SessionKey"].ToString();
                //string userInfo = DESEncrypt.Decrypt(context.Request.Cookies[SessionKey].Value);
                string userID = "0";// userInfo.Split('|')[2]; //"0";// userInfo.Split('|')[2];
                switch (action)
                {
                    case "hotweibo":
                        string jsondata = EarlyWarnHotFacade.GetList(strwhere, strorder, pagesize, start).ToJson(true);
                        string totalcount = EarlyWarnHotFacade.GetTotalCount(strwhere).ToString();
                        string strJson = "{\"success\":1,\"data\":" + jsondata + ",\"totalcount\":" + totalcount + "}";
                        context.Response.Write(strJson);
                        break;
                    case "getspeciallist":
                        context.Response.Write(CategoryFacade.GetPagerJsonStr(strwhere, strorder, pagesize, start));
                        //context.Response.Write(ColumnFacade.GetJsonStr(start, pagesize, strwhere, strorder));
                        break;
                    case "getreportlist":
                        //context.Response.Write(ReportListFacade.GetPagerJsonStr(strwhere, strorder, pagesize, start));
                        context.Response.Write(ReportListFacade.GetPagerJsonStr(strwhere, strorder, pagesize, start));
                        break;
                    case "getindexspeciallist":
                        string pagerstr = CategoryFacade.GetPagerJsonStr(strwhere, strorder, pagesize, start);
                        string findWhere = " PARENTCATE=" + ConfigurationManager.AppSettings["ThemeParent"] + " AND CATETYPE=1";
                        //string pagerstr = ColumnFacade.GetJsonStr(start, pagesize, strwhere, strorder);
                        CATEGORYEntity newentity = CategoryFacade.Find(findWhere);
                        string newcategorystr = GetNewCategoryJsonStr(newentity);
                        context.Response.Write("{\"pagerstr\":" + pagerstr + ",\"newcategory\":" + newcategorystr + "}");

                        break;
                    case "getcategoryentity":
                        CATEGORYEntity categoryentity = CategoryFacade.GetCategoryEntity(category);
                        context.Response.Write("{\"sorttype\":\"" + categoryentity.CATEPATH + "\",\"eventdata\":\"" + categoryentity.EVENTDATE.Value.ToString("yyyy-MM-dd") + "\"}");
                        break;
                    case "getleaderinfo":
                        //最新舆情推送
                        if (!string.IsNullOrEmpty(userID))
                        {
                            context.Response.Write(PushInfoFacade.GetJsonStr(Convert.ToInt32(userID), 1));
                        }
                        break;
                    case "getylfllist":
                        context.Response.Write(CategoryFacade.GetPagerJsonStr(strwhere, strorder, pagesize, start));
                        break;
                    case "getleaderinfodate":
                        if (!string.IsNullOrEmpty(userID))
                        {
                            context.Response.Write(PushInfoFacade.GetPushInfoDateList(Convert.ToInt32(userID)));
                        }
                        break;
                    case "getleaderlist":
                        if (!string.IsNullOrEmpty(strwhere))
                        {
                            strwhere = strwhere + " AND USERID=" + userID;
                        }
                        else
                        {
                            strwhere = " USERID=" + userID;
                        }
                        string timestr = context.Request["time_str"];
                        if (!string.IsNullOrEmpty(timestr))
                        {
                            string startTime = GetTimeStr(timestr, 0);
                            string endTime = GetTimeStr(timestr, 1);
                            strwhere = strwhere + " AND PUSHDATE>=to_date('" + startTime + "','yyyy-MM-dd') AND PUSHDATE<to_date('" + endTime + "','yyyy-MM-dd')";
                        }
                        context.Response.Write(PushInfoFacade.GetPagerList(strwhere, strorder, pagesize, start));
                        break;
                    case "getclusterlist":
                        context.Response.Write(ClusterListFacade.GetPagerStr(pagesize, start));
                        break;
                    case "getweibolist":
                        context.Response.Write(WEIBOFacade.GetPagerJsonStr(strwhere, strorder, pagesize, start));
                        break;
                    case "getinfolist":
                        if (!string.IsNullOrEmpty(strwhere))
                        {
                            strwhere = strwhere + " AND Accepterid=" + userID;
                        }
                        else
                        {
                            strwhere = " Accepterid=" + userID;
                        }
                        context.Response.Write(NoteMessageFacade.GetPagerJsonStr(strwhere, strorder, pagesize, start));
                        break;
                    case "getListByColumnId":
                        context.Response.Write(ArticleFacade.GetPager(strwhere, strorder, pagesize, start));
                        break;
                    case "accidentreportlist":
                        //ReportListFacade.GetPagerList(strwhere, strorder, pagesize, start);
                        context.Response.Write(Demo.BLL.facade.AccidentReportFacade.GetPager(strwhere, strorder, pagesize, start));
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                context.Response.Write(e.ToString());
            }
        }
    }

    private string GetTimeStr(string timestr, int day)
    {
        DateTime basetime = Convert.ToDateTime(timestr);
        DateTime distime = basetime.AddDays(day);
        return distime.ToString("yyyy-MM-dd");
    }

    private string GetNewCategoryJsonStr(CATEGORYEntity newentity)
    {
        StringBuilder jsonstr = new StringBuilder();
        if (newentity != null)
        {
            QueryParamEntity paramentity = new QueryParamEntity();
            string action = string.Empty;
            if (newentity.QUERYTYPE == "commonquery")
            {
                action = "query";
                paramentity.Text = newentity.KEYWORD;
            }
            else
            {
                action = "categoryquery";
                paramentity.Category = newentity.CATEGORYID.ToString();
            }
            string minscore = newentity.MINSCORE;
            if (!string.IsNullOrEmpty(minscore))
            {
                paramentity.MinScore = minscore;
            }
            else
            {
                paramentity.MinScore = "20";
            }
            paramentity.TotalResults = true;
            paramentity.Print = "NoResults";
            paramentity.PrintFields = null;
            paramentity.Predict = "false";
            IdolQuery query = IdolQueryFactory.GetDisStyle(action);
            query.queryParamsEntity = paramentity;
            string totalcount = query.GetTotalCount();
            paramentity.MinDate = ConfigurationManager.AppSettings["TimeSpan"];
            query.queryParamsEntity = paramentity;
            string daycount = query.GetTotalCount();

            jsonstr.Append("{");
            jsonstr.AppendFormat("\"id\":\"{0}\",", newentity.ID.ToString());
            jsonstr.AppendFormat("\"parentcate\":\"{0}\",", newentity.PARENTCATE.ToString());
            jsonstr.AppendFormat("\"categoryname\":\"{0}\",", EncodeByEscape.GetEscapeStr(newentity.CATEGORYNAME));
            jsonstr.AppendFormat("\"categoryimgpath\":\"{0}\",", EncodeByEscape.GetEscapeStr(newentity.CATEDISPLAY));
            jsonstr.AppendFormat("\"categoryid\":\"{0}\",", newentity.CATEGORYID);
            jsonstr.AppendFormat("\"totalcount\":\"{0}\",", totalcount);
            jsonstr.AppendFormat("\"daycount\":\"{0}\"", daycount);
            jsonstr.Append("}");
        }
        if (jsonstr.Length > 0)
        {
            return jsonstr.ToString();
        }
        else
        {
            return "\"\"";
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}