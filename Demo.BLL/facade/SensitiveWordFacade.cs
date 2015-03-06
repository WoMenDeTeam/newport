using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Demo.DAL;

namespace Demo.BLL
{
    public static class SensitiveWordFacade
    {

        private static SENSITIVEWORDSEntity.SENSITIVEWORDSDAO dao = new SENSITIVEWORDSEntity.SENSITIVEWORDSDAO();

        public static int AddEntity(SENSITIVEWORDSEntity entity)
        {
            return dao.AddEntity(entity);
        }

        public static void UpdateSet(int ID, string ColumnName, string value) {
            dao.UpdateSet(ID, ColumnName, value);
        }

        public static void Delete(int ID) {
            dao.Delete(ID);
        }

        public static IList<SENSITIVEWORDSEntity> GetList(string strwhere)
        {
            return dao.Find(strwhere);
        }
    }
}
