using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IdolACINet;
using System.Xml;
using System.Configuration;

namespace Demo.BLL
{
    public class IdolHelp
    {
        public static long GetLastStamp(string jobName)
        {
            string hostname = ConfigurationManager.AppSettings["IdolHttp"];
            //excute command to show jobs
            Command cmd = new Command("ClusterShowJobs");
            Connection conn = new Connection(hostname, 9000);
            Response r = conn.Execute(cmd);
            XmlDocument xdJobs = r.Data;

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xdJobs.NameTable);
            nsmgr.AddNamespace("autn", @"http://schemas.autonomy.com/aci/");

            string format = "//autn:cluster/autn:name[text()='{0}']/..//autn:timestamp[last()]";
            string xPathStamp = string.Format(format, jobName);
            string strStamp = xdJobs.SelectSingleNode(xPathStamp, nsmgr).InnerText;
            return long.Parse(strStamp);
        }
    }
}
