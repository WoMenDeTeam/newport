using System;
using System.Collections.Generic;
using System.Text;
using Demo.DAL;
using System.Data.SqlClient;
using System.Data;
namespace Demo.DAL.SQLEntity
{

    [Serializable]
    public partial class EventsEntity
    {
        private SqlHelper sqlHelper;

        #region const fields
        public const string DBName = "MinZhengDB";
        public const string TableName = "Events";
        public const string PrimaryKey = "PK_Events";
        #endregion

        #region columns
        public struct Columns
        {
            public const string EventId = "EventId";
            public const string Summary = "Summary";
            public const string EventName = "EventName";
            public const string EventTime = "EventTime";
            public const string KeyWords = "KeyWords";
        }
        #endregion

        #region constructors
        public EventsEntity()
        {
            sqlHelper = new SqlHelper(DBName);
        }

        public EventsEntity(int eventid, string summary, string eventname, DateTime eventtime, string keywords)
        {
            this.EventId = eventid;

            this.Summary = summary;

            this.EventName = eventname;

            this.EventTime = eventtime;

            this.KeyWords = keywords;

        }
        #endregion

        #region Properties

        public int? EventId
        {
            get;
            set;
        }


        public string Summary
        {
            get;
            set;
        }


        public string EventName
        {
            get;
            set;
        }


        public DateTime? EventTime
        {
            get;
            set;
        }


        public string KeyWords
        {
            get;
            set;
        }

        #endregion

        public class EventsDAO : SqlDAO<EventsEntity>
        {
            private SqlHelper sqlHelper;
            public const string DBName = "SqlEventsConnStr";

            public EventsDAO()
            {
                sqlHelper = new SqlHelper(DBName);
            }

            public override void Add(EventsEntity entity)
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into Events(");
                strSql.Append("Summary,EventName,EventTime,KeyWords)");
                strSql.Append(" values (");
                strSql.Append("@Summary,@EventName,@EventTime,@KeyWords)");
                SqlParameter[] parameters = {
					new SqlParameter("@Summary",SqlDbType.NVarChar),
					new SqlParameter("@EventName",SqlDbType.NVarChar),
					new SqlParameter("@EventTime",SqlDbType.DateTime),
					new SqlParameter("@KeyWords",SqlDbType.NVarChar)
					};
                parameters[0].Value = entity.Summary;
                parameters[1].Value = entity.EventName;
                parameters[2].Value = entity.EventTime;
                parameters[3].Value = entity.KeyWords;

                sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            }

            public override void Update(EventsEntity entity)
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("update Events set ");
                strSql.Append("Summary=@Summary,");
                strSql.Append("EventName=@EventName,");
                strSql.Append("EventTime=@EventTime,");
                strSql.Append("KeyWords=@KeyWords");

                strSql.Append(" where EventId=@EventId");
                SqlParameter[] parameters = {
					new SqlParameter("@EventId",SqlDbType.Int),
					new SqlParameter("@Summary",SqlDbType.NVarChar),
					new SqlParameter("@EventName",SqlDbType.NVarChar),
					new SqlParameter("@EventTime",SqlDbType.DateTime),
					new SqlParameter("@KeyWords",SqlDbType.NVarChar)
					};
                parameters[0].Value = entity.EventId;
                parameters[1].Value = entity.Summary;
                parameters[2].Value = entity.EventName;
                parameters[3].Value = entity.EventTime;
                parameters[4].Value = entity.KeyWords;

                sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            }

            public override void Delete(EventsEntity entity)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from Events ");
                strSql.Append(" where EventId=@primaryKeyId");
                SqlParameter[] parameters = {
						new SqlParameter("@primaryKeyId", SqlDbType.Int)
					};
                parameters[0].Value = entity.EventId;
                sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            }

            public override EventsEntity FindById(long primaryKeyId)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select * from Events ");
                strSql.Append(" where EventId=@primaryKeyId");
                SqlParameter[] parameters = {
						new SqlParameter("@primaryKeyId", SqlDbType.Int)};
                parameters[0].Value = primaryKeyId;
                DataSet ds = sqlHelper.ExecuteDateSet(strSql.ToString(), parameters);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    EventsEntity entity = new EventsEntity();
                    if (!Convert.IsDBNull(row["EventId"]))
                    {
                        entity.EventId = (int)row["EventId"];
                    }
                    entity.Summary = row["Summary"].ToString();
                    entity.EventName = row["EventName"].ToString();
                    if (!Convert.IsDBNull(row["EventTime"]))
                    {
                        entity.EventTime = (DateTime)row["EventTime"];
                    }
                    entity.KeyWords = row["KeyWords"].ToString();
                    return entity;
                }
                else
                {
                    return null;
                }
            }

            public override List<EventsEntity> Find(string strWhere, SqlParameter[] parameters)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select *");
                strSql.Append(" FROM Events(nolock) ");
                if (strWhere.Trim() != "")
                {
                    strSql.Append(" where " + strWhere);
                }

                DataSet ds = sqlHelper.ExecuteDateSet(strSql.ToString(), parameters);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<EventsEntity> list = new List<EventsEntity>();
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        EventsEntity entity = new EventsEntity();
                        if (!Convert.IsDBNull(row["EventId"]))
                        {
                            entity.EventId = (int)row["EventId"];
                        }
                        entity.Summary = row["Summary"].ToString();
                        entity.EventName = row["EventName"].ToString();
                        if (!Convert.IsDBNull(row["EventTime"]))
                        {
                            entity.EventTime = (DateTime)row["EventTime"];
                        }
                        entity.KeyWords = row["KeyWords"].ToString();

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
                strSql.Append(" FROM Events(nolock)");
                if (strWhere.Trim() != "")
                {
                    strSql.Append(" where " + strWhere);
                }
                return sqlHelper.ExecuteDateSet(strSql.ToString(), param);
            }

            #region paging methods

            /// <summary>
            /// 获取分页记录总数
            /// </summary>
            /// <param name="where">条件，等同于GetPaer()方法的where</param>
            /// <returns>返回记录总数</returns>
            public int GetPagerRowsCount(string where, SqlParameter[] param)
            {
                string sql = "select count(*) from Events ";
                if (!string.IsNullOrEmpty(where))
                {
                    sql += "where " + where;
                }

                object obj = sqlHelper.GetSingle(sql, param);

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
                int startNumber = pageSize * (pageNumber - 1);

                string sql = string.Format("SELECT TOP {0} * FROM (SELECT ROW_NUMBER() OVER", pageSize);

                if (!string.IsNullOrEmpty(orderBy))
                {
                    sql += string.Format(" (ORDER BY {0})", orderBy);
                }
                else
                {

                    sql += " (ORDER BY EventId)";//默认按主键排序

                }

                sql += " AS RowNumber,* FROM Events";

                if (!string.IsNullOrEmpty(where))
                {
                    sql += " where " + where;
                }

                sql += " ) _myResults WHERE RowNumber>" + startNumber.ToString();

                return sqlHelper.ExecuteDateSet(sql, param).Tables[0];
            }

            #endregion

        }
    }

}
