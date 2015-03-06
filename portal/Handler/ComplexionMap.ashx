<%@ WebHandler Language="C#" Class="ComplexionMap" %>


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Demo.BLL;
using Demo.Util;
using System.Text;
using System.Data;

public class ComplexionMap : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        string backstr = GetMapJsonStr();
        context.Response.Write(backstr);
    }

    private string GetMapJsonStr() {
        StringBuilder jsonstr = new StringBuilder();
        DataTable dt = CityTotalHitsFacade.GetLastHitDtByCategory();
        if (dt.Rows.Count > 0) {
            jsonstr.Append("{");
            foreach (DataRow row in dt.Rows) {
                jsonstr.AppendFormat("\"{0}\":", row["TAG"]);
                jsonstr.Append("{");
                jsonstr.AppendFormat("\"totalhits\":\"{0}\",", row["TOTALHITS"]);
                jsonstr.AppendFormat("\"queryrule\":\"{0}\"", row["CATEGORYID"]);
                jsonstr.Append("},");
            }
            jsonstr.Append("\"SuccessCode\":1}");
        }
        return jsonstr.ToString();
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}

