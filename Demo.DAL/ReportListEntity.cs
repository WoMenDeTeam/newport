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
    public partial class REPORTLISTEntity
    {
        private SqlHelper _sqlHelper;

        #region const fields
        public const string DBName = "Sentiment";
        public const string TableName = "REPORTLIST";
        public const string PrimaryKey = "PK_REPORTLIST";
        #endregion

        #region columns
        public struct Columns
        {
            public const string ID = "ID";
            public const string TITLE = "TITLE";
            public const string STATUS = "STATUS";
            public const string JSONDATA = "JSONDATA";
            public const string CREATETIME = "CREATETIME";
            public const string CREATER = "CREATER";
            public const string URL = "URL";
        }
        #endregion

        #region constructors
        public REPORTLISTEntity()
        {
            _sqlHelper = new SqlHelper(DBName);
        }

        public REPORTLISTEntity(int id, string title, DateTime createtime, string creater, string url, string status, string jsondata)
        {
            this.ID = id;

            this.TITLE = title;

            this.CREATETIME = createtime;

            this.CREATER = creater;

            this.URL = url;
            this.JSONDATA = jsondata;
            this.STATUS = status;
        }
        #endregion

        #region Properties

        public int? ID
        {
            get;
            set;
        }
        public string STATUS
        {
            get;
            set;
        }
        public string JSONDATA
        {
            get;
            set;
        }
        public string TITLE
        {
            get;
            set;
        }


        public DateTime? CREATETIME
        {
            get;
            set;
        }


        public string CREATER
        {
            get;
            set;
        }


        public string URL
        {
            get;
            set;
        }

        #endregion

        public class REPORTLISTDAO : SqlDAO<REPORTLISTEntity>
        {
            private SqlHelper _sqlHelper;
            public const string DBName = "SentimentConnStr";

            public REPORTLISTDAO()
            {
                _sqlHelper = new SqlHelper(DBName);
            }

            public override void Add(REPORTLISTEntity entity)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into REPORTLIST(");
                strSql.Append("TITLE,CREATETIME,CREATER,URL,STATUS,JSONDATA)");
                strSql.Append(" values (");
                strSql.Append("@TITLE,@CREATETIME,@CREATER,@URL,@STATUS,@JSONDATA)");
                SqlParameter[] parameters = {
					new SqlParameter("@TITLE",SqlDbType.NVarChar),
					new SqlParameter("@CREATETIME",SqlDbType.DateTime),
					new SqlParameter("@CREATER",SqlDbType.NVarChar),
					new SqlParameter("@URL",SqlDbType.NVarChar),
					new SqlParameter("@STATUS",SqlDbType.Int),
					new SqlParameter("@JSONDATA",SqlDbType.NVarChar)
					};
                parameters[0].Value = entity.TITLE;
                parameters[1].Value = entity.CREATETIME;
                parameters[2].Value = entity.CREATER;
                parameters[3].Value = entity.URL;
                parameters[4].Value = entity.STATUS;
                parameters[5].Value = entity.JSONDATA;
                _sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            }

            public override void Update(REPORTLISTEntity entity)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update REPORTLIST set ");
                strSql.Append("TITLE=@TITLE,");
                strSql.Append("CREATETIME=@CREATETIME,");
                strSql.Append("CREATER=@CREATER,");
                strSql.Append("URL=@URL");
                strSql.Append("STATUS=@STATUS");
                strSql.Append("JSONDATA=@JSONDATA");

                strSql.Append(" where ID=@ID");
                SqlParameter[] parameters = {
					new SqlParameter("@TITLE",SqlDbType.NVarChar),
					new SqlParameter("@CREATETIME",SqlDbType.DateTime),
					new SqlParameter("@CREATER",SqlDbType.NVarChar),
					new SqlParameter("@URL",SqlDbType.NVarChar),
					new SqlParameter("@STATUS",SqlDbType.Int),
					new SqlParameter("@JSONDATA",SqlDbType.NVarChar),
                    new SqlParameter("@ID",SqlDbType.Int)
				};
                parameters[0].Value = entity.TITLE;
                parameters[1].Value = entity.CREATETIME;
                parameters[2].Value = entity.CREATER;
                parameters[3].Value = entity.URL;
                parameters[4].Value = entity.STATUS;
                parameters[5].Value = entity.JSONDATA;
                parameters[6].Value = entity.ID;

                _sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            }

            public bool UpdateSet(int ID, string ColumnName, string Value)
            {
                try
                {
                    StringBuilder StrSql = new StringBuilder();
                    StrSql.Append("update REPORTLIST set ");
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
            public bool UpdateSetScalar(int ID, string ColumnName, string Value)
            {
                try
                {
                    StringBuilder StrSql = new StringBuilder();
                    StrSql.Append("update REPORTLIST set ");
                    StrSql.Append(ColumnName + "=" + Value + "");
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
                string strSql = "delete from REPORTLIST where ID=" + ID;
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
                string strSql = "delete from REPORTLIST where ID in (" + ID + ")";
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

            public override void Delete(REPORTLISTEntity entity)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from REPORTLIST ");
                strSql.Append(" where ID=@primaryKeyId");
                SqlParameter[] parameters = {
						new SqlParameter("@primaryKeyId", SqlDbType.Int)
					};
                parameters[0].Value = entity.ID;
                _sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            }

            public override REPORTLISTEntity FindById(long primaryKeyId)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select * from REPORTLIST ");
                strSql.Append(" where ID=@primaryKeyId");
                SqlParameter[] parameters = {
						new SqlParameter("@primaryKeyId", SqlDbType.Int)};
                parameters[0].Value = primaryKeyId;
                DataSet ds = _sqlHelper.ExecuteDateSet(strSql.ToString(), parameters);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    REPORTLISTEntity entity = new REPORTLISTEntity();
                    if (!Convert.IsDBNull(row["ID"]))
                    {
                        entity.ID = Convert.ToInt32(row["ID"]);
                    }
                    entity.TITLE = row["TITLE"].ToString();
                    if (!Convert.IsDBNull(row["CREATETIME"]))
                    {
                        entity.CREATETIME = Convert.ToDateTime(row["CREATETIME"]);
                    }
                    entity.CREATER = row["CREATER"].ToString();
                    entity.URL = row["URL"].ToString();
                    entity.STATUS = row["STATUS"].ToString();
                    entity.JSONDATA = row["JSONDATA"].ToString();
                    return entity;
                }
                else
                {
                    return null;
                }
            }
            public List<REPORTLISTEntity> Find(string strWhere)
            {
                return Find(strWhere, null);
            }
            public override List<REPORTLISTEntity> Find(string strWhere, SqlParameter[] parameters)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select *");
                strSql.Append(" FROM REPORTLIST ");
                if (strWhere.Trim() != "")
                {
                    strSql.Append(" where " + strWhere);
                }

                DataSet ds = _sqlHelper.ExecuteDateSet(strSql.ToString(), parameters);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<REPORTLISTEntity> list = new List<REPORTLISTEntity>();
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        REPORTLISTEntity entity = new REPORTLISTEntity();
                        if (!Convert.IsDBNull(row["ID"]))
                        {
                            entity.ID = Convert.ToInt32(row["ID"]);
                        }
                        entity.TITLE = row["TITLE"].ToString();
                        if (!Convert.IsDBNull(row["CREATETIME"]))
                        {
                            entity.CREATETIME = Convert.ToDateTime(row["CREATETIME"]);
                        }
                        entity.CREATER = row["CREATER"].ToString();
                        entity.URL = row["URL"].ToString();
                        entity.STATUS = row["STATUS"].ToString();
                        entity.JSONDATA = row["JSONDATA"].ToString();

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
                strSql.Append(" FROM REPORTLIST");
                if (strWhere.Trim() != "")
                {
                    strSql.Append(" where " + strWhere);
                }
                return _sqlHelper.ExecuteDateSet(strSql.ToString(), param);
            }

            #region paging methods

            public int GetPagerRowsCount(string where)
            {
                return GetPagerRowsCount(where, null);
            }

            /// <summary>
            /// 获取分页记录总数
            /// </summary>
            /// <param name="where">条件，等同于GetPaer()方法的where</param>
            /// <returns>返回记录总数</returns>
            public int GetPagerRowsCount(string where, SqlParameter[] param)
            {
                string sql = "select count(*) from REPORTLIST ";
                if (!string.IsNullOrEmpty(where))
                {
                    sql += "where " + where;
                }

                object obj = _sqlHelper.GetSingle(sql, param);

                return obj == null ? 0 : Convert.ToInt32(obj);
            }

            public List<REPORTLISTEntity> GetPager(string where, string orderBy, int pageSize, int pageNumber)
            {
                DataTable dt = GetPager(where, null, orderBy, pageSize, pageNumber);
                List<REPORTLISTEntity> list = new List<REPORTLISTEntity>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        REPORTLISTEntity entity = new REPORTLISTEntity();
                        if (!Convert.IsDBNull(row["ID"]))
                        {
                            entity.ID = Convert.ToInt32(row["ID"]);
                        }
                        entity.TITLE = row["TITLE"].ToString();
                        if (!Convert.IsDBNull(row["CREATETIME"]))
                        {
                            entity.CREATETIME = Convert.ToDateTime(row["CREATETIME"]);
                        }
                        entity.CREATER = row["CREATER"].ToString();
                        entity.URL = row["URL"].ToString();
                        entity.STATUS = row["STATUS"].ToString();
                        entity.JSONDATA = row["JSONDATA"].ToString();

                        list.Add(entity);
                    }
                }
                return list;
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

                if (string.IsNullOrEmpty(orderBy))
                {
                    orderBy = "ID";
                }
                if (string.IsNullOrEmpty(where))
                {
                    where = "1=1";
                }

                string sql = string.Format("SELECT * FROM(  SELECT ROW_NUMBER()OVER(ORDER BY {0} ) AS RN ,* FROM REPORTLIST WHERE {1} ) AS A WHERE A.RN BETWEEN {2} AND {3}", orderBy, where, startNumber, endNumber);

                return _sqlHelper.ExecuteDateSet(sql, param).Tables[0];
            }

            #endregion

        }
    }
}

