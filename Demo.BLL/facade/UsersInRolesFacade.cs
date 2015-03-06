using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Demo.DAL;
using System.Data;


namespace Demo.BLL
{
    public class UsersInRolesFacade
    {
        private static readonly UsersInRolesEntity.UsersInRolesDAO dao = new UsersInRolesEntity.UsersInRolesDAO();

        public static bool DeleteUserFromUserInRoles(int userid, int roleid)
        {
            return dao.DeletebyRoleIDAndUserId(userid, roleid);
        }

        public static DataTable GetTreeDt() {
            return dao.GetTreeDT();
        }

        public static string GetTreeStr() {
            DataTable dt = GetTreeDt();
            Dictionary<int, string> roledict = new Dictionary<int, string>();
            Dictionary<int, IList<TreeUser>> userdict = new Dictionary<int, IList<TreeUser>>();
            foreach (DataRow row in dt.Rows) {
                int roleid = Convert.ToInt32(row["ROLEID"]);
                string rolename = row["ROLENAME"].ToString();
                int userid = Convert.ToInt32(row["USERID"]);
                string username = row["USERNAME"].ToString();
                if (!roledict.ContainsKey(roleid)) {
                    roledict.Add(roleid, rolename);
                }
                if (!userdict.ContainsKey(roleid))
                {
                    IList<TreeUser> treeUserList = new List<TreeUser>();
                    TreeUser entity = new TreeUser();
                    entity.userid = userid;
                    entity.userName = username;
                    treeUserList.Add(entity);
                    userdict.Add(roleid, treeUserList);
                }
                else { 
                    TreeUser entity = new TreeUser();
                    entity.userid = userid;
                    entity.userName = username;
                    if (!userdict[roleid].Contains(entity)) {
                        userdict[roleid].Add(entity);
                    }
                }
            }
            StringBuilder treestr = new StringBuilder();
            if (roledict.Count > 0 && userdict.Count > 0) {
                treestr.Append("[");
                foreach (int key in roledict.Keys) {
                    treestr.Append("{");
                    treestr.AppendFormat("\"name\":\"{0}\",", roledict[key]);
                    treestr.AppendFormat("\"id\":\"{0}\",", key);
                    if (userdict.ContainsKey(key))
                    {
                        getchildstr(userdict[key], ref treestr);
                    }
                    treestr.Append("\"successcode\":1},");
                }
                treestr.Append("]");
            }
            string backstr = treestr.ToString();
            backstr = backstr.Replace(",\"successcode\":1", "");
            //backstr = backstr.Replace(",nodes:[]", "");
            backstr = backstr.Replace(",]", "]");
            return backstr;            
        }

        private static void getchildstr(IList<TreeUser> list, ref StringBuilder treestr)
        {            
            treestr.Append("nodes:[");
            foreach (TreeUser entity in list)
            {
                
                treestr.Append("{");
                treestr.AppendFormat("\"name\":\"{0}\",", entity.userName);
                treestr.AppendFormat("\"id\":\"{0}\",", entity.userid);               
                treestr.Append("\"successcode\":1},");
                
            }
            treestr.Append("],");
        }

        public static string GetUserRoleIdList(int userId)
        {
            string strWhere = " USERID=" + userId.ToString();
            IList<UsersInRolesEntity> list = dao.Find(strWhere);
            StringBuilder roleIdList = new StringBuilder();
            foreach (UsersInRolesEntity entity in list)
            {
                if (roleIdList.Length > 0)
                {
                    roleIdList.Append(",");
                }
                roleIdList.Append(entity.ROLEID);
            }
            return roleIdList.ToString();            
        }

        public static bool IsLeader(int userid)
        {
            string strWhere = " USERID=" + userid.ToString();
            IList<UsersInRolesEntity> list = dao.Find(strWhere);
            StringBuilder roleIdList = new StringBuilder();
            foreach (UsersInRolesEntity entity in list)
            {
                if (entity.ROLEID == 24 || entity.ROLEID == 44 || entity.ROLEID == 64)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool DeleteByUserId(int userId)
        {
            return dao.DeletebyUserID(userId);
        }

        public static void Add(UsersInRolesEntity entity)
        {
            dao.Add(entity);
        }
    }

    public class TreeUser
    {
        public int userid
        {
            set;
            get;
        }

        public string userName
        {
            set;
            get;
        }
    }
}
