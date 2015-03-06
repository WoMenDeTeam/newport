<%@ WebHandler Language="C#" Class="GetSiteTrend" %>

using System;
using System.Web;
using System.Xml;
using System.Text;
using System.Collections.Generic;
using System.Configuration;
using Demo.DAL;
using Demo.Util;

public class GetSiteTrend : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        string category = context.Request["category"];
        if (!string.IsNullOrEmpty(category))
        {
            TRANSROUTEEntity.TRANSROUTEDAO dao = new TRANSROUTEEntity.TRANSROUTEDAO();
            string strWhere = " CATEGORY =" + category + " order by FIRSTTIME ASC";
            IList<TRANSROUTEEntity> list = dao.Find(strWhere);
            StringBuilder jsonstr = new StringBuilder();            
            jsonstr.Append("{");
            Dictionary<DateTime, string> dict = new Dictionary<DateTime, string>();
            foreach (TRANSROUTEEntity entity in list)
            {
                DateTime key = entity.FIRSTTIME.Value;
                if (dict.ContainsKey(key))
                {
                    dict[key] = dict[key] + "," + EncodeByEscape.GetEscapeStr(entity.SITENAME);
                }
                else {
                    dict.Add(entity.FIRSTTIME.Value, EncodeByEscape.GetEscapeStr(entity.SITENAME));
                }
            }
            foreach (DateTime dt in dict.Keys)
            {
                jsonstr.AppendFormat("\"{0}\":\"{1}\",", dt, dict[dt]);
            }
            jsonstr.AppendFormat("\"Count\":{0}", dict.Keys.Count).Append("}");
            context.Response.Write(jsonstr.ToString());
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
