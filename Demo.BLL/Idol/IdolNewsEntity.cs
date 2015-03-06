using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using IdolACINet;
using System.Configuration;
using Demo.Util;

namespace Demo.BLL
{

    public class IdolNewsEntity
    {
        private static string HtmlTag = ConfigurationManager.AppSettings["HtmlTag"].ToString();
        private static string imagebase = ConfigurationManager.AppSettings["imgbase"].ToString();
        public string Href
        {
            set;
            get;
        }
        public string Author
        {
            set;
            get;
        }

        public string DocId
        {
            get;
            set;
        }

        public string Content
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string SiteName
        {
            get;
            set;
        }

        public string TimeStr
        {
            get;
            set;
        }

        public string weight
        {
            get;
            set;
        }

        public string DocType
        {
            get;
            set;
        }

        public string Tag
        {
            get;
            set;
        }

        public string TotalCount
        {
            get;
            set;
        }

        public string ClusterId
        {
            get;
            set;
        }

        public string ClusterTitle
        {
            get;
            set;
        }

        public string ReplyNum
        {
            get;
            set;
        }

        public string ClickNum
        {
            get;
            set;
        }

        public string BBSViewNum
        {
            get;
            set;
        }

        public string BBSReplyNum
        {
            get;
            set;
        }

        public string ShowContent
        {
            get;
            set;
        }
        public string AllContent
        {
            get;
            set;
        }

        public string ForwardNum
        {
            get;
            set;
        }

        public string AuthorUrl
        {
            set;
            get;
        }

        public string SourceUrl
        {
            set;
            get;
        }
        public string ContUrl
        {
            set;
            get;
        }
        public string SameNum
        {
            set;
            get;
        }
        public string ReadNum { get; set; }

        //视频新增属性
        /// <summary>
        /// 字符串时间格式
        /// </summary>
        public string StrDatetime { get; set; }
        /// <summary>
        /// 视频ID（关联sql）
        /// </summary>
        public string VideoId { get; set; }
        /// <summary>
        /// 视频文件名称（视频地址）
        /// </summary>
        public string VideoFilePath { get; set; }
        /// <summary>
        /// 视频段数
        /// </summary>
        public string VideoFileCount { get; set; }
        /// <summary>
        /// 视频短连接
        /// </summary>
        public string VideoShortLink { get; set; }
        /// <summary>
        /// 视频缩略图
        /// </summary>
        public string VideoThumbPic { get; set; }
        /// <summary>
        /// 关键字
        /// </summary>
        public string Links { get; set; }
        public string WeiboUrl { get; set; }

        public class IdolNewsDao
        {
            public IList<IdolNewsEntity> GetNewsList(XmlDocument xmldoc)
            {
                IList<IdolNewsEntity> newsList = new List<IdolNewsEntity>();
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmldoc.NameTable);
                nsmgr.AddNamespace("autn", "http://schemas.autonomy.com/aci/");
                XmlNodeList hits = xmldoc.SelectNodes("//autn:hit", nsmgr);
                foreach (XmlNode hit in hits)
                {
                    IdolNewsEntity entity = new IdolNewsEntity();
                    entity.TotalCount = GetArticleCount(xmldoc);
                    entity.DocId = GetNodeText(hit.SelectSingleNode("autn:id", nsmgr));
                    entity.Content = GetNodeText(hit.SelectSingleNode("autn:summary", nsmgr));
                    entity.weight = GetNodeText(hit.SelectSingleNode("autn:weight", nsmgr));
                    entity.Href = GetNodeText(hit.SelectSingleNode("autn:reference", nsmgr));
                    entity.ClusterId = GetNodeText(hit.SelectSingleNode("autn:cluster", nsmgr));
                    entity.ClusterTitle = GetNodeText(hit.SelectSingleNode("autn:clustertitle", nsmgr));
                    entity.Title = GetNodeText(hit.SelectSingleNode("autn:title", nsmgr));
                    XmlNode document = hit.SelectSingleNode("autn:content/DOCUMENT", nsmgr);
                    if (document != null)
                    {
                        string doctype = GetNodeText(document.SelectSingleNode("MYSRCTYPE"));
                        string doctag = GetNodeText(document.SelectSingleNode("SENTIMENT"));

                        string click = GetNodeText(document.SelectSingleNode("READNUM"));
                        click = string.IsNullOrEmpty(click) ? "0" : click;

                        entity.ClickNum = click;// GetNodeText(document.SelectSingleNode("READNUM"));
                        entity.Author = GetNodeText(document.SelectSingleNode("AUTHORNAME"));

                        entity.AuthorUrl = GetNodeText(document.SelectSingleNode("AUTHIRURL"));
                        string reply = GetNodeText(document.SelectSingleNode("REPLYNUM"));
                        reply = string.IsNullOrEmpty(reply) ? "0" : reply;
                        entity.ReplyNum = reply;

                        string bbsviewnum = GetNodeText(document.SelectSingleNode("BBSVIEWNUM"));
                        bbsviewnum = string.IsNullOrEmpty(bbsviewnum) ? "0" : bbsviewnum;
                        entity.BBSViewNum = bbsviewnum;

                        string bbsreplynum = GetNodeText(document.SelectSingleNode("BBSREPLYNUM"));
                        bbsreplynum = string.IsNullOrEmpty(bbsreplynum) ? "0" : bbsreplynum;
                        entity.BBSReplyNum = bbsreplynum;

                        entity.SiteName = GetSiteName(GetNodeText(document.SelectSingleNode("MYSITENAME")), GetNodeText(document.SelectSingleNode("DOMAINSITENAME")), GetNodeText(document.SelectSingleNode("SITENAME")));
                        entity.TimeStr = TimeHelp.ConvertToDateTimeString(GetNodeText(document.SelectSingleNode("DREDATE")));
                        entity.AllContent = GetNodeText(document.SelectSingleNode("DISPLAYCONTENT"));
                        if (!string.IsNullOrEmpty(entity.AllContent))
                        {
                            entity.AllContent = entity.AllContent.Replace(HtmlTag, imagebase);
                        }
                        entity.ShowContent = GetNodeText(document.SelectSingleNode("SHOWCONTENT"));
                        entity.ForwardNum = GetNodeText(document.SelectSingleNode("FORWARDNUM"));
                        entity.SourceUrl = GetNodeText(document.SelectSingleNode("SOURCEURL"));
                        entity.DocType = doctype == null ? "other" : doctype;
                        entity.Tag = doctag == null ? "other" : doctag;
                        entity.ContUrl = GetNodeText(document.SelectSingleNode("CONTURL"));
                        entity.SameNum = GetNodeText(document.SelectSingleNode("SAMENUM"));
                        entity.ReadNum = GetNodeText(document.SelectSingleNode("READNUM"));

                        /*视频新增字段*/
                        entity.StrDatetime = GetNodeText(document.SelectSingleNode("MYPUBDATE"));
                        entity.VideoId = GetNodeText(document.SelectSingleNode("VIDEOID"));
                        entity.VideoFilePath = GetNodeText(document.SelectSingleNode("FILEPATH"));
                        entity.VideoFileCount = GetNodeText(document.SelectSingleNode("FILECOUNT"));
                        entity.VideoShortLink = GetNodeText(document.SelectSingleNode("SHORTLINK"));
                        entity.VideoThumbPic = GetNodeText(document.SelectSingleNode("THUMBPIC"));
                        entity.WeiboUrl = GetNodeText(document.SelectSingleNode("URL"));

                    }
                    newsList.Add(entity);
                }
                return newsList;
            }
            private string GetSiteName(string a, string b, string c)
            {
                if (!string.IsNullOrEmpty(a))
                {
                    return a;
                }
                if (!string.IsNullOrEmpty(c))
                {
                    return c;
                }
                return b;
            }

