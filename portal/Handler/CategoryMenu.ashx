<%@ WebHandler Language="C#" Class="CategoryMenu" %>
using System;
using System.Web;
using System.Xml;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Demo.DAL;
using Demo.Util;
using IdolACINet;

public class CategoryMenu : IHttpHandler
{

    private CATEGORYEntity.CATEGORYDAO Dao = new CATEGORYEntity.CATEGORYDAO();
    private IList<int> parentList = new List<int>();
    //private static string l_width = "250px";
    //private static string l_left = "190px";
    public void ProcessRequest(HttpContext context)
    {
        string strWhere = context.Request.Form["str_where"];
        string page = context.Request.Form["page"];
        //string left = context.Request.Form["left"];
        //if (!string.IsNullOrEmpty(left))
        //{
        //    l_left = left + "px";
        //}
        IList<CATEGORYEntity> entityList = Dao.Find(strWhere);
        IList<CATEGORYEntity> AllList = Dao.Find(" ID > 0 order by ID");
        foreach (CATEGORYEntity Lentity in AllList)
        {
            parentList.Add(Lentity.PARENTCATE.Value);
        }
        StringBuilder HtmlStr = new StringBuilder();
        if (string.IsNullOrEmpty(page))
        {
            int i = 0;
            string curClass = "tab_off";
            foreach (CATEGORYEntity entity in entityList)
            {
                if (i == 0)
                {
                    curClass = "tab_on";
                }
                HtmlStr.AppendFormat("<li class=\"" + curClass + "\"><a href=\"javascript:void(null);\" pid=\"{0}_{1}_{2}\"><b>{3}</b></a>", entity.CATEGORYID, entity.PARENTCATE, entity.ID, entity.CATEGORYNAME);
                GetChildHtml(entity.ID.Value, ref HtmlStr, AllList, 1);
                HtmlStr.Append("</li>");
                i++;
            }
        }
        else
        {
            foreach (CATEGORYEntity entity in entityList)
            {
                //HtmlStr.AppendFormat("<li class=\"" + curClass + "\"><a href=\"javascript:void(null);\" pid=\"{0}_{1}_{2}\"><b>{3}</b></a>", entity.CATEGORYID, entity.PARENTCATE, entity.ID, entity.CATEGORYNAME);
                //GetChildHtml(entity.ID.Value, ref HtmlStr, AllList, 1);
                //HtmlStr.Append("</li>");

                HtmlStr.AppendFormat("<a href=\"javascript:void(null);\" class=\"mainmenu\" pid=\"{0}_{1}_{2}\">{3}舆情</a>", entity.CATEGORYID, entity.PARENTCATE, entity.ID, entity.CATEGORYNAME);
            }
        }
        context.Response.Write(EncodeByEscape.GetEscapeStr(HtmlStr.ToString()));
    }

    private void GetChildHtml(int parentCate, ref StringBuilder HtmlStr, IList<CATEGORYEntity> list, int level)
    {
        if (parentList.Contains(parentCate))
        {
            HtmlStr.Append("<ul class=\"submenu\">");
            int count = 1;
            foreach (CATEGORYEntity entity in list)
            {
                if (entity.PARENTCATE.Value == parentCate)
                {
                    //if (count == 1)
                    //{
                    //    HtmlStr.AppendFormat("<li style=\"padding-left:10px; border-bottom:gray dotted 1px;\">→&nbsp;&nbsp;<a href=\"javascript:void(null)\" pid=\"{0}_{1}_{2}\">{3}</a>", entity.CategoryID, entity.ParentCate, entity.ID, entity.CategoryName);
                    //    GetChildHtml(entity.ID.Value, ref HtmlStr, list, (level + 1));
                    //    HtmlStr.Append("</li>");
                    //}
                    //else
                    //{

                    //            <li><a href="javascript:void(null);"><b>胡锦涛<span class="color_5">（<code class="color_2">76</code>）</span></b></a></li>

                    HtmlStr.AppendFormat("<li><a href=\"javascript:void(null)\" pid=\"{0}_{1}_{2}\"><b>{3}</b></a>", entity.CATEGORYID, entity.PARENTCATE, entity.ID, entity.CATEGORYNAME);
                    GetChildHtml(entity.ID.Value, ref HtmlStr, list, (level + 1));
                    HtmlStr.Append("</li>");
                    //}
                    count++;
                }
            }
            HtmlStr.Append("</ul>");
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
