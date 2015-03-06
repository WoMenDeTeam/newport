using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo.BLL
{
    public class MSLineChart
    {
        public static string GetChartXmlStr(ChartParams param, IList<string> categoriesList, Dictionary<string, IList<string>> dataList)
        {
            StringBuilder xmlstr = new StringBuilder();
            xmlstr.Append(param.GetHeadXMLStr());
            xmlstr.Append(GetCategoriesXMLStr(categoriesList));
            xmlstr.Append(GetAllDataSetStr(dataList));
            xmlstr.Append("</graph>");
            return xmlstr.ToString();
        }

        private static string GetCategoriesXMLStr(IList<string> categoriesList) {
            StringBuilder xmlstr = new StringBuilder();
            xmlstr.Append("<categories>");
            foreach (string key in categoriesList) {
                xmlstr.AppendFormat("<category name='{0}' />", key);
            }
            xmlstr.Append("</categories>");
            return xmlstr.ToString();
        }

        private static string GetAllDataSetStr(Dictionary<string, IList<string>> dataList)
        {
            StringBuilder xmlstr = new StringBuilder();
            int count = 1;
            foreach (string key in dataList.Keys) {
                xmlstr.Append(GetLineNodeStr(count, key));
                xmlstr.Append(GetDataSetStr(dataList[key]));
                xmlstr.Append("</dataset>");
                count++;
            }
            return xmlstr.ToString();
        }

        private static string GetDataSetStr(IList<string> dataList)
        {
            StringBuilder xmlstr = new StringBuilder();
            foreach (string data in dataList) {
                xmlstr.AppendFormat("<set value='{0}' />", data);
            }
            return xmlstr.ToString();
        }

        private static string GetLineNodeStr(int type,string name){
            switch (type) { 
                case 1:
                    return "<dataset seriesName='" + name + "' color='1D8BD1' anchorBorderColor='1D8BD1' anchorBgColor='1D8BD1'>";
                case 2:
                    return "<dataset seriesName='" + name + "' color='F1683C' anchorBorderColor='F1683C' anchorBgColor='F1683C'>";
                case 3:
                    return "<dataset seriesName='" + name + "' color='2AD62A' anchorBorderColor='2AD62A' anchorBgColor='2AD62A'>";
                default:
                    return "";
            }
        }
    }
}
