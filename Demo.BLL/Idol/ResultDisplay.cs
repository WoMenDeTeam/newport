using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Demo.DAL;
using Demo.Util;

namespace Demo.BLL
{
    public abstract class ResultDisplay
    {
        public IList<IdolNewsEntity> list = new List<IdolNewsEntity>();

        public abstract string Display();


    }
    public class FirstStyleDis : ResultDisplay
    {
        public override string Display()
        {

            Dictionary<string, string> tagDict = new Dictionary<string, string>();
            tagDict.Add("other", "");
            tagDict.Add("news", "n");
            tagDict.Add("comment", "c");
            tagDict.Add("p", "p");
            tagDict.Add("n", "n");
            tagDict.Add("m", "");
            string totalcount = "0";
            StringBuilder jsonstr = new StringBuilder();
            if (list != null)
            {
                totalcount = list[0].TotalCount;
                StringBuilder html = new StringBuilder();
                foreach (IdolNewsEntity entity in list)
                {
                    string Tag = tagDict[entity.DocType] + tagDict[entity.Tag];
                    html.AppendFormat("<li><dl><dt><input id=\"" + entity.DocId + "\" type=\"checkbox\" class=\"selected_doc\" pid=\"{1}\" />{0}</dt><dd class=\"l_title\"><a href=\"{1}\" title=\"{2}\" target=\"_blank\">", entity.weight + "%", entity.Href, entity.Title);
                    html.AppendFormat("{0}</a></dd>", (entity.Title.Length > 27) ? entity.Title.Substring(0, 27) + "..." : entity.Title);
                    html.Append("<dd class=\"l_tag\"><span><font color='black'>" + Tag + "</font>&nbsp;&nbsp;" + entity.SiteName + "</span> - " + entity.TimeStr + "</dd>");
                    html.AppendFormat("<dd class=\"l_content\">{0}<b>...</b></dd>", entity.Content);
                    html.Append("</dl></li>");
                }
                jsonstr.Append("{\"HtmlStr\":\"");
                jsonstr.Append(EncodeByEscape.GetEscapeStr(html.ToString()));
                jsonstr.Append("\",\"TotalCount\":\"");
                jsonstr.Append(totalcount);
                jsonstr.Append("\"}");
            }
            return jsonstr.ToString();
        }
    }

    public class SecoendStyleDis : ResultDisplay
    {
        public override string Display()
        {
            string totalcount = "0";
            StringBuilder jsonstr = new StringBuilder();

            if (list != null)
            {
                totalcount = list[0].TotalCount;
                StringBuilder html = new StringBuilder();
                foreach (IdolNewsEntity entity in list)
                {
                    html.AppendFormat("<li><h2><a href=\"{0}\" title=\"{1}\" target=\"_blank\">", entity.Href, entity.Title);
                    html.AppendFormat("{0}</a></h2>", (entity.Title.Length > 27) ? entity.Title.Substring(0, 27) + "..." : entity.Title);
                    html.Append("<div class=\"d\"><span>&nbsp;&nbsp;" + entity.SiteName + "</span> - " + entity.TimeStr + "</div>");
                    html.AppendFormat("<p>{0}<b>...</b></p>", entity.Content);
                    html.Append("</li>");
                }
                jsonstr.Append("{\"HtmlStr\":\"");
                jsonstr.Append(EncodeByEscape.GetEscapeStr(html.ToString()));
                jsonstr.Append("\",\"TotalCount\":\"");
                jsonstr.Append(totalcount);
                jsonstr.Append("\"}");
            }
            return jsonstr.ToString();
        }
    }

