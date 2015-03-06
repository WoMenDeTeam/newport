<%@ WebHandler Language="C#" Class="GetClusterResults" %>

using System;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using System.Xml;
using System.Configuration;

using IdolACINet;

public class GetClusterResults : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        string clusterId = context.Request.QueryString["cluster_id"];
        string endDate = context.Request.QueryString["end_date"];
        string jobName = context.Request.QueryString["job_name"];
        if (string.IsNullOrEmpty(jobName))
        {
            jobName = ConfigurationManager.AppSettings["HotJobName"];
        }
        Command query = Command.ClusterResults;
        query.SetParam("SourceJobName", jobName);
        query.SetParam(QueryParams.Cluster, clusterId);
        query.SetParam("DREOutputEncoding", "utf8");
        query.SetParam("NumResults", 50);
        query.SetParam("MaxTerms", 0);
        query.SetParam("EndDate", endDate);
        Connection cnn = new Connection(ConfigurationManager.AppSettings["IdolHttp"], 9000);
        Response result = cnn.Execute(query);
        XmlDocument contentDoc = result.Data;

        //Create an XmlNamespaceManager for resolving namespaces.
        XmlNamespaceManager nsmgr = new XmlNamespaceManager(contentDoc.NameTable);
        nsmgr.AddNamespace("autn", "http://schemas.autonomy.com/aci/");

        //Select the book node with the matching attribute value.
        XmlNodeList clusterDocs = contentDoc.SelectNodes("autnresponse/responsedata/autn:clusters/autn:cluster/autn:docs/autn:doc", nsmgr);

        StringBuilder html = new StringBuilder();
        string title;
        string reference;
        int NodeCount = clusterDocs.Count;
        //if (NodeCount > 10)
        //{
        //    NodeCount = 10;
        //}
        int nodeNumber = 0;
        //   foreach (XmlNode clusterDoc in clusterDocs)
        for (int index = 0; index < NodeCount && nodeNumber < 15; index++)
        {
            XmlNode clusterDoc = clusterDocs[index];
            XmlNode titleNode = clusterDoc.SelectSingleNode("autn:title", nsmgr);
            if (null == titleNode)
            {
                continue;
            }

            title = titleNode.InnerText;
            string distitle = title.Length > 35 ? title.Substring(0, 35) + "..." : title;
            reference = clusterDoc.SelectSingleNode("autn:ref", nsmgr).InnerText;
            html.Append("<li>");
            html.Append("<a target=\"_blank\" name=\"look_info_snapshot\" title=\"").Append(title).Append("\" href=\"").Append(reference).Append("\">").Append(distitle).Append("</a>");
            html.Append("</li>");

            nodeNumber++;
        }
        context.Response.Write(html.ToString());
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}