using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Web;
using Demo.Util;

namespace Demo.BLL
{
    public static class GetCategoryList
    {
        private static XmlDocument xmldoc = null;

        private static void LoadXml() {
            XmlDocument newxmldoc = new XmlDocument();
            string path = System.Web.HttpContext.Current.Server.MapPath("~/config/RoleCategory.xml");
            newxmldoc.Load(path);
            xmldoc = newxmldoc;
        }

        public static Dictionary<string, string> GetList(string roleid) {
            if (xmldoc == null) {
                LoadXml();
            }
            XmlNodeList nodelist = xmldoc.SelectNodes("//list[@roleid='" + roleid + "']/category");
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (nodelist != null && nodelist.Count > 0) {
                foreach (XmlNode node in nodelist) {
                    string categoryname = node.InnerText;
                    string categoryid = node.Attributes["id"].InnerText;
                    if (!dict.ContainsKey(categoryid)) {
                        dict.Add(categoryid, categoryname);
                    }
                }
            }
            return dict;
        }
    }
}
