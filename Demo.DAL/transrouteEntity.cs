using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Reflection;
using System.Text;

namespace Demo.DAL
{
    [Serializable]
    public partial class TRANSROUTEEntity
    {
        private SqlHelper _sqlHelper;

        #region const fields
        public const string DBName = "Sentiment";
        public const string TableName = "TRANSROUTE";
        public const string PrimaryKey = "PK_TRANSROUTE";
        #endregion

        #region columns
        public struct Columns
        {
            public const string ID = "ID";
            public const string CATEGORY = "CATEGORY";
            public const string SITENAME = "SITENAME";
            public const string FIRSTTIME = "FIRSTTIME";
        }
        #endregion

        #region constructors
        public TRANSROUTEEntity()
        {
            _sqlHelper = new SqlHelper(DBName);
        }

        public TRANSROUTEEntity(int id, long category, string sitename, DateTime firsttime)
        {
            this.ID = id;

            this.CATEGORY = category;

            this.SITENAME = sitename;

            this.FIRSTTIME = firsttime;

        }
        #endregion

        #region Properties

        public int? ID
        {
            get;
            set;
        }


        public long? CATEGORY
        {
            get;
            set;
        }


        public string SITENAME
        {
            get;
            set;
        }


        public DateTime? FIRSTTIME
        {
            get;
            set;
        }

        #endregion

        public class TRANSROUTEDAO : SqlDAO<TRANSROUTEEntity>
        {
            private SqlHelper _sqlHelper;
            public const string DBName = "SentimentConnStr";

            public TRANSROUTEDAO()
            {
                _sqlHelper = new SqlHelper(DBName);
            }

            public override void Add(TRANSROUTEEntity entity)
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into TRANSROUTE(");
                strSql.Append("CATEGORY,SITENAME,FIRSTTIME)");
                strSql.Append(" values (");
                strSql.Append("@CATEGORY,@SITENAME,@FIRSTTIME)");
                SqlParameter[] parameters = {
					new SqlParameter("@CATEGORY",SqlDbType.BigInt),
					new SqlParameter("@SITENAME",SqlDbType.NVarChar),
					new SqlParameter("@FIRSTTIME",SqlDbType.DateTime)
					};
                parameters[0].Value = entity.CATEGORY;
                parameters[1].Value = entity.SITENAME;
                parameters[2].Value = entity.FIRSTTIME;
                _sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            }

            public override void Update(TRANSROUTEEntity entity)
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("update TRANSROUTE set ");
                strSql.Append("CATEGORY=@CATEGORY,");
                strSql.Append("SITENAME=@SITENAME,");
                strSql.Append("FIRSTTIME=@FIRSTTIME");

                strSql.Append(" where ID=@ID");
                SqlParameter[] parameters = {
					new SqlParameter("@CATEGORY",SqlDbType.BigInt),
					new SqlParameter("@SITENAME",SqlDbType.NVarChar),
					new SqlParameter("@FIRSTTIME",SqlDbType.DateTime),
					new SqlParameter("@ID",SqlDbType.Int)
				};
                parameters[0].Value = entity.CATEGORY;
                parameters[1].Value = entity.SITENAME;
                parameters[2].Value = entity.FIRSTTIME;
                parameters[3].Value = entity.ID;

                _sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            }

            public bool UpdateSet(int ID, string ColumnName, string Value)
            {
                try
                {
                    StringBuilder StrSql = new StringBuilder();
                    StrSql.Append("update TRANSROUTE set ");
                    StrSql.Append(ColumnName + "='" + Value + "'");
                    StrSql.Append(" where ID=" + ID);
                    _sqlHelper.ExecuteSql(StrSql.ToString(), null);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            public bool Delete(int ID)
            {
                string strSql = "delete from TRANSROUTE where ID=" + ID;
                try
                {
                    _sqlHelper.ExecuteSql(strSql, null);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            public bool Delete(string ID)
            {
                string strSql = "delete from TRANSROUTE where ID in (" + ID + ")";
                try
                {
                    _sqlHelper.ExecuteSql(strSql, null);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            public override void Delete(TRANSROUTEEntity entity)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from TRANSROUTE ");
                strSql.Append(" where ID=@primaryKeyId");
                SqlParameter[] parameters = {
						new SqlParameter("@primaryKeyId", SqlDbType.Int)
					};
                parameters[0].Value = entity.ID;
                _sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            }

            public override TRANSROUTEEntity FindById(long primaryKeyId)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select * from TRANSROUTE ");
                strSql.Append(" where ID=@primaryKeyId");
                SqlParameter[] parameters = {
						new SqlParameter("@primaryKeyId", SqlDbType.Int)};
                parameters[0].Value = primaryKeyId;
                DataSet ds = _sqlHelper.ExecuteDateSet(strSql.ToString(), parameters);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    TRANSROUTEEntity entity = new TRANSROUTEEntity();
                    if (!Convert.IsDBNull(row["ID"]))
                    {
                        entity.ID = Convert.ToInt32(row["ID"]);
                    }
                    if (!Convert.IsDBNull(row["CATEGORY"]))
                    {
                        entity.CATEGORY = Convert.ToInt64(row["CATEGORY"]);
                    }
                    entity.SITENAME = row["SITENAME"].ToString();
                    if (!Convert.IsDBNull(row["FIRSTTIME"]))
                    {
                        entity.FIRSTTIME = Convert.ToDateTime(row["FIRSTTIME"]);
                    }
                    return entity;
                }
                else
                {
                    return null;
                }
            }

            public List<TRANSROUTEEntity> Find(string strWhere) {
                return Find(strWhere, null);
            }

            public override List<TRANSROUTEEntity> Find(string strWhere, SqlParameter[] parameters)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select *");
                strSql.Append(" FROM TRANSROUTE ");
                if (strWhere.Trim() != "")
                {
                    strSql.Append(" where " + strWhere);
                }

                DataSet ds = _sqlHelper.ExecuteDateSet(strSql.ToString(), parameters);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<TRANSROUTEEntity> list = new List<TRANSROUTEEntity>();
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        TRANSROUTEEntity entity = new TRANSROUTEEntity();
                        if (!Convert.IsDBNull(row["ID"]))
                        {
                            entity.ID = Convert.ToInt32(row["ID"]);
                        }
                        if (!Convert.IsDBNull(row["CATEGORY"]))
                        {
                            entity.CATEGORY = Convert.ToInt64(row["CATEGORY"]);
                        }
                        entity.SITENAME = row["SITENAME"].ToString();
                        if (!Convert.IsDBNull(row["FIRSTTIME"]))
                        {
                            entity.FIRSTTIME = Convert.ToDateTime(row["FIRSTTIME"]);
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
                strSql.Append(" FROM TRANSROUTE");
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
                string sql = "select count(*) from TRANSROUTE ";
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
                PagerSql.Append("FROM (SELECT * FROM TRANSROUTE ");
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

