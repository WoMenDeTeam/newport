using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Demo.Util;

namespace Demo.BLL
{
    public class IdolStaticFacade
    {
        private static string datafilepath = HttpContext.Current.Server.MapPath("~/statistic/data.csv");

        public static Dictionary<string,string> GetSiteStaticByTop(int top,string fieldname,string mindate,string maxdate,bool disother){
            QueryParamEntity paramEntity = new QueryParamEntity();
            paramEntity.FieldName = fieldname;
            paramEntity.MinDate = mindate;
            paramEntity.MaxDate = maxdate;
            paramEntity.Sort = "DocumentCount";
            paramEntity.DisOther = disother;
            paramEntity.DisNum = top;
            IdolQuery query = IdolQueryFactory.GetDisStyle("getquerytagvalues");
            query.queryParamsEntity = paramEntity;
            return query.GetStaticAllInfo();
        }

        public static Dictionary<string, string> GetSiteStaticInfo(QueryParamEntity paramEntity)
        {
            IdolQuery query = IdolQueryFactory.GetDisStyle("getquerytagvalues");
            query.queryParamsEntity = paramEntity;
            return query.GetStaticAllInfo();
        }

        public static Dictionary<string, string> GetTrendStaticInfo(QueryParamEntity paramEntity, string formatstr, out string Remindate, out string Remaxdate)
        {
            IdolQuery query = IdolQueryFactory.GetDisStyle("getquerytagvalues");
            query.queryParamsEntity = paramEntity;
            Dictionary<string, string> dict = query.GetStaticAllInfo();
            Dictionary<string, string> returndict = ReLoadDict(dict, formatstr, out Remindate, out Remaxdate);
            return returndict;
        }

        private static Dictionary<string, string> ReLoadDict(Dictionary<string, string> dict, string formatstr, out string mindate, out string maxdate)
        {
            dict.Remove("0");
            mindate = null;
            maxdate = null;
            Dictionary<string, string> returnDict = new Dictionary<string, string>();
            int count = 1;
            int keyscount = dict.Keys.Count;
            
            foreach (string key in dict.Keys)
            {
                string timeStr = TimeHelp.ConvertToDateTimeString(key, formatstr);
                string value = dict[key];
                if (count == keyscount)
                {
                    mindate = timeStr;
                }
                if (count == 1) {
                    maxdate = timeStr;
                }
                returnDict.Add(timeStr, value);
                count++;
            }
            return returnDict;
        }

        public static string GetSiteStaticDataJson(HttpContext context) {
            StringBuilder jsonstr = new StringBuilder();
            QueryParamEntity paramEntity = QueryParamsDao.GetEntity(context);            
            Dictionary<string, string> dict = GetSiteStaticInfo(paramEntity);
            if (dict.Keys.Count > 0)
            {
                
                jsonstr.Append("{");
                foreach (string key in dict.Keys)
                {
                    jsonstr.AppendFormat("\"{0}\":\"{1}\",", EncodeByEscape.GetEscapeStr(key), dict[key]);
                }
                
                jsonstr.Append("\"SuccessCode\":1}");
            }
            return jsonstr.ToString(); 
        }
    }
}