    public class ThirdStyleDis : ResultDisplay
    {
        public override string Display()
        {
            string totalcount = "0";
            StringBuilder jsonstr = new StringBuilder();
            if (list != null)
            {
                totalcount = list[0].TotalCount;
                StringBuilder html = new StringBuilder();
                foreach (IdolNewsEntity entity in list)
                {
                    var Articletitle = entity.Title;
                    html.Append("<li><div class=\"trainSelect\"><input  type=\"checkbox\" name=\"train_article_list\" id=\"article_" + entity.DocId + "\" pid=\"" + entity.Href + "\"/>" + entity.weight + "%</div>");

                    html.AppendFormat("<h2><a href=\"{0}\" title=\"{1}\" target=\"_blank\">", entity.Href, Articletitle);
                    html.AppendFormat("{0}</a></h2>", (Articletitle.Length > 27) ? Articletitle.Substring(0, 27) + "..." : Articletitle);
                    html.AppendFormat("<div class=\"d\"><span>{0}</span> - {1}</div>", entity.SiteName, entity.TimeStr);
                    html.AppendFormat("<p>{0}<b>...</b></p>", entity.Content);
                    html.Append("</li>");
                }
                jsonstr.Append("{\"HtmlStr\":\"");
                jsonstr.Append(EncodeByEscape.GetEscapeStr(html.ToString()));
                jsonstr.Append("\",\"TotalCount\":\"");
                jsonstr.Append(totalcount);
                jsonstr.Append("\"}");
            }
            return jsonstr.ToString();
        }
    }
    public class ForthStyleDis : ResultDisplay
    {
        public override string Display()
        {
            string totalcount = "0";
            StringBuilder jsonstr = new StringBuilder();
            if (list != null)
            {
                totalcount = list[0].TotalCount;
                StringBuilder html = new StringBuilder();
                foreach (IdolNewsEntity entity in list)
                {
                    html.AppendFormat("<div id=\"idol_article_{0}\">", entity.DocId);
                    html.AppendFormat("<div class=\"gw_news_title\"><a href=\"{0}\" target=\"_blank\">{1}</a><span>{2}</span><span>{3}</span></div>", entity.Href, entity.Title, entity.TimeStr, entity.SiteName);
                    html.AppendFormat("<div class=\"gw_news_text\">{0}</div>", entity.Content);
                    html.AppendFormat("<a name=\"article_delete\" class=\"btn_delete\" pid=\"{0}\" href=\"javascript:void(null);\">删除</a>", entity.DocId);
                    html.Append("<div style=\"display: none; width:100%;\" name=\"doc_").Append(entity.DocId).Append("\" class=\"gw_news_info\"> <img border=\"0\"  src=\"images/info.gif\"></div><br/>");
                    html.AppendFormat("<div style=\"display: none; width:100%;\" name=\"doc_{0}\" id=\"suggest_{1}\"></div></div>", entity.DocId, entity.DocId);
                }
                jsonstr.Append("{\"HtmlStr\":\"");
                jsonstr.Append(EncodeByEscape.GetEscapeStr(html.ToString()));
                jsonstr.Append("\",\"TotalCount\":\"");
                jsonstr.Append(totalcount);
                jsonstr.Append("\"}");
            }
            return jsonstr.ToString();
        }
    }


    public class FifthStyleDIs : ResultDisplay
    {
        public override string Display()
        {
            string totalcount = "0";
            StringBuilder jsonstr = new StringBuilder();
            if (list != null)
            {
                totalcount = list[0].TotalCount;
                StringBuilder html = new StringBuilder();
                foreach (IdolNewsEntity entity in list)
                {
                    html.Append("<li><h2><span id=\"sentiment_" + entity.DocId + "\">");
                    if (entity.Tag == "n")
                    {
                        html.Append("【负面】&nbsp;&nbsp;");
                    }
                    else if (entity.Tag == "p")
                    {
                        html.Append("【正面】&nbsp;&nbsp;");
                    }
                    else if (entity.Tag == "m")
                    {
                        html.Append("【中立】&nbsp;&nbsp;");
                    }
                    html.AppendFormat("</span><a href=\"{0}\" title=\"{1}\" target=\"_blank\">", entity.Href, entity.Title);
                    html.AppendFormat("{0}</a></h2>", (entity.Title.Length > 20) ? entity.Title.Substring(0, 20) + "..." : entity.Title);
                    html.Append("<div class=\"d\"><span>&nbsp;&nbsp;" + entity.SiteName + "</span> - " + entity.TimeStr + "</div>");
                    html.AppendFormat("<p>{0}<b>...</b></p>", entity.Content);
                    html.Append("<div style=\"text-align:center; height:25px; line-height:25px;\"><span  name=\"comment_div\" style=\"display:none;\">");
                    html.AppendFormat("【<a href=\"javascript:void(null);\" pid=\"{0}\"  id=\"btn_design_bad\">设置为有害</a>】&nbsp;&nbsp;&nbsp;&nbsp;", entity.DocId);
                    html.AppendFormat("【<a href=\"javascript:void(null);\" pid=\"{0}\" id=\"btn_design_neutral\">删除该文章</a>】", entity.DocId);
                    html.Append("</span></div></li>");
                }
                jsonstr.Append("{\"HtmlStr\":\"");
                jsonstr.Append(EncodeByEscape.GetEscapeStr(html.ToString()));
                jsonstr.Append("\",\"TotalCount\":\"");
                jsonstr.Append(totalcount);
                jsonstr.Append("\"}");
            }
            return jsonstr.ToString();
        }
    }

