using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Demo.DAL;
namespace Demo.DAL.SQLEntity
{
    [Serializable]
    public partial class EventImgEntity
    {
        private SqlHelper sqlHelper;

        #region const fields
        public const string DBName = "MinZhengDB";
        public const string TableName = "EventImg";
        public const string PrimaryKey = "PK_EventImg";
        #endregion

        #region columns
        public struct Columns
        {
            public const string ImgId = "ImgId";
            public const string EventId = "EventId";
            public const string ImgPath = "ImgPath";
            public const string ImgUrl = "ImgUrl";
        }
        #endregion

        #region constructors
        public EventImgEntity()
        {
            sqlHelper = new SqlHelper(DBName);
        }

        public EventImgEntity(int imgid, int eventid, string imgpath, string imgurl)
        {
            this.ImgId = imgid;

            this.EventId = eventid;

            this.ImgPath = imgpath;

            this.ImgUrl = imgurl;

        }
        #endregion

        #region Properties

        public int? ImgId
        {
            get;
            set;
        }


        public int? EventId
        {
            get;
            set;
        }


        public string ImgPath
        {
            get;
            set;
        }


        public string ImgUrl
        {
            get;
            set;
        }

        #endregion

        public class EventImgDAO : SqlDAO<EventImgEntity>
        {
            private SqlHelper sqlHelper;
            public const string DBName = "SqlEventsConnStr";

            public EventImgDAO()
            {
                sqlHelper = new SqlHelper(DBName);
            }

            public override void Add(EventImgEntity entity)
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into EventImg(");
                strSql.Append("EventId,ImgPath,ImgUrl)");
                strSql.Append(" values (");
                strSql.Append("@EventId,@ImgPath,@ImgUrl)");
                SqlParameter[] parameters = {
					new SqlParameter("@EventId",SqlDbType.Int),
					new SqlParameter("@ImgPath",SqlDbType.VarChar),
					new SqlParameter("@ImgUrl",SqlDbType.VarChar)
					};
                parameters[0].Value = entity.EventId;
                parameters[1].Value = entity.ImgPath;
                parameters[2].Value = entity.ImgUrl;

                sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            }

            public override void Update(EventImgEntity entity)
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("update EventImg set ");
                strSql.Append("EventId=@EventId,");
                strSql.Append("ImgPath=@ImgPath,");
                strSql.Append("ImgUrl=@ImgUrl");

                strSql.Append(" where ImgId=@ImgId");
                SqlParameter[] parameters = {
					new SqlParameter("@ImgId",SqlDbType.Int),
					new SqlParameter("@EventId",SqlDbType.Int),
					new SqlParameter("@ImgPath",SqlDbType.VarChar),
					new SqlParameter("@ImgUrl",SqlDbType.VarChar)
					};
                parameters[0].Value = entity.ImgId;
                parameters[1].Value = entity.EventId;
                parameters[2].Value = entity.ImgPath;
                parameters[3].Value = entity.ImgUrl;

                sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            }

            public override void Delete(EventImgEntity entity)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from EventImg ");
                strSql.Append(" where ImgId=@primaryKeyId");
                SqlParameter[] parameters = {
						new SqlParameter("@primaryKeyId", SqlDbType.Int)
					};
                parameters[0].Value = entity.ImgId;
                sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            }

            public override EventImgEntity FindById(long primaryKeyId)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select * from EventImg ");
                strSql.Append(" where ImgId=@primaryKeyId");
                SqlParameter[] parameters = {
						new SqlParameter("@primaryKeyId", SqlDbType.Int)};
                parameters[0].Value = primaryKeyId;
                DataSet ds = sqlHelper.ExecuteDateSet(strSql.ToString(), parameters);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    EventImgEntity entity = new EventImgEntity();
                    if (!Convert.IsDBNull(row["ImgId"]))
                    {
                        entity.ImgId = (int)row["ImgId"];
                    }
                    if (!Convert.IsDBNull(row["EventId"]))
                    {
                        entity.EventId = (int)row["EventId"];
                    }
                    entity.ImgPath = row["ImgPath"].ToString();
                    entity.ImgUrl = row["ImgUrl"].ToString();
                    return entity;
                }
                else
                {
                    return null;
                }
            }

            public override List<EventImgEntity> Find(string strWhere, SqlParameter[] parameters)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select *");
                strSql.Append(" FROM EventImg(nolock) ");
                if (strWhere.Trim() != "")
                {
                    strSql.Append(" where " + strWhere);
                }

                DataSet ds = sqlHelper.ExecuteDateSet(strSql.ToString(), parameters);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<EventImgEntity> list = new List<EventImgEntity>();
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        EventImgEntity entity = new EventImgEntity();
                        if (!Convert.IsDBNull(row["ImgId"]))
                        {
                            entity.ImgId = (int)row["ImgId"];
                        }
                        if (!Convert.IsDBNull(row["EventId"]))
                        {
                            entity.EventId = (int)row["EventId"];
                        }
                        entity.ImgPath = row["ImgPath"].ToString();
                        entity.ImgUrl = row["ImgUrl"].ToString();

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
                strSql.Append(" FROM EventImg(nolock)");
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
                string sql = "select count(*) from EventImg ";
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

                    sql += " (ORDER BY ImgId)";//默认按主键排序

                }

                sql += " AS RowNumber,* FROM EventImg";

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
