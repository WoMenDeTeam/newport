<%@ WebHandler Language="C#"  Class="Agent" %>

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
using Demo.DAL;
using Demo.Util;

public class Agent : IHttpHandler
{
    private static Connection conn = new Connection(ConfigurationManager.AppSettings["IdolHttp"], 9000);
    public void ProcessRequest(HttpContext context)
    {
        int type = Convert.ToInt32(context.Request.Form["agent_type"]);
        string action = context.Request.Form["action"];
        if (!string.IsNullOrEmpty(action))
        {
            Command query = new Command(action);
            foreach (string key in context.Request.Form.Keys)
            {
                if (key.Equals("action"))
                {
                    continue;
                }
                else if (key.Equals("agent_type"))
                {
                    continue;
                }
                else
                {
                    query.SetParam(key, EncodeByEscape.GetUnEscapeStr(context.Request[key]));
                }
            }
            query.SetParam("username", "admin");
            XmlDocument xmldoc = conn.Execute(query).Data;
            StringBuilder jsonStr = new StringBuilder();
            if (xmldoc != null) {
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmldoc.NameTable);
                nsmgr.AddNamespace("autn", "http://schemas.autonomy.com/aci/");
                switch (type) {
                    case 5:                        
                        XmlNodeList list = xmldoc.SelectNodes("autnresponse/responsedata/autn:agent", nsmgr);
                        if (list.Count > 0) {
                            jsonStr.Append("{");	
                            foreach (XmlNode node in list) {
                                string Name = GetNodeText(node.SelectSingleNode("autn:agentname",nsmgr));
                                string TrainCodition = GetNodeText(node.SelectSingleNode("autn:training", nsmgr));
                                string Dabase = GetNodeText(node.SelectSingleNode("autn:fields/dredatabasematch", nsmgr));
                                string MaxResult = GetNodeText(node.SelectSingleNode("autn:fields/dremaxresults", nsmgr));
                                string Score = GetNodeText(node.SelectSingleNode("autn:fields/dreminscore", nsmgr));
                                string Sort = GetNodeText(node.SelectSingleNode("autn:fields/dresort", nsmgr));
                                string reflist = GetRefList(node.SelectNodes("autn:positivedocs/autn:reference", nsmgr));
							    jsonStr.Append("\"agent_"+ EncodeByEscape.GetEscapeStr(Name) +"\":{");
							    jsonStr.Append("\"name\":\""+EncodeByEscape.GetEscapeStr(Name)+"\",");
							    jsonStr.Append("\"train\":\""+EncodeByEscape.GetEscapeStr(TrainCodition)+"\",");
							    jsonStr.Append("\"Dabase\":\""+Dabase+"\",");
							    jsonStr.Append("\"MaxResult\":\""+MaxResult+"\",");
							    jsonStr.Append("\"Score\":\""+Score+"\",");
							    jsonStr.Append("\"Sort\":\""+Sort+"\",");
							    jsonStr.Append("\"reflist\":\""+reflist+"\"},");
                            }
                            jsonStr.Append("\"SuccessCode\":1}");
                        }
                        break;
                    case 6:
                        XmlNodeList agentList = xmldoc.SelectNodes("autnresponse/responsedata/autn:agent", nsmgr);
                        if (agentList.Count > 0) {
                            jsonStr.Append("{");
                            int count = 1;
                            foreach (XmlNode node in agentList) {
                                XmlNodeList CommunityList = node.SelectNodes("autn:results/autn:hit/autn:content/DOCUMENT", nsmgr);
                                if (CommunityList.Count > 0) {
                                    string agentname = node.SelectSingleNode("autn:agentname", nsmgr).InnerText;
                                    jsonStr.AppendFormat("\"agentlist{0}\":",count);
                                    jsonStr.Append("{");
                                    jsonStr.AppendFormat("\"agentname\":\"{0}\",", agentname);
                                    jsonStr.Append("\"communitylist\":");
                                    jsonStr.Append("{");
                                    int lcount = 1;
                                    foreach (XmlNode lnode in CommunityList) {
                                        string Communityname = lnode.SelectSingleNode("NAME").InnerText;
                                        string Communityusername = lnode.SelectSingleNode("USERNAME").InnerText;
                                        jsonStr.AppendFormat("\"communitylist{0}\":", lcount);
                                        jsonStr.Append("{");
                                        jsonStr.AppendFormat("\"communityname\":\"{0}\",", Communityname);
                                        jsonStr.AppendFormat("\"communityusername\":\"{0}\"", Communityusername);
                                        jsonStr.Append("},");
                                        lcount++;
                                    }
                                    jsonStr.Append("\"SuccessCode\":1}");
                                    jsonStr.Append("},");

                                }
                                count++;
                            }
                            jsonStr.Append("\"SuccessCode\":1}");
                        }
                        break;
                    default:
                        jsonStr.Append("{\"SuccessCode\":1}");
                        break;
                }
            }
            context.Response.Write(jsonStr.ToString());
        }
    }
    
    private string GetRefList(XmlNodeList nodelist){
		if(nodelist.Count > 0)
		{
			StringBuilder str = new StringBuilder();
			foreach(XmlNode node in nodelist){
				if(str.Length == 0){
					str.Append(node.InnerText);
				}else{
                    str.Append(",").Append(node.InnerText);					
				}
			}
			return str.ToString();
		}else{
			return "";			
		}
	}

    private string GetNodeText(XmlNode node) {
        try
        {
            return node.InnerText;
        }
        catch
        {
            return null;
        }
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}
