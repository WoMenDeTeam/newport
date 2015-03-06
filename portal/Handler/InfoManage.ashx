<%@ WebHandler Language="C#"  Class="InfoManage"%>

using System;
using System.Web;
using System.Net;
using System.Configuration;
using System.IO;
using System.Text;
using Demo.Util;
using Demo.BLL;

public class InfoManage : IHttpHandler
{
    
    public void ProcessRequest (HttpContext context) {
        string DocIdList = context.Request["doc_id_list"];
        string type = context.Request["type"];
        switch (type)
        { 
            case "delete":
                try
                {
                    string url = "http://" + ConfigurationManager.AppSettings["IdolHttp"] + ":9001/DREDELETEDOC?docs=" + DocIdList;
                    HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
                    myRequest.Method = "GET";
                    myRequest.ContentType = "application/x-www-form-urlencoded";
                    myRequest.ReadWriteTimeout = 15 * 1000;
                    HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
                    StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
                    string content = reader.ReadToEnd();
                    myRequest.Abort();
                    myResponse.Close();
                    context.Response.Write("{\"SuccessCode\":\"1\",\"content\":\"" + EncodeByEscape.GetEscapeStr(content) + "\"}");
                }
                catch (Exception ex)
                {
                    context.Response.Write("{\"SuccessCode\":\"0\"}");
                }           
                break;
            case "deletenote":
                try {
                    NoteMessageFacade.Delete(DocIdList);
                    context.Response.Write("{\"SuccessCode\":\"1\"}");
                }catch{
                    context.Response.Write("{\"SuccessCode\":\"0\"}");
                }
                break;
            default:
                break;
        }
    }
    public bool IsReusable {
        get {
            return false;
        }
    }

}

