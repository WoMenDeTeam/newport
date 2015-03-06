using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Demo.DAL;

namespace Demo.BLL
{
    public static class RolesFacade
    {
        private static readonly RolesEntity.RolesDAO dao = new RolesEntity.RolesDAO();

        public static IList<RolesEntity> GetAllRole()
        {
            return dao.Find("");
        }

    }
}
