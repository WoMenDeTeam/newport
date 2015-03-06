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
    public partial class TRENDDATAEntity
    {
        private SqlHelper _sqlHelper;

        #region const fields
        public const string DBName = "Sentiment";
        public const string TableName = "TRENDDATA";
        public const string PrimaryKey = "PK_TRENDDATA";
        #endregion

        #region columns
        public struct Columns
        {
            public const string CATEGORYID = "CATEGORYID";
            public const string ARTICLECOUNT = "ARTICLECOUNT";
            public const string DATE = "DATE";
            public const string TAG = "TAG";
        }
        #endregion

        #region constructors
        public TRENDDATAEntity()
        {
            _sqlHelper = new SqlHelper(DBName);
        }

        public TRENDDATAEntity(long categoryid, long articlecount, DateTime date, int tag)
        {

            this.CATEGORYID = categoryid;

            this.ARTICLECOUNT = articlecount;

            this.DATE = date;

            this.TAG = tag;

        }
        #endregion

        #region Properties


        public long? CATEGORYID
        {
            get;
            set;
        }


        public long? ARTICLECOUNT
        {
            get;
            set;
        }


        public DateTime? DATE
        {
            get;
            set;
        }


        public int? TAG
        {
            get;
            set;
        }

        #endregion

        public class TRENDDATADAO : SqlDAO<TRENDDATAEntity>
        {
            private SqlHelper _sqlHelper;
            public const string DBName = "SentimentConnStr";

            public TRENDDATADAO()
            {
                _sqlHelper = new SqlHelper(DBName);
            }

            public override void Add(TRENDDATAEntity entity)
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into TRENDDATA(");
                strSql.Append("CATEGORYID,ARTICLECOUNT,\"DATE\",TAG)");
                strSql.Append(" values (");
                strSql.Append("@CATEGORYID,@ARTICLECOUNT,@LDATE,@TAG)");
                SqlParameter[] parameters = {
					new SqlParameter("@CATEGORYID",SqlDbType.BigInt),
					new SqlParameter("@ARTICLECOUNT",SqlDbType.BigInt),
					new SqlParameter("@LDATE",SqlDbType.DateTime),
					new SqlParameter("@TAG",SqlDbType.Int)
					};
                parameters[0].Value = entity.CATEGORYID;
                parameters[1].Value = entity.ARTICLECOUNT;
                parameters[2].Value = entity.DATE;
                parameters[3].Value = entity.TAG;
                _sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            }

            public bool DeleteByCategoryid(long CategoryID)
            {
                string strSql = "delete from TRENDDATA where CATEGORYID=@CATEGORYID";
                SqlParameter[] parameters = {
					new SqlParameter("@CATEGORYID",SqlDbType.BigInt)					
					};
                parameters[0].Value = CategoryID;
                try
                {
                    _sqlHelper.ExecuteSql(strSql, parameters);
                    return true;
                }
                catch
                {
                    Console.WriteLine("Delete TRENDDATA Error");
                    return false;
                }
            }
            public List<TRENDDATAEntity> Find(string strWhere)
            {
                return Find(strWhere, null);
            }

            public DateTime GetDate(long categoryid, int type)
            {
                string strSql = string.Empty;
                if (type == 1)
                {
                    strSql = "SELECT MIN(\"DATE\") AS LDATE FROM TRENDDATA WHERE CATEGORYID=@CATEGORYID";
                }
                else
                {
                    strSql = "SELECT MAX(\"DATE\") AS LDATE FROM TRENDDATA WHERE CATEGORYID=@CATEGORYID";
                }
                SqlParameter[] parameters = {
					new SqlParameter("@CATEGORYID",SqlDbType.BigInt)					
					};
                parameters[0].Value = categoryid;
                DataSet ds = _sqlHelper.ExecuteDateSet(strSql, parameters);
                DataTable dt = ds.Tables[0];
                return Convert.ToDateTime(dt.Rows[0]["LDATE"].ToString());
            }

            public List<TRENDDATAEntity> Find(string categoryid, string starttime, string endtime)
            {
                StringBuilder strWhere = new StringBuilder();
                strWhere.AppendFormat(" CATEGORYID={0}", categoryid);
                if (!string.IsNullOrEmpty(starttime))
                {
                    strWhere.AppendFormat("AND \"DATE\">=to_date('{0}','yyyy-MM-dd')", starttime);
                }
                if (!string.IsNullOrEmpty(endtime))
                {
                    strWhere.AppendFormat("AND \"DATE\"<=to_date('{0}','yyyy-MM-dd')", endtime);
                }
                strWhere.Append("ORDER BY \"DATE\" ASC");
                return Find(strWhere.ToString(), null);
            }

            public override List<TRENDDATAEntity> Find(string strWhere, SqlParameter[] parameters)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select *");
                strSql.Append(" FROM TRENDDATA ");
                if (strWhere.Trim() != "")
                {
                    strSql.Append(" where " + strWhere);
                }

                DataSet ds = _sqlHelper.ExecuteDateSet(strSql.ToString(), parameters);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<TRENDDATAEntity> list = new List<TRENDDATAEntity>();
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        TRENDDATAEntity entity = new TRENDDATAEntity();
                        if (!Convert.IsDBNull(row["CATEGORYID"]))
                        {
                            entity.CATEGORYID = Convert.ToInt64(row["CATEGORYID"]);
                        }
                        if (!Convert.IsDBNull(row["ARTICLECOUNT"]))
                        {
                            entity.ARTICLECOUNT = Convert.ToInt64(row["ARTICLECOUNT"]);
                        }
                        if (!Convert.IsDBNull(row["DATE"]))
                        {
                            entity.DATE = Convert.ToDateTime(row["DATE"]);
                        }
                        if (!Convert.IsDBNull(row["TAG"]))
                        {
                            entity.TAG = Convert.ToInt32(row["TAG"]);
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
                strSql.Append(" FROM TRENDDATA");
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
            /// 

            public int GetPagerRowsCountByCategoryId(long CategoryId)
            {
                string strWhere = " CATEGORYID=@CATEGORYID";
                SqlParameter[] parameters = {
					new SqlParameter("@CATEGORYID",SqlDbType.BigInt)					
					};
                parameters[0].Value = CategoryId;
                return GetPagerRowsCount(strWhere, parameters);
            }

            public int GetPagerRowsCount(string where, SqlParameter[] param)
            {
                string sql = "select count(*) from TRENDDATA ";
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
                PagerSql.Append("FROM (SELECT * FROM TRENDDATA ");
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

