<%@ WebHandler Language="C#" Class="GetSurveyData" %>

using System;
using System.Web;
using System.Collections.Generic;
using Demo.BLL;
using Demo.Util;
using Demo.DAL;
using System.Text;
using System.Configuration;

public class GetSurveyData : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {        
        //object SurveyData = HttpContext.Current.Session["SurveyData"];
        //if (SurveyData == null)
        //{   
        try
        {
            StringBuilder strWhere = new StringBuilder();
            StringBuilder parentcatelist = new StringBuilder();
            Dictionary<string, int> keylist = new Dictionary<string, int>();
            strWhere.Append(" ISEFFECT=1");
            int count = 1;
            foreach (string key in context.Request.Form.Keys)
            {
                string parentCate = context.Request.Form[key];
                if (!string.IsNullOrEmpty(parentCate))
                {
                    if (count == 1)
                    {
                        strWhere.Append(" AND ");
                    }
                    else
                    {
                        strWhere.Append(" OR ");
                    }
                    strWhere.AppendFormat(" PARENTCATE={0}", parentCate);
                    keylist.Add(key, Convert.ToInt32(parentCate));
                    if (parentcatelist.Length > 0) {
                        parentcatelist.Append(",");
                    }
                    parentcatelist.Append(parentCate);
                }
                count++;
            }
            if (strWhere.Length > 0)
            {
                strWhere.Append(" order by SEQUEUE");
                IList<CATEGORYEntity> categoryList = CategoryFacade.GetCategoryEntityList(strWhere.ToString());
                Dictionary<string, string> ColumnCategoryDict = CategoryFacade.GetCategoryColumnDict(parentcatelist.ToString());
                StringBuilder jsonstr = new StringBuilder();
                jsonstr.Append("{");
                foreach (string key in keylist.Keys)
                {
                    if (key == "subnav_1")
                    {
                        string SessionKey = ConfigurationManager.AppSettings["SessionKey"].ToString();
                        string userInfo = DESEncrypt.Decrypt(context.Request.Cookies[SessionKey].Value);
                        string roleID = userInfo.Split('|')[1];
                        Dictionary<string, string> dict = GetCategoryList.GetList(roleID);
                        if (dict.Keys.Count > 0)
                        {
                            jsonstr.AppendFormat("{0},", GetJsonStr(key, keylist[key], dict, ColumnCategoryDict, categoryList));
                        }
                        else
                        {
                            jsonstr.AppendFormat("{0},", GetJsonStr(key, keylist[key], categoryList, ColumnCategoryDict));
                        }
                    }
                    else
                    {
                        jsonstr.AppendFormat("{0},", GetJsonStr(key, keylist[key], categoryList, ColumnCategoryDict));
                    }
                }
                jsonstr.Append("\"SuccessCode\":1}");
                //HttpContext.Current.Session["SurveyData"] = jsonstr.ToString();
                context.Response.Write(jsonstr.ToString());
            }
        }
        catch (Exception e)
        {
            context.Response.Write(e.ToString());
        }
        //}
        //else {
        //    string jsonstr = (string)SurveyData;
        //    context.Response.Write(jsonstr);
        //}
    }

    private string GetJsonStr(string key, int parentcate, IList<CATEGORYEntity> categoryList, Dictionary<string, string> ColumnCategoryDict)
    {
        StringBuilder jsonstr = new StringBuilder();
        jsonstr.AppendFormat("\"{0}\":", key);
        jsonstr.Append("{");
        foreach (CATEGORYEntity entity in categoryList) {
            if (entity.PARENTCATE.Value == parentcate)
            {
                string categoryid = entity.CATEGORYID.ToString();
                if (ColumnCategoryDict.ContainsKey(categoryid))
                {
                    jsonstr.AppendFormat("\"{0}_{2}\":\"{1}\",", categoryid, EncodeByEscape.GetEscapeStr(entity.CATEGORYNAME), ColumnCategoryDict[categoryid]);
                }
            }
        }
        jsonstr.Append("\"SuccessCode\":1}");
        return jsonstr.ToString();
    }

    private string GetJsonStr(string key, int parentcate, Dictionary<string, string> categorydict, Dictionary<string, string> ColumnCategoryDict, IList<CATEGORYEntity> categoryList)
    {
        StringBuilder jsonstr = new StringBuilder();
        jsonstr.AppendFormat("\"{0}\":", key);
        jsonstr.Append("{");
        foreach (string categoryid in categorydict.Keys)
        {
            if (!categorydict.ContainsKey(categoryid) && ColumnCategoryDict.ContainsKey(categoryid))
            {
                jsonstr.AppendFormat("\"{0}_{2}\":\"{1}\",", categoryid, EncodeByEscape.GetEscapeStr(categorydict[categoryid]), ColumnCategoryDict[categoryid]);
            }
        }
        foreach (CATEGORYEntity entity in categoryList)
        {
            string l_categoryid = entity.CATEGORYID.ToString();
            if (!categorydict.ContainsKey(l_categoryid) && entity.PARENTCATE.Value == 226 && ColumnCategoryDict.ContainsKey(l_categoryid))
            {
                jsonstr.AppendFormat("\"{0}_{2}\":\"{1}\",", l_categoryid, EncodeByEscape.GetEscapeStr(entity.CATEGORYNAME), ColumnCategoryDict[l_categoryid]);
            }
        }
        jsonstr.Append("\"SuccessCode\":1}");
        return jsonstr.ToString();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}