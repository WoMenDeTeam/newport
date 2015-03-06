using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Oracle.DataAccess.Client;
using System.Data;
using Demo.Util;

namespace Demo.DAL
{
    public class OracleHelper
    {
        private string connectionString;

        public string ConnectionString
        {
            get { return connectionString; }
        }

        public OracleHelper(string connKey)
        {
            string connString = ConfigUtil.GetConnStr(connKey);
            if (connString != null)
            {
                connectionString = DESEncrypt.Decrypt(connString);
            }
        }


        private void PrepareCommand(OracleCommand cmd, OracleConnection conn, OracleTransaction trans, string cmdText, OracleParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
            {
                cmd.Transaction = trans;
            }

            cmd.CommandType = CommandType.Text;//cmdType;

            if (cmdParms != null)
            {
                OracleParameter[] clonedParameters = new OracleParameter[cmdParms.Length];

                for (int i = 0, j = cmdParms.Length; i < j; i++)
                {
                    clonedParameters[i] = (OracleParameter)((ICloneable)cmdParms[i]).Clone();
                }

                foreach (OracleParameter parameter in clonedParameters)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }

                    cmd.Parameters.Add(parameter);
                }
            }
        }

        /// <summary>
        /// 执行SQL语句，返回影响的记录�?
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public int ExecuteSql(string SQLString, params OracleParameter[] cmdParms)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                using (OracleCommand cmd = new OracleCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        return cmd.ExecuteNonQuery();
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
        }

        public int ExecuteSql(string SQLString)
        {
            return this.ExecuteSql(SQLString, null);
        }

        
        public object GetSingle(string SQLString, params OracleParameter[] cmdParms)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                using (OracleCommand cmd = new OracleCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        object obj = cmd.ExecuteScalar();

                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
        }

        public object GetSingle(string SQLString)
        {
            return this.GetSingle(SQLString, null);
        }

        /// <summary>
        /// 执行查询语句，返回SqlDataReader ( 注意：使用后一定要对SqlDataReader进行Close )
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>SqlDataReader</returns>
        public OracleDataReader ExecuteReader(string SQLString, params OracleParameter[] cmdParms)
        {
            OracleConnection connection = new OracleConnection(connectionString);
            OracleCommand cmd = new OracleCommand();
            try
            {
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                return cmd.ExecuteReader();
            }
            catch
            {
                throw;
            }
        }

        public OracleDataReader ExecuteReader(string SQLString)
        {
            return this.ExecuteReader(SQLString, null);
        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public DataSet ExecuteDateSet(string SQLString, params OracleParameter[] cmdParms)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                using (OracleCommand cmd = new OracleCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);

                        using (OracleDataAdapter da = new OracleDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            da.Fill(ds, "ds");
                            return ds;
                        }
                    }
                    catch
                    {
                        throw;
                    }
                }

            }
        }

        public DataSet ExecuteDateSet(string SQLString)
        {
            return this.ExecuteDateSet(SQLString, null);
        }

        public object ExecuteScalar(CommandType cmdType, string cmdText, params OracleParameter[] commandParameters)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {

                using (OracleCommand cmd = new OracleCommand())
                {
                    PrepareCommand(cmd, connection, null, cmdText, commandParameters);
                    object val = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    return val;
                }
            }
        }

        public object ExecuteScalar(CommandType cmdType, string cmdText)
        {
            return this.ExecuteScalar(cmdType, cmdText, null);
        }

        public DataTable GetTableByTop(int top, string TableName, string StrWhere, string OrderBy)
        {
            string StrSql = "select top " + top + " * from " + TableName;
            if (!string.IsNullOrEmpty(StrWhere))
            {
                StrSql += " where " + StrWhere;
            }
            if (!string.IsNullOrEmpty(OrderBy))
            {
                StrSql += " order by " + OrderBy;
            }
            DataSet ds = ExecuteDateSet(StrSql, null);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }

    }
}