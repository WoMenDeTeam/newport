<%@ WebHandler Language="C#" Class="SuggestForXml" %>
using System;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using System.Xml;
using System.Collections.Generic;
using System.Configuration;
using Demo.Util;
using Demo.BLL;
using IdolACINet;

public class SuggestForXml : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        string docId = context.Request["doc_id"];
        if (!string.IsNullOrEmpty(docId)) {
            docId = docId.ToLower().Replace("s", "");
            Connection conn = new Connection(ConfigurationManager.AppSettings["IdolHttp"], 9000);
            Command query = new Command("Suggest");
            query.SetParam("ID", docId);
            query.SetParam("MaxResults", 10);
            query.SetParam("databasematch", ConfigurationManager.AppSettings["DATABASE"]);
            query.SetParam("Combine", "DRETITLE");        
            XmlDocument xmldoc = conn.Execute(query).Data;
            if (xmldoc != null)
            {
                IdolNewsEntity.IdolNewsDao dao = new IdolNewsEntity.IdolNewsDao();
                IList<IdolNewsEntity> list = dao.GetNewsList(xmldoc);
                if (list.Count > 0) {
                    StringBuilder xmlstr = new StringBuilder();
                    IdolNewsEntity NewsEntity = GetTheNewsInfo(docId, conn);
                    xmlstr.Append(GetXmlHead(docId));
                    xmlstr.Append(GetXmlNodes(list, docId, NewsEntity));
                    xmlstr.Append(GetXmlLinks(list, docId));
                    xmlstr.Append("</Map>");
                    context.Response.ContentType = "text/xml";                    
                    context.Response.Write(xmlstr.ToString());
                }
            }
        }
    }

    private IdolNewsEntity GetTheNewsInfo(string docid,Connection conn) {
        QueryCommand query = new QueryCommand();
        query.Text = "*";
        query.SetParam("MatchID", docid);
        XmlDocument xmldoc = conn.Execute(query).Data;
        if (xmldoc != null)
        {
            IdolNewsEntity.IdolNewsDao dao = new IdolNewsEntity.IdolNewsDao();
            IList<IdolNewsEntity> list = dao.GetNewsList(xmldoc);
            if (list.Count > 0)
            {
                return list[0];
            }
            else {
                return null;
            }
        }
        else {
            return null;
        }    
    }
    
    private string GetXmlHead(string docid) {
        StringBuilder xmlStr = new StringBuilder();
        xmlStr.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        xmlStr.AppendFormat("<Map id=\"Map\" idshow=\"s{0}\" type=\"Topic\">", docid);
        return xmlStr.ToString();
    }

    private string GetXmlNodes(IList<IdolNewsEntity> list,string docid,IdolNewsEntity newsEntity)
    {
        StringBuilder xmlStr = new StringBuilder();
        xmlStr.Append("<Nodes id=\"Nodes\">");
        if (newsEntity != null) {
            xmlStr.AppendFormat("<Node searchWord=\"s{0}\"", newsEntity.DocId);
            xmlStr.AppendFormat(" html=\"{0}\" type=\"Story\"", newsEntity.Href);
            xmlStr.AppendFormat(" infoid=\"{0}\" id=\"s{0}\">", newsEntity.DocId);
            xmlStr.AppendFormat("<![CDATA[{0}]]>", newsEntity.Title);
            xmlStr.Append("</Node>");
        }
        foreach (IdolNewsEntity entity in list) {
            xmlStr.AppendFormat("<Node searchWord=\"s{0}\"", entity.DocId);
            xmlStr.AppendFormat(" html=\"{0}\" type=\"Story\"", entity.Href);
            xmlStr.AppendFormat(" infoid=\"{0}\" id=\"s{0}\">", entity.DocId);
            xmlStr.AppendFormat("<![CDATA[{0}]]>", entity.Title);
            xmlStr.Append("</Node>");
        }
        xmlStr.Append("</Nodes>");
        return xmlStr.ToString();
    }

    private string GetXmlLinks(IList<IdolNewsEntity> list, string docid) {
        StringBuilder xmlStr = new StringBuilder();
        xmlStr.Append("<Links id=\"Links\">");
        foreach (IdolNewsEntity entity in list)
        {
            xmlStr.AppendFormat("<Link type=\"SS\" from=\"s{0}\" to=\"s{1}\"></Link>", docid, entity.DocId);            
        }
        xmlStr.Append("</Links>");
        return xmlStr.ToString();
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}
