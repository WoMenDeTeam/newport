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
    public partial class WARNINGMSGEntity
    {
        private SqlHelper _sqlHelper;

        #region const fields
        public const string DBName = "Sentiment";
        public const string TableName = "WARNINGMSG";
        public const string PrimaryKey = "PK_WARNINGMSG";
        #endregion

        #region columns
        public struct Columns
        {
            public const string ID = "ID";
            public const string WEBSITEID = "WEBSITEID";
            public const string PAGEVIEW = "PAGEVIEW";
            public const string INVITATION = "INVITATION";
            public const string USERNAME = "USERNAME";
            public const string ACCEPTERS = "ACCEPTERS";
        }
        #endregion

        #region constructors
        public WARNINGMSGEntity()
        {
            _sqlHelper = new SqlHelper(DBName);
        }

        public WARNINGMSGEntity(int id, int websiteid, int pageview, int invitation, string username, string accepters)
        {
            this.ID = id;

            this.WEBSITEID = websiteid;

            this.PAGEVIEW = pageview;

            this.INVITATION = invitation;

            this.USERNAME = username;

            this.ACCEPTERS = accepters;

        }
        #endregion

        #region Properties

        public int? ID
        {
            get;
            set;
        }


        public int? WEBSITEID
        {
            get;
            set;
        }


        public int? PAGEVIEW
        {
            get;
            set;
        }


        public int? INVITATION
        {
            get;
            set;
        }


        public string USERNAME
        {
            get;
            set;
        }


        public string ACCEPTERS
        {
            get;
            set;
        }

        #endregion

        public class WARNINGMSGDAO : SqlDAO<WARNINGMSGEntity>
        {
            private SqlHelper _sqlHelper;
            public const string DBName = "SentimentConnStr";

            public WARNINGMSGDAO()
            {
                _sqlHelper = new SqlHelper(DBName);
            }

            public override void Add(WARNINGMSGEntity entity)
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into WARNINGMSG(");
                strSql.Append("WEBSITEID,PAGEVIEW,INVITATION,USERNAME,ACCEPTERS)");
                strSql.Append(" values (");
                strSql.Append("@WEBSITEID,@PAGEVIEW,@INVITATION,@USERNAME,@ACCEPTERS)");
                SqlParameter[] parameters = {
					new SqlParameter("@WEBSITEID",SqlDbType.Int),
					new SqlParameter("@PAGEVIEW",SqlDbType.Int),
					new SqlParameter("@INVITATION",SqlDbType.Int),
					new SqlParameter("@USERNAME",SqlDbType.NVarChar),
					new SqlParameter("@ACCEPTERS",SqlDbType.NText)
					};
                parameters[0].Value = entity.WEBSITEID;
                parameters[1].Value = entity.PAGEVIEW;
                parameters[2].Value = entity.INVITATION;
                parameters[3].Value = entity.USERNAME;
                parameters[4].Value = entity.ACCEPTERS;
                _sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            }

            public override void Update(WARNINGMSGEntity entity)
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("update WARNINGMSG set ");
                strSql.Append("WEBSITEID=@WEBSITEID,");
                strSql.Append("PAGEVIEW=@PAGEVIEW,");
                strSql.Append("INVITATION=@INVITATION,");
                strSql.Append("USERNAME=@USERNAME,");
                strSql.Append("ACCEPTERS=@ACCEPTERS");

                strSql.Append(" where ID=@ID");
                SqlParameter[] parameters = {
					new SqlParameter("@WEBSITEID",SqlDbType.Int),
					new SqlParameter("@PAGEVIEW",SqlDbType.Int),
					new SqlParameter("@INVITATION",SqlDbType.Int),
					new SqlParameter("@USERNAME",SqlDbType.NVarChar),
					new SqlParameter("@ACCEPTERS",SqlDbType.NText),
					new SqlParameter("@ID",SqlDbType.Int)
				};
                parameters[0].Value = entity.WEBSITEID;
                parameters[1].Value = entity.PAGEVIEW;
                parameters[2].Value = entity.INVITATION;
                parameters[3].Value = entity.USERNAME;
                parameters[4].Value = entity.ACCEPTERS;
                parameters[5].Value = entity.ID;

                _sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            }

            public bool UpdateSet(int ID, string ColumnName, string Value)
            {
                try
                {
                    StringBuilder StrSql = new StringBuilder();
                    StrSql.Append("update WARNINGMSG set ");
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
                string strSql = "delete from WARNINGMSG where ID=" + ID;
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
                string strSql = "delete from WARNINGMSG where ID in (" + ID + ")";
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

            public override void Delete(WARNINGMSGEntity entity)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from WARNINGMSG ");
                strSql.Append(" where ID=@primaryKeyId");
                SqlParameter[] parameters = {
						new SqlParameter("@primaryKeyId", SqlDbType.Int)
					};
                parameters[0].Value = entity.ID;
                _sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            }

            public override WARNINGMSGEntity FindById(long primaryKeyId)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select * from WARNINGMSG ");
                strSql.Append(" where ID=@primaryKeyId");
                SqlParameter[] parameters = {
						new SqlParameter("@primaryKeyId", SqlDbType.Int)};
                parameters[0].Value = primaryKeyId;
                DataSet ds = _sqlHelper.ExecuteDateSet(strSql.ToString(), parameters);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    WARNINGMSGEntity entity = new WARNINGMSGEntity();
                    if (!Convert.IsDBNull(row["ID"]))
                    {
                        entity.ID = Convert.ToInt32(row["ID"]);
                    }
                    if (!Convert.IsDBNull(row["WEBSITEID"]))
                    {
                        entity.WEBSITEID = Convert.ToInt32(row["WEBSITEID"]);
                    }
                    if (!Convert.IsDBNull(row["PAGEVIEW"]))
                    {
                        entity.PAGEVIEW = Convert.ToInt32(row["PAGEVIEW"]);
                    }
                    if (!Convert.IsDBNull(row["INVITATION"]))
                    {
                        entity.INVITATION = Convert.ToInt32(row["INVITATION"]);
                    }
                    entity.USERNAME = row["USERNAME"].ToString();
                    entity.ACCEPTERS = row["ACCEPTERS"].ToString();
                    return entity;
                }
                else
                {
                    return null;
                }
            }

            public List<WARNINGMSGEntity> Find(string strWhere) {
                return Find(strWhere, null);
            }

            public override List<WARNINGMSGEntity> Find(string strWhere, SqlParameter[] parameters)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select *");
                strSql.Append(" FROM WARNINGMSG ");
                if (strWhere.Trim() != "")
                {
                    strSql.Append(" where " + strWhere);
                }

                DataSet ds = _sqlHelper.ExecuteDateSet(strSql.ToString(), parameters);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<WARNINGMSGEntity> list = new List<WARNINGMSGEntity>();
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        WARNINGMSGEntity entity = new WARNINGMSGEntity();
                        if (!Convert.IsDBNull(row["ID"]))
                        {
                            entity.ID = Convert.ToInt32(row["ID"]);
                        }
                        if (!Convert.IsDBNull(row["WEBSITEID"]))
                        {
                            entity.WEBSITEID = Convert.ToInt32(row["WEBSITEID"]);
                        }
                        if (!Convert.IsDBNull(row["PAGEVIEW"]))
                        {
                            entity.PAGEVIEW = Convert.ToInt32(row["PAGEVIEW"]);
                        }
                        if (!Convert.IsDBNull(row["INVITATION"]))
                        {
                            entity.INVITATION = Convert.ToInt32(row["INVITATION"]);
                        }
                        entity.USERNAME = row["USERNAME"].ToString();
                        entity.ACCEPTERS = row["ACCEPTERS"].ToString();

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
                strSql.Append(" FROM WARNINGMSG");
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
                string sql = "select count(*) from WARNINGMSG ";
                if (!string.IsNullOrEmpty(where))
                {
                    sql += "where " + where;
                }

                object obj = _sqlHelper.GetSingle(sql, param);

                return obj == null ? 0 : Convert.ToInt32(obj);
            }

            public int GetPagerRowsCountByUserName(string userName)
            {
                string strWhere = "USERNAME=@USERNAME";
                SqlParameter[] parameters = {
						new SqlParameter("@USERNAME", SqlDbType.NVarChar)};
                parameters[0].Value = userName;
                return GetPagerRowsCount(strWhere, parameters);
            }

            public DataTable GetDataTableByUserName(string userName)
            {
                StringBuilder strsql = new StringBuilder();
                strsql.Append("SELECT * FROM WARNINGMSG A,WEBSITE B WHERE A.WEBSITEID = B.ID AND");
                strsql.Append("A.USERNAME=@USERNAME");
                SqlParameter[] parameters = {
						new SqlParameter("@USERNAME", SqlDbType.NVarChar)};
                parameters[0].Value = userName;
                DataSet ds = _sqlHelper.ExecuteDateSet(strsql.ToString(), parameters);
                if (ds != null)
                {
                    return ds.Tables[0];
                }
                else
                {
                    return null;
                }
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
                PagerSql.Append("FROM (SELECT * FROM WARNINGMSG ");
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

