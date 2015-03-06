<%@ WebHandler Language="C#" Class="SuggestResult" %>

using System;
using System.Web;
using System.Xml;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Demo.BLL;
using Demo.Util;
using IdolACINet;

public class SuggestResult : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        string type = context.Request["type"];
        string DocIdList = context.Request["doc_id_list"];
        string DocUrlList = context.Request["doc_url_list"];
        string Keyword = context.Request["keyword"];
        string DocLinkList = context.Request["doc_link_list"];
        Connection conn = new Connection(ConfigurationManager.AppSettings["IdolHttp"], 9000);
        IdolNewsEntity.IdolNewsDao Dao = new IdolNewsEntity.IdolNewsDao();

        switch (type)
        {
            case "suggest":
                StringBuilder htmlstr = new StringBuilder();
                string[] docId = DocIdList.Split(',');
                htmlstr.Append("{");
                foreach (string id in docId)
                {
                    try
                    {
                        Command query = new Command("Suggest");
                        query.SetParam("ID", id);
                        query.SetParam("MaxResults", 4);
                        query.SetParam(QueryParams.Print, QueryParamValue.Fields);
                        query.SetParam(QueryParams.PrintFields, "DREDATE,DOMAINSITENAME");
                        query.SetParam(QueryParams.DatabaseMatch, ConfigurationManager.AppSettings["DATABASE"]);
                        query.SetParam(QueryParams.Combine, "DRETITLE");
                        IList<IdolNewsEntity> list = Dao.GetNewsList(conn.Execute(query).Data);
                        htmlstr.AppendFormat("\"{0}\":\"", id);
                        StringBuilder jsonstr = new StringBuilder();

                        foreach (IdolNewsEntity entity in list)
                        {
                            string basetitle = entity.Title;
                            string title = basetitle.Length > 20 ? basetitle.Substring(0, 20) + "..." : basetitle;
                            jsonstr.Append("<li><a ");
                            jsonstr.AppendFormat("href=\"{0}\" name=\"look_info_snapshot\" title=\"{3}\" target=\"_blank\"><span class=\"text color_7\">{1}</span><span class=\"date\">{2}</span>", entity.Href, title, entity.TimeStr.Substring(0, 10), basetitle);
                            jsonstr.AppendFormat("<span class=\"rss\">{0}</span>", entity.SiteName);
                            jsonstr.Append("</a></li>");
                        }
                        htmlstr.Append(EncodeByEscape.GetEscapeStr(jsonstr.ToString())).Append("\",");

                    }
                    catch (Exception ex)
                    {
                        //LogWriter.WriteErrLog(ex.ToString());
                        continue;
                    }
                }
                context.Response.Write(htmlstr.ToString().Substring(0, htmlstr.Length - 1) + "}");
                break;
            case "sameNewsSuggest":
                StringBuilder _SNShtmlstr = new StringBuilder();
                string[] docLink = DocLinkList.Split(',');
                _SNShtmlstr.Append("{");
                foreach (string link in docLink)
                {
                    try
                    {
                        string[] dObj = link.Split('~');
                        Command query = new Command("query");
                        query.SetParam("ID", dObj[0]);
                        query.SetParam(QueryParams.FieldText, "MATCH{" + EncodeByEscape.GetUnEscapeStr(dObj[1]) + "}:CONTURL");
                        query.SetParam("MaxResults", dObj[2]);
                        query.SetParam(QueryParams.Print, QueryParamValue.Fields);
                        query.SetParam(QueryParams.PrintFields, "DREDATE,DOMAINSITENAME");
                        query.SetParam(QueryParams.DatabaseMatch, ConfigurationManager.AppSettings["ALLDATABASE"]);
                        query.SetParam(QueryParams.Combine, "DRETITLE");
                        IList<IdolNewsEntity> list = Dao.GetNewsList(conn.Execute(query).Data);
                        _SNShtmlstr.AppendFormat("\"{0}\":\"", EncodeByEscape.GetUnEscapeStr(dObj[0]));
                        StringBuilder jsonstr = new StringBuilder();

                        foreach (IdolNewsEntity entity in list)
                        {
                            string basetitle = entity.Title;
                            string title = basetitle.Length > 20 ? basetitle.Substring(0, 20) + "..." : basetitle;
                            jsonstr.Append("<li><a ");
                            jsonstr.AppendFormat("href=\"{0}\" name=\"look_info_snapshot\" title=\"{3}\" target=\"_blank\"><span class=\"text color_7\">{1}</span><span class=\"date\">{2}</span>", entity.Href, title, entity.TimeStr.Substring(0, 10), basetitle);
                            jsonstr.AppendFormat("<span class=\"rss\">{0}</span>", entity.SiteName);
                            jsonstr.Append("</a></li>");
                        }
                        _SNShtmlstr.Append(EncodeByEscape.GetEscapeStr(jsonstr.ToString())).Append("\",");

                    }
                    catch (Exception ex)
                    {
                        //LogWriter.WriteErrLog(ex.ToString());
                        continue;
                    }
                }
                context.Response.Write(_SNShtmlstr.ToString().Substring(0, _SNShtmlstr.Length - 1) + "}");
                break;
            case "BbsOrBlog":
                context.Response.Write("{\"bbs\":\"" + GetHtmlStr(conn, Dao, Keyword, "bbs") + "\",\"blog\":\"" + GetHtmlStr(conn, Dao, Keyword, "blog") + "\"}");
                break;
            case "weibosuggest":
                if (!string.IsNullOrEmpty(DocUrlList))
                {
                    string[] urlList = EncodeByEscape.GetUnEscapeStr(DocUrlList).Split(',');
                    context.Response.Write(GetWeiBoSuggestResult(urlList));
                }
                break;
            default:
                break;
        }
    }
    private string GetHtmlStr(Connection conn, IdolNewsEntity.IdolNewsDao dao, string keyword, string type)
    {
        StringBuilder htmlstr = new StringBuilder();
        try
        {
            QueryCommand query = new QueryCommand();
            query.Text = keyword;
            query.SetParam(QueryParams.FieldText, "MATCH{" + type + "}:MYSRCTYPE");
            query.MaxResults = 4;
            query.SetParam(QueryParams.Print, "none");
            query.SetParam(QueryParams.DatabaseMatch, ConfigurationManager.AppSettings["DATABASE"]);
            query.SetParam(QueryParams.Combine, "DRETITLE");
            query.MinScore = 50;
            XmlDocument xmldoc = conn.Execute(query).Data;
            IList<IdolNewsEntity> list = dao.GetNewsList(xmldoc);
            htmlstr.Append("<ul>");
            foreach (IdolNewsEntity entity in list)
            {
                htmlstr.AppendFormat("<li><a href=\"{0}\" title=\"{1}\" target=\"_blank\">{2}</a></li>", entity.Href, entity.Title, entity.Title.Length > 11 ? entity.Title.Substring(0, 11) + "..." : entity.Title);
            }
            htmlstr.Append("</ul>");
        }
        catch (Exception ex)
        {
            //LogWriter.WriteErrLog("加载博客论坛数据出错：" + ex.ToString());
        }
        return EncodeByEscape.GetEscapeStr(htmlstr.ToString());
    }

    private string GetWeiBoSuggestResult(string[] urllist)
    {
        StringBuilder htmlstr = new StringBuilder();
        htmlstr.Append("{");
        foreach (string url in urllist)
        {
            QueryParamEntity QueryParamEntity = new QueryParamEntity();
            QueryParamEntity.MatchReference = url;
            QueryParamEntity.PrintFields = "AUTHORNAME,FORWARDNUM,DOMAINSITENAME,AUTHIRURL,SOURCEURL";
            QueryParamEntity.Start = 1;
            QueryParamEntity.PageSize = 5;
            QueryParamEntity.DataBase = "Source";
            IdolQuery query = IdolQueryFactory.GetDisStyle("query");
            query.queryParamsEntity = QueryParamEntity;
            IList<IdolNewsEntity> list = query.GetResultList();
            htmlstr.AppendFormat("\"{0}\":\"", EncodeByEscape.GetEscapeStr(url));
            StringBuilder jsonstr = new StringBuilder();

            foreach (IdolNewsEntity entity in list)
            {
                string basecontent = entity.Content;
                string content = basecontent.Length > 20 ? basecontent.Substring(0, 20) + "..." : basecontent;
                string basetimestr = entity.TimeStr;
                string distimestr = basetimestr.Length > 0 ? basetimestr.Substring(0, 10) : basetimestr;
                jsonstr.Append("<li><a ");
                jsonstr.AppendFormat("href=\"{0}\" name=\"look_info_snapshot\" title=\"{3}\" target=\"_blank\"><span class=\"text color_7\">{1}</span><span class=\"date\">{2}</span>", entity.Href, basecontent, distimestr, basecontent);
                jsonstr.AppendFormat("<span class=\"rss\">{0}</span>", entity.SiteName);
                jsonstr.Append("</a></li>");
            }
            htmlstr.Append(EncodeByEscape.GetEscapeStr(jsonstr.ToString())).Append("\",");
        }
        return htmlstr.ToString().Substring(0, htmlstr.Length - 1) + "}";
    }


    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}
