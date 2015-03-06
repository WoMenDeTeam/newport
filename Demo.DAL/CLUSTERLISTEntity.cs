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
    public partial class CLUSTERLISTEntity
    {
        private SqlHelper _sqlHelper;

        #region const fields
        public const string DBName = "Sentiment";
        public const string TableName = "CLUSTERLIST";
        public const string PrimaryKey = "PK_CLUSTERLIST";
        #endregion

        #region columns
        public struct Columns
        {
            public const string ID = "ID";
            public const string CLUSTERNAME = "CLUSTERNAME";
            public const string EDITDATE = "EDITDATE";
            public const string DISTYPE = "DISTYPE";
            public const string PARAM = "PARAM";
        }
        #endregion

        #region constructors
        public CLUSTERLISTEntity()
        {
            _sqlHelper = new SqlHelper(DBName);
        }

        public CLUSTERLISTEntity(int id, string clustername, DateTime editdate, int distype, int param)
        {
            this.ID = id;

            this.CLUSTERNAME = clustername;

            this.EDITDATE = editdate;

            this.DISTYPE = distype;

            this.PARAM = param;

        }
        #endregion

        #region Properties

        public int? ID
        {
            get;
            set;
        }


        public string CLUSTERNAME
        {
            get;
            set;
        }


        public DateTime? EDITDATE
        {
            get;
            set;
        }


        public int? DISTYPE
        {
            get;
            set;
        }


        public int? PARAM
        {
            get;
            set;
        }

        #endregion

        public class CLUSTERLISTDAO : SqlDAO<CLUSTERLISTEntity>
        {
            private SqlHelper _sqlHelper;
            public const string DBName = "SentimentConnStr";

            public CLUSTERLISTDAO()
            {
                _sqlHelper = new SqlHelper(DBName);
            }

            public override void Add(CLUSTERLISTEntity entity)
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into CLUSTERLIST(");
                strSql.Append("CLUSTERNAME,EDITDATE,DISTYPE,PARAM)");
                strSql.Append(" values (");
                strSql.Append("@CLUSTERNAME,@EDITDATE,@DISTYPE,@PARAM)");
                SqlParameter[] parameters = {
					new SqlParameter("@CLUSTERNAME",SqlDbType.NVarChar),
					new SqlParameter("@EDITDATE",SqlDbType.DateTime),
					new SqlParameter("@DISTYPE",SqlDbType.Int),
					new SqlParameter("@PARAM",SqlDbType.Int)
					};
                parameters[0].Value = entity.CLUSTERNAME;
                parameters[1].Value = entity.EDITDATE;
                parameters[2].Value = entity.DISTYPE;
                parameters[3].Value = entity.PARAM;
                _sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            }

            public override void Update(CLUSTERLISTEntity entity)
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("update CLUSTERLIST set ");
                strSql.Append("CLUSTERNAME=@CLUSTERNAME,");
                strSql.Append("EDITDATE=@EDITDATE,");
                strSql.Append("DISTYPE=@DISTYPE,");
                strSql.Append("PARAM=@PARAM");

                strSql.Append(" where ID=@ID");
                SqlParameter[] parameters = {
					new SqlParameter("@CLUSTERNAME",SqlDbType.NVarChar),
					new SqlParameter("@EDITDATE",SqlDbType.DateTime),
					new SqlParameter("@DISTYPE",SqlDbType.Int),
					new SqlParameter("@PARAM",SqlDbType.Int),
					new SqlParameter("@ID",SqlDbType.Int)
				};
                parameters[0].Value = entity.CLUSTERNAME;
                parameters[1].Value = entity.EDITDATE;
                parameters[2].Value = entity.DISTYPE;
                parameters[3].Value = entity.PARAM;
                parameters[4].Value = entity.ID;

                _sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            }

            public bool UpdateSet(int ID, string ColumnName, string Value)
            {
                try
                {
                    StringBuilder StrSql = new StringBuilder();
                    StrSql.Append("update CLUSTERLIST set ");
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
                string strSql = "delete from CLUSTERLIST where ID=" + ID;
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
                string strSql = "delete from CLUSTERLIST where ID in (" + ID + ")";
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

            public override void Delete(CLUSTERLISTEntity entity)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from CLUSTERLIST ");
                strSql.Append(" where ID=@primaryKeyId");
                SqlParameter[] parameters = {
						new SqlParameter("@primaryKeyId", SqlDbType.Int)
					};
                parameters[0].Value = entity.ID;
                _sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            }

            public override CLUSTERLISTEntity FindById(long primaryKeyId)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select * from CLUSTERLIST ");
                strSql.Append(" where ID=@primaryKeyId");
                SqlParameter[] parameters = {
						new SqlParameter("@primaryKeyId", SqlDbType.Int)};
                parameters[0].Value = primaryKeyId;
                DataSet ds = _sqlHelper.ExecuteDateSet(strSql.ToString(), parameters);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    CLUSTERLISTEntity entity = new CLUSTERLISTEntity();
                    if (!Convert.IsDBNull(row["ID"]))
                    {
                        entity.ID = Convert.ToInt32(row["ID"]);
                    }
                    entity.CLUSTERNAME = row["CLUSTERNAME"].ToString();
                    if (!Convert.IsDBNull(row["EDITDATE"]))
                    {
                        entity.EDITDATE = Convert.ToDateTime(row["EDITDATE"]);
                    }
                    if (!Convert.IsDBNull(row["DISTYPE"]))
                    {
                        entity.DISTYPE = Convert.ToInt32(row["DISTYPE"]);
                    }
                    if (!Convert.IsDBNull(row["PARAM"]))
                    {
                        entity.PARAM = Convert.ToInt32(row["PARAM"]);
                    }
                    return entity;
                }
                else
                {
                    return null;
                }
            }

            public override List<CLUSTERLISTEntity> Find(string strWhere, SqlParameter[] parameters)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select *");
                strSql.Append(" FROM CLUSTERLIST ");
                if (strWhere.Trim() != "")
                {
                    strSql.Append(" where " + strWhere);
                }

                DataSet ds = _sqlHelper.ExecuteDateSet(strSql.ToString(), parameters);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<CLUSTERLISTEntity> list = new List<CLUSTERLISTEntity>();
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        CLUSTERLISTEntity entity = new CLUSTERLISTEntity();
                        if (!Convert.IsDBNull(row["ID"]))
                        {
                            entity.ID = Convert.ToInt32(row["ID"]);
                        }
                        entity.CLUSTERNAME = row["CLUSTERNAME"].ToString();
                        if (!Convert.IsDBNull(row["EDITDATE"]))
                        {
                            entity.EDITDATE = Convert.ToDateTime(row["EDITDATE"]);
                        }
                        if (!Convert.IsDBNull(row["DISTYPE"]))
                        {
                            entity.DISTYPE = Convert.ToInt32(row["DISTYPE"]);
                        }
                        if (!Convert.IsDBNull(row["PARAM"]))
                        {
                            entity.PARAM = Convert.ToInt32(row["PARAM"]);
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
                strSql.Append(" FROM CLUSTERLIST");
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

            public int GetPagerRowsCount(string where)
            {
                return GetPagerRowsCount(where, null);
            }

            public int GetPagerRowsCount(string where, SqlParameter[] param)
            {
                string sql = "select count(*) from CLUSTERLIST ";
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
            /// 

            public DataTable GetPager(string where, string orderBy, int pageSize, int pageNumber)
            {
                return GetPager(where, null, orderBy, pageSize, pageNumber);
            }

            public DataTable GetPager(string where, SqlParameter[] param, string orderBy, int pageSize, int pageNumber)
            {
                int startNumber = pageSize * (pageNumber - 1) + 1;
                int endNumber = pageSize * pageNumber;

                if (string.IsNullOrEmpty(orderBy))
                {
                    orderBy = "ID";
                }
                // StringBuilder PagerSql = new StringBuilder();

                string sql = string.Format("SELECT A.* FROM (SELECT ROW_NUMBER() OVER(ORDER BY {0}) AS RN,  * FROM CLUSTERLIST ) A WHERE a.RN  BETWEEN {1} AND {2}", orderBy, startNumber, endNumber);
                //PagerSql.Append("SELECT * FROM (");
                //PagerSql.Append(" SELECT A.*, ROWNUM RN ");
                //PagerSql.Append("FROM (SELECT * FROM CLUSTERLIST ");
                //if (!string.IsNullOrEmpty(where))
                //{
                //    PagerSql.Append(" where " + where);
                //}
                //if (!string.IsNullOrEmpty(orderBy))
                //{
                //    PagerSql.AppendFormat(" ORDER BY {0}", orderBy);
                //}
                //else
                //{

                //    PagerSql.Append(" ORDER BY ID");//默认按主键排序

                //}
                //PagerSql.AppendFormat(" ) A WHERE ROWNUM <= {0})", endNumber);
                //PagerSql.AppendFormat(" WHERE RN >= {0}", startNumber);

                return _sqlHelper.ExecuteDateSet(sql, param).Tables[0];
            }

            #endregion

        }
    }
}

