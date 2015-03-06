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
    public class PushInfoFacade
    {
        private static readonly PUSHINFOEntity.PUSHINFODAO dao = new PUSHINFOEntity.PUSHINFODAO();

        public static IList<PUSHINFOEntity> GetListByUserId(int userid) {
            return dao.FindByUserid(userid);
        }

        public static IList<PUSHINFOEntity> GetListByRoleId(int roleid) {
            return dao.FindByRoleid(roleid);
        }

        public static string GetPushInfoDateList(int userid) {
            IList<string> list = dao.GetDateList(userid);
            StringBuilder jsonstr = new StringBuilder();
            if (list.Count > 0) {
                jsonstr.Append("{");
                jsonstr.AppendFormat("\"date\":\"{0}\",", list[0]);                   
                jsonstr.Append("\"SuccessCode\":1}");
            }
            return jsonstr.ToString();
        }

        public static string GetJsonStr(int RoleOrUserId, int type) {
            IList<PUSHINFOEntity> list = new List<PUSHINFOEntity>();
            if (type == 1) {
                list = GetListByUserId(RoleOrUserId);
            }
            else if (type == 2) {
                list = GetListByRoleId(RoleOrUserId);
            }
            if (list != null && list.Count > 0)
            {
                Dictionary<string, int> dict = new Dictionary<string, int>();
                StringBuilder url = new StringBuilder();
                foreach (PUSHINFOEntity entity in list)
                {                    
                    if (url.Length > 0) {
                        url.Append("+");
                    }
                    url.Append(entity.URL);
                    if (!dict.ContainsKey(entity.URL)) {
                        dict.Add(entity.URL, entity.PUSHTYPE.Value);
                    }
                }
                QueryParamEntity queryparams = new QueryParamEntity();
                queryparams.Text = "*";
                queryparams.MatchReference = url.ToString();
                queryparams.Sort = "Date";
                IdolQuery query = IdolQueryFactory.GetDisStyle("query");
                query.queryParamsEntity = queryparams;
                IList<IdolNewsEntity> newslist = query.GetResultList();
                StringBuilder jsonstr = new StringBuilder();
                int count = 1;
                jsonstr.Append("{");
                foreach (IdolNewsEntity newentity in newslist) {                    
                    jsonstr.AppendFormat("\"entity_{0}\":", count);                    
                    jsonstr.Append("{");
                    jsonstr.AppendFormat("\"href\":\"{0}\",", EncodeByEscape.GetEscapeStr(newentity.Href));
                    if (dict.ContainsKey(newentity.Href))
                    {
                        jsonstr.AppendFormat("\"type\":\"{0}\",", dict[newentity.Href]);
                    }
                    jsonstr.AppendFormat("\"title\":\"{0}\",", EncodeByEscape.GetEscapeStr(newentity.Title));
                    jsonstr.AppendFormat("\"time\":\"{0}\",", EncodeByEscape.GetEscapeStr(newentity.TimeStr.Substring(0, 10)));
                    jsonstr.AppendFormat("\"site\":\"{0}\"", EncodeByEscape.GetEscapeStr(newentity.SiteName));
                    jsonstr.Append("},");
                    count++;
                }
                jsonstr.Append("\"Success\":1}");
                return jsonstr.ToString();
            }
            else
            {
                return "";
            }
        }

        public static DataTable GetPagerDt(string where, string orderBy, int pageSize, int start) {
            return dao.GetPager(where, orderBy, pageSize, start);
        }

        private static int GetPagerCount(string where)
        {
            return dao.GetPagerRowsCount(where); 
        }

        public static string GetPagerList(string where, string orderBy, int pageSize, int start)
        {
            int count = GetPagerCount(where);
            int pageNumber = start / pageSize + 1;
            DataTable dt = GetPagerDt(where, orderBy, pageSize, pageNumber);
            string urls = GetUrlList(dt);
            if (!string.IsNullOrEmpty(urls))
            {
                IList<IdolNewsEntity> newslist = GetIdolNewsList(urls, pageSize);
                if (newslist.Count > 0)
                {
                    return GetBackStr(newslist, count);
                }
                else
                {
                    return "";
                }
            }
            else {
                return "";
            }
        }

        private static string GetUrlList(DataTable dt)
        {
            StringBuilder urllist = new StringBuilder();
            foreach (DataRow row in dt.Rows) {
                if (urllist.Length > 0) {
                    urllist.Append("+");
                }
                urllist.Append(row["URL"].ToString());
            }
            return urllist.ToString();            
        }

        private static string GetBackStr(IList<IdolNewsEntity> list,int totalcount)
        {
            StringBuilder jsonstr = new StringBuilder();
            jsonstr.Append("{");            
            jsonstr.AppendFormat("\"totalcount\":{0},", totalcount);
            int count = 1;
            foreach (IdolNewsEntity entity in list)
            {
                jsonstr.AppendFormat("\"entity_{0}\":", count);
                jsonstr.Append("{");
                jsonstr.AppendFormat("\"title\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.Title));
                jsonstr.AppendFormat("\"href\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.Href));
                jsonstr.AppendFormat("\"time\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.TimeStr));
                jsonstr.AppendFormat("\"site\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.SiteName));
                jsonstr.AppendFormat("\"author\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.Author));
                jsonstr.AppendFormat("\"replynum\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.ReplyNum));
                jsonstr.AppendFormat("\"clicknum\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.ClickNum));
                jsonstr.AppendFormat("\"docid\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.DocId));
                jsonstr.AppendFormat("\"content\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.Content));
                jsonstr.AppendFormat("\"allcontent\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.AllContent));
                jsonstr.AppendFormat("\"weight\":\"{0}\"", EncodeByEscape.GetEscapeStr(entity.weight));
                jsonstr.Append("},");
                count++;
            }
            jsonstr.Append("\"Success\":1}");
            return jsonstr.ToString();
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
