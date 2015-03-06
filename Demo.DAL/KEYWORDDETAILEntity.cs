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
    public partial class KEYWORDDETAILEntity
    {
        private SqlHelper _sqlHelper;

        #region const fields
        public const string DBName = "Sentiment";
        public const string TableName = "KEYWORDDETAIL";
        public const string PrimaryKey = "PK_KEYWORDDETAIL";
        #endregion

        #region columns
        public struct Columns
        {
            public const string ID = "ID";
            public const string KEYWORD = "KEYWORD";
            public const string ACCID = "ACCID";
            public const string SEARCHTIME = "SEARCHTIME";
            public const string SEARCHTIMESTR = "SEARCHTIMESTR";
            public const string IPADDRESS = "IPADDRESS";
        }
        #endregion

        #region constructors
        public KEYWORDDETAILEntity()
        {
            _sqlHelper = new SqlHelper(DBName);
        }

        public KEYWORDDETAILEntity(long id, string keyword, string accid, DateTime searchtime, string searchtimestr, string ipaddress)
        {
            this.ID = id;

            this.KEYWORD = keyword;

            this.ACCID = accid;

            this.SEARCHTIME = searchtime;

            this.SEARCHTIMESTR = searchtimestr;

            this.IPADDRESS = ipaddress;

        }
        #endregion

        #region Properties

        public long? ID
        {
            get;
            set;
        }


        public string KEYWORD
        {
            get;
            set;
        }


        public string ACCID
        {
            get;
            set;
        }


        public DateTime? SEARCHTIME
        {
            get;
            set;
        }


        public string SEARCHTIMESTR
        {
            get;
            set;
        }


        public string IPADDRESS
        {
            get;
            set;
        }

        #endregion

        public class KEYWORDDETAILDAO : SqlDAO<KEYWORDDETAILEntity>
        {
            private SqlHelper _sqlHelper;
            public const string DBName = "SentimentConnStr";

            public KEYWORDDETAILDAO()
            {
                _sqlHelper = new SqlHelper(DBName);
            }

            public override void Add(KEYWORDDETAILEntity entity)
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into KEYWORDDETAIL(");
                strSql.Append("KEYWORD,ACCID,SEARCHTIME,SEARCHTIMESTR,IPADDRESS)");
                strSql.Append(" values (");
                strSql.Append("@KEYWORD,@ACCID,@SEARCHTIME,@SEARCHTIMESTR,@IPADDRESS)");
                SqlParameter[] parameters = {
					new SqlParameter("@KEYWORD",SqlDbType.NVarChar),
					new SqlParameter("@ACCID",SqlDbType.NVarChar),
					new SqlParameter("@SEARCHTIME",SqlDbType.DateTime),
					new SqlParameter("@SEARCHTIMESTR",SqlDbType.NVarChar),
					new SqlParameter("@IPADDRESS",SqlDbType.NVarChar)
					};
                parameters[0].Value = entity.KEYWORD;
                parameters[1].Value = entity.ACCID;
                parameters[2].Value = entity.SEARCHTIME;
                parameters[3].Value = entity.SEARCHTIMESTR;
                parameters[4].Value = entity.IPADDRESS;
                _sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            }

            public override void Update(KEYWORDDETAILEntity entity)
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("update KEYWORDDETAIL set ");
                strSql.Append("KEYWORD=@KEYWORD,");
                strSql.Append("ACCID=@ACCID,");
                strSql.Append("SEARCHTIME=@SEARCHTIME,");
                strSql.Append("SEARCHTIMESTR=@SEARCHTIMESTR,");
                strSql.Append("IPADDRESS=@IPADDRESS");

                strSql.Append(" where ID=@ID");
                SqlParameter[] parameters = {
					new SqlParameter("@KEYWORD",SqlDbType.NVarChar),
					new SqlParameter("@ACCID",SqlDbType.NVarChar),
					new SqlParameter("@SEARCHTIME",SqlDbType.DateTime),
					new SqlParameter("@SEARCHTIMESTR",SqlDbType.NVarChar),
					new SqlParameter("@IPADDRESS",SqlDbType.NVarChar),
					new SqlParameter("@ID",SqlDbType.BigInt)
				};
                parameters[0].Value = entity.KEYWORD;
                parameters[1].Value = entity.ACCID;
                parameters[2].Value = entity.SEARCHTIME;
                parameters[3].Value = entity.SEARCHTIMESTR;
                parameters[4].Value = entity.IPADDRESS;
                parameters[5].Value = entity.ID;

                _sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            }

            public bool UpdateSet(int ID, string ColumnName, string Value)
            {
                try
                {
                    StringBuilder StrSql = new StringBuilder();
                    StrSql.Append("update KEYWORDDETAIL set ");
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
                string strSql = "delete from KEYWORDDETAIL where ID=" + ID;
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
                string strSql = "delete from KEYWORDDETAIL where ID in (" + ID + ")";
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

            public override void Delete(KEYWORDDETAILEntity entity)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from KEYWORDDETAIL ");
                strSql.Append(" where ID=@primaryKeyId");
                SqlParameter[] parameters = {
						new SqlParameter("@primaryKeyId", SqlDbType.Int)
					};
                parameters[0].Value = entity.ID;
                _sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            }

            public override KEYWORDDETAILEntity FindById(long primaryKeyId)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select * from KEYWORDDETAIL ");
                strSql.Append(" where ID=@primaryKeyId");
                SqlParameter[] parameters = {
						new SqlParameter("@primaryKeyId", SqlDbType.Int)};
                parameters[0].Value = primaryKeyId;
                DataSet ds = _sqlHelper.ExecuteDateSet(strSql.ToString(), parameters);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    KEYWORDDETAILEntity entity = new KEYWORDDETAILEntity();
                    if (!Convert.IsDBNull(row["ID"]))
                    {
                        entity.ID = Convert.ToInt64(row["ID"]);
                    }
                    entity.KEYWORD = row["KEYWORD"].ToString();
                    entity.ACCID = row["ACCID"].ToString();
                    if (!Convert.IsDBNull(row["SEARCHTIME"]))
                    {
                        entity.SEARCHTIME = Convert.ToDateTime(row["SEARCHTIME"]);
                    }
                    entity.SEARCHTIMESTR = row["SEARCHTIMESTR"].ToString();
                    entity.IPADDRESS = row["IPADDRESS"].ToString();
                    return entity;
                }
                else
                {
                    return null;
                }
            }

            public override List<KEYWORDDETAILEntity> Find(string strWhere, SqlParameter[] parameters)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select *");
                strSql.Append(" FROM KEYWORDDETAIL ");
                if (strWhere.Trim() != "")
                {
                    strSql.Append(" where " + strWhere);
                }

                DataSet ds = _sqlHelper.ExecuteDateSet(strSql.ToString(), parameters);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<KEYWORDDETAILEntity> list = new List<KEYWORDDETAILEntity>();
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        KEYWORDDETAILEntity entity = new KEYWORDDETAILEntity();
                        if (!Convert.IsDBNull(row["ID"]))
                        {
                            entity.ID = Convert.ToInt64(row["ID"]);
                        }
                        entity.KEYWORD = row["KEYWORD"].ToString();
                        entity.ACCID = row["ACCID"].ToString();
                        if (!Convert.IsDBNull(row["SEARCHTIME"]))
                        {
                            entity.SEARCHTIME = Convert.ToDateTime(row["SEARCHTIME"]);
                        }
                        entity.SEARCHTIMESTR = row["SEARCHTIMESTR"].ToString();
                        entity.IPADDRESS = row["IPADDRESS"].ToString();

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
                strSql.Append(" FROM KEYWORDDETAIL");
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
                string sql = "select count(*) from KEYWORDDETAIL ";
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
                PagerSql.Append("FROM (SELECT * FROM KEYWORDDETAIL ");
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

