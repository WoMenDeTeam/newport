<%@ WebHandler Language="C#" Class="GetSGData" %>

using System;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using System.Xml;
using System.Configuration;
using Demo.Util;
using IdolACINet;

public class GetSGData : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) 
    {
        double hotScalsize = Convert.ToDouble(context.Request["hot_scal_size"]);
        double heightSize = Convert.ToDouble(context.Request["height_size"]);
        
        Connection Conn = new Connection(ConfigurationManager.AppSettings["IdolHttp"], 9000);        
        Command query = new Command("ClusterSGDataServe");
        query.SetParam("SourceJobname", ConfigurationManager.AppSettings["SGJobName"]);
        query.SetParam("StructuredXML", QueryParamValue.True);
        XmlDocument contentDoc = Conn.Execute(query).Data;       

        //Create an XmlNamespaceManager for resolving namespaces.
        XmlNamespaceManager nsmgr = new XmlNamespaceManager(contentDoc.NameTable);
        nsmgr.AddNamespace("autn", "http://schemas.autonomy.com/aci/");


        //Select the book node with the matching attribute value.
        XmlNodeList clusters = contentDoc.SelectNodes("autnresponse/responsedata/autn:clusters/autn:cluster", nsmgr);

        string nodeId;
        int nodeOneTop;
        int nodeOneLeft;
        int nodeTwoTop;
        int nodeTwoLeft;
        string nodeTitleId;
        string nodeTitle;
        string pointId;

        int clusterNum = 0;
        StringBuilder html = new StringBuilder();

        foreach (XmlNode cluster in clusters)
        {
            float radius = float.Parse(cluster.SelectSingleNode("autn:radius_from", nsmgr).InnerText);
        
            //radius 实际代表数据的密度，密度大于6，才显示
            if (radius > 4f)
            {
                nodeId = "hotclusternode_" + clusterNum.ToString();
                nodeTitleId = "hotclustertitle_" + clusterNum.ToString();
                nodeTitle = cluster.SelectSingleNode("autn:title", nsmgr).InnerText;
                int numDocs = int.Parse(cluster.SelectSingleNode("autn:numdocs", nsmgr).InnerText);
                pointId = cluster.ChildNodes[0].InnerText + "※" + cluster.ChildNodes[3].InnerText + "※" + cluster.ChildNodes[3].InnerText;
                //TO DO: 换算成日期格式
                string fromDate = TimeHelp.GetDateTime(long.Parse(cluster.SelectSingleNode("autn:fromdate", nsmgr).InnerText)).ToString("yyyy-MM-dd");
                string toDate = TimeHelp.GetDateTime(long.Parse(cluster.SelectSingleNode("autn:todate", nsmgr).InnerText)).ToString("yyyy-MM-dd");

                nodeOneLeft = (int)Math.Truncate(double.Parse(cluster.SelectSingleNode("autn:x1", nsmgr).InnerText) * hotScalsize);
                nodeOneTop = (int)Math.Truncate((double.Parse(cluster.SelectSingleNode("autn:y1", nsmgr).InnerText) - 5) * heightSize);
                nodeTwoLeft = (int)Math.Truncate(double.Parse(cluster.SelectSingleNode("autn:x2", nsmgr).InnerText) * hotScalsize);
                nodeTwoTop = (int)Math.Truncate(double.Parse(cluster.SelectSingleNode("autn:y2", nsmgr).InnerText) * heightSize);
                int width = nodeTwoLeft - nodeOneLeft - (int)(4 * hotScalsize);
                html.AppendFormat("<div class=\"hotnode\" id=\"{0}\"  style=\"position: absolute; width:{3}px; font-size:12px; height:12px; line-height:12px;top:{1}px; left:{2}px; cursor:pointer; zoom:1; \" pid=\"{4}\" ></div>", nodeId, nodeOneTop, nodeOneLeft, width, pointId);
                html.AppendFormat("<div class=\"hot_node_text\" id=\"{0}\" style=\"position: absolute; top: {1}px; left: {2}px; z-index:3; \" >", nodeTitleId, nodeOneTop, nodeOneLeft + width + (int)(30 * hotScalsize));
                html.Append("<table cellpadding=\"3\"><tr>");
                html.Append("<td nowrap=\"nowrap\" style=\"background-color:#FFFF00;color:#000000;border:solid #000000 1px;font-size:9pt;font-family:sans-serif\">");
                html.AppendFormat("{2}—{3}<br/><b>{0}</b><br/>{1} 篇文章 </td></tr></table></div>", nodeTitle, numDocs, fromDate, toDate);

                clusterNum++;
            }
        }

        context.Response.Write(html.ToString());   
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}