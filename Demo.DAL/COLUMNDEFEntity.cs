using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using log4net;

namespace Demo.DAL
{
    [Serializable]
    public partial class COLUMNDEFEntity
    {
        private SqlHelper _sqlHelper;

        #region const fields
        public const string DBName = "Sentiment";
        public const string TableName = "COLUMNDEF";
        public const string PrimaryKey = "PK_COLUNMNDB";
        #endregion

        #region columns
        public struct Columns
        {
            public const string ID = "ID";
            public const string COLUMNNAME = "COLUMNNAME";
            public const string COLUMNDES = "COLUMNDES";
            public const string COLUMNSTATUS = "COLUMNSTATUS";
            public const string COLUMNORDER = "COLUMNORDER";
            public const string COLUMNPUBLISH = "COLUMNPUBLISH";
            public const string COLUMNNAMERULE = "COLUMNNAMERULE";
            public const string COLUMNIMGPATH = "COLUMNIMGPATH";
            public const string COLUMNTELEMPLATEPATH = "COLUMNTELEMPLATEPATH";
            public const string COLUMNCONTENT = "COLUMNCONTENT";
            public const string ISDIS = "ISDIS";
            public const string PARENTID = "PARENTID";
            public const string SITEID = "SITEID";
        }
        #endregion

        #region constructors
        public COLUMNDEFEntity()
        {
            _sqlHelper = new SqlHelper(DBName);
        }

        public COLUMNDEFEntity(int id, string columnname, string columndes, int columnstatus, int columnorder, string columnpublish, string columnnamerule, string columnimgpath, string columntelemplatepath, string columncontent, int isdis, int parentid, int siteid)
        {
            this.ID = id;

            this.COLUMNNAME = columnname;

            this.COLUMNDES = columndes;

            this.COLUMNSTATUS = columnstatus;

            this.COLUMNORDER = columnorder;

            this.COLUMNPUBLISH = columnpublish;

            this.COLUMNNAMERULE = columnnamerule;

            this.COLUMNIMGPATH = columnimgpath;

            this.COLUMNTELEMPLATEPATH = columntelemplatepath;

            this.COLUMNCONTENT = columncontent;

            this.ISDIS = isdis;

            this.PARENTID = parentid;

            this.SITEID = siteid;

        }
        #endregion

        #region Properties

        public int? ID
        {
            get;
            set;
        }


        public string COLUMNNAME
        {
            get;
            set;
        }


        public string COLUMNDES
        {
            get;
            set;
        }


        public int? COLUMNSTATUS
        {
            get;
            set;
        }


        public int? COLUMNORDER
        {
            get;
            set;
        }


        public string COLUMNPUBLISH
        {
            get;
            set;
        }


        public string COLUMNNAMERULE
        {
            get;
            set;
        }


        public string COLUMNIMGPATH
        {
            get;
            set;
        }


        public string COLUMNTELEMPLATEPATH
        {
            get;
            set;
        }


        public string COLUMNCONTENT
        {
            get;
            set;
        }


        public int? ISDIS
        {
            get;
            set;
        }


        public int? PARENTID
        {
            get;
            set;
        }


        public int? SITEID
        {
            get;
            set;
        }

        #endregion

        public class COLUMNDEFDAO : SqlDAO<COLUMNDEFEntity>
        {
            private SqlHelper _sqlHelper;
            public const string DBName = "SentimentConnStr";

            public COLUMNDEFDAO()
            {
                _sqlHelper = new SqlHelper(DBName);
            }

