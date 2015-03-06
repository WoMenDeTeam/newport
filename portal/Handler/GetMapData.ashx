<%@ WebHandler Language="C#" Class="GetMapData" %>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using System.Xml;
using System.Configuration;
using IdolACINet;

public class GetMapData : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        #region "以前的代码"
        double scalSize = Convert.ToDouble(context.Request["scal_size"]);
        double heightSize = Convert.ToDouble(context.Request["height_size"]);
        string endDate = context.Request["end_date"];
        string jobName = context.Request["job_name"];
        string type = context.Request["type"];
        if (string.IsNullOrEmpty(type))
        {
            type = "news";
        }
        if (string.IsNullOrEmpty(jobName))
        {
            jobName = ConfigurationManager.AppSettings["HotJobName"];
        }
        Command query = Command.ClusterResults;
        query.SetParam("SourceJobName", jobName);
        query.SetParam("NumResults", 0);
        query.SetParam("MaxTerms", 0);
        query.SetParam("EndDate", endDate);
        Connection cnn = new Connection(ConfigurationManager.AppSettings["IdolHttp"], 9000);
        Response result = cnn.Execute(query);
        XmlDocument contentDoc = result.Data;


        //Create an XmlNamespaceManager for resolving namespaces.
        XmlNamespaceManager nsmgr = new XmlNamespaceManager(contentDoc.NameTable);
        nsmgr.AddNamespace("autn", "http://schemas.autonomy.com/aci/");


        //Select the book node with the matching attribute value.
        XmlNodeList clusters = contentDoc.SelectNodes("autnresponse/responsedata/autn:clusters/autn:cluster", nsmgr);

        string nodeId;
        int nodeTop;
        int nodeLeft;
        string nodeTitleId;
        string nodeTitle;

        int clusterNum = 0;
        string numDoc;
        StringBuilder html = new StringBuilder();

        foreach (XmlNode cluster in clusters)
        {
            nodeId = "clusternode_" + type + "_" + clusterNum.ToString();
            nodeTitleId = "clustertitle_" + type + "_" + clusterNum.ToString();
            nodeTitle = cluster.SelectSingleNode("autn:title", nsmgr).InnerText;
            nodeLeft = (int)Math.Ceiling(int.Parse(cluster.SelectSingleNode("autn:x_coord", nsmgr).InnerText) * scalSize);
            nodeTop = (int)Math.Ceiling(int.Parse(cluster.SelectSingleNode("autn:y_coord", nsmgr).InnerText) * heightSize);
            numDoc = cluster.SelectSingleNode("autn:docs/autn:totaldocs", nsmgr).InnerText;

            html.AppendFormat("<div class=\"node\" id=\"{0}\" style=\"border: 1px solid rgb(0, 0, 0); position: absolute;  background-color: rgb(255, 0, 0); font-size: 1px; width: 8px; height: 8px; visibility: visible; top: {1}px; left: {2}px; cursor:pointer;\" ></div>", nodeId, nodeTop, nodeLeft);
            html.AppendFormat("<div class=\"node_text\" id=\"{0}\" style=\"position: absolute; z-index:3;  top: {1}px; left: {2}px;\" >", nodeTitleId, nodeTop - 5, nodeLeft);
            html.Append("<table cellpadding=\"3\"><tr>");
            html.Append("<td nowrap=\"nowrap\" style=\"background-color:#FFFF00;color:#000000;border:solid #000000 1px;font-size:9pt;font-family:sans-serif\">");
            html.AppendFormat("<b>{0}</b><br/>{1} 篇文章</td></tr></table></div>", nodeTitle, numDoc);

            clusterNum++;
        }
        context.Response.Write(html.ToString());
        #endregion

    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}