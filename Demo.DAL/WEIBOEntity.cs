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
    public partial class WEIBOEntity
    {
        private SqlHelper _sqlHelper;

        #region const fields
        public const string DBName = "Sentiment";
        public const string TableName = "WEIBO";
        public const string PrimaryKey = "PK_WEIBO";
        #endregion

        #region columns
        public struct Columns
        {
            public const string ID = "ID";
            public const string CONTENTHTML = "CONTENTHTML";
            public const string AUTHORNAME = "AUTHORNAME";
            public const string AUTHORURL = "AUTHORURL";
            public const string FORWARDNUM = "FORWARDNUM";
            public const string SOURCEURL = "SOURCEURL";
            public const string COMMENTNUM = "COMMENTNUM";
            public const string URL = "URL";
            public const string PUBLISHTIME = "PUBLISHTIME";
            public const string CREATETIME = "CREATETIME";
            public const string UPDATETIME = "UPDATETIME";
        }
        #endregion

        #region constructors
        public WEIBOEntity()
        {
            _sqlHelper = new SqlHelper(DBName);
        }

        public WEIBOEntity(long id, string contenthtml, string authorname, string authorurl, int forwardnum, string sourceurl, int commentnum, string url, DateTime publishtime, DateTime createtime, DateTime updatetime)
        {
            this.ID = id;

            this.CONTENTHTML = contenthtml;

            this.AUTHORNAME = authorname;

            this.AUTHORURL = authorurl;

            this.FORWARDNUM = forwardnum;

            this.SOURCEURL = sourceurl;

            this.COMMENTNUM = commentnum;

            this.URL = url;

            this.PUBLISHTIME = publishtime;

            this.CREATETIME = createtime;

            this.UPDATETIME = updatetime;

        }
        #endregion

        #region Properties

        public long? ID
        {
            get;
            set;
        }


        public string CONTENTHTML
        {
            get;
            set;
        }


        public string AUTHORNAME
        {
            get;
            set;
        }


        public string AUTHORURL
        {
            get;
            set;
        }


        public int? FORWARDNUM
        {
            get;
            set;
        }


        public string SOURCEURL
        {
            get;
            set;
        }


        public int? COMMENTNUM
        {
            get;
            set;
        }


        public string URL
        {
            get;
            set;
        }


        public DateTime? PUBLISHTIME
        {
            get;
            set;
        }


        public DateTime? CREATETIME
        {
            get;
            set;
        }


        public DateTime? UPDATETIME
        {
            get;
            set;
        }

        #endregion

        public class WEIBODAO : SqlDAO<WEIBOEntity>
        {
            private SqlHelper _sqlHelper;
            public const string DBName = "SentimentConnStr";

            public WEIBODAO()
            {
                _sqlHelper = new SqlHelper(DBName);
            }

            public override void Add(WEIBOEntity entity)
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into WEIBO(");
                strSql.Append("CONTENTHTML,AUTHORNAME,AUTHORURL,FORWARDNUM,SOURCEURL,COMMENTNUM,URL,PUBLISHTIME,CREATETIME,UPDATETIME)");
                strSql.Append(" values (");
                strSql.Append("@CONTENTHTML,@AUTHORNAME,@AUTHORURL,@FORWARDNUM,@SOURCEURL,@COMMENTNUM,@URL,@PUBLISHTIME,@CREATETIME,@UPDATETIME)");
                SqlParameter[] parameters = {
					new SqlParameter("@CONTENTHTML",SqlDbType.Text),
					new SqlParameter("@AUTHORNAME",SqlDbType.NVarChar),
					new SqlParameter("@AUTHORURL",SqlDbType.NVarChar),
					new SqlParameter("@FORWARDNUM",SqlDbType.Int),
					new SqlParameter("@SOURCEURL",SqlDbType.NVarChar),
					new SqlParameter("@COMMENTNUM",SqlDbType.Int),
					new SqlParameter("@URL",SqlDbType.NVarChar),
					new SqlParameter("@PUBLISHTIME",SqlDbType.DateTime),
					new SqlParameter("@CREATETIME",SqlDbType.DateTime),
					new SqlParameter("@UPDATETIME",SqlDbType.DateTime)
					};
                parameters[0].Value = entity.CONTENTHTML;
                parameters[1].Value = entity.AUTHORNAME;
                parameters[2].Value = entity.AUTHORURL;
                parameters[3].Value = entity.FORWARDNUM;
                parameters[4].Value = entity.SOURCEURL;
                parameters[5].Value = entity.COMMENTNUM;
                parameters[6].Value = entity.URL;
                parameters[7].Value = entity.PUBLISHTIME;
                parameters[8].Value = entity.CREATETIME;
                parameters[9].Value = entity.UPDATETIME;
                _sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            }

            public override void Update(WEIBOEntity entity)
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("update WEIBO set ");
                strSql.Append("CONTENTHTML=@CONTENTHTML,");
                strSql.Append("AUTHORNAME=@AUTHORNAME,");
                strSql.Append("AUTHORURL=@AUTHORURL,");
                strSql.Append("FORWARDNUM=@FORWARDNUM,");
                strSql.Append("SOURCEURL=@SOURCEURL,");
                strSql.Append("COMMENTNUM=@COMMENTNUM,");
                strSql.Append("URL=@URL,");
                strSql.Append("PUBLISHTIME=@PUBLISHTIME,");
                strSql.Append("CREATETIME=@CREATETIME,");
                strSql.Append("UPDATETIME=@UPDATETIME");

                strSql.Append(" where ID=@ID");
                SqlParameter[] parameters = {
					new SqlParameter("@CONTENTHTML",SqlDbType.Text),
					new SqlParameter("@AUTHORNAME",SqlDbType.NVarChar),
					new SqlParameter("@AUTHORURL",SqlDbType.NVarChar),
					new SqlParameter("@FORWARDNUM",SqlDbType.Int),
					new SqlParameter("@SOURCEURL",SqlDbType.NVarChar),
					new SqlParameter("@COMMENTNUM",SqlDbType.Int),
					new SqlParameter("@URL",SqlDbType.NVarChar),
					new SqlParameter("@PUBLISHTIME",SqlDbType.DateTime),
					new SqlParameter("@CREATETIME",SqlDbType.DateTime),
					new SqlParameter("@UPDATETIME",SqlDbType.DateTime),
					new SqlParameter("@ID",SqlDbType.BigInt)
				};
                parameters[0].Value = entity.CONTENTHTML;
                parameters[1].Value = entity.AUTHORNAME;
                parameters[2].Value = entity.AUTHORURL;
                parameters[3].Value = entity.FORWARDNUM;
                parameters[4].Value = entity.SOURCEURL;
                parameters[5].Value = entity.COMMENTNUM;
                parameters[6].Value = entity.URL;
                parameters[7].Value = entity.PUBLISHTIME;
                parameters[8].Value = entity.CREATETIME;
                parameters[9].Value = entity.UPDATETIME;
                parameters[10].Value = entity.ID;

                _sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            }

            public bool UpdateSet(int ID, string ColumnName, string Value)
            {
                try
                {
                    StringBuilder StrSql = new StringBuilder();
                    StrSql.Append("update WEIBO set ");
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
                string strSql = "delete from WEIBO where ID=" + ID;
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
                string strSql = "delete from WEIBO where ID in (" + ID + ")";
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

            public override void Delete(WEIBOEntity entity)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from WEIBO ");
                strSql.Append(" where ID=@primaryKeyId");
                SqlParameter[] parameters = {
						new SqlParameter("@primaryKeyId", SqlDbType.Int)
					};
                parameters[0].Value = entity.ID;
                _sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            }

            public override WEIBOEntity FindById(long primaryKeyId)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select * from WEIBO ");
                strSql.Append(" where ID=@primaryKeyId");
                SqlParameter[] parameters = {
						new SqlParameter("@primaryKeyId", SqlDbType.Int)};
                parameters[0].Value = primaryKeyId;
                DataSet ds = _sqlHelper.ExecuteDateSet(strSql.ToString(), parameters);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    WEIBOEntity entity = new WEIBOEntity();
                    if (!Convert.IsDBNull(row["ID"]))
                    {
                        entity.ID = Convert.ToInt64(row["ID"]);
                    }
                    entity.CONTENTHTML = row["CONTENTHTML"].ToString();
                    entity.AUTHORNAME = row["AUTHORNAME"].ToString();
                    entity.AUTHORURL = row["AUTHORURL"].ToString();
                    if (!Convert.IsDBNull(row["FORWARDNUM"]))
                    {
                        entity.FORWARDNUM = Convert.ToInt32(row["FORWARDNUM"]);
                    }
                    entity.SOURCEURL = row["SOURCEURL"].ToString();
                    if (!Convert.IsDBNull(row["COMMENTNUM"]))
                    {
                        entity.COMMENTNUM = Convert.ToInt32(row["COMMENTNUM"]);
                    }
                    entity.URL = row["URL"].ToString();
                    if (!Convert.IsDBNull(row["PUBLISHTIME"]))
                    {
                        entity.PUBLISHTIME = Convert.ToDateTime(row["PUBLISHTIME"]);
                    }
                    if (!Convert.IsDBNull(row["CREATETIME"]))
                    {
                        entity.CREATETIME = Convert.ToDateTime(row["CREATETIME"]);
                    }
                    if (!Convert.IsDBNull(row["UPDATETIME"]))
                    {
                        entity.UPDATETIME = Convert.ToDateTime(row["UPDATETIME"]);
                    }
                    return entity;
                }
                else
                {
                    return null;
                }
            }

            public override List<WEIBOEntity> Find(string strWhere, SqlParameter[] parameters)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select *");
                strSql.Append(" FROM WEIBO ");
                if (strWhere.Trim() != "")
                {
                    strSql.Append(" where " + strWhere);
                }

                DataSet ds = _sqlHelper.ExecuteDateSet(strSql.ToString(), parameters);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<WEIBOEntity> list = new List<WEIBOEntity>();
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        WEIBOEntity entity = new WEIBOEntity();
                        if (!Convert.IsDBNull(row["ID"]))
                        {
                            entity.ID = Convert.ToInt64(row["ID"]);
                        }
                        entity.CONTENTHTML = row["CONTENTHTML"].ToString();
                        entity.AUTHORNAME = row["AUTHORNAME"].ToString();
                        entity.AUTHORURL = row["AUTHORURL"].ToString();
                        if (!Convert.IsDBNull(row["FORWARDNUM"]))
                        {
                            entity.FORWARDNUM = Convert.ToInt32(row["FORWARDNUM"]);
                        }
                        entity.SOURCEURL = row["SOURCEURL"].ToString();
                        if (!Convert.IsDBNull(row["COMMENTNUM"]))
                        {
                            entity.COMMENTNUM = Convert.ToInt32(row["COMMENTNUM"]);
                        }
                        entity.URL = row["URL"].ToString();
                        if (!Convert.IsDBNull(row["PUBLISHTIME"]))
                        {
                            entity.PUBLISHTIME = Convert.ToDateTime(row["PUBLISHTIME"]);
                        }
                        if (!Convert.IsDBNull(row["CREATETIME"]))
                        {
                            entity.CREATETIME = Convert.ToDateTime(row["CREATETIME"]);
                        }
                        if (!Convert.IsDBNull(row["UPDATETIME"]))
                        {
                            entity.UPDATETIME = Convert.ToDateTime(row["UPDATETIME"]);
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
                strSql.Append(" FROM WEIBO");
                if (strWhere.Trim() != "")
                {
                    strSql.Append(" where " + strWhere);
                }
                return _sqlHelper.ExecuteDateSet(strSql.ToString(), param);
            }

            public DataTable GetStaticDtByTime()
            {
                //string strSql = "SELECT COUNT(*) AS TOTALCOUNT,TO_CHAR(PUBLISHTIME,'yyyy-MM-dd') as PUBLISHTIME"
                //+ " FROM WEIBO GROUP BY TO_CHAR(PUBLISHTIME,'yyyy-MM-dd')";
                string strSql = "SELECT COUNT(1) AS TOTALCOUNT, CONVERT(VARCHAR(10),PUBLISHTIME,120)  AS PUBLISHTIME FROM dbo.WEIBO GROUP BY CONVERT(VARCHAR(10),PUBLISHTIME,120)";
                DataSet ds = _sqlHelper.ExecuteDateSet(strSql, null);
                if (ds != null)
                {
                    return ds.Tables[0];
                }
                else
                {
                    return null;
                }
            }

            #region paging methods

            /// <summary>
            /// 获取分页记录总数
            /// </summary>
            /// <param name="where">条件，等同于GetPaer()方法的where</param>
            /// <returns>返回记录总数</returns>
            public int GetPagerRowsCount(string where, SqlParameter[] param)
            {
                string sql = "select count(*) from WEIBO ";
                if (!string.IsNullOrEmpty(where))
                {
                    sql += "where " + where;
                }

                object obj = _sqlHelper.GetSingle(sql, param);

                return obj == null ? 0 : Convert.ToInt32(obj);
            }

            public int GetPagerRowsCount(string where)
            {
                return GetPagerRowsCount(where, null);
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
                PagerSql.Append("FROM (SELECT * FROM WEIBO ");
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


            public DataTable GetPager(string where, string orderBy, int pageSize, int pageNumber)
            {
                return GetPager(where, null, orderBy, pageSize, pageNumber);
            }
            #endregion

        }
    }
}

