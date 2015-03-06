using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Demo.DAL;
using Demo.Util;

namespace Demo.BLL
{
    public static class ColumnFacade
    {
        private static COLUMNDEFEntity.COLUMNDEFDAO dao = new COLUMNDEFEntity.COLUMNDEFDAO();
        public static COLUMNDEFEntity GetEntityById(long id) {
            COLUMNDEFEntity entity = dao.FindById(id);
            return entity;
        }

        public static IList<COLUMNDEFEntity> GetListBySiteId(int siteid) {
            string strWhere = " SITEID=" + siteid.ToString();
            return dao.Find(strWhere);
        }

        public static IList<COLUMNDEFEntity> GetList(string strwhere)
        {
            return dao.Find(strwhere);
        }

        public static DataTable GetPagerDT(int start, int pageSize, string strwhere, string strorder)
        {
            return dao.GetPagerDT(start, strwhere, pageSize, strorder);
        }

        public static string GetJsonStr(int start, int pageSize, string strwhere,string strorder)
        {
            DataTable categorydt = GetPagerDT(start, pageSize, strwhere, strorder);
            StringBuilder jsonstr = new StringBuilder();
            jsonstr.Append("{\"totalcount\":\"").Append(categorydt.Rows.Count).Append("\",");
            jsonstr.Append("\"entitylist\":{");
            int count = 1;
            foreach (DataRow row in categorydt.Rows)
            {
                jsonstr.AppendFormat("\"entity_{0}\":", count);
                jsonstr.Append("{");
                jsonstr.AppendFormat("\"id\":\"{0}\",", row["ID"]);
                jsonstr.AppendFormat("\"parentcate\":\"{0}\",", row["PARENTCATE"]);
                jsonstr.AppendFormat("\"categoryname\":\"{0}\",", EncodeByEscape.GetEscapeStr(row["CATEGORYNAME"].ToString()));
                jsonstr.AppendFormat("\"categoryimgpath\":\"{0}\",", EncodeByEscape.GetEscapeStr(row["CATEDISPLAY"].ToString()));
                jsonstr.AppendFormat("\"categoryid\":\"{0}\",", row["CATEGORYID"]);
                jsonstr.AppendFormat("\"columnid\":\"{0}\",", row["COLUMNID"]);
                string datetime = row["EVENTDATE"].ToString();
                string datetimestr = string.Empty;
                if (!string.IsNullOrEmpty(datetime))
                {
                    datetimestr = Convert.ToDateTime(datetime).ToString("yyyy-MM-dd");
                }
                jsonstr.AppendFormat("\"eventdate\":\"{0}\"", EncodeByEscape.GetEscapeStr(datetimestr));
                jsonstr.Append("},");
                count++;
            }
            jsonstr.Append("\"SuccessCode\":1}}");
            return jsonstr.ToString();
        }
        public static string GetColumnByParentId(string parentid)
        {
            List<COLUMNDEFEntity> list = GetList("parentid=" + parentid) as List<COLUMNDEFEntity>;
            StringBuilder CJson = new StringBuilder();
            if(list==null){return "";}
            CJson.Append("{");
            for (int i = 0; i < list.Count; i++)
            {
                CJson.AppendFormat("\"entity_{0}\":", i);
                CJson.Append("{");
                CJson.AppendFormat("\"id\":\"{0}\",",list[i].ID);
                CJson.AppendFormat("\"columnName\":\"{0}\"", EncodeByEscape.GetEscapeStr(list[i].COLUMNNAME));
                CJson.Append("},");
            }
            CJson.Append("\"Success\":1}");

            return CJson.ToString();
        }
    }
}