            public override void Add(COLUMNDEFEntity entity)
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into COLUMNDEF(");
                strSql.Append("COLUMNNAME,COLUMNDES,COLUMNSTATUS,COLUMNORDER,COLUMNPUBLISH,COLUMNNAMERULE,COLUMNIMGPATH,COLUMNTELEMPLATEPATH,COLUMNCONTENT,ISDIS,PARENTID,SITEID)");
                strSql.Append(" values (");
                strSql.Append("@COLUMNNAME,@COLUMNDES,@COLUMNSTATUS,@COLUMNORDER,@COLUMNPUBLISH,@COLUMNNAMERULE,@COLUMNIMGPATH,@COLUMNTELEMPLATEPATH,@COLUMNCONTENT,@ISDIS,@PARENTID,@SITEID)");
                SqlParameter[] parameters = {
					new SqlParameter("@COLUMNNAME",SqlDbType.NVarChar),
					new SqlParameter("@COLUMNDES",SqlDbType.NVarChar),
					new SqlParameter("@COLUMNSTATUS",SqlDbType.Int),
					new SqlParameter("@COLUMNORDER",SqlDbType.Int),
					new SqlParameter("@COLUMNPUBLISH",SqlDbType.VarChar),
					new SqlParameter("@COLUMNNAMERULE",SqlDbType.VarChar),
					new SqlParameter("@COLUMNIMGPATH",SqlDbType.VarChar),
					new SqlParameter("@COLUMNTELEMPLATEPATH",SqlDbType.VarChar),
					new SqlParameter("@COLUMNCONTENT",SqlDbType.NText),
					new SqlParameter("@ISDIS",SqlDbType.Int),
					new SqlParameter("@PARENTID",SqlDbType.Int),
					new SqlParameter("@SITEID",SqlDbType.Int)
					};
                parameters[0].Value = entity.COLUMNNAME;
                parameters[1].Value = entity.COLUMNDES;
                parameters[2].Value = entity.COLUMNSTATUS;
                parameters[3].Value = entity.COLUMNORDER;
                parameters[4].Value = entity.COLUMNPUBLISH;
                parameters[5].Value = entity.COLUMNNAMERULE;
                parameters[6].Value = entity.COLUMNIMGPATH;
                parameters[7].Value = entity.COLUMNTELEMPLATEPATH;
                parameters[8].Value = entity.COLUMNCONTENT;
                parameters[9].Value = entity.ISDIS;
                parameters[10].Value = entity.PARENTID;
                parameters[11].Value = entity.SITEID;
                _sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            }

            public override void Update(COLUMNDEFEntity entity)
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("update COLUMNDEF set ");
                strSql.Append("COLUMNNAME=@COLUMNNAME,");
                strSql.Append("COLUMNDES=@COLUMNDES,");
                strSql.Append("COLUMNSTATUS=@COLUMNSTATUS,");
                strSql.Append("COLUMNORDER=@COLUMNORDER,");
                strSql.Append("COLUMNPUBLISH=@COLUMNPUBLISH,");
                strSql.Append("COLUMNNAMERULE=@COLUMNNAMERULE,");
                strSql.Append("COLUMNIMGPATH=@COLUMNIMGPATH,");
                strSql.Append("COLUMNTELEMPLATEPATH=@COLUMNTELEMPLATEPATH,");
                strSql.Append("COLUMNCONTENT=@COLUMNCONTENT,");
                strSql.Append("ISDIS=@ISDIS,");
                strSql.Append("PARENTID=@PARENTID,");
                strSql.Append("SITEID=@SITEID");

                strSql.Append(" where ID=@ID");
                SqlParameter[] parameters = {
					new SqlParameter("@COLUMNNAME",SqlDbType.NVarChar),
					new SqlParameter("@COLUMNDES",SqlDbType.NVarChar),
					new SqlParameter("@COLUMNSTATUS",SqlDbType.Int),
					new SqlParameter("@COLUMNORDER",SqlDbType.Int),
					new SqlParameter("@COLUMNPUBLISH",SqlDbType.VarChar),
					new SqlParameter("@COLUMNNAMERULE",SqlDbType.VarChar),
					new SqlParameter("@COLUMNIMGPATH",SqlDbType.VarChar),
					new SqlParameter("@COLUMNTELEMPLATEPATH",SqlDbType.VarChar),
					new SqlParameter("@COLUMNCONTENT",SqlDbType.NText),
					new SqlParameter("@ISDIS",SqlDbType.Int),
					new SqlParameter("@PARENTID",SqlDbType.Int),
					new SqlParameter("@SITEID",SqlDbType.Int),
					new SqlParameter("@ID",SqlDbType.Int)
				};
                parameters[0].Value = entity.COLUMNNAME;
                parameters[1].Value = entity.COLUMNDES;
                parameters[2].Value = entity.COLUMNSTATUS;
                parameters[3].Value = entity.COLUMNORDER;
                parameters[4].Value = entity.COLUMNPUBLISH;
                parameters[5].Value = entity.COLUMNNAMERULE;
                parameters[6].Value = entity.COLUMNIMGPATH;
                parameters[7].Value = entity.COLUMNTELEMPLATEPATH;
                parameters[8].Value = entity.COLUMNCONTENT;
                parameters[9].Value = entity.ISDIS;
                parameters[10].Value = entity.PARENTID;
                parameters[11].Value = entity.SITEID;
                parameters[12].Value = entity.ID;

