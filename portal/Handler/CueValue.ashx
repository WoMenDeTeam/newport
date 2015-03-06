<%@ WebHandler Language="C#" Class="CueValue" %>

using System;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using Demo.DAL;
using Demo.Util;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;

public class CueValue : IHttpHandler
{
    private static Regex regex = new Regex("^[\u4e00-\u9fa5]$");
    private static TrieTree t = null;
    private static Dictionary<string, string> itemdict = null;
    public void ProcessRequest(HttpContext context)
    {
        if (t == null) {
            t = (TrieTree)context.Application.Get("trieTree");
        }
        if (itemdict == null) { 
            itemdict = (Dictionary<string, string>)context.Application.Get("ItemDict");
        }
        string InputStr = context.Request["input_str"];

        if (!string.IsNullOrEmpty(InputStr))
        {
            StringBuilder JsonStr = new StringBuilder();
            JsonStr.Append("{");

            

            int chineseCount = 0;
            string searStr = GetPYStr(InputStr, context, ref chineseCount);
            IList<TermEntity> list = t.Search(searStr);

            if (list.Count > 0)
            {
                int count = 1;
                int Num = 10;
                if (list.Count < 10)
                    Num = list.Count;
                if (chineseCount > 0)
                {
                    JsonStr.Append("\"term_list\":{");
                    IList<string> termlist = GetFitList(list, InputStr);
                    Num = termlist.Count;
                    foreach (string s in termlist)
                    {                        
                        if (count == Num)
                        {
                            JsonStr.Append("\"term_" + count + "\":\"" + s + "\"");
                            break;
                        }
                        JsonStr.Append("\"term_" + count + "\":\"" + s + "\",");
                        count++;                        
                    }
                    JsonStr.Append("},");
                }
                else
                {
                    JsonStr.Append("\"term_list\":{");
                    foreach (TermEntity entity in list)
                    {
                        if (count == Num)
                        {
                            JsonStr.Append("\"term_" + count + "\":\"" + entity.termValue + "\"");
                            break;
                        }
                        JsonStr.Append("\"term_" + count + "\":\"" + entity.termValue + "\",");
                        count++;
                    }
                    JsonStr.Append("},");
                }
                JsonStr.Append("\"successCode\":1");
            }
            else
            {
                JsonStr.Append("\"successCode\":0");
            }
            JsonStr.Append("}");
            context.Response.Write(JsonStr.ToString());

        }
        else
        {
            context.Response.Write("{\"successCode\":0}");
        }


    }


    private string GetPYStr(string hzString, HttpContext context, ref int count)
    {
        string pyString = string.Empty;
        char[] noWChar = hzString.ToCharArray();
        for (int j = 0; j < noWChar.Length; j++)
        {
            // 中文字符
            string key = noWChar[j].ToString();
            if (regex.IsMatch(key))
            {
                count++;
                pyString += itemdict[key];
            }
            // 非中文字符
            else
            {
                pyString += key;
            }
        }
        return pyString;
    }

    private IList<string> GetFitList(IList<TermEntity> list, string checkStr)
    {
        IList<string> termList = new List<string>();
        int count = 1;
        foreach (TermEntity entity in list)
        {

            if (entity.termValue.StartsWith(checkStr) && entity.termValue != checkStr)
            {
                termList.Add(entity.termValue);
                count++;
            }
            if (count == 11)
                break;
        }
        return termList;
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}
