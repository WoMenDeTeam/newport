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
    public partial class CLICKDETAILEntity
    {
        private SqlHelper _sqlHelper;

        #region const fields
        public const string DBName = "Sentiment";
        public const string TableName = "CLICKDETAIL";
        public const string PrimaryKey = "PK_CLICKDETAIL";
        #endregion

        #region columns
        public struct Columns
        {
            public const string ID = "ID";
            public const string ACCID = "ACCID";
            public const string CLICKTIME = "CLICKTIME";
            public const string CLICKTIMESTR = "CLICKTIMESTR";
            public const string IPADDRESS = "IPADDRESS";
            public const string CLICKREFRENCE = "CLICKREFRENCE";
            public const string RAWREFRENCE = "RAWREFRENCE";
        }
        #endregion

        #region constructors
        public CLICKDETAILEntity()
        {
            _sqlHelper = new SqlHelper(DBName);
        }

        public CLICKDETAILEntity(long id, string accid, DateTime clicktime, string clicktimestr, string ipaddress, string clickrefrence, string rawrefrence)
        {
            this.ID = id;

            this.ACCID = accid;

            this.CLICKTIME = clicktime;

            this.CLICKTIMESTR = clicktimestr;

            this.IPADDRESS = ipaddress;

            this.CLICKREFRENCE = clickrefrence;

            this.RAWREFRENCE = rawrefrence;

        }
        #endregion

        #region Properties

        public long? ID
        {
            get;
            set;
        }


        public string ACCID
        {
            get;
            set;
        }


        public DateTime? CLICKTIME
        {
            get;
            set;
        }


        public string CLICKTIMESTR
        {
            get;
            set;
        }


        public string IPADDRESS
        {
            get;
            set;
        }


        public string CLICKREFRENCE
        {
            get;
            set;
        }


        public string RAWREFRENCE
        {
            get;
            set;
        }

        #endregion

        public class CLICKDETAILDAO : SqlDAO<CLICKDETAILEntity>
        {
            private SqlHelper _sqlHelper;
            public const string DBName = "SentimentConnStr";

            public CLICKDETAILDAO()
            {
                _sqlHelper = new SqlHelper(DBName);
            }

            public override void Add(CLICKDETAILEntity entity)
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into CLICKDETAIL(");
                strSql.Append("ACCID,CLICKTIME,CLICKTIMESTR,IPADDRESS,CLICKREFRENCE,RAWREFRENCE)");
                strSql.Append(" values (");
                strSql.Append("@ACCID,@CLICKTIME,@CLICKTIMESTR,@IPADDRESS,@CLICKREFRENCE,@RAWREFRENCE)");
                SqlParameter[] parameters = {
					new SqlParameter("@ACCID",SqlDbType.NVarChar),
					new SqlParameter("@CLICKTIME",SqlDbType.DateTime),
					new SqlParameter("@CLICKTIMESTR",SqlDbType.NVarChar),
					new SqlParameter("@IPADDRESS",SqlDbType.NVarChar),
					new SqlParameter("@CLICKREFRENCE",SqlDbType.NVarChar),
					new SqlParameter("@RAWREFRENCE",SqlDbType.NVarChar)
					};
                parameters[0].Value = entity.ACCID;
                parameters[1].Value = entity.CLICKTIME;
                parameters[2].Value = entity.CLICKTIMESTR;
                parameters[3].Value = entity.IPADDRESS;
                parameters[4].Value = entity.CLICKREFRENCE;
                parameters[5].Value = entity.RAWREFRENCE;
                _sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            }

            public override void Update(CLICKDETAILEntity entity)
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("update CLICKDETAIL set ");
                strSql.Append("ACCID=@ACCID,");
                strSql.Append("CLICKTIME=@CLICKTIME,");
                strSql.Append("CLICKTIMESTR=@CLICKTIMESTR,");
                strSql.Append("IPADDRESS=@IPADDRESS,");
                strSql.Append("CLICKREFRENCE=@CLICKREFRENCE,");
                strSql.Append("RAWREFRENCE=@RAWREFRENCE");

                strSql.Append(" where ID=@ID");
                SqlParameter[] parameters = {
					new SqlParameter("@ACCID",SqlDbType.NVarChar),
					new SqlParameter("@CLICKTIME",SqlDbType.DateTime),
					new SqlParameter("@CLICKTIMESTR",SqlDbType.NVarChar),
					new SqlParameter("@IPADDRESS",SqlDbType.NVarChar),
					new SqlParameter("@CLICKREFRENCE",SqlDbType.NVarChar),
					new SqlParameter("@RAWREFRENCE",SqlDbType.NVarChar),
					new SqlParameter("@ID",SqlDbType.BigInt)
				};
                parameters[0].Value = entity.ACCID;
                parameters[1].Value = entity.CLICKTIME;
                parameters[2].Value = entity.CLICKTIMESTR;
                parameters[3].Value = entity.IPADDRESS;
                parameters[4].Value = entity.CLICKREFRENCE;
                parameters[5].Value = entity.RAWREFRENCE;
                parameters[6].Value = entity.ID;

                _sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            }

            public bool UpdateSet(int ID, string ColumnName, string Value)
            {
                try
                {
                    StringBuilder StrSql = new StringBuilder();
                    StrSql.Append("update CLICKDETAIL set ");
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
                string strSql = "delete from CLICKDETAIL where ID=" + ID;
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
                string strSql = "delete from CLICKDETAIL where ID in (" + ID + ")";
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

            public override void Delete(CLICKDETAILEntity entity)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from CLICKDETAIL ");
                strSql.Append(" where ID=@primaryKeyId");
                SqlParameter[] parameters = {
						new SqlParameter("@primaryKeyId", SqlDbType.Int)
					};
                parameters[0].Value = entity.ID;
                _sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            }

            public override CLICKDETAILEntity FindById(long primaryKeyId)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select * from CLICKDETAIL ");
                strSql.Append(" where ID=@primaryKeyId");
                SqlParameter[] parameters = {
						new SqlParameter("@primaryKeyId", SqlDbType.Int)};
                parameters[0].Value = primaryKeyId;
                DataSet ds = _sqlHelper.ExecuteDateSet(strSql.ToString(), parameters);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    CLICKDETAILEntity entity = new CLICKDETAILEntity();
                    if (!Convert.IsDBNull(row["ID"]))
                    {
                        entity.ID = Convert.ToInt64(row["ID"]);
                    }
                    entity.ACCID = row["ACCID"].ToString();
                    if (!Convert.IsDBNull(row["CLICKTIME"]))
                    {
                        entity.CLICKTIME = Convert.ToDateTime(row["CLICKTIME"]);
                    }
                    entity.CLICKTIMESTR = row["CLICKTIMESTR"].ToString();
                    entity.IPADDRESS = row["IPADDRESS"].ToString();
                    entity.CLICKREFRENCE = row["CLICKREFRENCE"].ToString();
                    entity.RAWREFRENCE = row["RAWREFRENCE"].ToString();
                    return entity;
                }
                else
                {
                    return null;
                }
            }

            public override List<CLICKDETAILEntity> Find(string strWhere, SqlParameter[] parameters)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select *");
                strSql.Append(" FROM CLICKDETAIL ");
                if (strWhere.Trim() != "")
                {
                    strSql.Append(" where " + strWhere);
                }

                DataSet ds = _sqlHelper.ExecuteDateSet(strSql.ToString(), parameters);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<CLICKDETAILEntity> list = new List<CLICKDETAILEntity>();
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        CLICKDETAILEntity entity = new CLICKDETAILEntity();
                        if (!Convert.IsDBNull(row["ID"]))
                        {
                            entity.ID = Convert.ToInt64(row["ID"]);
                        }
                        entity.ACCID = row["ACCID"].ToString();
                        if (!Convert.IsDBNull(row["CLICKTIME"]))
                        {
                            entity.CLICKTIME = Convert.ToDateTime(row["CLICKTIME"]);
                        }
                        entity.CLICKTIMESTR = row["CLICKTIMESTR"].ToString();
                        entity.IPADDRESS = row["IPADDRESS"].ToString();
                        entity.CLICKREFRENCE = row["CLICKREFRENCE"].ToString();
                        entity.RAWREFRENCE = row["RAWREFRENCE"].ToString();

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
                strSql.Append(" FROM CLICKDETAIL");
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
                string sql = "select count(*) from CLICKDETAIL ";
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
                PagerSql.Append("FROM (SELECT * FROM CLICKDETAIL ");
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

