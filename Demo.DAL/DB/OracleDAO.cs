using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace Demo.DAL
{
    public class OracleDAO : DAO
    {
        public override IDbConnection CreateConnection()
        {
            return new OracleConnection();
        }

        #region parameter type
        protected override IDbDataParameter String
        {
            get { return new OracleParameter(string.Empty, OracleDbType.Varchar2); }
        }
        protected override IDbDataParameter TinyInt
        {
            get { return new OracleParameter(string.Empty, OracleDbType.Byte); }
        }
        protected override IDbDataParameter SmallInt
        {
            get { return new OracleParameter(string.Empty, OracleDbType.Int16); }
        }
        protected override IDbDataParameter Int
        {
            get { return new OracleParameter(string.Empty, OracleDbType.Int32); }
        }
        protected override IDbDataParameter BigInt
        {
            get { return new OracleParameter(string.Empty, OracleDbType.Long); }
        }
        protected override IDbDataParameter Float
        {
            get { return new OracleParameter(string.Empty, OracleDbType.Double); }
        }
        protected override IDbDataParameter DateTime
        {
            get { return new OracleParameter(string.Empty, OracleDbType.Date); }
        }
        protected override IDbDataParameter Bool
        {
            get { return new OracleParameter(string.Empty, OracleDbType.Byte); }
        }
        protected override IDbDataParameter Text
        {
            get { return new OracleParameter(string.Empty, OracleDbType.NClob); }
        }
        protected override IDbDataParameter Image
        {
            get { return new OracleParameter(string.Empty, OracleDbType.Blob); }
        }
        protected override IDbDataParameter GUID
        {
            get { return new OracleParameter(string.Empty, OracleDbType.Raw); }
        }
        #endregion

    }
}
