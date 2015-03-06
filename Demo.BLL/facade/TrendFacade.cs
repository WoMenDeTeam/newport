using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Demo.DAL;
using Demo.Util;

namespace Demo.BLL
{
    public class TrendFacade
    {
        private static readonly TRENDDATAEntity.TRENDDATADAO dao = new TRENDDATAEntity.TRENDDATADAO();

        private static IList<TRENDDATAEntity> GetList(string categoryid, string starttime, string endtime)
        {
            return dao.Find(categoryid, starttime, endtime);
        }

        private static DateTime GetDate(long categoryid, int type)
        {
            return dao.GetDate(categoryid, type);
        }
        public static string GetTrendJson(string categoryid, string starttime, string endtime)
        {
            IList<TRENDDATAEntity> list = GetListByWhere(categoryid, starttime, endtime);
            int displayCount = 30;
            IList<string> categorieslist = new List<string>();
            StringBuilder ArticleCountJson = new StringBuilder();
            StringBuilder oneTag = new StringBuilder();
            StringBuilder twoTag = new StringBuilder();
            StringBuilder threeTag = new StringBuilder();
            StringBuilder categories = new StringBuilder();
            StringBuilder allDate = new StringBuilder();
            oneTag.Append("[");
            twoTag.Append("[");
            threeTag.Append("[");
            categories.Append("[");
            allDate.Append("[");
            foreach (TRENDDATAEntity entity in list)
            {
                string timestr = entity.DATE.Value.ToString("yyyy/MM/dd");
                if (categorieslist.Count <= displayCount && !categorieslist.Contains(timestr))
                {
                    categorieslist.Add(timestr);
                }
                int type = entity.TAG.Value;
                long count = entity.ARTICLECOUNT.Value;
                if (categorieslist.Contains(timestr))
                {
                    switch (type)
                    {
                        case 1:
                            oneTag.AppendFormat("{0},", count);
                            break;
                        case 2:
                            twoTag.AppendFormat("{0},", count);
                            break;
                        case 3:
                            threeTag.AppendFormat("{0},", count);
                            break;
                    }
                }
            }

            for (int i = 0; i < categorieslist.Count; i++)
            {
                if ((displayCount - i) % 10 == 0)
                {
                    categories.AppendFormat("\"{0}\",", categorieslist[i]);
                }
                else
                {
                    categories.Append("\" \",");
                }
                allDate.AppendFormat("\"{0}\",", categorieslist[i]);
            }
            categories.Remove(categories.Length - 1, 1);
            allDate.Remove(allDate.Length - 1, 1);
            oneTag.Remove(oneTag.Length - 1, 1);
            twoTag.Remove(twoTag.Length - 1, 1);
            threeTag.Remove(threeTag.Length - 1, 1);
            oneTag.Append("]");
            twoTag.Append("]");
            threeTag.Append("]");
            categories.Append("]");
            allDate.Append("]");
            ArticleCountJson.Append("{\"success\":1,\"data\":[{\"name\": \"新闻\",\"data\":");
            ArticleCountJson.Append(oneTag);
            ArticleCountJson.Append("}, {\"name\": \"博客\",\"data\":");
            ArticleCountJson.Append(twoTag);
            ArticleCountJson.Append("},{\"name\": \"论坛\",\"data\":");
            ArticleCountJson.Append(threeTag + "}],\"categories\":");
            ArticleCountJson.Append(categories + ",\"alldate\":" + allDate + "}");
            return ArticleCountJson.ToString();
        }
        public static string GetTrendStr(string categoryid, string starttime, string endtime)
        {
            IList<TRENDDATAEntity> list = GetList(categoryid, starttime, endtime);
            IList<string> categorieslist = new List<string>();
            IList<long> newsCountList = new List<long>();
            IList<long> blogCountList = new List<long>();
            IList<long> bbsCountList = new List<long>();
            foreach (TRENDDATAEntity entity in list)
            {
                string timestr = entity.DATE.Value.ToString("yyyy/MM/dd");
                if (!categorieslist.Contains(timestr))
                {
                    categorieslist.Add(timestr);
                }
                int type = entity.TAG.Value;
                long count = entity.ARTICLECOUNT.Value;
                if (type == 1)
                {
                    newsCountList.Add(count);
                }
                else if (type == 2)
                {
                    blogCountList.Add(count);
                }
                else if (type == 3)
                {
                    bbsCountList.Add(count);
                }
            }
            ChartParams param = new ChartParams();
            param.Caption = null;
            param.FormatNumberScale = "0";
            param.AnchorRadius = "3";
            param.BaseFontSize = "12";
            param.NumberSuffix = "%c6%aa";
            IList<string> categories = GetCategoriesByTime(starttime, endtime, categorieslist, categoryid);
            Dictionary<string, IList<string>> datadict = new Dictionary<string, IList<string>>();
            datadict.Add("新闻", GetValueByTime(starttime, endtime, newsCountList, categoryid));
            datadict.Add("博客", GetValueByTime(starttime, endtime, blogCountList, categoryid));
            datadict.Add("论坛", GetValueByTime(starttime, endtime, bbsCountList, categoryid));
            return MSLineChart.GetChartXmlStr(param, categories, datadict);
        }
        private static IList<TRENDDATAEntity> GetListByWhere(string categoryid, string starttime, string endtime)
        {
            StringBuilder strWhere = new StringBuilder();
            strWhere.AppendFormat(" CATEGORYID={0}", categoryid);
            if (!string.IsNullOrEmpty(starttime))
            {
                strWhere.AppendFormat("AND \"DATE\">=to_date('{0}','yyyy-MM-dd')", starttime);
            }
            if (!string.IsNullOrEmpty(endtime))
            {
                strWhere.AppendFormat("AND \"DATE\"<=to_date('{0}','yyyy-MM-dd')", endtime);
            }
            strWhere.Append("ORDER BY \"DATE\" DESC");
            return dao.Find(strWhere.ToString());
        }
        private static IList<string> GetCategoriesByTime(string starttime, string endtime, IList<string> categorieslist, string categoryid)
        {
            IList<string> list = new List<string>();
            int dayspan = GetDaysSpan(starttime, endtime, categoryid);
            int count = 1;
            string categorystr = string.Empty;
            foreach (string category in categorieslist)
            {
                if (count == 1)
                {
                    categorystr = category;
                }
                if (count == dayspan && dayspan > 1)
                {
                    categorystr = categorystr + "-" + category;
                    list.Add(categorystr);
                    count = 0;
                    categorystr = string.Empty;
                }
                if (count == dayspan && dayspan == 1)
                {
                    list.Add(categorystr);
                    count = 0;
                    categorystr = string.Empty;
                }
                count++;
            }
            return list;
        }

