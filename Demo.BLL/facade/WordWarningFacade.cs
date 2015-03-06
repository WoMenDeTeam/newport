using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Demo.DAL;
using Demo.Util;
using System.Configuration;

namespace Demo.BLL
{
    public static class WordWarningFacade
    {
        private static readonly WORDWARNINGEntity.WORDWARNINGDAO dao = new WORDWARNINGEntity.WORDWARNINGDAO();
        private static Dictionary<string, string> urldict = new Dictionary<string, string>();
        private static Dictionary<string, IdolNewsEntity> newsdict = new Dictionary<string, IdolNewsEntity>();
        private static Dictionary<string, int> sequeuecedict = new Dictionary<string, int>();
        public static IList<WORDWARNINGEntity> GetList(string where) {
            return dao.Find(where);
        }

        public static string GetWordListWarningJsonStr(int top, string where) {
            urldict.Clear();
            newsdict.Clear();
            sequeuecedict.Clear();
            IList<WORDWARNINGEntity> list = GetList(where);
            if (list.Count > 0)
            {  
                foreach (WORDWARNINGEntity entity in list)
                {
                    InitDict(entity.WORDRULE);
                }                
            }
            StringBuilder jsonstr = new StringBuilder();
            var result = from pair in sequeuecedict orderby pair.Value descending select pair;
            int count = 1;
            jsonstr.Append("{");
            foreach (KeyValuePair<string, int> pair in result)
            {
                if (count == top)
                {
                    break;
                }
                string url = pair.Key;
                string words = urldict[url];
                IdolNewsEntity docentity = newsdict[url];
                jsonstr.AppendFormat("\"{0}_{1}\":", EncodeByEscape.GetEscapeStr(words), count);
                jsonstr.Append("{");                
                jsonstr.AppendFormat("\"href\":\"{0}\",", EncodeByEscape.GetEscapeStr(docentity.Href));
                jsonstr.AppendFormat("\"time\":\"{0}\",", EncodeByEscape.GetEscapeStr(docentity.TimeStr));
                jsonstr.AppendFormat("\"title\":\"{0}\"", EncodeByEscape.GetEscapeStr(docentity.Title));
                jsonstr.Append("},");
                count++;
            }
            jsonstr.Append("\"SuccessCode\":1}");
            return jsonstr.ToString(); 
        }

        public static string GetWordWarningJsonStr(int top ,string where)
        {
            StringBuilder jsonstr = new StringBuilder();
            IList<WORDWARNINGEntity> list = GetList(where);
            if (list.Count > 0)
            {
                jsonstr.Append("{");
                int count = 0;
                foreach (WORDWARNINGEntity entity in list)
                {
                    if (count == top) {
                        break;
                    }
                    string keyword = entity.WORDRULE;
                    jsonstr.AppendFormat("\"{0}\":", EncodeByEscape.GetEscapeStr(keyword));
                    jsonstr.Append("{");
                    IdolNewsEntity docentity = GetDocEntity(keyword);
                    jsonstr.AppendFormat("\"href\":\"{0}\",", EncodeByEscape.GetEscapeStr(docentity.Href));
                    jsonstr.AppendFormat("\"time\":\"{0}\",", EncodeByEscape.GetEscapeStr(docentity.TimeStr));
                    jsonstr.AppendFormat("\"title\":\"{0}\"", EncodeByEscape.GetEscapeStr(docentity.Title));
                    jsonstr.Append("},");
                    count++;
                }
                jsonstr.Append("\"SuccessCode\":1}");
            }
            return jsonstr.ToString();
        }

        private static IdolNewsEntity GetDocEntity(string keyword) {
            QueryParamEntity paramsentity = new QueryParamEntity();
            paramsentity.Text = "\"" + keyword + "\"";
            paramsentity.Sort = "Date";
            paramsentity.Start = 1;
            paramsentity.PageSize = 1;
            paramsentity.Combine = "DRETITLE";
            paramsentity.DataBase = ConfigurationManager.AppSettings["ALLDATABASE"];
            IdolQuery query = IdolQueryFactory.GetDisStyle("query");
            query.queryParamsEntity = paramsentity;
            IList<IdolNewsEntity> doclist = query.GetResultList();
            if (doclist.Count > 0)
            {
                return doclist[0];
            }
            else {
                return null;
            }
        }

        private static void InitDict(string keyword)
        {
            QueryParamEntity paramsentity = new QueryParamEntity();
            paramsentity.Text = "\"" + keyword + "\"";
            paramsentity.Sort = "Date";
            paramsentity.Start = 1;
            paramsentity.PageSize = 5;
            paramsentity.MinDate = DateTime.Now.AddDays(-30).ToString("dd/MM/yyyy");
            paramsentity.DataBase = ConfigurationManager.AppSettings["DATABASE"];
            IdolQuery query = IdolQueryFactory.GetDisStyle("query");
            query.queryParamsEntity = paramsentity;
            IList<IdolNewsEntity> doclist = query.GetResultList();
            if (doclist.Count > 0)
            {
                foreach (IdolNewsEntity entity in doclist) {
                    string url = entity.Href;
                    if (!newsdict.ContainsKey(url))
                    {
                        newsdict.Add(url, entity);
                    }
                    if (urldict.ContainsKey(url))
                    {
                        int count = sequeuecedict[url];
                        sequeuecedict[url] = count + 1;
                        string keywords = urldict[url];
                        if (sequeuecedict[url] < 4)
                        {
                            urldict[url] = keywords + "," + keyword;
                        }
                    }
                    else {
                        sequeuecedict[url] = 1;
                        urldict[url] = keyword;
                    }
                }
            }
        }

        
    }
}
