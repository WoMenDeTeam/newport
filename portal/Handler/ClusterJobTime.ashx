﻿<%@ WebHandler Language="C#" Class="ClusterJobTime" %>

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

public class ClusterJobTime : IHttpHandler {

    public void ProcessRequest(HttpContext context)
    {
        Command query = new Command("ClusterShowJobs");
        Connection cnn = new Connection(ConfigurationManager.AppSettings["IdolHttp"], 9000);
        string jobName = context.Request.Form["job_name"];
        if (string.IsNullOrEmpty(jobName))
        {        
            jobName = ConfigurationManager.AppSettings["HotJobName"];
        }
        Response result = cnn.Execute(query);
        XmlDocument contentDoc = result.Data;

        //Create an XmlNamespaceManager for resolving namespaces.
        XmlNamespaceManager nsmgr = new XmlNamespaceManager(contentDoc.NameTable);
        nsmgr.AddNamespace("autn", "http://schemas.autonomy.com/aci/");
        StringBuilder jsonStr = new StringBuilder();
        if (contentDoc != null)
        {
            XmlNodeList nodeList = contentDoc.SelectNodes("autnresponse/responsedata/autn:clusters/autn:cluster", nsmgr);
            if (nodeList.Count > 0)
            {
                foreach (XmlNode node in nodeList)
                {
                    string name = node.SelectSingleNode("autn:name", nsmgr).InnerText;
                    if (name == jobName.ToLower())
                    {
                        XmlNodeList timelist = node.SelectNodes("autn:timestamp", nsmgr);
                        if (timelist.Count > 0)
                        {
                            jsonStr.Append("{");
                            int count = 1;
                            foreach (XmlNode lnode in timelist)
                            {
                                jsonStr.AppendFormat("\"item_{0}\":{1},", count, lnode.InnerText);
                                count++;
                            }
                            jsonStr.Append("\"SuccessCode\":1}");
                        }
                    }
                }
            }
        }
        context.Response.Write(jsonStr.ToString());
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}