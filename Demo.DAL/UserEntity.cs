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
    public partial class UsersEntity
    {
        private SqlHelper _sqlHelper;

        #region const fields
        public const string DBName = "Sentiment";
        public const string TableName = "USERS";
        public const string PrimaryKey = "PK_USERS";
        #endregion

        #region columns
        public struct Columns
        {
            public const string USERID = "USERID";
            public const string USERNAME = "USERNAME";
            public const string PASSWORD = "PASSWORD";
            public const string EMAIL = "EMAIL";
            public const string CREATEDATE = "CREATEDATE";
            public const string LASTLOGINDATE = "LASTLOGINDATE";
            public const string MOBILE = "MOBILE";
            public const string LASTLOGINIP = "LASTLOGINIP";
            public const string GENDER = "GENDER";
            public const string EMPLOYEENUMBER = "EMPLOYEENUMBER";
            public const string TELNUMBER = "TELNUMBER";
            public const string DESCRIPTION = "DESCRIPTION";
            public const string SERIALNUMBER = "SERIALNUMBER";
            public const string ROOMNUMBER = "ROOMNUMBER";
            public const string POSITION = "POSITION";
            public const string STATUS = "STATUS";
            public const string ROLEINFO = "ROLEINFO";
            public const string ORGNUMBER = "ORGNUMBER";
            public const string ORGNAME = "ORGNAME";
            public const string ACCID = "ACCID";
        }
        #endregion

        #region constructors
        public UsersEntity()
        {
            _sqlHelper = new SqlHelper(DBName);
        }

        public UsersEntity(int userid, string username, string password, string email, DateTime createdate, DateTime lastlogindate, string mobile, string lastloginip, string gender, string employeenumber, string telnumber, string description, string serialnumber, string roomnumber, string position, string status, string roleinfo, string orgnumber, string orgname, string accid)
        {
            this.USERID = userid;

            this.USERNAME = username;

            this.PASSWORD = password;

            this.EMAIL = email;

            this.CREATEDATE = createdate;

            this.LASTLOGINDATE = lastlogindate;

            this.MOBILE = mobile;

            this.LASTLOGINIP = lastloginip;

            this.GENDER = gender;

            this.EMPLOYEENUMBER = employeenumber;

            this.TELNUMBER = telnumber;

            this.DESCRIPTION = description;

            this.SERIALNUMBER = serialnumber;

            this.ROOMNUMBER = roomnumber;

            this.POSITION = position;

            this.STATUS = status;

            this.ROLEINFO = roleinfo;

            this.ORGNUMBER = orgnumber;

            this.ORGNAME = orgname;

            this.ACCID = accid;

        }
        #endregion

        #region Properties

        public int? USERID
        {
            get;
            set;
        }


        public string USERNAME
        {
            get;
            set;
        }


        public string PASSWORD
        {
            get;
            set;
        }


        public string EMAIL
        {
            get;
            set;
        }


        public DateTime? CREATEDATE
        {
            get;
            set;
        }


        public DateTime? LASTLOGINDATE
        {
            get;
            set;
        }


        public string MOBILE
        {
            get;
            set;
        }


        public string LASTLOGINIP
        {
            get;
            set;
        }


        public string GENDER
        {
            get;
            set;
        }


        public string EMPLOYEENUMBER
        {
            get;
            set;
        }


        public string TELNUMBER
        {
            get;
            set;
        }


        public string DESCRIPTION
        {
            get;
            set;
        }


        public string SERIALNUMBER
        {
            get;
            set;
        }


        public string ROOMNUMBER
        {
            get;
            set;
        }


        public string POSITION
        {
            get;
            set;
        }


        public string STATUS
        {
            get;
            set;
        }


        public string ROLEINFO
        {
            get;
            set;
        }


        public string ORGNUMBER
        {
            get;
            set;
        }


        public string ORGNAME
        {
            get;
            set;
        }

        public string ACCID
        {
            get;
            set;
        }

        #endregion

        public class UsersDAO : SqlDAO<UsersEntity>
        {
            private SqlHelper _sqlHelper;
            public const string DBName = "SentimentConnStr";

            public UsersDAO()
            {
                _sqlHelper = new SqlHelper(DBName);
            }

            public override void Add(UsersEntity entity)
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into USERS(");
                strSql.Append("USERNAME,PASSWORD,EMAIL,CREATEDATE,LASTLOGINDATE,MOBILE,LASTLOGINIP,GENDER,EMPLOYEENUMBER,TELNUMBER,DESCRIPTION,SERIALNUMBER,ROOMNUMBER,POSITION,STATUS,ROLEINFO,ORGNUMBER,ORGNAME,ACCID)");
                strSql.Append(" values (");
                strSql.Append("@USERNAME,@PASSWORD,@EMAIL,@CREATEDATE,@LASTLOGINDATE,@MOBILE,@LASTLOGINIP,@GENDER,@EMPLOYEENUMBER,@TELNUMBER,@DESCRIPTION,@SERIALNUMBER,@ROOMNUMBER,@POSITION,@STATUS,@ROLEINFO,@ORGNUMBER,@ORGNAME,@ACCID)");
                SqlParameter[] parameters = {
					new SqlParameter("@USERNAME",SqlDbType.NVarChar),
					new SqlParameter("@PASSWORD",SqlDbType.NVarChar),
					new SqlParameter("@EMAIL",SqlDbType.NVarChar),
					new SqlParameter("@CREATEDATE",SqlDbType.DateTime),
					new SqlParameter("@LASTLOGINDATE",SqlDbType.DateTime),
					new SqlParameter("@MOBILE",SqlDbType.NVarChar),
					new SqlParameter("@LASTLOGINIP",SqlDbType.NVarChar),
					new SqlParameter("@GENDER",SqlDbType.NVarChar),
					new SqlParameter("@EMPLOYEENUMBER",SqlDbType.NVarChar),
					new SqlParameter("@TELNUMBER",SqlDbType.NVarChar),
					new SqlParameter("@DESCRIPTION",SqlDbType.NVarChar),
					new SqlParameter("@SERIALNUMBER",SqlDbType.NVarChar),
					new SqlParameter("@ROOMNUMBER",SqlDbType.NVarChar),
					new SqlParameter("@POSITION",SqlDbType.NVarChar),
					new SqlParameter("@STATUS",SqlDbType.NVarChar),
					new SqlParameter("@ROLEINFO",SqlDbType.NVarChar),
					new SqlParameter("@ORGNUMBER",SqlDbType.NVarChar),
					new SqlParameter("@ORGNAME",SqlDbType.NVarChar),
                    new SqlParameter("@ACCID",SqlDbType.NVarChar)
					};
                parameters[0].Value = entity.USERNAME;
                parameters[1].Value = entity.PASSWORD;
                parameters[2].Value = entity.EMAIL;
                parameters[3].Value = entity.CREATEDATE;
                parameters[4].Value = entity.LASTLOGINDATE;
                parameters[5].Value = entity.MOBILE;
                parameters[6].Value = entity.LASTLOGINIP;
                parameters[7].Value = entity.GENDER;
                parameters[8].Value = entity.EMPLOYEENUMBER;
                parameters[9].Value = entity.TELNUMBER;
                parameters[10].Value = entity.DESCRIPTION;
                parameters[11].Value = entity.SERIALNUMBER;
                parameters[12].Value = entity.ROOMNUMBER;
                parameters[13].Value = entity.POSITION;
                parameters[14].Value = entity.STATUS;
                parameters[15].Value = entity.ROLEINFO;
                parameters[16].Value = entity.ORGNUMBER;
                parameters[17].Value = entity.ORGNAME;
                parameters[18].Value = entity.ACCID;
                _sqlHelper.ExecuteSql(strSql.ToString(), parameters);
                entity.USERID = ReturnNewRowId();
            }

            private int ReturnNewRowId()
            {
                string sql = "select USERS_ID_SEQ.currval NEWID from dual";
                object NewId = _sqlHelper.GetSingle(sql, null);
                return Convert.ToInt32(NewId);
            }

            public override void Update(UsersEntity entity)
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("update USERS set ");
                strSql.Append("USERNAME=@USERNAME,");
                strSql.Append("PASSWORD=@PASSWORD,");
                strSql.Append("EMAIL=@EMAIL,");
                strSql.Append("CREATEDATE=@CREATEDATE,");
                strSql.Append("LASTLOGINDATE=@LASTLOGINDATE,");
                strSql.Append("MOBILE=@MOBILE,");
                strSql.Append("LASTLOGINIP=@LASTLOGINIP,");
                strSql.Append("GENDER=@GENDER,");
                strSql.Append("EMPLOYEENUMBER=@EMPLOYEENUMBER,");
                strSql.Append("TELNUMBER=@TELNUMBER,");
                strSql.Append("DESCRIPTION=@DESCRIPTION,");
                strSql.Append("SERIALNUMBER=@SERIALNUMBER,");
                strSql.Append("ROOMNUMBER=@ROOMNUMBER,");
                strSql.Append("POSITION=@POSITION,");
                strSql.Append("STATUS=@STATUS,");
                strSql.Append("ROLEINFO=@ROLEINFO,");
                strSql.Append("ORGNUMBER=@ORGNUMBER,");
                strSql.Append("ORGNAME=@ORGNAME,");
                strSql.Append("ACCID=@ACCID");
                strSql.Append(" where USERID=@USERID");
                SqlParameter[] parameters = {
					new SqlParameter("@USERNAME",SqlDbType.NVarChar),
					new SqlParameter("@PASSWORD",SqlDbType.NVarChar),
					new SqlParameter("@EMAIL",SqlDbType.NVarChar),
					new SqlParameter("@CREATEDATE",SqlDbType.DateTime),
					new SqlParameter("@LASTLOGINDATE",SqlDbType.DateTime),
					new SqlParameter("@MOBILE",SqlDbType.NVarChar),
					new SqlParameter("@LASTLOGINIP",SqlDbType.NVarChar),
					new SqlParameter("@GENDER",SqlDbType.NVarChar),
					new SqlParameter("@EMPLOYEENUMBER",SqlDbType.NVarChar),
					new SqlParameter("@TELNUMBER",SqlDbType.NVarChar),
					new SqlParameter("@DESCRIPTION",SqlDbType.NVarChar),
					new SqlParameter("@SERIALNUMBER",SqlDbType.NVarChar),
					new SqlParameter("@ROOMNUMBER",SqlDbType.NVarChar),
					new SqlParameter("@POSITION",SqlDbType.NVarChar),
					new SqlParameter("@STATUS",SqlDbType.NVarChar),
					new SqlParameter("@ROLEINFO",SqlDbType.NVarChar),
					new SqlParameter("@ORGNUMBER",SqlDbType.NVarChar),
					new SqlParameter("@ORGNAME",SqlDbType.NVarChar),
                    new SqlParameter("@ACCID",SqlDbType.NVarChar),
					new SqlParameter("@USERID",SqlDbType.Int)
				};
                parameters[0].Value = entity.USERNAME;
                parameters[1].Value = entity.PASSWORD;
                parameters[2].Value = entity.EMAIL;
                parameters[3].Value = entity.CREATEDATE;
                parameters[4].Value = entity.LASTLOGINDATE;
                parameters[5].Value = entity.MOBILE;
                parameters[6].Value = entity.LASTLOGINIP;
                parameters[7].Value = entity.GENDER;
                parameters[8].Value = entity.EMPLOYEENUMBER;
                parameters[9].Value = entity.TELNUMBER;
                parameters[10].Value = entity.DESCRIPTION;
                parameters[11].Value = entity.SERIALNUMBER;
                parameters[12].Value = entity.ROOMNUMBER;
                parameters[13].Value = entity.POSITION;
                parameters[14].Value = entity.STATUS;
                parameters[15].Value = entity.ROLEINFO;
                parameters[16].Value = entity.ORGNUMBER;
                parameters[17].Value = entity.ORGNAME;
                parameters[18].Value = entity.ACCID;
                parameters[19].Value = entity.USERID;

                _sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            }

            public bool UpdateSet(int USERID, string ColumnName, string Value)
            {
                try
                {
                    StringBuilder StrSql = new StringBuilder();
                    StrSql.Append("update USERS set ");
                    StrSql.Append(ColumnName + "='" + Value + "'");
                    StrSql.Append(" where USERID=" + USERID);
                    _sqlHelper.ExecuteSql(StrSql.ToString(), null);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            public List<UsersEntity> GetUserByUserName(string userName, string passWord)
            {
                string strWhere = " USERNAME=@USERNAME AND PASSWORD=@PASSWORD";
                //string strWhere = " USERNAME='" + userName + "' AND PASSWORD='" + passWord + "'";
                SqlParameter[] parameters = {
                        new SqlParameter("@USERNAME", SqlDbType.NVarChar),
                        new SqlParameter("@PASSWORD", SqlDbType.Char)
                                            };
                parameters[0].Value = userName;
                parameters[1].Value = passWord;
                return Find(strWhere, parameters);
            }

            public List<UsersEntity> GetUser(string userName, string passWord)
            {
                string strWhere = " ACCID=@USERNAME AND PASSWORD=@PASSWORD";
                //string strWhere = " USERNAME='" + userName + "' AND PASSWORD='" + passWord + "'";
                SqlParameter[] parameters = {
                        new SqlParameter("@USERNAME", SqlDbType.NVarChar),
                        new SqlParameter("@PASSWORD", SqlDbType.Char)
                                            };
                parameters[0].Value = userName;
                parameters[1].Value = passWord;
                return Find(strWhere, parameters);
            }

            public List<UsersEntity> GetUser(string userName)
            {
                string strWhere = " USERNAME=@USERNAME";
                //string strWhere = " USERNAME='" + userName + "' AND PASSWORD='" + passWord + "'";
                SqlParameter[] parameters = {
                        new SqlParameter("@USERNAME", SqlDbType.NVarChar)                        
                                            };
                parameters[0].Value = userName;
                return Find(strWhere, parameters);
            }

            public bool Delete(int USERID)
            {
                string strSql = "delete from USERS where USERID=" + USERID;
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

            public bool Delete(string USERID)
            {
                string strSql = "delete from USERS where USERID in (" + USERID + ")";
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

            public override void Delete(UsersEntity entity)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from USERS ");
                strSql.Append(" where USERID=@primaryKeyId");
                SqlParameter[] parameters = {
						new SqlParameter("@primaryKeyId", SqlDbType.Int)
					};
                parameters[0].Value = entity.USERID;
                _sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            }

            public override UsersEntity FindById(long primaryKeyId)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select * from USERS ");
                strSql.Append(" where USERID=@primaryKeyId");
                SqlParameter[] parameters = {
						new SqlParameter("@primaryKeyId", SqlDbType.Int)};
                parameters[0].Value = primaryKeyId;
                DataSet ds = _sqlHelper.ExecuteDateSet(strSql.ToString(), parameters);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    UsersEntity entity = new UsersEntity();
                    if (!Convert.IsDBNull(row["USERID"]))
                    {
                        entity.USERID = Convert.ToInt32(row["USERID"]);
                    }
                    entity.USERNAME = row["USERNAME"].ToString();
                    entity.PASSWORD = row["PASSWORD"].ToString();
                    entity.EMAIL = row["EMAIL"].ToString();
                    if (!Convert.IsDBNull(row["CREATEDATE"]))
                    {
                        entity.CREATEDATE = Convert.ToDateTime(row["CREATEDATE"]);
                    }
                    if (!Convert.IsDBNull(row["LASTLOGINDATE"]))
                    {
                        entity.LASTLOGINDATE = Convert.ToDateTime(row["LASTLOGINDATE"]);
                    }
                    entity.MOBILE = row["MOBILE"].ToString();
                    entity.LASTLOGINIP = row["LASTLOGINIP"].ToString();
                    entity.GENDER = row["GENDER"].ToString();
                    entity.EMPLOYEENUMBER = row["EMPLOYEENUMBER"].ToString();
                    entity.TELNUMBER = row["TELNUMBER"].ToString();
                    entity.DESCRIPTION = row["DESCRIPTION"].ToString();
                    entity.SERIALNUMBER = row["SERIALNUMBER"].ToString();
                    entity.ROOMNUMBER = row["ROOMNUMBER"].ToString();
                    entity.POSITION = row["POSITION"].ToString();
                    entity.STATUS = row["STATUS"].ToString();
                    entity.ROLEINFO = row["ROLEINFO"].ToString();
                    entity.ORGNUMBER = row["ORGNUMBER"].ToString();
                    entity.ORGNAME = row["ORGNAME"].ToString();
                    entity.ACCID = row["ACCID"].ToString();
                    return entity;
                }
                else
                {
                    return null;
                }
            }

            public UsersEntity FindEntityByAccID(string AccId)
            {
                string strWhere = " ACCID=@ACCID";
                SqlParameter[] parameters = {
						new SqlParameter("@ACCID", SqlDbType.NVarChar)};
                parameters[0].Value = AccId;
                IList<UsersEntity> list = Find(strWhere, parameters);
                if (list != null && list.Count > 0)
                {
                    return list[0];
                }
                else
                {
                    return null;
                }
            }

            public override List<UsersEntity> Find(string strWhere, SqlParameter[] parameters)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select *");
                strSql.Append(" FROM USERS ");
                if (strWhere.Trim() != "")
                {
                    strSql.Append(" where " + strWhere);
                }

                DataSet ds = _sqlHelper.ExecuteDateSet(strSql.ToString(), parameters);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<UsersEntity> list = new List<UsersEntity>();
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        UsersEntity entity = new UsersEntity();
                        if (!Convert.IsDBNull(row["USERID"]))
                        {
                            entity.USERID = Convert.ToInt32(row["USERID"]);
                        }
                        entity.USERNAME = row["USERNAME"].ToString();
                        entity.PASSWORD = row["PASSWORD"].ToString();
                        entity.EMAIL = row["EMAIL"].ToString();
                        if (!Convert.IsDBNull(row["CREATEDATE"]))
                        {
                            entity.CREATEDATE = Convert.ToDateTime(row["CREATEDATE"]);
                        }
                        if (!Convert.IsDBNull(row["LASTLOGINDATE"]))
                        {
                            entity.LASTLOGINDATE = Convert.ToDateTime(row["LASTLOGINDATE"]);
                        }
                        entity.MOBILE = row["MOBILE"].ToString();
                        entity.LASTLOGINIP = row["LASTLOGINIP"].ToString();
                        entity.GENDER = row["GENDER"].ToString();
                        entity.EMPLOYEENUMBER = row["EMPLOYEENUMBER"].ToString();
                        entity.TELNUMBER = row["TELNUMBER"].ToString();
                        entity.DESCRIPTION = row["DESCRIPTION"].ToString();
                        entity.SERIALNUMBER = row["SERIALNUMBER"].ToString();
                        entity.ROOMNUMBER = row["ROOMNUMBER"].ToString();
                        entity.POSITION = row["POSITION"].ToString();
                        entity.STATUS = row["STATUS"].ToString();
                        entity.ROLEINFO = row["ROLEINFO"].ToString();
                        entity.ORGNUMBER = row["ORGNUMBER"].ToString();
                        entity.ORGNAME = row["ORGNAME"].ToString();
                        entity.ACCID = row["ACCID"].ToString();
                        list.Add(entity);
                    }

                    return list;
                }
                else
                {
                    return null;
                }
            }
            public IList<UsersEntity> GetUsersList(string idlist, bool tag)
            {
                string strWhere = string.Empty;
                if (tag)
                {
                    if (!string.IsNullOrEmpty(idlist))
                    {
                        strWhere = "USERID IN (" + idlist + ")";
                    }
                    else
                    {
                        strWhere = "USERID < 0";
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(idlist))
                    {
                        strWhere = "USERID NOT IN (" + idlist + ")";
                    }
                    else
                    {
                        strWhere = "";
                    }
                }
                return Find(strWhere, null);
            }

            public List<RolesEntity> GetRolesOfUser(int userID)
            {
                List<RolesEntity> roles = new List<RolesEntity>();
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select r.* from USERS u,USERSINROLES ur,ROLES r ");
                strSql.Append(" where u.USERID=ur.USERID AND ur.ROLEID = r.ROLEID AND u.USERID=@USERID");
                SqlParameter[] parameters = {
                                                   new SqlParameter("@USERID",SqlDbType.Int)
                                                                               };
                parameters[0].Value = userID;
                try
                {
                    SqlDataReader odr = _sqlHelper.ExecuteReader(strSql.ToString(), parameters);
                    if (null != odr)
                    {
                        while (odr.Read())
                        {
                            RolesEntity item = ConvertReaderToEntity(odr);
                            roles.Add(item);
                        }
                    }
                    odr.Close();
                    odr.Dispose();
                }
                catch (Exception ex)
                {

                }

                return roles;
            }

            private static RolesEntity ConvertReaderToEntity(SqlDataReader odr)
            {
                RolesEntity item = new RolesEntity();
                if (!Convert.IsDBNull(odr["ROLEID"]))
                {
                    item.ROLEID = Convert.ToInt32(odr["ROLEID"]);
                }
                item.ROLENAME = odr["ROLENAME"].ToString();
                item.ROLENAME = odr["DESCRIPTION"].ToString();
                return item;
            }

            public DataSet GetDataSet(string strWhere, SqlParameter[] param)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select *");
                strSql.Append(" FROM USERS");
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
                string sql = "select count(*) from USERS ";
                if (!string.IsNullOrEmpty(where))
                {
                    sql += "where " + where;
                }

                object obj = _sqlHelper.GetSingle(sql, param);

                return obj == null ? 0 : Convert.ToInt32(obj);
            }

            public DataTable GetUserInnerRoleDataTable(string strSql)
            {
                DataSet ds = _sqlHelper.ExecuteDateSet(strSql, null);
                DataTable dt = new DataTable();
                if (ds != null)
                {
                    dt = ds.Tables[0];
                }
                return dt;
            }

            /// <summary>
            /// 查询分页信息，返回当前页码的记录集
            /// </summary>
            /// <param name="where">查询条件，可为empty</param>
            /// <param name="orderBy">排序条件，可为empty</param>
            /// <param name="pageSize">每页显示记录数</param>
            /// <param name="pageNumber">当前页码</param>
            /// <returns>datatable</returns>
            /// 

            public DataTable GetPager(string where, string orderBy, int pageSize, int pageNumber)
            {
                return GetPager(where, null, orderBy, pageSize, pageNumber);
            }

            public DataTable GetPager(string where, SqlParameter[] param, string orderBy, int pageSize, int pageNumber)
            {
                int startNumber = pageSize * (pageNumber - 1) + 1;
                int endNumber = pageSize * pageNumber;

                StringBuilder PagerSql = new StringBuilder();
                PagerSql.Append("SELECT * FROM (");
                PagerSql.Append(" SELECT A.*, ROWNUM RN ");
                PagerSql.Append("FROM (SELECT * FROM USERS ");
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

                    PagerSql.Append(" ORDER BY USERID");//默认按主键排序

                }
                PagerSql.AppendFormat(" ) A WHERE ROWNUM <= {0})", endNumber);
                PagerSql.AppendFormat(" WHERE RN >= {0}", startNumber);

                return _sqlHelper.ExecuteDateSet(PagerSql.ToString(), param).Tables[0];
            }

            #endregion

        }
    }
}

