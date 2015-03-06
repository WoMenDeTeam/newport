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
    public partial class RolesEntity
    {
        private SqlHelper _sqlHelper;

        #region const fields
        public const string DBName = "SentimentConnStr";
        public const string TableName = "ROLES";
        public const string PrimaryKey = "PK_WIDGET";
        #endregion

        #region columns
        public struct Columns
        {
            public const string ROLEID = "ROLEID";
            public const string ROLENAME = "ROLENAME";
            public const string DESCRIPTION = "DESCRIPTION";
        }
        #endregion

        #region constructors
        public RolesEntity()
        {
            _sqlHelper = new SqlHelper(DBName);
        }

        public RolesEntity(int id, string name, string description)
        {
            this.ROLEID = id;

            this.ROLENAME = name;

            this.DESCRIPTION = description;
        }
        #endregion

        #region Properties

        public int? ROLEID
        {
            get;
            set;
        }

        public string ROLENAME
        {
            get;
            set;
        }

        public string DESCRIPTION
        {
            get;
            set;
        }

        #endregion

        public class RolesDAO : SqlDAO<RolesEntity>
        {
            private SqlHelper _sqlHelper;
            public const string DBName = "SentimentConnStr";

            public RolesDAO()
            {
                _sqlHelper = new SqlHelper(DBName);
            }

            public override void Add(RolesEntity entity)
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into ROLES(");
                strSql.Append("ROLENAME,DESCRIPTION)");
                strSql.Append(" values (");
                strSql.Append("@ROLENAME,@DESCRIPTION)");
                SqlParameter[] parameters = {
					new SqlParameter("@ROLENAME",SqlDbType.NVarChar),
					new SqlParameter("@DESCRIPTION",SqlDbType.NVarChar)
					};
                parameters[0].Value = entity.ROLENAME;
                parameters[1].Value = entity.DESCRIPTION;

                _sqlHelper.ExecuteSql(strSql.ToString(), parameters);
                entity.ROLEID = ReturnNewRowId();
            }

            private int ReturnNewRowId()
            {
                string sql = "select ROLES_ID_SEQ.currval NEWID from dual";
                object NewId = _sqlHelper.GetSingle(sql, null);
                return Convert.ToInt32(NewId);
            }

            public override void Update(RolesEntity entity)
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("update ROLES set ");
                strSql.Append("ROLENAME=@ROLENAME,");
                strSql.Append("DESCRIPTION=@DESCRIPTION");

                strSql.Append(" where ROLEID=@ROLEID");
                SqlParameter[] parameters = {
					new SqlParameter("@ROLENAME",SqlDbType.NVarChar),
					new SqlParameter("@DESCRIPTION",SqlDbType.NVarChar),
   					new SqlParameter("@ROLEID",SqlDbType.Int)
					};
                parameters[0].Value = entity.ROLENAME;
                parameters[1].Value = entity.DESCRIPTION;
                parameters[2].Value = entity.ROLEID;

                _sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            }

            public bool UpdateSet(int ROLEID, string ColumnName, string Value)
            {
                try
                {
                    StringBuilder StrSql = new StringBuilder();
                    StrSql.Append("update ROLES set ");
                    StrSql.Append(ColumnName + "='" + Value + "'");
                    StrSql.Append(" where ROLEID=" + ROLEID);
                    _sqlHelper.ExecuteSql(StrSql.ToString(), null);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            public bool Delete(int ROLEID)
            {
                string strSql = "delete from ROLES where ROLEID=" + ROLEID;
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

            public bool Delete(string ROLEID)
            {
                string strSql = "delete from ROLES where ROLEID in (" + ROLEID + ")";
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

            public override void Delete(RolesEntity entity)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from ROLES ");
                strSql.Append(" where ROLEID=@primaryKeyId");
                SqlParameter[] parameters = {
						new SqlParameter("@primaryKeyId", SqlDbType.Int)
					};
                parameters[0].Value = entity.ROLEID;
                _sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            }

            public List<RolesEntity> Find(string strWhere)
            {
                return Find(strWhere, null);
            }

            public override RolesEntity FindById(long primaryKeyId)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select * from ROLES ");
                strSql.Append(" where ROLEID=@primaryKeyId");
                SqlParameter[] parameters = {
						new SqlParameter("@primaryKeyId", SqlDbType.Int)};
                parameters[0].Value = primaryKeyId;
                DataSet ds = _sqlHelper.ExecuteDateSet(strSql.ToString(), parameters);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    RolesEntity entity = new RolesEntity();
                    if (!Convert.IsDBNull(row["ROLEID"]))
                    {
                        entity.ROLEID = Convert.ToInt32(row["ROLEID"]);
                    }
                    entity.ROLENAME = row["ROLENAME"].ToString();
                    entity.DESCRIPTION = row["DESCRIPTION"].ToString();
                    return entity;
                }
                else
                {
                    return null;
                }
            }

            public override List<RolesEntity> Find(string strWhere, SqlParameter[] parameters)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select *");
                strSql.Append(" FROM ROLES ");
                if (strWhere.Trim() != "")
                {
                    strSql.Append(" where " + strWhere);
                }

                DataSet ds = _sqlHelper.ExecuteDateSet(strSql.ToString(), parameters);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<RolesEntity> list = new List<RolesEntity>();
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        RolesEntity entity = new RolesEntity();
                        if (!Convert.IsDBNull(row["ROLEID"]))
                        {
                            entity.ROLEID = Convert.ToInt32(row["ROLEID"]);
                        }
                        entity.ROLENAME = row["ROLENAME"].ToString();
                        entity.DESCRIPTION = row["DESCRIPTION"].ToString();

                        list.Add(entity);
                    }

                    return list;
                }
                else
                {
                    return null;
                }
            }

            public override DataSet GetDataSet(string strWhere, SqlParameter[] param)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select *");
                strSql.Append(" FROM ROLES");
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
                string sql = "select count(*) from ROLES ";
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
                PagerSql.Append("FROM (SELECT * FROM ROLES ");
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

                    PagerSql.Append(" ORDER BY ROLEID");//默认按主键排序

                }
                PagerSql.AppendFormat(" ) A WHERE ROWNUM <= {0})", endNumber);
                PagerSql.AppendFormat(" WHERE RN >= {0}", startNumber);

                return _sqlHelper.ExecuteDateSet(PagerSql.ToString(), param).Tables[0];
            }

            #endregion

        }
    }
}