            public string GetArticleCount(XmlDocument xmldoc)
            {
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmldoc.NameTable);
                nsmgr.AddNamespace("autn", "http://schemas.autonomy.com/aci/");
                string totalCount = "0";
                XmlNode totalCountElement = xmldoc.SelectSingleNode("//autn:totalhits", nsmgr);
                XmlNode MaxResult = xmldoc.SelectSingleNode("//dremaxresults");
                if (MaxResult != null)
                {
                    totalCount = GetMaxCount(totalCountElement, MaxResult);
                }
                else
                {
                    totalCount = GetNodeText(totalCountElement);
                }
                return totalCount;
            }

            private String GetMaxCount(XmlNode node1, XmlNode node2)
            {
                int value1 = Convert.ToInt32(GetNodeText(node1));
                int value2 = Convert.ToInt32(GetNodeText(node2));
                int comparevalue = value1 <= value2 ? value1 : value2;
                return comparevalue.ToString();
            }

            public IList<IdolNewsEntity> GetPagerList(string action, Dictionary<string, string> dict)
            {
                XmlDocument xmldoc = GetXmlDoc(action, dict);
                if (xmldoc != null)
                {
                    return GetNewsList(xmldoc);
                }
                else
                {
                    return null;
                }
            }
            public IList<IdolNewsEntity> GetPagerList(string action, Dictionary<string, string> dict, string idolIpKey)
            {
                XmlDocument xmldoc = GetXmlDoc(action, dict, idolIpKey);
                if (xmldoc != null)
                {
                    return GetNewsList(xmldoc);
                }
                else
                {
                    return null;
                }
            }

            public XmlDocument GetXmlDoc(string action, Dictionary<string, string> dict)
            {
                Connection conn = new Connection(ConfigurationManager.AppSettings["IdolHttp"], 9000);
                Command query = new Command(action);
                if (dict != null)
                {
                    foreach (string key in dict.Keys)
                    {
                        query.SetParam(key, dict[key]);
                    }
                }
                return conn.Execute(query).Data;
            }
            //重载，以方便去不同IDOl库的数据
            public XmlDocument GetXmlDoc(string action, Dictionary<string, string> dict, string IdolIPKey)
            {
                Connection conn = new Connection(ConfigurationManager.AppSettings[IdolIPKey], 9000);
                Command query = new Command(action);
                if (dict != null)
                {
                    foreach (string key in dict.Keys)
                    {
                        query.SetParam(key, dict[key]);
                    }
                }
                return conn.Execute(query).Data;
            }

            private string GetNodeText(XmlNode node)
            {
                try
                {
                    return node.InnerText;
                }
                catch
                {
                    return null;
                }
            }
        }


    }
}
