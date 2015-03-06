using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Demo.DAL;
using Demo.Util;
using System.Data;

namespace Demo.BLL
{
    public class WEIBOFacade
    {
        private static readonly WEIBOEntity.WEIBODAO dao = new WEIBOEntity.WEIBODAO();

        private static int GetPagerCount(string strWhere) {
            return dao.GetPagerRowsCount(strWhere);
        }

        private static DataTable GetPagerDT(string where, string orderBy, int pageSize, int pageNumber)
        {            
            return dao.GetPager(where, orderBy, pageSize, pageNumber);
        }

        public static string GetPagerJsonStr(string where, string orderBy, int pageSize, int start) {
            int totalcount = GetPagerCount(where);
            int pageNumber = start / pageSize + 1;
            DataTable dt = GetPagerDT(where, orderBy, pageSize, pageNumber);
            StringBuilder jsonstr = new StringBuilder();
            if (dt.Rows.Count > 0)
            {
                jsonstr.Append("{\"totalcount\":\"").Append(totalcount).Append("\",");
                jsonstr.Append("\"entitylist\":{");
                int count = 1;
                foreach (DataRow row in dt.Rows)
                {
                    jsonstr.AppendFormat("\"entity_{0}\":", count);
                    jsonstr.Append("{");

                    jsonstr.AppendFormat("\"id\":\"{0}\",", row["ID"]);
                    jsonstr.AppendFormat("\"contenthtml\":\"{0}\",", EncodeByEscape.GetEscapeStr(row["CONTENTHTML"].ToString()));
                    jsonstr.AppendFormat("\"authorname\":\"{0}\",", EncodeByEscape.GetEscapeStr(row["AUTHORNAME"].ToString()));
                    jsonstr.AppendFormat("\"authorurl\":\"{0}\",", EncodeByEscape.GetEscapeStr(row["AUTHORURL"].ToString()));
                    jsonstr.AppendFormat("\"forwardnum\":\"{0}\",", row["FORWARDNUM"]);
                    jsonstr.AppendFormat("\"sourceurl\":\"{0}\",", EncodeByEscape.GetEscapeStr(row["SOURCEURL"].ToString()));
                    jsonstr.AppendFormat("\"commentnum\":\"{0}\",", row["COMMENTNUM"]);
                    jsonstr.AppendFormat("\"url\":\"{0}\",", EncodeByEscape.GetEscapeStr(row["URL"].ToString()));
                    string publish_datetime = row["PUBLISHTIME"].ToString();
                    string publish_datetimestr = string.Empty;
                    if (!string.IsNullOrEmpty(publish_datetime))
                    {
                        publish_datetimestr = (Convert.ToDateTime(publish_datetime)).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    jsonstr.AppendFormat("\"publishtime\":\"{0}\",", EncodeByEscape.GetEscapeStr(publish_datetimestr));
                    jsonstr.AppendFormat("\"createtime\":\"{0}\",", EncodeByEscape.GetEscapeStr(row["CREATETIME"].ToString()));
                    jsonstr.AppendFormat("\"updatetime\":\"{0}\"", EncodeByEscape.GetEscapeStr(row["UPDATETIME"].ToString()));
                    jsonstr.Append("},");
                    count++;
                }
                jsonstr.Append("\"SuccessCode\":1}}");
                
            }
            return jsonstr.ToString();
        }

        private static DataTable GetStaticDtByTime() {
            return dao.GetStaticDtByTime();
        }

        public static string GetStaticStrByTime() {
            DataTable dt = GetStaticDtByTime();
            IList<string> categories = new List<string>();
            IList<string> datalist = new List<string>();
            if (dt != null && dt.Rows.Count > 0) {                
                foreach (DataRow row in dt.Rows) {
                    string time_str = row["PUBLISHTIME"].ToString();
                    string totalcount = row["TOTALCOUNT"].ToString();
                    if (!string.IsNullOrEmpty(time_str)) {
                        categories.Add(time_str);
                        datalist.Add(totalcount);
                    }
                }
                
            }
            ChartParams param = new ChartParams();
            param.Caption = null;
            param.FormatNumberScale = "0";
            param.AnchorRadius = "3";
            param.BaseFontSize = "12";
            param.NumberSuffix = "%c6%aa";
            Dictionary<string, IList<string>> datadict = new Dictionary<string, IList<string>>();
            datadict.Add("新浪微博", datalist);
            return MSLineChart.GetChartXmlStr(param, categories, datadict);
        }
    }
}
