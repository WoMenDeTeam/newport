using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Demo.DAL.SQLEntity;
using System.Data;
using Demo.Util;

namespace Demo.BLL.facade
{
    public class AccidentReportFacade
    {

        //private static readonly ARTICLEEntity.ARTICLEDAO dao = new ARTICLEEntity.ARTICLEDAO();
        private static readonly AccidentReportEntity.AccidentReportDAO dao = new AccidentReportEntity.AccidentReportDAO();
        public static string GetPager(string where, string orderby, int pageSize, int start)
        {
            string res = string.Empty;
            int pageNumber = start / pageSize + 1;
            int rowcount = dao.GetPagerRowsCount(where, null);
            DataTable dt = dao.GetPager(where, null, orderby, pageSize, pageNumber);
            //DataTableToExcel.ExportExcel(dt);
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
                    jsonstr.AppendFormat("\"title\":\"{0}\",", EncodeByEscape.GetEscapeStr(row["Title"].ToString()));
                    jsonstr.AppendFormat("\"occurrencetime\":\"{0}\",", EncodeByEscape.GetEscapeStr(row["OccurrenceTime"].ToString()));
                    jsonstr.AppendFormat("\"publishtime\":\"{0}\",", EncodeByEscape.GetEscapeStr(row["PublishTime"].ToString()));
                    jsonstr.AppendFormat("\"createtime\":\"{0}\",", EncodeByEscape.GetEscapeStr(row["CreateTime"].ToString()));
                    jsonstr.AppendFormat("\"department\":\"{0}\",", EncodeByEscape.GetEscapeStr(row["Department"].ToString()));
                    jsonstr.AppendFormat("\"accidentlevel\":\"{0}\",", EncodeByEscape.GetEscapeStr(row["AccidentLevel"].ToString()));
                    jsonstr.AppendFormat("\"regulatorydepartment\":\"{0}\",", EncodeByEscape.GetEscapeStr(row["RegulatoryDepartment"].ToString()));
                    jsonstr.AppendFormat("\"area\":\"{0}\",", EncodeByEscape.GetEscapeStr(row["Area"].ToString()));
                    jsonstr.AppendFormat("\"url\":\"{0}\"", EncodeByEscape.GetEscapeStr(row["Url"].ToString()));

                    jsonstr.Append("},");
                    count++;
                }
                jsonstr.Append("\"Success\":1}}");
            }
            else
            {
                jsonstr.Append("{\"Success\":1,\"TotalCount\":0,\"totalcount\":0}");
                //if (data || parseInt(data["TotalCount"]) == 0) {

            }
            return jsonstr.ToString();
        }
        public static void ToExcel(string ids, string path, string title)
        {
            var dt = GetReportExcleTable(ids);
            Dictionary<string, string> clom = new Dictionary<string, string>();
            DataTableToExcel.ExportExcel(title, dt, clom, path, 1, 1, 2, dt.Columns.Count);
        }
        private static DataTable GetReportExcleTable(string ids)
        {
            string sql = string.Format("SELECT ROW_NUMBER() OVER(ORDER BY ID) AS '序号', Title AS'标题',CONVERT(VARCHAR(10), OccurrenceTime,23) AS '发生时间',Department AS '发布单位',CONVERT(VARCHAR(10), PublishTime,23) AS '发布时间',Url AS '链接'  FROM dbo.AccidentReport where id in({0}) ", ids);
            return dao.GetDataSet(sql).Tables[0];
        }

    }
}