        private static IList<string> GetValueByTime(string starttime, string endtime, IList<long> datalist, string categoryid)
        {
            IList<string> list = new List<string>();
            int dayspan = GetDaysSpan(starttime, endtime, categoryid);
            int count = 1;
            long datacount = 0;
            foreach (int category in datalist)
            {
                if (count == 1)
                {
                    datacount = datacount + category;
                }
                if (count == dayspan && dayspan > 1)
                {
                    datacount = datacount + category;
                    list.Add(datacount.ToString());
                    count = 0;
                    datacount = 0;
                }
                if (count == dayspan && dayspan == 1)
                {
                    list.Add(datacount.ToString());
                    count = 0;
                    datacount = 0;
                }
                count++;
            }
            return list;
        }

        private static int GetDays(DateTime starttime, DateTime endtime)
        {
            TimeSpan ts_start = new TimeSpan(starttime.Ticks);     //当前时间的 TimeSpan 结构对象；
            TimeSpan ts_end = new TimeSpan(endtime.Ticks);    //当前时间的 TimeSpan 结构对象；

            TimeSpan ts_diff = ts_start.Subtract(ts_end).Duration();   // 两时间相差的TimeSpan
            int days = ts_diff.Days;
            return days;
        }

        private static int GetDaysSpan(string starttime, string endtime, string categoryid)
        {
            DateTime fromtime = GetDate(Convert.ToInt64(categoryid), 1);
            DateTime totime = GetDate(Convert.ToInt64(categoryid), 2);
            if (!string.IsNullOrEmpty(starttime))
            {
                fromtime = Convert.ToDateTime(starttime);
            }
            if (!string.IsNullOrEmpty(endtime))
            {
                totime = Convert.ToDateTime(endtime);
            }
            int days = GetDays(fromtime, totime);
            if (days <= 20)
            {
                return 1;
            }
            else if (days > 20 && days <= 40)
            {
                return 2;
            }
            else if (days > 40 && days <= 60)
            {
                return 3;
            }
            else if (days > 60 && days <= 90)
            {
                return 4;
            }
            else if (days > 90 && days <= 110)
            {
                return 5;
            }
            else if (days > 110 && days <= 130)
            {
                return 6;
            }
            else
            {
                return 7;
            }
        }
    }
}
