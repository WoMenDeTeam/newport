using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Demo.DAL;


namespace Demo.DAL.SQLEntity
{
    [Serializable]
    public partial class EventClueEntity
    {
        private SqlHelper sqlHelper;

        #region const fields
        public const string DBName = "MinZhengDB";
        public const string TableName = "EventClue";
        public const string PrimaryKey = "PK_EventClue";
        #endregion

        #region columns
        public struct Columns
        {
            public const string ClueId = "ClueId";
            public const string EventId = "EventId";
            public const string ClueTime = "ClueTime";
            public const string ClueTitle = "ClueTitle";
            public const string ClueDocid = "ClueDocid";
        }
        #endregion

        #region constructors
        public EventClueEntity()
        {
            sqlHelper = new SqlHelper(DBName);
        }

        public EventClueEntity(int clueid, int eventid, DateTime cluetime, string cluetitle, int cluedocid)
        {
            this.ClueId = clueid;

            this.EventId = eventid;

            this.ClueTime = cluetime;

            this.ClueTitle = cluetitle;

            this.ClueDocid = cluedocid;

        }
        #endregion

        #region Properties

        public int? ClueId
        {
            get;
            set;
        }


        public int? EventId
        {
            get;
            set;
        }


        public DateTime? ClueTime
        {
            get;
            set;
        }


        public string ClueTitle
        {
            get;
            set;
        }


        public int? ClueDocid
        {
            get;
            set;
        }

        #endregion

        public class EventClueDAO : SqlDAO<EventClueEntity>
        {
            private SqlHelper sqlHelper;
            public const string DBName = "SqlEventsConnStr";

            public EventClueDAO()
            {
                sqlHelper = new SqlHelper(DBName);
            }

            public override void Add(EventClueEntity entity)
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into EventClue(");
                strSql.Append("EventId,ClueTime,ClueTitle,ClueDocid)");
                strSql.Append(" values (");
                strSql.Append("@EventId,@ClueTime,@ClueTitle,@ClueDocid)");
                SqlParameter[] parameters = {
					new SqlParameter("@EventId",SqlDbType.Int),
					new SqlParameter("@ClueTime",SqlDbType.DateTime),
					new SqlParameter("@ClueTitle",SqlDbType.NVarChar),
					new SqlParameter("@ClueDocid",SqlDbType.Int)
					};
                parameters[0].Value = entity.EventId;
                parameters[1].Value = entity.ClueTime;
                parameters[2].Value = entity.ClueTitle;
                parameters[3].Value = entity.ClueDocid;

                sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            }

            public override void Update(EventClueEntity entity)
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("update EventClue set ");
                strSql.Append("EventId=@EventId,");
                strSql.Append("ClueTime=@ClueTime,");
                strSql.Append("ClueTitle=@ClueTitle,");
                strSql.Append("ClueDocid=@ClueDocid");

                strSql.Append(" where ClueId=@ClueId");
                SqlParameter[] parameters = {
					new SqlParameter("@ClueId",SqlDbType.Int),
					new SqlParameter("@EventId",SqlDbType.Int),
					new SqlParameter("@ClueTime",SqlDbType.DateTime),
					new SqlParameter("@ClueTitle",SqlDbType.NVarChar),
					new SqlParameter("@ClueDocid",SqlDbType.Int)
					};
                parameters[0].Value = entity.ClueId;
                parameters[1].Value = entity.EventId;
                parameters[2].Value = entity.ClueTime;
                parameters[3].Value = entity.ClueTitle;
                parameters[4].Value = entity.ClueDocid;

                sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            }

            public override void Delete(EventClueEntity entity)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from EventClue ");
                strSql.Append(" where ClueId=@primaryKeyId");
                SqlParameter[] parameters = {
						new SqlParameter("@primaryKeyId", SqlDbType.Int)
					};
                parameters[0].Value = entity.ClueId;
                sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            }

            public override EventClueEntity FindById(long primaryKeyId)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select * from EventClue ");
                strSql.Append(" where ClueId=@primaryKeyId");
                SqlParameter[] parameters = {
						new SqlParameter("@primaryKeyId", SqlDbType.Int)};
                parameters[0].Value = primaryKeyId;
                DataSet ds = sqlHelper.ExecuteDateSet(strSql.ToString(), parameters);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    EventClueEntity entity = new EventClueEntity();
                    if (!Convert.IsDBNull(row["ClueId"]))
                    {
                        entity.ClueId = (int)row["ClueId"];
                    }
                    if (!Convert.IsDBNull(row["EventId"]))
                    {
                        entity.EventId = (int)row["EventId"];
                    }
                    if (!Convert.IsDBNull(row["ClueTime"]))
                    {
                        entity.ClueTime = (DateTime)row["ClueTime"];
                    }
                    entity.ClueTitle = row["ClueTitle"].ToString();
                    if (!Convert.IsDBNull(row["ClueDocid"]))
                    {
                        entity.ClueDocid = (int)row["ClueDocid"];
                    }
                    return entity;
                }
                else
                {
                    return null;
                }
            }

            public override List<EventClueEntity> Find(string strWhere, SqlParameter[] parameters)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select *");
                strSql.Append(" FROM EventClue(nolock) ");
                if (strWhere.Trim() != "")
                {
                    strSql.Append(" where " + strWhere);
                }

                DataSet ds = sqlHelper.ExecuteDateSet(strSql.ToString(), parameters);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<EventClueEntity> list = new List<EventClueEntity>();
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        EventClueEntity entity = new EventClueEntity();
                        if (!Convert.IsDBNull(row["ClueId"]))
                        {
                            entity.ClueId = (int)row["ClueId"];
                        }
                        if (!Convert.IsDBNull(row["EventId"]))
                        {
                            entity.EventId = (int)row["EventId"];
                        }
                        if (!Convert.IsDBNull(row["ClueTime"]))
                        {
                            entity.ClueTime = (DateTime)row["ClueTime"];
                        }
                        entity.ClueTitle = row["ClueTitle"].ToString();
                        if (!Convert.IsDBNull(row["ClueDocid"]))
                        {
                            entity.ClueDocid = (int)row["ClueDocid"];
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
                strSql.Append(" FROM EventClue(nolock)");
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
                string sql = "select count(*) from EventClue ";
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

                    sql += " (ORDER BY ClueId)";//默认按主键排序

                }

                sql += " AS RowNumber,* FROM EventClue";

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
