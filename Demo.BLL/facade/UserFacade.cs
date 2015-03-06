using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Demo.DAL;
using System.Configuration;
using System.Web;
using System.Data;

namespace Demo.BLL
{
    public static class UsersFacade
    {
        private static readonly UsersEntity.UsersDAO dao = new UsersEntity.UsersDAO();
        private static string SessionKey = ConfigurationManager.AppSettings["SessionKey"].ToString();
        public static IList<UsersEntity> GetUserList(string idlist, bool tag)
        {
            return dao.GetUsersList(idlist, tag);
        }

        public static DataTable GetUserByRoleId(int roleid)
        {
            string strSql = "select A.*,B.* from USERS A,USERSINROLES B where A.USERID = B.USERID AND B.ROLEID=" + roleid.ToString();
            return dao.GetUserInnerRoleDataTable(strSql);
        }

        public static UsersEntity GetUser(string userName, string passWord)
        {
            IList<UsersEntity> list = dao.GetUser(userName, passWord);
            if (list.Count > 0)
            {
                return list[0];
            }
            else
            {
                return null;
            }
        }

        public static UsersEntity GetUserByUserName(string userName, string passWord) {
            IList<UsersEntity> list = dao.GetUserByUserName(userName, passWord);
            if (list.Count > 0)
            {
                return list[0];
            }
            else
            {
                return null;
            }
        }

        public static UsersEntity GetUser(string userName)
        {
            IList<UsersEntity> list = dao.GetUser(userName);
            if (list.Count > 0)
            {
                return list[0];
            }
            else
            {
                return null;
            }
        }

        public static string GetUserName(LoginUser user)
        {
            if (user != null)
            {
                return user.UsersEntity.USERNAME;
            }
            else
            {
                return null;
            }
        }

        public static string GetUserRoleList(LoginUser user)
        {
            if (user != null)
            {
                return user.RoleIDStr;
            }
            else
            {
                return null;
            }
        }

        public static DataTable GetUserRoleDataTable(string strWhere)
        {
            string strSql = "select * from USERS where USERID > 0";
            if (!string.IsNullOrEmpty(strWhere))
            {
                strSql += strWhere;
            }
            return dao.GetUserInnerRoleDataTable(strSql);
        }

        public static bool DeleteUserFromUserInRoles(int userid, int roleid)
        {
            return UsersInRolesFacade.DeleteUserFromUserInRoles(userid, roleid);
        }

        public static int GetUserCountByStrWhere(string strWhere)
        {
            int count = 0;
            DataTable dt = GetUserRoleDataTable(strWhere);
            if (dt.Rows.Count > 0)
            {
                count = dt.Rows.Count;
            }
            return count;
        }

        public static bool DeleteUser(int userid)
        {
            return dao.Delete(userid);
        }

        //public static int CreateUser(UsersEntity entity)
        //{
        //    return dao.CreateUser(entity);
        //}

        public static UsersEntity GetUser(int userid)
        {
            return dao.FindById(userid);
        }

        public static void UpdateUser(UsersEntity entity)
        {
            dao.Update(entity);
        }
    }

    public class LoginUser
    {
        public LoginUser() { }

        public UsersEntity UsersEntity
        {
            get;
            set;
        }

        public string RoleIDStr
        {
            set;
            get;
        }
    }
}