                _sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            }

            public bool UpdateSet(int ID, string ColumnName, string Value)
            {
                try
                {
                    StringBuilder StrSql = new StringBuilder();
                    StrSql.Append("update COLUMNDEF set ");
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
                string strSql = "delete from COLUMNDEF where ID=" + ID;
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
                string strSql = "delete from COLUMNDEF where ID in (" + ID + ")";
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

            public override void Delete(COLUMNDEFEntity entity)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from COLUMNDEF ");
                strSql.Append(" where ID=@primaryKeyId");
                SqlParameter[] parameters = {
						new SqlParameter("@primaryKeyId", SqlDbType.Int)
					};
                parameters[0].Value = entity.ID;
                _sqlHelper.ExecuteSql(strSql.ToString(), parameters);
            }

            public override COLUMNDEFEntity FindById(long primaryKeyId)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select * from COLUMNDEF ");
                strSql.Append(" where ID=@primaryKeyId");
                SqlParameter[] parameters = {
						new SqlParameter("@primaryKeyId", SqlDbType.Int)};
                parameters[0].Value = primaryKeyId;
                DataSet ds = _sqlHelper.ExecuteDateSet(strSql.ToString(), parameters);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    COLUMNDEFEntity entity = new COLUMNDEFEntity();
                    if (!Convert.IsDBNull(row["ID"]))
                    {
                        entity.ID = Convert.ToInt32(row["ID"]);
                    }
                    entity.COLUMNNAME = row["COLUMNNAME"].ToString();
                    entity.COLUMNDES = row["COLUMNDES"].ToString();
                    if (!Convert.IsDBNull(row["COLUMNSTATUS"]))
                    {
                        entity.COLUMNSTATUS = Convert.ToInt32(row["COLUMNSTATUS"]);
                    }
                    if (!Convert.IsDBNull(row["COLUMNORDER"]))
                    {
                        entity.COLUMNORDER = Convert.ToInt32(row["COLUMNORDER"]);
                    }
                    entity.COLUMNPUBLISH = row["COLUMNPUBLISH"].ToString();
                    entity.COLUMNNAMERULE = row["COLUMNNAMERULE"].ToString();
                    entity.COLUMNIMGPATH = row["COLUMNIMGPATH"].ToString();
                    entity.COLUMNTELEMPLATEPATH = row["COLUMNTELEMPLATEPATH"].ToString();
                    entity.COLUMNCONTENT = row["COLUMNCONTENT"].ToString();
                    if (!Convert.IsDBNull(row["ISDIS"]))
                    {
                        entity.ISDIS = Convert.ToInt32(row["ISDIS"]);
                    }
                    if (!Convert.IsDBNull(row["PARENTID"]))
                    {
                        entity.PARENTID = Convert.ToInt32(row["PARENTID"]);
                    }
                    if (!Convert.IsDBNull(row["SITEID"]))
                    {
                        entity.SITEID = Convert.ToInt32(row["SITEID"]);
                    }
                    return entity;
                }
                else
                {
                    return null;
                }
            }

            public List<COLUMNDEFEntity> Find(string strWhere)
            {
                return Find(strWhere, null);
            }

