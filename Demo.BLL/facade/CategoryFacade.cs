using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Demo.DAL;
using Demo.Util;

namespace Demo.BLL
{
    public static class CategoryFacade
    {
        private static CATEGORYEntity.CATEGORYDAO Dao = new CATEGORYEntity.CATEGORYDAO();
        public static IList<CATEGORYEntity> GetCategoryEntityList(string strwhere)
        {

            IList<CATEGORYEntity> list = Dao.Find(strwhere);
            return list;
        }

        public static bool Delete(int categoryid)
        {
            return Dao.Delete(categoryid);
        }

        public static void Add(CATEGORYEntity entity)
        {
            Dao.Add(entity);
        }

        public static CATEGORYEntity FindById(long id)
        {
            return Dao.FindById(id);
        }
        public static IList<CATEGORYEntity> Find(int top, string where)
        {
            return Dao.Find(top, where);
        }

        public static CATEGORYEntity Find(string where)
        {
            IList<CATEGORYEntity> list = Dao.Find(1, where);
            if (list.Count > 0)
            {
                return list[0];
            }
            else
            {
                return null;
            }
        }

        public static void Update(CATEGORYEntity entity)
        {
            Dao.Update(entity);
        }

        public static DataTable GetDtByCategoryId(long categoryid)
        {
            return Dao.GetDTByCategoryId(categoryid);
        }

        public static DataTable GetTrendDt(long categoryid)
        {
            DataSet ds = Dao.GetTrebdDataSet(categoryid);
            if (ds != null)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

        public static CATEGORYEntity GetCategoryEntity(string categoryid)
        {
            string strWhere = " CATEGORYID=" + categoryid;
            IList<CATEGORYEntity> list = Dao.Find(strWhere);
            if (list.Count > 0)
            {
                return list[0];
            }
            else
            {
                return null;
            }
        }

        public static int GetRowCount(string strWhere)
        {
            return Dao.GetPagerRowsCount(strWhere);
        }

        public static IList<CATEGORYEntity> GetPagerList(string where, string orderBy, int pageSize, int start)
        {
            int pageNumber = start / pageSize + 1;
            return Dao.GetPager(where, orderBy, pageSize, pageNumber);
        }


        public static string GetPagerJsonStr(string where, string orderBy, int pageSize, int start)
        {
            int totalcount = GetRowCount(where);
            IList<CATEGORYEntity> categorylist = GetPagerList(where, orderBy, pageSize, start);
            StringBuilder jsonstr = new StringBuilder();
            jsonstr.Append("{\"totalcount\":\"").Append(totalcount).Append("\",");
            jsonstr.Append("\"entitylist\":{");
            int count = 1;
            foreach (CATEGORYEntity entity in categorylist)
            {
                jsonstr.AppendFormat("\"entity_{0}\":", count);
                jsonstr.Append("{");
                jsonstr.AppendFormat("\"id\":\"{0}\",", entity.ID.ToString());
                jsonstr.AppendFormat("\"parentcate\":\"{0}\",", entity.PARENTCATE.ToString());
                jsonstr.AppendFormat("\"categoryname\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.CATEGORYNAME));
                jsonstr.AppendFormat("\"categoryimgpath\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.CATEDISPLAY));
                jsonstr.AppendFormat("\"categoryid\":\"{0}\",", entity.CATEGORYID);
                object datetime = entity.EVENTDATE;
                string datetimestr = string.Empty;
                if (datetime != null)
                {
                    datetimestr = ((DateTime)datetime).ToString("yyyy-MM-dd");
                }
                jsonstr.AppendFormat("\"eventdate\":\"{0}\"", EncodeByEscape.GetEscapeStr(datetimestr));
                jsonstr.Append("},");
                count++;
            }
            jsonstr.Append("\"SuccessCode\":1}}");
            return jsonstr.ToString();
        }

        public static Dictionary<string, string> GetCategoryColumnDict(string parentcatelist)
        {
            DataTable dt = Dao.GetColumnAndCategoryDt(parentcatelist);
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string key = row["CATEGORYID"].ToString();
                    string columnid = row["ID"].ToString();
                    if (!dict.ContainsKey(key))
                    {
                        dict.Add(key, columnid);
                    }
                }
            }
            return dict;
        }
    }
}
