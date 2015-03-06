using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Demo.DAL;
using Demo.Util;

namespace Demo.BLL
{
    public static class ReportListFacade
    {
        private static readonly REPORTLISTEntity.REPORTLISTDAO Dao = new REPORTLISTEntity.REPORTLISTDAO();

        public static int GetRowCount(string strWhere)
        {
            return Dao.GetPagerRowsCount(strWhere);
        }

        public static IList<REPORTLISTEntity> GetPagerList(string where, string orderBy, int pageSize, int start)
        {
            int pageNumber = start / pageSize + 1;
            return Dao.GetPager(where, orderBy, pageSize, pageNumber);
        }


        public static string GetPagerJsonStr(string where, string orderBy, int pageSize, int start)
        {
            int totalcount = GetRowCount(where);
            IList<REPORTLISTEntity> categorylist = GetPagerList(where, orderBy, pageSize, start);
            StringBuilder jsonstr = new StringBuilder();
            jsonstr.Append("{\"totalcount\":\"").Append(totalcount).Append("\",");
            jsonstr.Append("\"entitylist\":{");
            int count = 1;
            foreach (REPORTLISTEntity entity in categorylist)
            {
                jsonstr.AppendFormat("\"entity_{0}\":", count);
                jsonstr.Append("{");
                jsonstr.AppendFormat("\"id\":\"{0}\",", entity.ID.ToString());
                jsonstr.AppendFormat("\"title\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.TITLE));
                jsonstr.AppendFormat("\"creater\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.CREATER));
                jsonstr.AppendFormat("\"url\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.URL));
                object datetime = entity.CREATETIME;
                string datetimestr = string.Empty;
                if (datetime != null)
                {
                    datetimestr = ((DateTime)datetime).ToString("yyyy-MM-dd");
                }
                jsonstr.AppendFormat("\"creattime\":\"{0}\"", EncodeByEscape.GetEscapeStr(datetimestr));
                jsonstr.Append("},");
                count++;
            }
            jsonstr.Append("\"SuccessCode\":1}}");
            return jsonstr.ToString();
        }
    }
}
