<%@ WebHandler Language="C#" Class="NoteMessage" %>

using System;
using System.Web;
using Demo.Util;
using System.Configuration;
using Demo.DAL;
using Demo.BLL;

public class NoteMessage : IHttpHandler {
    private static string SessionKey = ConfigurationManager.AppSettings["SessionKey"].ToString();
    public void ProcessRequest (HttpContext context) {
        try
        {
            if (context.Request.Cookies[SessionKey] != null)
            {
                string adduserInfo = context.Request.Cookies[SessionKey].Value;
                string addueserid = DESEncrypt.Decrypt(adduserInfo).Split('|')[2];
                string addusername = DESEncrypt.Decrypt(adduserInfo).Split('|')[0];
                string useridlist = context.Request["userid_list"];
                string usernamelist = context.Request["username_list"];
                string hreflist = context.Request["href_list"];
                string datelist = context.Request["date_list"];
                string titlelist = context.Request["title_list"];
                if (!string.IsNullOrEmpty(useridlist) && !string.IsNullOrEmpty(hreflist))
                {
                    string[] useridInfo = useridlist.Split('|');
                    string[] userNameInfo = usernamelist.Split('|');
                    string[] hrefInfo = hreflist.Split('|');
                    string[] dateInfo = datelist.Split('|');
                    string[] titleInfo = titlelist.Split('|');
                    for (int i = 0, j = useridInfo.Length; i < j; i++)
                    {
                        int Accepterid = Convert.ToInt32(useridInfo[i]);
                        string Acceptername = EncodeByEscape.GetUnEscapeStr(userNameInfo[i]);
                        for (int k = 0, l = hrefInfo.Length; k < l; k++)
                        {
                            string url = EncodeByEscape.GetUnEscapeStr(hrefInfo[k]);
                            string title = EncodeByEscape.GetUnEscapeStr(titleInfo[k]);
                            string datestr = EncodeByEscape.GetUnEscapeStr(dateInfo[k]);
                            NoteMessageEntity entity = new NoteMessageEntity();
                            entity.AccepterId = Accepterid;
                            entity.Accepter = Acceptername;
                            entity.AddUserId = Convert.ToInt32(addueserid);
                            entity.AddUserName = addusername;
                            entity.AddDate = DateTime.Now;
                            if (!string.IsNullOrEmpty(datestr))
                            {
                                entity.InfoDate = Convert.ToDateTime(datestr);
                            }
                            entity.InfoTitle = title;
                            entity.InfoUrl = url;
                            entity.Status = 0;
                            NoteMessageFacade.Insert(entity);
                        }
                    }
                    context.Response.Write("{\"Success\":1}");
                }
            }
        }
        catch {
            context.Response.Write("{\"Error\":1}");
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}