            public override List<COLUMNDEFEntity> Find(string strWhere, SqlParameter[] parameters)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select *");
                strSql.Append(" FROM COLUMNDEF ");
                if (strWhere.Trim() != "")
                {
                    strSql.Append(" where " + strWhere);
                }

                DataSet ds = _sqlHelper.ExecuteDateSet(strSql.ToString(), parameters);
                if (ds != null && ds.Tables.Count > 0)
                {
                    List<COLUMNDEFEntity> list = new List<COLUMNDEFEntity>();
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        COLUMNDEFEntity entity = new COLUMNDEFEntity();
                        if (!Convert.IsDBNull(row["ID"]))
                        {
                            entity.ID = Convert.ToInt32(row["ID"]);
                        }
                        entity.COLUMNNAME = row["COLUMNNAME"].ToString();
                        entity.COLUMNDES = row["COLUMNDES"].ToString();
                        if (!Convert.IsDBNull(row["COLUMNSTATUS"]))
                        {
                            entity.COLUMNSTATUS = Convert.ToInt32(row["COLUMNSTATUS"]);
                        }
                        if (!Convert.IsDBNull(row["COLUMNORDER"]))
                        {
                            entity.COLUMNORDER = Convert.ToInt32(row["COLUMNORDER"]);
                        }
                        entity.COLUMNPUBLISH = row["COLUMNPUBLISH"].ToString();
                        entity.COLUMNNAMERULE = row["COLUMNNAMERULE"].ToString();
                        entity.COLUMNIMGPATH = row["COLUMNIMGPATH"].ToString();
                        entity.COLUMNTELEMPLATEPATH = row["COLUMNTELEMPLATEPATH"].ToString();
                        entity.COLUMNCONTENT = row["COLUMNCONTENT"].ToString();
                        if (!Convert.IsDBNull(row["ISDIS"]))
                        {
                            entity.ISDIS = Convert.ToInt32(row["ISDIS"]);
                        }
                        if (!Convert.IsDBNull(row["PARENTID"]))
                        {
                            entity.PARENTID = Convert.ToInt32(row["PARENTID"]);
                        }
                        if (!Convert.IsDBNull(row["SITEID"]))
                        {
                            entity.SITEID = Convert.ToInt32(row["SITEID"]);
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
                strSql.Append(" FROM COLUMNDEF");
                if (strWhere.Trim() != "")
                {
                    strSql.Append(" where " + strWhere);
                }
                return _sqlHelper.ExecuteDateSet(strSql.ToString(), param);
            }

            public DataTable GetPagerDT(int pageNumber, string strwhere, int pageSize, string strorder)
            {
                int startNumber = pageSize * (pageNumber - 1) + 1;
                int endNumber = pageSize * pageNumber;
                StringBuilder strSql = new StringBuilder();
                strSql.Append("SELECT * FROM ( SELECT C.*, ROWNUM RN FROM");
                strSql.Append(" (select A.CATEGORYID,A.PARENTCATE,A.EVENTDATE,A.ID AS ID,B.ID AS COLUMNID,A.CATEGORYNAME,A.CATEDISPLAY");
                strSql.Append(" from CATEGORY A,COLUMNDEF B WHERE A.CATEGORYNAME = B.COLUMNNAME");                
                if (!string.IsNullOrEmpty(strwhere)) {
                    strSql.Append(" AND ").Append(strwhere);
                }
                strSql.AppendFormat(" ORDER BY B.COLUMNORDER ");
                if (!string.IsNullOrEmpty(strorder)) {
                    strSql.Append(",").Append(strorder);
                }
                strSql.AppendFormat(") C WHERE ROWNUM >= {0} ", startNumber);
                strSql.AppendFormat(" AND ROWNUM <= {0})", endNumber);
                
                DataSet ds = _sqlHelper.ExecuteDateSet(strSql.ToString(), null);
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
                string sql = "select count(*) from COLUMNDEF ";
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
                PagerSql.Append("FROM (SELECT * FROM COLUMNDEF ");
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

