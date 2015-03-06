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
    public partial class UsersInRolesEntity
    {
        private SqlHelper _sqlHelper; 

        #region const fields
        public const string DBName = "SentimentConnStr";
        public const string TableName = "USERSINROLES";
        public const string PrimaryKey = "PK_USERSINROLES";
        #endregion

        #region columns
        public struct Columns
        {
            public const string USERID = "USERID";
            public const string ID = "ID";
            public const string ROLEID = "ROLEID";
        }
        #endregion

        #region constructors
        public UsersInRolesEntity()
        {
            _sqlHelper = new SqlHelper(DBName);
        }

        public UsersInRolesEntity(int userID, int roleID, int id)
        {
            this.USERID = userID;
            this.ID = id;
            this.ROLEID = roleID;

        }
        #endregion

        #region Properties
        public int? ID
        {
            get;
            set;
        }

        public int? USERID
        {
            get;
            set;
        }

        public int? ROLEID
        {
            get;
            set;
        }

        #endregion

        public class UsersInRolesDAO : SqlDAO<UsersInRolesEntity>
        {
            private SqlHelper _sqlHelper;
            public const string DBName = "SentimentConnStr";

            public UsersInRolesDAO()
            {
                _sqlHelper = new SqlHelper(DBName);
            }

            public override void Add(UsersInRolesEntity entity)
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into USERSINROLES(");
                strSql.Append("USERID,ROLEID)");
                strSql.Append(" values (");
                strSql.Append("@USERID,@ROLEID)");
                SqlParameter[] parameters = {
					new SqlParameter("@USERID",SqlDbType.NVarChar),
					new SqlParameter("@ROLEID",SqlDbType.NVarChar)
					};
                parameters[0].Value = entity.USERID;
                parameters[1].Value = entity.ROLEID;

                _sqlHelper.ExecuteSql(strSql.ToString(), parameters);
                entity.ID = ReturnNewRowId();
            }

            private int ReturnNewRowId()
            {
                string sql = "select USERSINROLES_ID_SEQ.currval NEWID from dual";
                object NewId = _sqlHelper.GetSingle(sql, null);
                return Convert.ToInt32(NewId);
            }

            //public override void Update(UsersInRolesEntity entity)
            //{

            //    StringBuilder strSql = new StringBuilder();
            //    strSql.Append("update USERSINROLES set ");
            //    strSql.Append("USERID=@USERID,");
            //    strSql.Append("ROLEID=@ROLEID");

            //    strSql.Append(" where ID=@ID");
            //    SqlParameter[] parameters = {
            //        new SqlParameter("@USERID",SqlDbType.Int),
            //        new SqlParameter("@ROLEID",SqlDbType.NVarChar)
            //        };
            //    parameters[0].Value = entity.USERID;
            //    parameters[1].Value = entity.ROLEID;

            //    _sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            //}

            //public bool UpdateSet(int ID, string ColumnName, string Value)
            //{
            //    try
            //    {
            //        StringBuilder StrSql = new StringBuilder();
            //        StrSql.Append("update USERSINROLES set ");
            //        StrSql.Append(ColumnName + "='" + Value + "'");
            //        StrSql.Append(" where ID=" + ID);
            //        _sqlHelper.ExecuteSql(StrSql.ToString(), null);
            //        return true;
            //    }
            //    catch
            //    {
            //        return false;
            //    }
            //}

            public bool DeletebyUserID(int userID)
            {
                try
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("delete from USERSINROLES where");                    
                    strSql.Append(" USERID=@USERID");
                    SqlParameter[] parameters = {					    
					    new SqlParameter("@USERID",SqlDbType.Int)
					    };
                    parameters[0].Value = userID;                    
                    _sqlHelper.ExecuteSql(strSql.ToString(), parameters);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            public bool DeletebyRoleID(int roleID)
            {
                try
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("delete from USERSINROLES where");
                    strSql.Append(" ROLEID=@ROLEID");                    
                    SqlParameter[] parameters = {
					    new SqlParameter("@ROLEID",SqlDbType.Int)					   
					    };
                    parameters[0].Value = roleID;                    
                    _sqlHelper.ExecuteSql(strSql.ToString(), parameters);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            public bool DeletebyRoleIDAndUserId(int userID, int roleID)
            {
                try
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("delete from USERSINROLES where");
                    strSql.Append(" ROLEID=@ROLEID AND ");
                    strSql.Append(" USERID=@USERID");                
                    SqlParameter[] parameters = {
					    new SqlParameter("@ROLEID",SqlDbType.Int),
					    new SqlParameter("@USERID",SqlDbType.Int)
					    };
                    parameters[0].Value = roleID;
                    parameters[1].Value = userID;
                    _sqlHelper.ExecuteSql(strSql.ToString(), parameters);                     
                    return true;
                }
                catch
                {
                    return false;
                }
            }


            public override void Delete(UsersInRolesEntity entity)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from USERSINROLES ");
                strSql.Append(" where ID=@primaryKeyId");
                SqlParameter[] parameters = {
                        new SqlParameter("@primaryKeyId", SqlDbType.Int)
                    };
                parameters[0].Value = entity.USERID;
                _sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            }

            public override UsersInRolesEntity FindById(long userID)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select * from USERSINROLES ");
                strSql.Append(" where USERID=@userID AND ROLEID=@roleID");
                SqlParameter[] parameters = {
						new SqlParameter("@userID", SqlDbType.Int),
						new SqlParameter("@roleID", SqlDbType.Int)};
                parameters[0].Value = userID;
                //parameters[0].Value = roleID;
                DataSet ds = _sqlHelper.ExecuteDateSet(strSql.ToString(), parameters);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    UsersInRolesEntity entity = new UsersInRolesEntity();
                    if (!Convert.IsDBNull(row["USERID"]))
                    {
                        entity.USERID = Convert.ToInt32(row["USERID"]);
                    }
                    if (!Convert.IsDBNull(row["ID"]))
                    {
                        entity.USERID = Convert.ToInt32(row["ID"]);
                    }
                    if (!Convert.IsDBNull(row["ROLEID"]))
                    {
                        entity.USERID = Convert.ToInt32(row["ROLEID"]);
                    }
                    return entity;
                }
                else
                {
                    return null;
                }
            }
            public List<UsersInRolesEntity> Find(string strWhere)
            {
                return Find(strWhere, null);
            }
            public override List<UsersInRolesEntity> Find(string strWhere, SqlParameter[] parameters)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select *");
                strSql.Append(" FROM USERSINROLES ");
                if (strWhere.Trim() != "")
                {
                    strSql.Append(" where " + strWhere);
                }

                DataSet ds = _sqlHelper.ExecuteDateSet(strSql.ToString(), parameters);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<UsersInRolesEntity> list = new List<UsersInRolesEntity>();
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        UsersInRolesEntity entity = new UsersInRolesEntity();
                        if (!Convert.IsDBNull(row["ID"]))
                        {
                            entity.ID = Convert.ToInt32(row["ID"]);
                        }
                        if (!Convert.IsDBNull(row["ROLEID"]))
                        {
                            entity.ROLEID = Convert.ToInt32(row["ROLEID"]);
                        }
                        if (!Convert.IsDBNull(row["USERID"]))
                        {
                            entity.USERID = Convert.ToInt32(row["USERID"]);
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

            public DataTable GetTreeDT() {
                string strsql = "SELECT * FROM USERSINROLES A,USERS B ,ROLES C where A.USERID = B.USERID AND A.ROLEID = C.ROLEID";
                DataSet ds = _sqlHelper.ExecuteDateSet(strsql, null);
                if (ds != null)
                {
                    return ds.Tables[0];
                }
                else {
                    return null;
                }
            }

            public override DataSet GetDataSet(string strWhere, SqlParameter[] param)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select *");
                strSql.Append(" FROM USERSINROLES");
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
                string sql = "select count(*) from USERSINROLES ";
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
                PagerSql.Append("FROM (SELECT * FROM USERSINROLES ");
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

