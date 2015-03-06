using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Oracle.DataAccess.Client;

namespace Demo.DAL
{
    public interface OracleIDAO<T>
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        T FindById(long id);
        List<T> Find(string strWhere, OracleParameter[] parameters);
        DataSet GetDataSet(string strWhere, OracleParameter[] parameters);
    }

    public class OracleDAO<T> : OracleIDAO<T>
    {
        #region IDAO<T> Members

        public virtual void Add(T entity)
        {
            throw new NotImplementedException();
        }


        public virtual void Update(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual T FindById(long id)
        {
            throw new NotImplementedException();
        }

        public virtual List<T> Find(string strWhere, OracleParameter[] parameters)
        {
            throw new NotImplementedException();
        }
        public virtual List<T> Find(string strWhere, OracleParameter[] parameters, int top, string orderby)
        {
            throw new NotImplementedException();
        }
        public virtual DataSet GetDataSet(string strWhere, OracleParameter[] parameters)
        {
            throw new NotImplementedException();
        }
        public virtual object FindByWhere(string StrWhere)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

}