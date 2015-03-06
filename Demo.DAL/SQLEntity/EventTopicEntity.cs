using System;
using System.Collections.Generic;
using System.Text;
using Demo.DAL;
using System.Data.SqlClient;
using System.Data;
namespace Demo.DAL.SQLEntity
{

    [Serializable]
    public partial class EventTopicEntity
    {
        private SqlHelper sqlHelper;

        #region const fields
        public const string DBName = "SqlEventsConnStr";
        public const string TableName = "EventTopic";
        public const string PrimaryKey = "PK_EventTopic";
        #endregion

        #region columns
        public struct Columns
        {
            public const string TopicId = "TopicId";
            public const string EventId = "EventId";
            public const string TopicTitle = "TopicTitle";
            public const string TopicImage = "TopicImage";
            public const string TopicContent = "TopicContent";
            public const string TopicDocid = "TopicDocid";
        }
        #endregion

        #region constructors
        public EventTopicEntity()
        {
            sqlHelper = new SqlHelper(DBName);
        }

        public EventTopicEntity(int topicid, int eventid, string topictitle, string topicimage, string topiccontent, int topicdocid)
        {
            this.TopicId = topicid;

            this.EventId = eventid;

            this.TopicTitle = topictitle;

            this.TopicImage = topicimage;

            this.TopicContent = topiccontent;

            this.TopicDocid = topicdocid;

        }
        #endregion

        #region Properties

        public int? TopicId
        {
            get;
            set;
        }


        public int? EventId
        {
            get;
            set;
        }


        public string TopicTitle
        {
            get;
            set;
        }


        public string TopicImage
        {
            get;
            set;
        }


        public string TopicContent
        {
            get;
            set;
        }


        public int? TopicDocid
        {
            get;
            set;
        }

        #endregion

        public class EventTopicDAO : SqlDAO<EventTopicEntity>
        {
            private SqlHelper sqlHelper;
            public const string DBName = "SqlEventsConnStr";

            public EventTopicDAO()
            {
                sqlHelper = new SqlHelper(DBName);
            }

            public override void Add(EventTopicEntity entity)
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into EventTopic(");
                strSql.Append("EventId,TopicTitle,TopicImage,TopicContent,TopicDocid)");
                strSql.Append(" values (");
                strSql.Append("@EventId,@TopicTitle,@TopicImage,@TopicContent,@TopicDocid)");
                SqlParameter[] parameters = {
					new SqlParameter("@EventId",SqlDbType.Int),
					new SqlParameter("@TopicTitle",SqlDbType.NVarChar),
					new SqlParameter("@TopicImage",SqlDbType.VarChar),
					new SqlParameter("@TopicContent",SqlDbType.NVarChar),
					new SqlParameter("@TopicDocid",SqlDbType.Int)
					};
                parameters[0].Value = entity.EventId;
                parameters[1].Value = entity.TopicTitle;
                parameters[2].Value = entity.TopicImage;
                parameters[3].Value = entity.TopicContent;
                parameters[4].Value = entity.TopicDocid;

                sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            }

            public override void Update(EventTopicEntity entity)
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("update EventTopic set ");
                strSql.Append("EventId=@EventId,");
                strSql.Append("TopicTitle=@TopicTitle,");
                strSql.Append("TopicImage=@TopicImage,");
                strSql.Append("TopicContent=@TopicContent,");
                strSql.Append("TopicDocid=@TopicDocid");

                strSql.Append(" where TopicId=@TopicId");
                SqlParameter[] parameters = {
					new SqlParameter("@TopicId",SqlDbType.Int),
					new SqlParameter("@EventId",SqlDbType.Int),
					new SqlParameter("@TopicTitle",SqlDbType.NVarChar),
					new SqlParameter("@TopicImage",SqlDbType.VarChar),
					new SqlParameter("@TopicContent",SqlDbType.NVarChar),
					new SqlParameter("@TopicDocid",SqlDbType.Int)
					};
                parameters[0].Value = entity.TopicId;
                parameters[1].Value = entity.EventId;
                parameters[2].Value = entity.TopicTitle;
                parameters[3].Value = entity.TopicImage;
                parameters[4].Value = entity.TopicContent;
                parameters[5].Value = entity.TopicDocid;

                sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            }

            public override void Delete(EventTopicEntity entity)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from EventTopic ");
                strSql.Append(" where TopicId=@primaryKeyId");
                SqlParameter[] parameters = {
						new SqlParameter("@primaryKeyId", SqlDbType.Int)
					};
                parameters[0].Value = entity.TopicId;
                sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            }

            public override EventTopicEntity FindById(long primaryKeyId)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select * from EventTopic ");
                strSql.Append(" where TopicId=@primaryKeyId");
                SqlParameter[] parameters = {
						new SqlParameter("@primaryKeyId", SqlDbType.Int)};
                parameters[0].Value = primaryKeyId;
                DataSet ds = sqlHelper.ExecuteDateSet(strSql.ToString(), parameters);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    EventTopicEntity entity = new EventTopicEntity();
                    if (!Convert.IsDBNull(row["TopicId"]))
                    {
                        entity.TopicId = (int)row["TopicId"];
                    }
                    if (!Convert.IsDBNull(row["EventId"]))
                    {
                        entity.EventId = (int)row["EventId"];
                    }
                    entity.TopicTitle = row["TopicTitle"].ToString();
                    entity.TopicImage = row["TopicImage"].ToString();
                    entity.TopicContent = row["TopicContent"].ToString();
                    if (!Convert.IsDBNull(row["TopicDocid"]))
                    {
                        entity.TopicDocid = (int)row["TopicDocid"];
                    }
                    return entity;
                }
                else
                {
                    return null;
                }
            }

            public override List<EventTopicEntity> Find(string strWhere, SqlParameter[] parameters)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select *");
                strSql.Append(" FROM EventTopic(nolock) ");
                if (strWhere.Trim() != "")
                {
                    strSql.Append(" where " + strWhere);
                }

                DataSet ds = sqlHelper.ExecuteDateSet(strSql.ToString(), parameters);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<EventTopicEntity> list = new List<EventTopicEntity>();
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        EventTopicEntity entity = new EventTopicEntity();
                        if (!Convert.IsDBNull(row["TopicId"]))
                        {
                            entity.TopicId = (int)row["TopicId"];
                        }
                        if (!Convert.IsDBNull(row["EventId"]))
                        {
                            entity.EventId = (int)row["EventId"];
                        }
                        entity.TopicTitle = row["TopicTitle"].ToString();
                        entity.TopicImage = row["TopicImage"].ToString();
                        entity.TopicContent = row["TopicContent"].ToString();
                        if (!Convert.IsDBNull(row["TopicDocid"]))
                        {
                            entity.TopicDocid = (int)row["TopicDocid"];
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
                strSql.Append(" FROM EventTopic(nolock)");
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
                string sql = "select count(*) from EventTopic ";
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

                    sql += " (ORDER BY TopicId)";//默认按主键排序

                }

                sql += " AS RowNumber,* FROM EventTopic";

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
