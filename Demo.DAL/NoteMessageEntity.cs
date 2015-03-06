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
    public partial class NoteMessageEntity
    {
        private SqlHelper sqlHelper;

        #region const fields
        public const string DBName = "Test";
        public const string TableName = "NoteMessage";
        public const string PrimaryKey = "PK_NoteMessage";
        #endregion

        #region columns
        public struct Columns
        {
            public const string ID = "ID";
            public const string InfoTitle = "InfoTitle";
            public const string InfoUrl = "InfoUrl";
            public const string InfoDate = "InfoDate";
            public const string AddDate = "AddDate";
            public const string AddUserId = "AddUserId";
            public const string AddUserName = "AddUserName";
            public const string AccepterId = "AccepterId";
            public const string Accepter = "Accepter";
            public const string Status = "Status";
            public const string Message = "Message";
        }
        #endregion

        #region constructors
        public NoteMessageEntity()
        {
            sqlHelper = new SqlHelper(DBName);
        }

        public NoteMessageEntity(long id, string infotitle, string infourl, DateTime infodate, DateTime adddate, int adduserid, string addusername, int accepterid, string accepter, int status, string message)
        {
            this.ID = id;

            this.InfoTitle = infotitle;

            this.InfoUrl = infourl;

            this.InfoDate = infodate;

            this.AddDate = adddate;

            this.AddUserId = adduserid;

            this.AddUserName = addusername;

            this.AccepterId = accepterid;

            this.Accepter = accepter;

            this.Status = status;

            this.Message = message;

        }
        #endregion

        #region Properties

        public long? ID
        {
            get;
            set;
        }


        public string InfoTitle
        {
            get;
            set;
        }


        public string InfoUrl
        {
            get;
            set;
        }


        public DateTime? InfoDate
        {
            get;
            set;
        }


        public DateTime? AddDate
        {
            get;
            set;
        }


        public int? AddUserId
        {
            get;
            set;
        }


        public string AddUserName
        {
            get;
            set;
        }


        public int? AccepterId
        {
            get;
            set;
        }


        public string Accepter
        {
            get;
            set;
        }


        public int? Status
        {
            get;
            set;
        }


        public string Message
        {
            get;
            set;
        }

        #endregion

        public class NoteMessageDAO : SqlDAO<NoteMessageEntity>
        {
            private SqlHelper sqlHelper;
            public const string DBName = "SqlSentimentConnStr";

            public NoteMessageDAO()
            {
                sqlHelper = new SqlHelper(DBName);
            }

            public override void Add(NoteMessageEntity entity)
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into NoteMessage(");
                strSql.Append("InfoTitle,InfoUrl,InfoDate,AddDate,AddUserId,AddUserName,AccepterId,Accepter,Status,Message)");
                strSql.Append(" values (");
                strSql.Append("@InfoTitle,@InfoUrl,@InfoDate,@AddDate,@AddUserId,@AddUserName,@AccepterId,@Accepter,@Status,@Message)");
                SqlParameter[] parameters = {
					new SqlParameter("@InfoTitle",SqlDbType.NVarChar),
					new SqlParameter("@InfoUrl",SqlDbType.NVarChar),
					new SqlParameter("@InfoDate",SqlDbType.DateTime),
					new SqlParameter("@AddDate",SqlDbType.DateTime),
					new SqlParameter("@AddUserId",SqlDbType.Int),
					new SqlParameter("@AddUserName",SqlDbType.NVarChar),
					new SqlParameter("@AccepterId",SqlDbType.Int),
					new SqlParameter("@Accepter",SqlDbType.NVarChar),
					new SqlParameter("@Status",SqlDbType.Int),
					new SqlParameter("@Message",SqlDbType.NText)
					};
                parameters[0].Value = entity.InfoTitle;
                parameters[1].Value = entity.InfoUrl;
                parameters[2].Value = entity.InfoDate;
                parameters[3].Value = entity.AddDate;
                parameters[4].Value = entity.AddUserId;
                parameters[5].Value = entity.AddUserName;
                parameters[6].Value = entity.AccepterId;
                parameters[7].Value = entity.Accepter;
                parameters[8].Value = entity.Status;
                parameters[9].Value = entity.Message;

                sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            }

            public void UpdateSetStatus(int value, long id) {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update NoteMessage set ");
                strSql.Append("Status=@Status");               

                strSql.Append(" where ID=@ID");
                SqlParameter[] parameters = {
					new SqlParameter("@ID",SqlDbType.BigInt),					
					new SqlParameter("@Status",SqlDbType.Int)
					};
                parameters[0].Value = id;
                parameters[1].Value = value;               

                sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            }

            public override void Update(NoteMessageEntity entity)
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("update NoteMessage set ");
                strSql.Append("InfoTitle=@InfoTitle,");
                strSql.Append("InfoUrl=@InfoUrl,");
                strSql.Append("InfoDate=@InfoDate,");
                strSql.Append("AddDate=@AddDate,");
                strSql.Append("AddUserId=@AddUserId,");
                strSql.Append("AddUserName=@AddUserName,");
                strSql.Append("AccepterId=@AccepterId,");
                strSql.Append("Accepter=@Accepter,");
                strSql.Append("Status=@Status,");
                strSql.Append("Message=@Message");

                strSql.Append(" where ID=@ID");
                SqlParameter[] parameters = {
					new SqlParameter("@ID",SqlDbType.BigInt),
					new SqlParameter("@InfoTitle",SqlDbType.NVarChar),
					new SqlParameter("@InfoUrl",SqlDbType.NVarChar),
					new SqlParameter("@InfoDate",SqlDbType.DateTime),
					new SqlParameter("@AddDate",SqlDbType.DateTime),
					new SqlParameter("@AddUserId",SqlDbType.Int),
					new SqlParameter("@AddUserName",SqlDbType.NVarChar),
					new SqlParameter("@AccepterId",SqlDbType.Int),
					new SqlParameter("@Accepter",SqlDbType.NVarChar),
					new SqlParameter("@Status",SqlDbType.Int),
					new SqlParameter("@Message",SqlDbType.NText)
					};
                parameters[0].Value = entity.ID;
                parameters[1].Value = entity.InfoTitle;
                parameters[2].Value = entity.InfoUrl;
                parameters[3].Value = entity.InfoDate;
                parameters[4].Value = entity.AddDate;
                parameters[5].Value = entity.AddUserId;
                parameters[6].Value = entity.AddUserName;
                parameters[7].Value = entity.AccepterId;
                parameters[8].Value = entity.Accepter;
                parameters[9].Value = entity.Status;
                parameters[10].Value = entity.Message;

                sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            }

            public void Delete(long id) {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from NoteMessage ");
                strSql.Append(" where ID=@primaryKeyId");
                SqlParameter[] parameters = {
						new SqlParameter("@primaryKeyId", SqlDbType.BigInt)
					};
                parameters[0].Value = id;
                sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            }

            public void Delete(string idlist)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from NoteMessage ");
                strSql.Append(" where ID in (@primaryKeyId)");
                SqlParameter[] parameters = {
						new SqlParameter("@primaryKeyId", SqlDbType.VarChar)
					};
                parameters[0].Value = idlist;
                sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            }

            public override void Delete(NoteMessageEntity entity)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from NoteMessage ");
                strSql.Append(" where ID=@primaryKeyId");
                SqlParameter[] parameters = {
						new SqlParameter("@primaryKeyId", SqlDbType.Int)
					};
                parameters[0].Value = entity.ID;
                sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            }

            public override NoteMessageEntity FindById(long primaryKeyId)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select * from NoteMessage ");
                strSql.Append(" where ID=@primaryKeyId");
                SqlParameter[] parameters = {
						new SqlParameter("@primaryKeyId", SqlDbType.BigInt)};
                parameters[0].Value = primaryKeyId;
                DataSet ds = sqlHelper.ExecuteDateSet(strSql.ToString(), parameters);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    NoteMessageEntity entity = new NoteMessageEntity();
                    if (!Convert.IsDBNull(row["ID"]))
                    {
                        entity.ID = (long)row["ID"];
                    }
                    entity.InfoTitle = row["InfoTitle"].ToString();
                    entity.InfoUrl = row["InfoUrl"].ToString();
                    if (!Convert.IsDBNull(row["InfoDate"]))
                    {
                        entity.InfoDate = (DateTime)row["InfoDate"];
                    }
                    if (!Convert.IsDBNull(row["AddDate"]))
                    {
                        entity.AddDate = (DateTime)row["AddDate"];
                    }
                    if (!Convert.IsDBNull(row["AddUserId"]))
                    {
                        entity.AddUserId = (int)row["AddUserId"];
                    }
                    entity.AddUserName = row["AddUserName"].ToString();
                    if (!Convert.IsDBNull(row["AccepterId"]))
                    {
                        entity.AccepterId = (int)row["AccepterId"];
                    }
                    entity.Accepter = row["Accepter"].ToString();
                    if (!Convert.IsDBNull(row["Status"]))
                    {
                        entity.Status = (int)row["Status"];
                    }
                    entity.Message = row["Message"].ToString();
                    return entity;
                }
                else
                {
                    return null;
                }
            }
            public List<NoteMessageEntity> Find(string strWhere) {
                return Find(strWhere, null);
            }

            public override List<NoteMessageEntity> Find(string strWhere, SqlParameter[] parameters)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select *");
                strSql.Append(" FROM NoteMessage(nolock) ");
                if (strWhere.Trim() != "")
                {
                    strSql.Append(" where " + strWhere);
                }

                DataSet ds = sqlHelper.ExecuteDateSet(strSql.ToString(), parameters);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<NoteMessageEntity> list = new List<NoteMessageEntity>();
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        NoteMessageEntity entity = new NoteMessageEntity();
                        if (!Convert.IsDBNull(row["ID"]))
                        {
                            entity.ID = (long)row["ID"];
                        }
                        entity.InfoTitle = row["InfoTitle"].ToString();
                        entity.InfoUrl = row["InfoUrl"].ToString();
                        if (!Convert.IsDBNull(row["InfoDate"]))
                        {
                            entity.InfoDate = (DateTime)row["InfoDate"];
                        }
                        if (!Convert.IsDBNull(row["AddDate"]))
                        {
                            entity.AddDate = (DateTime)row["AddDate"];
                        }
                        if (!Convert.IsDBNull(row["AddUserId"]))
                        {
                            entity.AddUserId = (int)row["AddUserId"];
                        }
                        entity.AddUserName = row["AddUserName"].ToString();
                        if (!Convert.IsDBNull(row["AccepterId"]))
                        {
                            entity.AccepterId = (int)row["AccepterId"];
                        }
                        entity.Accepter = row["Accepter"].ToString();
                        if (!Convert.IsDBNull(row["Status"]))
                        {
                            entity.Status = (int)row["Status"];
                        }
                        entity.Message = row["Message"].ToString();

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
                strSql.Append(" FROM NoteMessage(nolock)");
                if (strWhere.Trim() != "")
                {
                    strSql.Append(" where " + strWhere);
                }
                return sqlHelper.ExecuteDateSet(strSql.ToString(), param);
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
                string sql = "select count(*) from NoteMessage ";
                if (!string.IsNullOrEmpty(where))
                {
                    sql += "where " + where;
                }

                object obj = sqlHelper.GetSingle(sql, param);

                return obj == null ? 0 : Convert.ToInt32(obj);
            }


            public DataTable GetPager(string where, string orderBy, int pageSize, int pageNumber)
            {
                return GetPager(where, null, orderBy, pageSize, pageNumber);
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
                int startNumber = pageSize * (pageNumber - 1);
                int endNumber = startNumber + pageSize + 1;
                string sql = string.Format("SELECT TOP {0} * FROM (SELECT ROW_NUMBER() OVER", pageSize);

                if (!string.IsNullOrEmpty(orderBy))
                {
                    sql += string.Format(" (ORDER BY {0})", orderBy);
                }
                else
                {

                    sql += " (ORDER BY ID)";//默认按主键排序

                }

                sql += " AS RowNumber,* FROM NoteMessage";

                if (!string.IsNullOrEmpty(where))
                {
                    sql += " where " + where;
                }

                sql += " ) _myResults WHERE RowNumber>" + startNumber.ToString() + " AND RowNumber<" + endNumber.ToString();

                return sqlHelper.ExecuteDateSet(sql, param).Tables[0];
            }

            #endregion

        }
    }
}

