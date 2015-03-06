using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;

namespace Demo.DAL
{
    [Serializable]
    public partial class CLUSTERINFOEntity
    {
        private SqlHelper _sqlHelper;

        #region const fields
        public const string DBName = "Sentiment";
        public const string TableName = "CLUSTERINFO";
        public const string PrimaryKey = "PK_CLUSTERINFO";
        #endregion

        #region columns
        public struct Columns
        {           
            public const string TITLE = "TITLE";
            public const string URL = "URL";
            public const string SITE = "SITE";
            public const string BASEDATE = "BASEDATE";
            public const string TAG = "TAG";
            public const string CLUSTERID = "CLUSTERID";
        }
        #endregion

        #region constructors
        public CLUSTERINFOEntity()
        {
            _sqlHelper = new SqlHelper(DBName);
        }

        public CLUSTERINFOEntity(string title, string url, string site, DateTime basedate, int tag, int clusterid)
        {

            this.TITLE = title;

            this.URL = url;

            this.SITE = site;

            this.BASEDATE = basedate;

            this.TAG = tag;

            this.CLUSTERID = clusterid;

        }
        #endregion

        #region Properties      


        public string TITLE
        {
            get;
            set;
        }


        public string URL
        {
            get;
            set;
        }


        public string SITE
        {
            get;
            set;
        }


        public DateTime? BASEDATE
        {
            get;
            set;
        }


        public int? TAG
        {
            get;
            set;
        }


        public int? CLUSTERID
        {
            get;
            set;
        }

        #endregion

        public class CLUSTERINFODAO : SqlDAO<CLUSTERINFOEntity>
        {
            private SqlHelper _sqlHelper;
            public const string DBName = "SentimentConnStr";

            public CLUSTERINFODAO()
            {
                _sqlHelper = new SqlHelper(DBName);
            }

            public override void Add(CLUSTERINFOEntity entity)
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into CLUSTERINFO(");
                strSql.Append("TITLE,URL,SITE,BASEDATE,TAG,CLUSTERID)");
                strSql.Append(" values (");
                strSql.Append("@TITLE,@URL,@SITE,@BASEDATE,@TAG,@CLUSTERID)");
                SqlParameter[] parameters = {
					new SqlParameter("@TITLE",SqlDbType.NVarChar),
					new SqlParameter("@URL",SqlDbType.NVarChar),
					new SqlParameter("@SITE",SqlDbType.NVarChar),
					new SqlParameter("@BASEDATE",SqlDbType.DateTime),
					new SqlParameter("@TAG",SqlDbType.Int),
					new SqlParameter("@CLUSTERID",SqlDbType.Int)
					};
                parameters[0].Value = entity.TITLE;
                parameters[1].Value = entity.URL;
                parameters[2].Value = entity.SITE;
                parameters[3].Value = entity.BASEDATE;
                parameters[4].Value = entity.TAG;
                parameters[5].Value = entity.CLUSTERID;
                _sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            }

            public bool Delete(int ClusterId)
            {
                string strSql = "delete from CLUSTERINFO where CLUSTERID=@CLUSTERID";
                try
                {
                    SqlParameter[] parameters = {
					new SqlParameter("@CLUSTERID",SqlDbType.Int)					
					};
                    parameters[0].Value = ClusterId;
                    _sqlHelper.ExecuteSql(strSql, parameters);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            public List<CLUSTERINFOEntity> FindByClusterID(int ClusterId)
            {
                string strwhere = " CLUSTERID=@CLUSTERID ORDER BY TAG DESC,BASEDATE DESC";
                SqlParameter[] parameters = {
					new SqlParameter("@CLUSTERID",SqlDbType.Int)					
					};
                parameters[0].Value = ClusterId;
                return Find(strwhere, parameters);
            }

            

            public override List<CLUSTERINFOEntity> Find(string strWhere, SqlParameter[] parameters)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select *");
                strSql.Append(" FROM CLUSTERINFO ");
                if (strWhere.Trim() != "")
                {
                    strSql.Append(" where " + strWhere);
                }

                DataSet ds = _sqlHelper.ExecuteDateSet(strSql.ToString(), parameters);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<CLUSTERINFOEntity> list = new List<CLUSTERINFOEntity>();
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        CLUSTERINFOEntity entity = new CLUSTERINFOEntity();                        
                        entity.TITLE = row["TITLE"].ToString();
                        entity.URL = row["URL"].ToString();
                        entity.SITE = row["SITE"].ToString();
                        if (!Convert.IsDBNull(row["BASEDATE"]))
                        {
                            entity.BASEDATE = Convert.ToDateTime(row["BASEDATE"]);
                        }
                        if (!Convert.IsDBNull(row["TAG"]))
                        {
                            entity.TAG = Convert.ToInt32(row["TAG"]);
                        }
                        if (!Convert.IsDBNull(row["CLUSTERID"]))
                        {
                            entity.CLUSTERID = Convert.ToInt32(row["CLUSTERID"]);
                        }

                        list.Add(entity);
                    }

                    return list;
                }
                else
                {
                    return null;
                }
            }

            public DataSet GetDataSet(string strWhere, SqlParameter[] param)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select *");
                strSql.Append(" FROM CLUSTERINFO");
                if (strWhere.Trim() != "")
                {
                    strSql.Append(" where " + strWhere);
                }
                return _sqlHelper.ExecuteDateSet(strSql.ToString(), param);
            }

            #region paging methods

            /// <summary>
            /// 获取分页记录总数
            /// </summary>
            /// <param name="where">条件，等同于GetPaer()方法的where</param>
            /// <returns>返回记录总数</returns>
            public int GetPagerRowsCount(string where, SqlParameter[] param)
            {
                string sql = "select count(*) from CLUSTERINFO ";
                if (!string.IsNullOrEmpty(where))
                {
                    sql += "where " + where;
                }

                object obj = _sqlHelper.GetSingle(sql, param);

                return obj == null ? 0 : Convert.ToInt32(obj);
            }

            /// <summary>
            /// 查询分页信息，返回当前页码的记录集
            /// </summary>
            /// <param name="where">查询条件，可为empty</param>
            /// <param name="orderBy">排序条件，可为empty</param>
            /// <param name="pageSize">每页显示记录数</param>
            /// <param name="pageNumber">当前页码</param>
            /// <returns>datatable</returns>
            public DataTable GetPager(string where, SqlParameter[] param, string orderBy, int pageSize, int pageNumber)
            {
                int startNumber = pageSize * (pageNumber - 1) + 1;
                int endNumber = pageSize * pageNumber;

                StringBuilder PagerSql = new StringBuilder();
                PagerSql.Append("SELECT * FROM (");
                PagerSql.Append(" SELECT A.*, ROWNUM RN ");
                PagerSql.Append("FROM (SELECT * FROM CLUSTERINFO ");
                if (!string.IsNullOrEmpty(where))
                {
                    PagerSql.Append(" where " + where);
                }
                if (!string.IsNullOrEmpty(orderBy))
                {
                    PagerSql.AppendFormat(" ORDER BY {0}", orderBy);
                }
                else
                {
                    PagerSql.Append(" ORDER BY ID");//默认按主键排序
                }
                PagerSql.AppendFormat(" ) A WHERE ROWNUM <= {0})", endNumber);
                PagerSql.AppendFormat(" WHERE RN >= {0}", startNumber);

                return _sqlHelper.ExecuteDateSet(PagerSql.ToString(), param).Tables[0];
            }

            #endregion

        }
    }
}

