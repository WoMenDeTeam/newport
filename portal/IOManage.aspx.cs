using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Demo.Util;
using System.Text;

public partial class IOManage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Ajax();
        }
    }

    protected void Ajax()
    {
        string action = Request["action"];
        if (!string.IsNullOrEmpty(action) && !string.IsNullOrEmpty(Request["AjaxString"]) && Request["AjaxString"] == "1")
        {
            string rootpath = ConfigurationManager.AppSettings["root"];
            string path = EncodeByEscape.GetUnEscapeStr(Request["path"]);
            if (string.IsNullOrEmpty(path))
            {
                path = rootpath;
            }
            string res = string.Empty;
            try
            {
                switch (action)
                {
                    case "getfolderlist":
                        string[] folderlist = IOFileManage.GetFolderList(path);
                        if (folderlist.Length > 0)
                        {
                            res = GetJsonStr(folderlist);
                        }
                        break;
                    case "getfilelist":
                        string[] filelist = IOFileManage.GetFileList(path);
                        if (filelist.Length > 0)
                        {
                            res = GetJsonStr(filelist);
                        }
                        break;
                    case "getroot":
                        res = "{\"path\":\"" + EncodeByEscape.GetEscapeStr(rootpath) + "\"}";
                        break;
                    case "removefolder":
                        IOFileManage.DeleteFolder(path);
                        res = "{\"Success\":1}";
                        break;
                    case "removefile":
                        IOFileManage.DeleteFile(path);
                        res = "{\"Success\":1}";
                        break;
                    case "createfolder":
                        if (IOFileManage.CreateFolder(path))
                        {
                            res = "{\"Success\":1}";
                        }
                        else
                        {
                            res = "{\"Success\":0,\"Error\":\"Lost!This folder is exist\"}";
                        }
                        break;
                    case "uploadfile":
                        HttpPostedFile file = Request.Files[0];
                        IOFileManage.UploadFile(file, path);
                        res = "{\"Success\":1}";
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                res = "{\"Success\":0,\"Error\":\" " + e.ToString() + "\"}";
            }
            finally
            {
                Response.Write(res);
                Response.End();
            }
        }
    }

    protected string GetJsonStr(string[] list)
    {
        StringBuilder jsonstr = new StringBuilder();
        jsonstr.Append("{");
        int count = 1;
        foreach (string key in list)
        {
            jsonstr.AppendFormat("\"entity_{0}\":", count);
            jsonstr.Append("{");
            jsonstr.AppendFormat("\"name\":\"{0}\",", EncodeByEscape.GetEscapeStr(IOFileManage.GetFileName(key)));
            jsonstr.AppendFormat("\"path\":\"{0}\"", EncodeByEscape.GetEscapeStr(key));
            jsonstr.Append("},");
            count++;
        }
        jsonstr.Append("\"Success\":1}");
        return jsonstr.ToString();
    }
}
