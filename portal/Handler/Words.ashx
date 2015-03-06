<%@ WebHandler Language="C#" Class="Words" %>

using System;
using System.Web;
using Demo.DAL;
using System.Collections.Generic;
using System.Text;
using Demo.Util;

public class Words : IHttpHandler {
    private SENSITIVEWORDSEntity.SENSITIVEWORDSDAO Dao = new SENSITIVEWORDSEntity.SENSITIVEWORDSDAO();
    private static IList<int> parentList = new List<int>(); 
    
    public void ProcessRequest (HttpContext context) {
        string type = context.Request["type"];
        string ID = context.Request["term_id"];
        string columnName = context.Request["column_name"];
        string termValue = context.Request["term_value"];
        string parentId = context.Request["parent_id"];
        StringBuilder JsonStr = new StringBuilder();
        JsonStr.Append("{");
        switch (type)
        { 
            case "add":
                SENSITIVEWORDSEntity entity = new SENSITIVEWORDSEntity();
                entity.TERMSTR = termValue;
                entity.PARENTID = System.Convert.ToInt32(parentId);
                try
                {
                    int termID = Dao.AddEntity(entity);
                    if (termID > 0)
                        JsonStr.Append("\"term_id\":" + termID.ToString());
                }
                catch { }
                break;
            case "edit":
                try
                {
                    Dao.UpdateSet(System.Convert.ToInt32(ID), "TermStr", termValue);
                    JsonStr.Append("\"successCode\":1");
                }
                catch { }
                break;
            case "delete":
                try
                {
                    Dao.Delete(System.Convert.ToInt32(ID));
                    JsonStr.Append("\"successCode\":1");
                }
                catch { }
                break;
            case "init":
                IList<SENSITIVEWORDSEntity> initentityList = Dao.Find("");
                foreach (SENSITIVEWORDSEntity Initentity in initentityList)
                {
                    parentList.Add(Initentity.PARENTID.Value);
                }
                JsonStr.Append("\"TermHtmlStr\":\"").Append(GetHtmlStr(initentityList, 0)).Append("\"");
                break;
            case "initMenu":
                IList<SENSITIVEWORDSEntity> entityList = Dao.Find("");
                foreach (SENSITIVEWORDSEntity MenuEntity in entityList)
                {
                    parentList.Add(MenuEntity.PARENTID.Value);
                }
                StringBuilder HtmlStr = new StringBuilder();
                foreach (SENSITIVEWORDSEntity lentity in entityList)
                {
                    if (lentity.PARENTID.Value == 0)
                    {
                        HtmlStr.AppendFormat("<li class=\"left-center-nav-ul\"><img src=\"img/yqzt_34.jpg\"/>&nbsp;&nbsp;<span><a href=\"javascript:void(null)\" id=\"childmenulist_{0}\">{1}</a></span>", lentity.ID, lentity.TERMSTR);
                        GetChildHtml(lentity.ID.Value, ref HtmlStr, entityList,1);
                        HtmlStr.Append("</li>");
                    }
                }
                JsonStr.Append("\"MenuHtmlStr\":\"").Append(EncodeByEscape.GetEscapeStr(HtmlStr.ToString())).Append("\"");
                break;
            default:
                break;        
        }
        JsonStr.Append("}");
        context.Response.Write(JsonStr.ToString());
    }

    private string GetHtmlStr(IList<SENSITIVEWORDSEntity> list, int parentid) 
    {
        StringBuilder htmlstr = new StringBuilder();
        if (parentList.Contains(parentid))
        {
            foreach (SENSITIVEWORDSEntity entity in list)
            {
                if (entity.PARENTID.Value == parentid)
                {
                    htmlstr.Append("<div><ul class=\"Sensitive_word_category\">");
                    htmlstr.AppendFormat("<span id=\"term_value_{0}\">{1}</span><span class=\"term_del\" id=\"term_del_{0}\">删除</span></ul>", entity.ID, entity.TERMSTR);
                    htmlstr.Append(GetChildStr(entity.ID.Value,list));
                    htmlstr.Append("</div>");
                    htmlstr.Append("<div class=\"clear\"></div>");
                }
            }
        }
        return EncodeByEscape.GetEscapeStr(htmlstr.ToString());
    }

    private string GetChildStr(int id, IList<SENSITIVEWORDSEntity> list)
    {
        StringBuilder htmlstr = new StringBuilder();
        if (parentList.Contains(id))
        {
            htmlstr.Append("<ul class=\"Sensitive_word_item\">");
            foreach (SENSITIVEWORDSEntity entity in list)
            {
                if (entity.PARENTID.Value == id)
                {
                    htmlstr.Append("<li>");
                    htmlstr.AppendFormat("<span id=\"term_value_{0}\">{1}</span><span class=\"term_del\" id=\"term_del_{0}\">删除</span>", entity.ID, entity.TERMSTR);
                    htmlstr.Append("</li>");
                }
            }
            htmlstr.AppendFormat("<li><a href=\"javascript:void(null);\" id=\"add_term_{0}\">添加</a></li>", id);
            htmlstr.Append("</ul>");
        }
        else {
            htmlstr.Append("<ul class=\"Sensitive_word_item\">");            
            htmlstr.AppendFormat("<li><a href=\"javascript:void(null);\" id=\"add_term_{0}\">添加</a></li>", id);
            htmlstr.Append("</ul>");
        }
        return htmlstr.ToString();
    }

    private void GetChildHtml(int parentCate, ref StringBuilder HtmlStr, IList<SENSITIVEWORDSEntity> list, int level)
    {
      if(parentList.Contains(parentCate)){
            string left = "150px";
            string width = "120px";
            if (level > 1)
            {
                left = "150px";
                width = "150px";
            }
            HtmlStr.Append("<ol style=\"position:absolute; left:" + left + ";  background:#e9f4f8; width:" + width + "; top:-1px; border-top:#8fa1a3 solid 1px; color:#6b7074; border-bottom:#8fa1a3 solid 1px; border-right:#8fa1a3 solid 1px; display:none;\">");
            int count = 1;
            foreach (SENSITIVEWORDSEntity entity in list)
            {
                if (entity.PARENTID.Value == parentCate)
                {
                    if (count == 1)
                    {
                        HtmlStr.AppendFormat("<li style=\"padding-left:10px; border-bottom:gray dotted 1px;\">→&nbsp;&nbsp;<a href=\"javascript:void(null)\" id=\"childmenulist_{0}\">{1}</a>", entity.ID, entity.TERMSTR);
                        GetChildHtml(entity.ID.Value, ref HtmlStr, list,(level + 1));
                        HtmlStr.Append("</li>");
                    }
                    else
                    {
                        HtmlStr.AppendFormat("<li style=\"border-left:#8fa1a3 solid 1px; padding-left:10px; border-bottom:gray dotted 1px;\">→&nbsp;&nbsp;<a href=\"javascript:void(null)\" id=\"childmenulist_{0}\">{1}</a>", entity.ID, entity.TERMSTR);
                        GetChildHtml(entity.ID.Value, ref HtmlStr, list,(level + 1));
                        HtmlStr.Append("</li>");
                    }
                    count++;
                }
            }
            HtmlStr.Append("</ol>");
        }
    }
    public bool IsReusable {
        get {
            return false;
        }
    }

}