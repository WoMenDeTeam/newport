using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Demo.Util;
using Microsoft.Office.Interop.Owc11;

namespace Demo.BLL
{
    public class TrendChart
    {
        /// <summary>
        /// 返回一个Json对象（包括图片路径以及各个热点的位置）
        /// </summary>
        /// <param name="DataName">X轴标识</param>
        /// <param name="Data">需要绘图的数据</param>
        /// <param name="XTitle">X轴的标题（为空不显示）</param>
        /// <param name="YTitle">Y轴的标题（为空不显示）</param>
        /// <param name="OutPutPath">图片输出的文件夹路径</param>
        /// <param name="ImgWidth">图片的宽度</param>
        /// <param name="ImgHeight">图片的高度</param>
        /// <returns></returns>
        public static string CreateImg(IList<string> DataName, IList<string> Data, string XTitle, string YTitle, string ReportTitle, int ImgWidth, int ImgHeight,string categoryid)
        {
            StringBuilder JsonStr = new StringBuilder();
            try
            {
                string strValue = "";
                string strCateory = "";
                //循环取得数据并格式化为OWC10需要的格式,(加'\t') 
                int NormNum = Convert.ToInt32(DataName.Count / 6);
                int count = 0;
                foreach (string name in DataName)
                {
                    if (count % NormNum == 0)
                    {
                        strCateory += name + "\t";
                    }else{
                        string str = string.Empty;
                        for(int i=0;i<count;i++){
                            str += "\n";
                        }
                        strCateory += str + "\t";
                    }
                    count++;
                }
                foreach (string num in Data)
                {
                    strValue += num + "\t";
                }

                ChartSpaceClass mySpace = new ChartSpaceClass();//创建ChartSpace对象来放置图表 
                mySpace.Interior.Color = "White";//设置mySpace的背景色


                ChChart myChart = mySpace.Charts.Add(0);//在ChartSpace对象中添加图表,Add方法返回chart对象 
                myChart.Type = ChartChartTypeEnum.chChartTypeLineMarkers;//指定图表的类型为折线图 

                myChart.Border.Color = "White"; //设置myChart的边框颜色为白色
                myChart.PlotArea.Interior.Color = "#a8dede";//设置myChart的绘图区背景色为白色
                myChart.PlotArea.Border.set_Weight(LineWeightEnum.owcLineWeightThin);//由于不能用weight=来赋值，所用调用set_Weight来设置粗细               

                //给定X\Y轴的图示说明 
                if (!string.IsNullOrEmpty(XTitle))
                {
                    myChart.Axes[0].HasTitle = true;
                    myChart.Axes[0].Title.Caption = XTitle; //横轴名称 
                    myChart.Axes[0].Title.Font.Size = 12; //横轴名称的字体大小
                    myChart.Axes[0].Title.Font.Color = "Black"; //横轴名称字体的颜色  
                }
                if (!string.IsNullOrEmpty(YTitle))
                {
                    myChart.Axes[1].HasTitle = true;
                    myChart.Axes[1].Title.Caption = YTitle; //纵轴名称 
                    myChart.Axes[1].Title.Font.Size = 12;
                    myChart.Axes[1].Title.Font.Color = "Black";
                }
                
                myChart.Axes[0].HasTickLabels = true;
                myChart.Axes[0].Font.Size = 12;
                myChart.Axes[1].HasMajorGridlines = true;//设置Y轴的主网格线为True
                myChart.Axes[1].MajorGridlines.Line.DashStyle = ChartLineDashStyleEnum.chLineLongDash;//设置网格线的样式为虚线形式
                myChart.Axes[1].MajorGridlines.Line.Color = "Black";//设置网格线的颜色为灰色      
                myChart.Axes[1].Font.Size = 12;
              
                //添加一个series(序列) 
                myChart.SeriesCollection.Add(0);
                //给定series的名字 
                //myChart.SeriesCollection[0].SetData(ChartDimensionsEnum.chDimSeriesNames, (int)ChartSpecialDataSourcesEnum.chDataLiteral, "购买");
                //给定series的分类 
                myChart.SeriesCollection[0].SetData(ChartDimensionsEnum.chDimCategories, (int)ChartSpecialDataSourcesEnum.chDataLiteral, strCateory);
                //给定具体值 
                myChart.SeriesCollection[0].SetData(ChartDimensionsEnum.chDimValues, (int)ChartSpecialDataSourcesEnum.chDataLiteral, strValue);

                //设置折线的颜色
                myChart.SeriesCollection[0].Line.Color = "Red";
                
                //设置折线的样式
                myChart.SeriesCollection[0].Line.DashStyle = ChartLineDashStyleEnum.chLineSolid;
                //设置折线的weight值（粗细）
                myChart.SeriesCollection[0].Line.set_Weight(LineWeightEnum.owcLineWeightThin);

                //加入数据标签
                myChart.SeriesCollection[0].DataLabelsCollection.Add();
                //设置数据标签的显示为true                
                myChart.SeriesCollection[0].DataLabelsCollection[0].HasValue = false;
                //设置数据标签的样式为正方形
                myChart.SeriesCollection[0].Marker.Style = ChartMarkerStyleEnum.chMarkerStyleSquare;
                //设置数据标签形体的大小
                myChart.SeriesCollection[0].Marker.Size = 1;                
                //设置数据标签形体的背景色
                myChart.SeriesCollection[0].Interior.Color = "Red";
                //输出成GIF文件 
                string OutPutPath = "../dataImg/";
                string fileName = ReportTitle + "zxt" + categoryid + DateTime.Now.ToString("yyyyMMddHHmmss") + ".gif";
                string strAbsolutePath = System.Web.HttpContext.Current.Server.MapPath(OutPutPath) + fileName;
                string filePath = OutPutPath + fileName;
                mySpace.ExportPicture(strAbsolutePath, "GIF", ImgWidth, ImgHeight); //输出图表 
                //创建GIF文件的相对路径并嵌入到JsonStr对象中 
                return filePath;

            }
            catch(Exception e) {
                //LogWriter.WriteErrLog("生成趋势图出错：" + e.ToString());
                return "";
            }
        }
        private static string getMapData(ChDataLabels CDList)
        {
            StringBuilder MapData = new StringBuilder();
            for (int i = 0; i < CDList.Count - 1; i++)
            {
                int XValue = (CDList[i].Left + CDList[i].Right) / 2;
                int YValue = CDList[i].Bottom;
                MapData.AppendFormat("\"DataList_{0}\":\"{1},{2},{3}\"", i.ToString(), XValue.ToString(), YValue.ToString(), "10");
                if (i < CDList.Count - 2)
                    MapData.Append(",");
            }

            return MapData.ToString();
        }
    }
}
