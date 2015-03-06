using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Demo.DAL;
using System.Data;
using Demo.Util;

namespace Demo.BLL
{
	public class NoteMessageFacade
	{
        private static NoteMessageEntity.NoteMessageDAO dao = new NoteMessageEntity.NoteMessageDAO();

        public static void Insert(NoteMessageEntity entity) {
            dao.Add(entity);
        }
        public static IList<NoteMessageEntity> GetList(string strWhere)
        {
            return dao.Find(strWhere);
        }

        public static void UpdateSetStatus(int value, long id)
        {
            dao.UpdateSetStatus(value, id);
        }

        public static int GetPagerCount(string strWhere)
        {
            return dao.GetPagerRowsCount(strWhere);
        }

        public static NoteMessageEntity GetEntity(long id)
        {
            return dao.FindById(id);
        }

        public static void Delete(long id)
        {
            dao.Delete(id);
        }

        public static void Delete(string idlist)
        {
            dao.Delete(idlist);
        }

        public static DataTable GetPageDt(string where, string orderby, int pageSize, int pageNum)
        {
            return dao.GetPager(where, orderby, pageSize, pageNum);
        }

        public static string GetPagerJsonStr(string where, string orderBy, int pageSize, int start)
        {
            int totalcount = GetPagerCount(where);
            int pageNumber = start / pageSize + 1;
            DataTable dt = GetPageDt(where, orderBy, pageSize, pageNumber);
            StringBuilder jsonstr = new StringBuilder();
            if (dt.Rows.Count > 0)
            {
                jsonstr.Append("{\"totalcount\":\"").Append(totalcount).Append("\",");
                jsonstr.Append("\"entitylist\":{");
                int count = 1;
                foreach (DataRow row in dt.Rows)
                {
                    jsonstr.AppendFormat("\"entity_{0}\":", count);
                    jsonstr.Append("{");

                    jsonstr.AppendFormat("\"id\":\"{0}\",", row["ID"]);
                    jsonstr.AppendFormat("\"url\":\"{0}\",", EncodeByEscape.GetEscapeStr(row["InfoUrl"].ToString()));
                    jsonstr.AppendFormat("\"title\":\"{0}\",", EncodeByEscape.GetEscapeStr(row["InfoTitle"].ToString()));
                    jsonstr.AppendFormat("\"addusername\":\"{0}\",", EncodeByEscape.GetEscapeStr(row["AddUserName"].ToString()));
                    
                    string datetime = row["AddDate"].ToString();
                    string datetimestr = string.Empty;
                    if (!string.IsNullOrEmpty(datetime))
                    {
                        datetimestr = (Convert.ToDateTime(datetime)).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    jsonstr.AppendFormat("\"adddate\":\"{0}\"", EncodeByEscape.GetEscapeStr(datetimestr));
                    
                    jsonstr.Append("},");
                    count++;
                }
                jsonstr.Append("\"SuccessCode\":1}}");

            }
            return jsonstr.ToString();
        }
	}
}
