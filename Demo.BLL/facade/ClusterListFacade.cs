using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Demo.DAL;
using Demo.Util;
using System.Data;
using System.Configuration;

namespace Demo.BLL
{
    public class ClusterListFacade
    {
        private static readonly CLUSTERLISTEntity.CLUSTERLISTDAO dao = new CLUSTERLISTEntity.CLUSTERLISTDAO();

        private static DataTable GetDt(int pagesize,int pagenumber) { 
            string orderby = " DISTYPE DESC,PARAM ASC,ID DESC";
            return dao.GetPager("", orderby, pagesize, pagenumber);
        }

        private static int GetPagerCount(string where) {
            return dao.GetPagerRowsCount(where);
        }

        public static string GetPagerStr(int pageSize, int start)
        {
            StringBuilder jsonstr = new StringBuilder();
            int count = GetPagerCount("");
            int pageNumber = start / pageSize + 1;
            DataTable dt = GetDt(pageSize, pageNumber);
            if (dt.Rows.Count > 0)
            {
                jsonstr.Append("{");
                jsonstr.AppendFormat("\"totalcount\":{0},", count);
                int clustercount = 0;
                foreach (DataRow row in dt.Rows)
                {
                    int clusterid = Convert.ToInt32(row["ID"]);
                    IList<CLUSTERINFOEntity> list = ClusterInfoFacade.FindByClusterID(clusterid);
                    if (list.Count > 0)
                    {
                        string weburl = GetUrl(list);
                        IList<IdolNewsEntity> newslit = GetIdolNewsList(weburl, list.Count);
                        Dictionary<string, IdolNewsEntity> docdict = new Dictionary<string, IdolNewsEntity>();
                        foreach (IdolNewsEntity docentity in newslit)
                        {
                            string key = docentity.Href;
                            if (!docdict.ContainsKey(key))
                            {
                                docdict.Add(key, docentity);
                            }
                        }
                        int docnum = 0;
                        bool tag = false;
                        //foreach (CLUSTERINFOEntity clusterentity in list)
                        for (int i=0;i<list.Count;i++)
                        {
                            CLUSTERINFOEntity clusterentity = list[i];
                            string clusterweburl = clusterentity.URL;
                            if (docdict.ContainsKey(clusterweburl))
                            {                                
                                IdolNewsEntity entity = docdict[clusterweburl];                                
                                if (docnum == 0)
                                {
                                    jsonstr.AppendFormat("\"entity_{0}\":", clustercount);
                                    jsonstr.Append("{");
                                    jsonstr.AppendFormat("\"title\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.Title));
                                    jsonstr.AppendFormat("\"href\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.Href));
                                    jsonstr.AppendFormat("\"time\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.TimeStr));
                                    jsonstr.AppendFormat("\"site\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.SiteName));
                                    jsonstr.AppendFormat("\"docid\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.DocId));
                                    jsonstr.AppendFormat("\"content\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.Content));
                                    jsonstr.AppendFormat("\"allcontent\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.AllContent));
                                    jsonstr.AppendFormat("\"weight\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.weight));
                                }
                                else {
                                    if (docnum == 1) {
                                        tag = true;
                                        jsonstr.Append("\"childlist\":");
                                        jsonstr.Append("{");
                                    }
                                    jsonstr.AppendFormat("\"entity_{0}\":", docnum);
                                    jsonstr.Append("{");
                                    jsonstr.AppendFormat("\"title\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.Title));
                                    jsonstr.AppendFormat("\"href\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.Href));
                                    jsonstr.AppendFormat("\"time\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.TimeStr));
                                    jsonstr.AppendFormat("\"site\":\"{0}\"", EncodeByEscape.GetEscapeStr(entity.SiteName));
                                    jsonstr.Append("},");
                                }
                                //if (docnum == list.Count - 1)
                                //{
                                //    if (tag)
                                //    {
                                //        jsonstr.Append("\"Success\":1},");
                                //    }
                                //    jsonstr.Append("\"Success\":1},");
                                //}
                                docnum++;
                            }
                            if (i==list.Count-1)
                            {
                                if (tag)
                                {
                                    jsonstr.Append("\"Success\":1},");
                                }
                                jsonstr.Append("\"Success\":1},");
                            }
                        }
                        clustercount++;
                    }
                }
                jsonstr.Append("\"Success\":1}");
            }
            return jsonstr.ToString();
        }

        public static string GetTopCluster(int top, int docnum)
        {
            StringBuilder jsonstr = new StringBuilder();
            DataTable dt = GetDt(top, 1);
            if (dt.Rows.Count > 0)
            {
                jsonstr.Append("{");
                int clustercount = 0;
                foreach (DataRow row in dt.Rows)
                {
                    int clusterid = Convert.ToInt32(row["ID"]);
                    IList<CLUSTERINFOEntity> list = ClusterInfoFacade.FindByClusterID(clusterid);
                    if (list.Count > 0)
                    {
                        string weburl = GetUrl(list);
                        IList<IdolNewsEntity> newslit = GetIdolNewsList(weburl, list.Count);
                        jsonstr.AppendFormat("\"Cluster_{0}\":", clustercount);
                        jsonstr.Append("{");
                        Dictionary<string, IdolNewsEntity> docdict = new Dictionary<string, IdolNewsEntity>();
                        foreach (IdolNewsEntity docentity in newslit)
                        {
                            string key = docentity.Href;
                            if (!docdict.ContainsKey(key)) {
                                docdict.Add(key, docentity);
                            }
                        }
                        int count = 1;
                        foreach (CLUSTERINFOEntity clusterentity in list)
                        {
                            string clusterweburl = clusterentity.URL;
                            if (docdict.ContainsKey(clusterweburl))
                            {
                                if (count > docnum) {
                                    break;
                                }
                                IdolNewsEntity entity = docdict[clusterweburl];
                                jsonstr.AppendFormat("\"entity_{0}\":", count);
                                jsonstr.Append("{");
                                jsonstr.AppendFormat("\"title\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.Title));
                                jsonstr.AppendFormat("\"href\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.Href));
                                jsonstr.AppendFormat("\"time\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.TimeStr));
                                jsonstr.AppendFormat("\"site\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.SiteName));
                                jsonstr.AppendFormat("\"author\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.Author));
                                jsonstr.AppendFormat("\"replynum\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.ReplyNum));
                                jsonstr.AppendFormat("\"docid\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.DocId));
                                jsonstr.AppendFormat("\"content\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.Content));
                                jsonstr.AppendFormat("\"allcontent\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.AllContent));
                                jsonstr.AppendFormat("\"weight\":\"{0}\"", EncodeByEscape.GetEscapeStr(entity.weight));
                                jsonstr.Append("},");
                                count++;
                            }
                        }
                        jsonstr.Append("\"SuccessCode\":1},");
                        clustercount++;
                    }
                }
                jsonstr.Append("\"SuccessCode\":1}");
            }
            return jsonstr.ToString();
        }

        private static string GetUrl(IList<CLUSTERINFOEntity> list)
        {
            StringBuilder weburl = new StringBuilder();
            foreach (CLUSTERINFOEntity entity in list)
            {
                if (weburl.Length > 0)
                {
                    weburl.Append("+");
                }
                weburl.Append(entity.URL);
            }
            return weburl.ToString();
        }

        private static IList<IdolNewsEntity> GetIdolNewsList(string urls, int count)
        {
            QueryParamEntity queryparams = new QueryParamEntity();
            queryparams.MatchReference = urls;
            queryparams.Start = 1;
            queryparams.PageSize = count;
            queryparams.DataBase = ConfigurationManager.AppSettings["ALLDATABASE"];
            IdolQuery query = IdolQueryFactory.GetDisStyle("query");
            query.queryParamsEntity = queryparams;
            return query.GetResultList();
        }
    }
}
