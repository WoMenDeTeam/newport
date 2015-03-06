using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Demo.DAL;
using Demo.Util;
using Demo.BLL;
using System.Data;
using System.Configuration;

namespace Demo.BLL
{
    public class ClusterResultFacade
    {
        private static readonly CITYTOTALHITSEntity.CITYTOTALHITSDAO dao = new CITYTOTALHITSEntity.CITYTOTALHITSDAO();

        public static DataTable GetDataTable(string jobName){
            return dao.GetClusterResultDT(jobName);
        }

        public static DataTable GetPagerDataTable(int pageNumber, int pageSize, string jobName, string clusterid) {
            return dao.GetClusterResultPagerDT(pageNumber, pageSize, jobName, clusterid);
        }

        public static IList<string> GetJobList()
        {
            DataTable dt = dao.GetJobListDT();
            IList<string> list = new List<string>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    list.Add(row["JOBNAME"].ToString());
                }
            }
            return list;
        }

        public static string GetLastJobName(string jobname)
        {
            string LastJobName = dao.GetLastJobName(jobname);
            return LastJobName;
        }

        public static string GetClusterJsonStr(int pageNumber, int pageSize, string jobName, string clusterid) {
            DataTable dt = GetPagerDataTable(pageNumber, pageSize, jobName, clusterid);
            StringBuilder jsonstr = new StringBuilder();
            StringBuilder urlstr = new StringBuilder();
            int totalcount = dt.Rows.Count;
            jsonstr.Append("{\"totalcount\":\"").Append(totalcount).Append("\",");
            jsonstr.Append("\"entitylist\":{");
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (DataRow row in dt.Rows)
            {
                string url = row["URL"].ToString();
                string num = row["NUM"].ToString();
                if(urlstr.Length > 0){
                    urlstr.Append("+");
                }
                urlstr.Append(url);
                if (!dict.ContainsKey(url))
                {
                    dict.Add(url, num);
                }
            }
            IList<IdolNewsEntity> newslit = GetIdolNewsList(urlstr.ToString(), dt.Rows.Count);
            int count = 1;
            foreach (IdolNewsEntity entity in newslit)
            {
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
                jsonstr.AppendFormat("\"allnum\":\"{0}\",", dict[entity.Href]);
                jsonstr.AppendFormat("\"weight\":\"{0}\"", EncodeByEscape.GetEscapeStr(entity.weight));
                jsonstr.Append("},");
                count++;
            }
            jsonstr.Append("\"SuccessCode\":1}}");
            return jsonstr.ToString();
        }

        public static string GetClusterJsonStr(string jobName, int ClusterNum, int DocNum)
        {
            DataTable dt = GetDataTable(jobName);
            StringBuilder jsonstr = new StringBuilder();            
            if (dt.Rows.Count > 0) {
                Dictionary<int, IList<string>> dict = new Dictionary<int, IList<string>>();
                Dictionary<string, string> numdict = new Dictionary<string, string>();
                foreach (DataRow row in dt.Rows) {
                    if (dict.Keys.Count > ClusterNum) {
                        break;
                    }
                    string url = row["URL"].ToString();
                    string num = row["NUM"].ToString();
                    int clusterid = Convert.ToInt32(row["CLUSTERID"]);
                    if (dict.ContainsKey(clusterid))
                    {
                        if (dict[clusterid].Count < DocNum)
                        {
                            dict[clusterid].Add(url);
                        }
                    }
                    else {
                        IList<string> newlist = new List<string>();
                        newlist.Add(url);
                        dict.Add(clusterid, newlist);
                    }
                    if (!numdict.ContainsKey(url))
                    {
                        numdict.Add(url, num);
                    }
                }
                jsonstr.Append("{");
                foreach (int key in dict.Keys) {
                    jsonstr.AppendFormat("\"Cluster_{0}\":", key);
                    jsonstr.Append("{");
                    string weburl = GetUrl(dict[key]);
                    IList<IdolNewsEntity> newslit = GetIdolNewsList(weburl, dict[key].Count);
                    int count = 1;
                    foreach (IdolNewsEntity entity in newslit)
                    {
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
                        jsonstr.AppendFormat("\"allnum\":\"{0}\",", numdict[entity.Href]);
                        jsonstr.AppendFormat("\"weight\":\"{0}\"", EncodeByEscape.GetEscapeStr(entity.weight));
                        jsonstr.Append("},");
                        count++;
                    }
                    jsonstr.Append("\"SuccessCode\":1},");

                }
                jsonstr.Append("\"SuccessCode\":1}");
            }
            return jsonstr.ToString();
        }

        public static string GetClusterJsonStr(string jobName)
        {
            DataTable dt = GetDataTable(jobName);
            StringBuilder jsonstr = new StringBuilder();
            if (dt.Rows.Count > 0)
            {
                Dictionary<int, IList<string>> dict = new Dictionary<int, IList<string>>();
                Dictionary<string, string> numdict = new Dictionary<string, string>();
                foreach (DataRow row in dt.Rows)
                {                   
                    string url = row["URL"].ToString();
                    string num = row["NUM"].ToString();
                    int clusterid = Convert.ToInt32(row["CLUSTERID"]);
                    if (dict.ContainsKey(clusterid))
                    {
                        dict[clusterid].Add(url);                      
                    }
                    else
                    {
                        IList<string> newlist = new List<string>();
                        newlist.Add(url);
                        dict.Add(clusterid, newlist);
                    }
                    if (!numdict.ContainsKey(url))
                    {
                        numdict.Add(url, num);
                    }
                }
                jsonstr.Append("{");
                foreach (int key in dict.Keys)
                {
                    jsonstr.AppendFormat("\"Cluster_{0}\":", key);
                    jsonstr.Append("{");
                    string weburl = GetUrl(dict[key]);
                    IList<IdolNewsEntity> newslit = GetIdolNewsList(weburl, dict[key].Count);
                    int count = 1;
                    foreach (IdolNewsEntity entity in newslit)
                    {
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
                        jsonstr.AppendFormat("\"allnum\":\"{0}\",", numdict[entity.Href]);
                        jsonstr.AppendFormat("\"weight\":\"{0}\"", EncodeByEscape.GetEscapeStr(entity.weight));
                        jsonstr.Append("},");
                        count++;
                    }
                    jsonstr.Append("\"SuccessCode\":1},");

                }
                jsonstr.Append("\"SuccessCode\":1}");
            }
            return jsonstr.ToString();
        }

        private static string GetUrl(IList<string> strlist) {
            StringBuilder weburl = new StringBuilder();
            foreach (string str in strlist) {
                if (weburl.Length > 0) {
                    weburl.Append("+");
                }
                weburl.Append(str);
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

    public class ClusterResultEntity{
        public string url
        {
            set;
            get;
        }

        public string title
        {
            set;
            get;
        }

        public string num
        {
            set;
            get;
        }
    }
}
