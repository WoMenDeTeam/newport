using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Demo.DAL;
using Demo.Util;
using System.Data;

namespace Demo.BLL
{
    public class ArticleFacade
    {
        private static readonly ARTICLEEntity.ARTICLEDAO dao = new ARTICLEEntity.ARTICLEDAO();


        public static IList<ARTICLEEntity> FindNew(int top, int columnid, int sitetype)
        {
            return dao.FindNew(top, columnid, sitetype);
        }

        public static string GetArticleUrlList(IList<ARTICLEEntity> list)
        {
            StringBuilder urllist = new StringBuilder();
            foreach (ARTICLEEntity entity in list)
            {
                if (urllist.Length > 0)
                {
                    urllist.Append("+");
                }
                urllist.Append(entity.ARTICLEEXTERNALURL);
            }
            return urllist.ToString();
        }

        public static string GetArticleInfoByColumnId(int id)
        {
            DataSet ds = dao.GetDataSet(id);
            DataTable dt = null;
            string res = string.Empty;
            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                res=GetArticleJsonStr(dt);
            }
            return res;
        }
        public static string GetPager(string where,string orderby,int pageSize,int start)
        {
            string res = string.Empty;
            int pageNumber = start / pageSize + 1;
            int rowcount=dao.GetPagerRowsCount(where);
            DataTable dt=dao.GetPager(where, orderby, pageSize, pageNumber);
            StringBuilder jsonstr = new StringBuilder();
            if (dt.Rows.Count > 0)
            {
                jsonstr.Append("{\"totalcount\":\"").Append(rowcount).Append("\",");
                jsonstr.Append("\"entitylist\":{");
                int count = 1;
                foreach (DataRow row in dt.Rows)
                {
                    jsonstr.AppendFormat("\"entity_{0}\":", count);
                    jsonstr.Append("{");
                    jsonstr.AppendFormat("\"id\":\"{0}\",", row["ID"]);
                    jsonstr.AppendFormat("\"articletitle\":\"{0}\",", EncodeByEscape.GetEscapeStr(row["articletitle"].ToString()));
                    jsonstr.AppendFormat("\"articleexternalurl\":\"{0}\",", EncodeByEscape.GetEscapeStr(row["articleexternalurl"].ToString()));
                    jsonstr.AppendFormat("\"articlecontent\":\"{0}\",", EncodeByEscape.GetEscapeStr(row["articlecontent"].ToString()));
                    jsonstr.AppendFormat("\"articlebasedate\":\"{0}\",", EncodeByEscape.GetEscapeStr(string.Format("{0:yyyy-MM-dd}",row["articlebasedate"])));
                    jsonstr.AppendFormat("\"articlesource\":\"{0}\"", EncodeByEscape.GetEscapeStr(row["articlesource"].ToString()));
                    jsonstr.Append("},");
                    count++;
                }
                jsonstr.Append("\"Success\":1}}");
            }
            return jsonstr.ToString();
        }
        public static string GetArticleJsonStr(DataTable dt)
        {
            StringBuilder jsonstr = new StringBuilder();
            jsonstr.Append("{");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonstr.AppendFormat("\"entity_{0}\":",i);
                jsonstr.Append("{");
                jsonstr.AppendFormat("\"id\":\"{0}\",", dt.Rows[i]["ID"].ToString());
                jsonstr.AppendFormat("\"articletitle\":\"{0}\",", EncodeByEscape.GetEscapeStr(dt.Rows[i]["articletitle"].ToString()));
                jsonstr.AppendFormat("\"articlesource\":\"{0}\",", EncodeByEscape.GetEscapeStr(dt.Rows[i]["articlesource"].ToString()));
                jsonstr.AppendFormat("\"articleexternalurl\":\"{0}\",", EncodeByEscape.GetEscapeStr(dt.Rows[i]["articleexternalurl"].ToString()));
                jsonstr.AppendFormat("\"articlebasedate\":\"{0}\",", EncodeByEscape.GetEscapeStr(string.Format("{0:yyyy-MM-dd}", dt.Rows[i]["articlebasedate"])));
                jsonstr.AppendFormat("\"columnid\":\"{0}\",", EncodeByEscape.GetEscapeStr(dt.Rows[i]["COLUMNID"].ToString()));
                jsonstr.AppendFormat("\"COLUMNNAME\":\"{0}\"", EncodeByEscape.GetEscapeStr(dt.Rows[i]["COLUMNNAME"].ToString()));
                jsonstr.Append("},");
            }
            jsonstr.Append("\"Success\":1}");
            return jsonstr.ToString();
        }
        public static string GetArticleJsonStr(IList<ARTICLEEntity> articlelist, IList<IdolNewsEntity> newslist, int docnum)
        {
            if (newslist.Count == 0)
            {
                return "";
            }
            Dictionary<string, IdolNewsEntity> dict = new Dictionary<string, IdolNewsEntity>();
            foreach (IdolNewsEntity newsentity in newslist)
            {
                string key = newsentity.Href;
                if (!dict.ContainsKey(key))
                {
                    dict.Add(key, newsentity);
                }
            }
            StringBuilder jsonstr = new StringBuilder();
            jsonstr.Append("{");
            string totalcount = newslist.Count.ToString();
            jsonstr.AppendFormat("\"totalcount\":{0},", totalcount);
            int count = 1;
            foreach (ARTICLEEntity articleEntity in articlelist)
            {
                if (count > docnum)
                {
                    break;
                }
                string key = articleEntity.ARTICLEEXTERNALURL;
                if (dict.ContainsKey(key))
                {
                    IdolNewsEntity docEntity = dict[key];
                    jsonstr.AppendFormat("\"entity_{0}\":", count);
                    jsonstr.Append("{");
                    jsonstr.AppendFormat("\"title\":\"{0}\",", EncodeByEscape.GetEscapeStr(docEntity.Title));
                    jsonstr.AppendFormat("\"href\":\"{0}\",", EncodeByEscape.GetEscapeStr(docEntity.Href));
                    jsonstr.AppendFormat("\"time\":\"{0}\",", EncodeByEscape.GetEscapeStr(docEntity.TimeStr));
                    jsonstr.AppendFormat("\"site\":\"{0}\",", EncodeByEscape.GetEscapeStr(docEntity.SiteName));
                    jsonstr.AppendFormat("\"author\":\"{0}\",", EncodeByEscape.GetEscapeStr(docEntity.Author));
                    jsonstr.AppendFormat("\"replynum\":\"{0}\",", EncodeByEscape.GetEscapeStr(docEntity.ReplyNum));
                    jsonstr.AppendFormat("\"clicknum\":\"{0}\",", EncodeByEscape.GetEscapeStr(docEntity.ClickNum));
                    jsonstr.AppendFormat("\"docid\":\"{0}\",", EncodeByEscape.GetEscapeStr(docEntity.DocId));
                    jsonstr.AppendFormat("\"content\":\"{0}\",", EncodeByEscape.GetEscapeStr(docEntity.Content));
                    jsonstr.AppendFormat("\"allcontent\":\"{0}\",", EncodeByEscape.GetEscapeStr(docEntity.AllContent));
                    jsonstr.AppendFormat("\"weight\":\"{0}\"", EncodeByEscape.GetEscapeStr(docEntity.weight));
                    jsonstr.Append("},");
                    count++;
                }
            }
            jsonstr.Append("\"Success\":1}");
            return jsonstr.ToString();
        }
    }
}
