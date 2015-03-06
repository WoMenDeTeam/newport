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
    public partial class SENSITIVEWORDSEntity
    {
        private SqlHelper _sqlHelper;

        #region const fields
        public const string DBName = "Sentiment";
        public const string TableName = "SENSITIVEWORDS";
        public const string PrimaryKey = "PK_SENSITIVEWORDS";
        #endregion

        #region columns
        public struct Columns
        {
            public const string ID = "ID";
            public const string TERMSTR = "TERMSTR";
            public const string ARTICLECOUNT = "ARTICLECOUNT";
            public const string PARENTID = "PARENTID";
            public const string USERID = "USERID";
            public const string TAG = "TAG";
        }
        #endregion

        #region constructors
        public SENSITIVEWORDSEntity()
        {
            _sqlHelper = new SqlHelper(DBName);
        }

        public SENSITIVEWORDSEntity(int id, string termstr, long articlecount, int parentid, int userid, int tag)
        {
            this.ID = id;

            this.TERMSTR = termstr;

            this.ARTICLECOUNT = articlecount;

            this.PARENTID = parentid;

            this.USERID = userid;

            this.TAG = tag;

        }
        #endregion

        #region Properties

        public int? ID
        {
            get;
            set;
        }


        public string TERMSTR
        {
            get;
            set;
        }


        public long? ARTICLECOUNT
        {
            get;
            set;
        }


        public int? PARENTID
        {
            get;
            set;
        }


        public int? USERID
        {
            get;
            set;
        }


        public int? TAG
        {
            get;
            set;
        }

        #endregion

        public class SENSITIVEWORDSDAO : SqlDAO<SENSITIVEWORDSEntity>
        {
            private SqlHelper _sqlHelper;
            public const string DBName = "SentimentConnStr";

            public SENSITIVEWORDSDAO()
            {
                _sqlHelper = new SqlHelper(DBName);
            }

            public override void Add(SENSITIVEWORDSEntity entity)
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into SENSITIVEWORDS(");
                strSql.Append("TERMSTR,ARTICLECOUNT,PARENTID,USERID,TAG)");
                strSql.Append(" values (");
                strSql.Append("@TERMSTR,@ARTICLECOUNT,@PARENTID,@USERID,@TAG)");
                SqlParameter[] parameters = {
					new SqlParameter("@TERMSTR",SqlDbType.NVarChar),
					new SqlParameter("@ARTICLECOUNT",SqlDbType.BigInt),
					new SqlParameter("@PARENTID",SqlDbType.Int),
					new SqlParameter("@USERID",SqlDbType.Int),
					new SqlParameter("@TAG",SqlDbType.Int)
					};
                parameters[0].Value = entity.TERMSTR;
                parameters[1].Value = entity.ARTICLECOUNT;
                parameters[2].Value = entity.PARENTID;
                parameters[3].Value = entity.USERID;
                parameters[4].Value = entity.TAG;
                _sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            }


            public int AddEntity(SENSITIVEWORDSEntity entity)
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into SENSITIVEWORDS(");
                strSql.Append("TERMSTR,ARTICLECOUNT,PARENTID,USERID,TAG)");
                strSql.Append(" values (");
                strSql.Append("@TERMSTR,@ARTICLECOUNT,@PARENTID,@USERID,@TAG)");
                SqlParameter[] parameters = {
					new SqlParameter("@TERMSTR",SqlDbType.NVarChar),
					new SqlParameter("@ARTICLECOUNT",SqlDbType.BigInt),
					new SqlParameter("@PARENTID",SqlDbType.Int),
					new SqlParameter("@USERID",SqlDbType.Int),
					new SqlParameter("@TAG",SqlDbType.Int)
					};
                parameters[0].Value = entity.TERMSTR;
                parameters[1].Value = entity.ARTICLECOUNT;
                parameters[2].Value = entity.PARENTID;
                parameters[3].Value = entity.USERID;
                parameters[4].Value = entity.TAG;
                _sqlHelper.ExecuteSql(strSql.ToString(), parameters);
                return ReturnNewRowId();
            }

            private int ReturnNewRowId()
            {
                string sql = "select SENSITIVEWORDS_ID_SEQ.currval NEWID from dual";
                object NewId = _sqlHelper.GetSingle(sql, null);
                return Convert.ToInt32(NewId);
            }



            public override void Update(SENSITIVEWORDSEntity entity)
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("update SENSITIVEWORDS set ");
                strSql.Append("TERMSTR=@TERMSTR,");
                strSql.Append("ARTICLECOUNT=@ARTICLECOUNT,");
                strSql.Append("PARENTID=@PARENTID,");
                strSql.Append("USERID=@USERID,");
                strSql.Append("TAG=@TAG");

                strSql.Append(" where ID=@ID");
                SqlParameter[] parameters = {
					new SqlParameter("@TERMSTR",SqlDbType.NVarChar),
					new SqlParameter("@ARTICLECOUNT",SqlDbType.BigInt),
					new SqlParameter("@PARENTID",SqlDbType.Int),
					new SqlParameter("@USERID",SqlDbType.Int),
					new SqlParameter("@TAG",SqlDbType.Int),
					new SqlParameter("@ID",SqlDbType.Int)
				};
                parameters[0].Value = entity.TERMSTR;
                parameters[1].Value = entity.ARTICLECOUNT;
                parameters[2].Value = entity.PARENTID;
                parameters[3].Value = entity.USERID;
                parameters[4].Value = entity.TAG;
                parameters[5].Value = entity.ID;

                _sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            }

            public bool UpdateSet(int ID, string ColumnName, string Value)
            {
                try
                {
                    StringBuilder StrSql = new StringBuilder();
                    StrSql.Append("update SENSITIVEWORDS set ");
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
                string strSql = "delete from SENSITIVEWORDS where ID=" + ID;
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
                string strSql = "delete from SENSITIVEWORDS where ID in (" + ID + ")";
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

            public override void Delete(SENSITIVEWORDSEntity entity)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from SENSITIVEWORDS ");
                strSql.Append(" where ID=@primaryKeyId");
                SqlParameter[] parameters = {
						new SqlParameter("@primaryKeyId", SqlDbType.Int)
					};
                parameters[0].Value = entity.ID;
                _sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            }

            public override SENSITIVEWORDSEntity FindById(long primaryKeyId)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select * from SENSITIVEWORDS ");
                strSql.Append(" where ID=@primaryKeyId");
                SqlParameter[] parameters = {
						new SqlParameter("@primaryKeyId", SqlDbType.Int)};
                parameters[0].Value = primaryKeyId;
                DataSet ds = _sqlHelper.ExecuteDateSet(strSql.ToString(), parameters);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    SENSITIVEWORDSEntity entity = new SENSITIVEWORDSEntity();
                    if (!Convert.IsDBNull(row["ID"]))
                    {
                        entity.ID = Convert.ToInt32(row["ID"]);
                    }
                    entity.TERMSTR = row["TERMSTR"].ToString();
                    if (!Convert.IsDBNull(row["ARTICLECOUNT"]))
                    {
                        entity.ARTICLECOUNT = Convert.ToInt64(row["ARTICLECOUNT"]);
                    }
                    if (!Convert.IsDBNull(row["PARENTID"]))
                    {
                        entity.PARENTID = Convert.ToInt32(row["PARENTID"]);
                    }
                    if (!Convert.IsDBNull(row["USERID"]))
                    {
                        entity.USERID = Convert.ToInt32(row["USERID"]);
                    }
                    if (!Convert.IsDBNull(row["TAG"]))
                    {
                        entity.TAG = Convert.ToInt32(row["TAG"]);
                    }
                    return entity;
                }
                else
                {
                    return null;
                }
            }

            public List<SENSITIVEWORDSEntity> Find(string strWhere)
            {
                return Find(strWhere, null);
            }

            public override List<SENSITIVEWORDSEntity> Find(string strWhere, SqlParameter[] parameters)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select *");
                strSql.Append(" FROM SENSITIVEWORDS ");
                if (strWhere.Trim() != "")
                {
                    strSql.Append(" where " + strWhere);
                }

                DataSet ds = _sqlHelper.ExecuteDateSet(strSql.ToString(), parameters);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<SENSITIVEWORDSEntity> list = new List<SENSITIVEWORDSEntity>();
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        SENSITIVEWORDSEntity entity = new SENSITIVEWORDSEntity();
                        if (!Convert.IsDBNull(row["ID"]))
                        {
                            entity.ID = Convert.ToInt32(row["ID"]);
                        }
                        entity.TERMSTR = row["TERMSTR"].ToString();
                        if (!Convert.IsDBNull(row["ARTICLECOUNT"]))
                        {
                            entity.ARTICLECOUNT = Convert.ToInt64(row["ARTICLECOUNT"]);
                        }
                        if (!Convert.IsDBNull(row["PARENTID"]))
                        {
                            entity.PARENTID = Convert.ToInt32(row["PARENTID"]);
                        }
                        if (!Convert.IsDBNull(row["USERID"]))
                        {
                            entity.USERID = Convert.ToInt32(row["USERID"]);
                        }
                        if (!Convert.IsDBNull(row["TAG"]))
                        {
                            entity.TAG = Convert.ToInt32(row["TAG"]);
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
                strSql.Append(" FROM SENSITIVEWORDS");
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
                string sql = "select count(*) from SENSITIVEWORDS ";
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
                PagerSql.Append("FROM (SELECT * FROM SENSITIVEWORDS ");
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