    public class JsonStyleDis : ResultDisplay
    {
        public override string Display()
        {
            int count = 1;
            StringBuilder jsonstr = new StringBuilder();
            if (list != null && list.Count > 0)
            {
                jsonstr.Append("{");
                string totalcount = list[0].TotalCount;
                jsonstr.AppendFormat("\"totalcount\":{0},", totalcount);
                foreach (IdolNewsEntity entity in list)
                {
                    jsonstr.AppendFormat("\"entity_{0}\":", count);
                    jsonstr.Append("{");
                    jsonstr.AppendFormat("\"title\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.Title));
                    jsonstr.AppendFormat("\"href\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.Href));
                    jsonstr.AppendFormat("\"url\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.WeiboUrl));
                    jsonstr.AppendFormat("\"time\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.TimeStr));
                    jsonstr.AppendFormat("\"site\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.SiteName));
                    jsonstr.AppendFormat("\"author\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.Author));
                    jsonstr.AppendFormat("\"authorurl\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.AuthorUrl));
                    jsonstr.AppendFormat("\"replynum\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.ReplyNum));
                    jsonstr.AppendFormat("\"clicknum\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.ClickNum));
                    jsonstr.AppendFormat("\"docid\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.DocId));
                    jsonstr.AppendFormat("\"content\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.Content));
                    jsonstr.AppendFormat("\"allcontent\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.AllContent));
                    jsonstr.AppendFormat("\"forwardnum\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.ForwardNum));
                    jsonstr.AppendFormat("\"authorurl\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.AuthorUrl));
                    jsonstr.AppendFormat("\"sourceurl\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.SourceUrl));
                    jsonstr.AppendFormat("\"weight\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.weight));
                    jsonstr.AppendFormat("\"conturl\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.ContUrl));
                    jsonstr.AppendFormat("\"samenum\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.SameNum));
                    jsonstr.AppendFormat("\"readnum\":\"{0}\"", EncodeByEscape.GetEscapeStr(entity.ReadNum));
                    jsonstr.Append("},");
                    count++;
                }
                jsonstr.Append("\"Success\":1}");
            }
            return jsonstr.ToString();
        }
    }


    public class WeiboStyleDis : ResultDisplay
    {
        public override string Display()
        {
            int count = 1;
            StringBuilder jsonstr = new StringBuilder();
            if (list != null && list.Count > 0)
            {
                jsonstr.Append("{");
                string totalcount = list[0].TotalCount;
                jsonstr.AppendFormat("\"totalcount\":{0},", totalcount);
                foreach (IdolNewsEntity entity in list)
                {
                    jsonstr.AppendFormat("\"entity_{0}\":", count);
                    jsonstr.Append("{");
                    jsonstr.AppendFormat("\"title\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.Title));
                    jsonstr.AppendFormat("\"href\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.Href));
                    jsonstr.AppendFormat("\"url\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.WeiboUrl));
                    jsonstr.AppendFormat("\"time\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.TimeStr));
                    jsonstr.AppendFormat("\"site\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.SiteName));
                    jsonstr.AppendFormat("\"author\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.Author));

                    jsonstr.AppendFormat("\"replynum\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.ReplyNum));
                    jsonstr.AppendFormat("\"clicknum\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.ClickNum));
                    jsonstr.AppendFormat("\"docid\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.DocId));
                    jsonstr.AppendFormat("\"content\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.Content));
                    jsonstr.AppendFormat("\"allcontent\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.AllContent));
                    jsonstr.AppendFormat("\"weight\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.weight));
                    jsonstr.Append("},");
                    count++;
                }
                jsonstr.Append("\"Success\":1}");
            }
            return jsonstr.ToString();
        }
    }
    public class EightStyleDis : ResultDisplay
    {

        public override string Display()
        {
            int count = 1;
            StringBuilder jsonstr = new StringBuilder();
            if (list != null && list.Count > 0)
            {
                jsonstr.Append("[");
                string totalcount = list[0].TotalCount;

                foreach (IdolNewsEntity entity in list)
                {

                    jsonstr.Append("{");
                    jsonstr.AppendFormat("\"title\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.Title));
                    jsonstr.AppendFormat("\"href\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.Href));
                    jsonstr.AppendFormat("\"time\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.TimeStr));
                    jsonstr.AppendFormat("\"site\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.SiteName));
                    jsonstr.AppendFormat("\"author\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.Author));
                    jsonstr.AppendFormat("\"replynum\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.ReplyNum));
                    jsonstr.AppendFormat("\"clicknum\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.ClickNum));
                    jsonstr.AppendFormat("\"docid\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.DocId));
                    jsonstr.AppendFormat("\"content\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.Content));
                    jsonstr.AppendFormat("\"allcontent\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.AllContent));
                    jsonstr.AppendFormat("\"forwardnum\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.ForwardNum));
                    jsonstr.AppendFormat("\"authorurl\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.AuthorUrl));
                    jsonstr.AppendFormat("\"sourceurl\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.SourceUrl));
                    jsonstr.AppendFormat("\"weight\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.weight));
                    jsonstr.AppendFormat("\"conturl\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.ContUrl));
                    jsonstr.AppendFormat("\"samenum\":\"{0}\"", EncodeByEscape.GetEscapeStr(entity.SameNum));
                    jsonstr.Append("},");
                    count++;
                }
                jsonstr.Append("{");
                jsonstr.AppendFormat("\"Success\":1,\"totalcount\":{0}", totalcount);
                jsonstr.Append("}]");
            }
            return jsonstr.ToString();
        }


    }
    public class VideoStyleDis : ResultDisplay
    {
        public override string Display()
        {
            int count = 1;
            StringBuilder jsonstr = new StringBuilder();
            if (list != null && list.Count > 0)
            {
                jsonstr.Append("{");
                string totalcount = list[0].TotalCount;
                jsonstr.AppendFormat("\"totalcount\":{0},", totalcount);
                jsonstr.Append("\"data\":[");
                foreach (IdolNewsEntity entity in list)
                {
                    if (entity.VideoFilePath == null)
                    {
                        continue;
                    }
                    jsonstr.Append("{");
                    jsonstr.AppendFormat("\"href\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.Href));
                    jsonstr.AppendFormat("\"title\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.Title));
                    jsonstr.AppendFormat("\"datetime\":\"{0}\",", entity.StrDatetime);
                    jsonstr.AppendFormat("\"videoId\":\"{0}\",", entity.VideoId);
                    jsonstr.AppendFormat("\"siteName\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.SiteName));
                    jsonstr.AppendFormat("\"videofilePath\":\"{0}\",", entity.VideoFilePath.Trim());
                    jsonstr.AppendFormat("\"videoFileCount\":\"{0}\",", entity.VideoFileCount);
                    jsonstr.AppendFormat("\"videoShortLink\":\"{0}\",", entity.VideoShortLink);
                    jsonstr.AppendFormat("\"videoThumbPic\":\"{0}\",", entity.VideoThumbPic);
                    jsonstr.AppendFormat("\"docid\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.DocId));
                    jsonstr.AppendFormat("\"content\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.Content));
                    jsonstr.AppendFormat("\"allcontent\":\"{0}\"", EncodeByEscape.GetEscapeStr(entity.AllContent));
                    //jsonstr.AppendFormat("\"creContent\":\"{0}\"", EncodeByEscape.GetEscapeStr(entity.DreContent.Trim()));
                    jsonstr.Append("},");
                }
                jsonstr.Remove(jsonstr.Length - 1, 1);
                jsonstr.Append("],");
                jsonstr.Append("\"Success\":1}");
            }
            return jsonstr.ToString();
        }
        /*
        public override string Display(bool backbone)
        {
            int count = 1;
            StringBuilder jsonstr = new StringBuilder();
            if (list != null && list.Count > 0)
            {
                jsonstr.Append("{");
                string totalcount = list[0].TotalCount;
                jsonstr.AppendFormat("\"totalcount\":{0},", totalcount);
                jsonstr.Append("\"data\":[");
                foreach (IdolNewsEntity entity in list)
                {
                    jsonstr.Append("{");
                    jsonstr.AppendFormat("\"href\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.Href));
                    jsonstr.AppendFormat("\"title\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.Title));
                    jsonstr.AppendFormat("\"datetime\":\"{0}\",", entity.StrDatetime);
                    jsonstr.AppendFormat("\"videoId\":\"{0}\",", entity.VideoId);
                    jsonstr.AppendFormat("\"siteName\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.SiteName));
                    jsonstr.AppendFormat("\"videofilePath\":\"{0}\",", entity.VideoFilePath);
                    jsonstr.AppendFormat("\"videoFileCount\":\"{0}\",", entity.VideoFileCount);
                    jsonstr.AppendFormat("\"videoShortLink\":\"{0}\",", entity.VideoShortLink);
                    jsonstr.AppendFormat("\"videoThumbPic\":\"{0}\",", entity.VideoThumbPic);
                    jsonstr.AppendFormat("\"docid\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.DocId));
                    jsonstr.AppendFormat("\"content\":\"{0}\",", EncodeByEscape.GetEscapeStr(entity.Content));
                    jsonstr.AppendFormat("\"allcontent\":\"{0}\"", EncodeByEscape.GetEscapeStr(entity.AllContent));
                    //jsonstr.AppendFormat("\"creContent\":\"{0}\"", EncodeByEscape.GetEscapeStr(entity.DreContent.Trim()));
                    jsonstr.Append("},");
                }
                jsonstr.Remove(jsonstr.Length - 1, 1);
                jsonstr.Append("],");
                jsonstr.Append("\"Success\":1}");
            }
            return jsonstr.ToString();
        }
        */
    }


    public class DisplayFactory
    {
        public static ResultDisplay GetDisStyle(DisplayType disType)
        {
            switch (disType)
            {
                case DisplayType.FirstDisplay:
                    return new FirstStyleDis();
                case DisplayType.SecondDisplay:
                    return new SecoendStyleDis();
                case DisplayType.ThirdStyleDis:
                    return new ThirdStyleDis();
                case DisplayType.ForthStyleDis:
                    return new ForthStyleDis();
                case DisplayType.FifthStyleDis:
                    return new FifthStyleDIs();
                case DisplayType.SixthStyleDis:
                    return new JsonStyleDis();
                case DisplayType.WeiboStyleDis:
                    return new WeiboStyleDis();
                case DisplayType.EightStyleDis:
                    return new EightStyleDis();
                case DisplayType.VideoStyleDis:
                    return new VideoStyleDis();
                default:
                    return null;
            }
        }
    }

    public enum DisplayType
    {
        FirstDisplay = 1,
        SecondDisplay = 2,
        ThirdStyleDis = 3,
        ForthStyleDis = 4,
        FifthStyleDis = 5,
        SixthStyleDis = 6,
        WeiboStyleDis = 7,
        EightStyleDis = 8,
        VideoStyleDis = 9

    }
}
