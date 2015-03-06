<%@ WebHandler Language="C#" Class="TrainTag" %>

using System;
using System.Web;
using System.Configuration;
using IdolACINet;
using System.Xml;
using System.Threading;

public class TrainTag : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        string docIdList = context.Request["docid_list"];
        string fieldName = context.Request["field_name"];
        string fieldValue = context.Request["field_value"];
        string type = context.Request["type"];
        switch (type)
        {
            case "editdoc":
                Connection cnn = new Connection(ConfigurationManager.AppSettings["IdolHttp"], 9001);
                Drereplace drereplace = new Drereplace();
                drereplace.ReplaceAllRefs = true;               
                drereplace.PostData = "#DREDOCID " + docIdList + "\n#DREFIELDNAME " + fieldName + "\n#DREFIELDVALUE " + fieldValue + "\n#DREENDDATANOOP\n\n";

                try
                {
                    cnn.Execute(drereplace);
                    Thread.Sleep(1000);
                    context.Response.Write("success");
                }
                catch (Exception e)
                {
                    context.Response.Write("lost");
                }
                break;
            case "deletedoc":
                Connection deletecn = new Connection(ConfigurationManager.AppSettings["IdolHttp"], 9000);
                QueryCommand query = new QueryCommand();
                query.Text = "*";
                query.Parameters.Add("MatchID", docIdList);
                query.Parameters.Add("DatabaseMatch", ConfigurationManager.AppSettings["DATABASE"]);
                query.Parameters.Add("Delete", "true");
                try
                {
                    deletecn.Execute(query);
                    Thread.Sleep(1000);
                    context.Response.Write("success");
                }
                catch (Exception e)
                {
                    context.Response.Write("lost");
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