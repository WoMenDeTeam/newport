using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Owc11;
using Demo.Util;

namespace Demo.BLL
{
    public class BarChart
    {
        public static string CreateImg(IList<string> DataName, IList<string> Data, string XTitle, string YTitle, string ReportTitle, int ImgWidth, int ImgHeight)
        {
            StringBuilder JsonStr = new StringBuilder();
            try
            {
                string strValue = "";
                string strCateory = "";
                //循环取得数据并格式化为OWC10需要的格式,(加'\t') 
                foreach (string name in DataName)
                {
                    strCateory += name + '\t';
                }
                foreach (string num in Data)
                {
                    strValue += num + '\t';
                }

                ChartSpaceClass mySpace = new ChartSpaceClass();//创建ChartSpace对象来放置图表 
                mySpace.Interior.Color = "#54c59a";//设置mySpace的背景色


                ChChart myChart = mySpace.Charts.Add(0);//在ChartSpace对象中添加图表,Add方法返回chart对象 
                myChart.Type = ChartChartTypeEnum.chChartTypeBarClustered;//指定图表的类型为折线图 

                myChart.Border.Color = "black"; //设置myChart的边框颜色为白色
                myChart.PlotArea.Interior.Color = "White";//设置myChart的绘图区背景色为白色
                myChart.PlotArea.Border.set_Weight(LineWeightEnum.owcLineWeightThin);//由于不能用weight=来赋值，所用调用set_Weight来设置粗细               

                //给定X\Y轴的图示说明 
                if (!string.IsNullOrEmpty(XTitle))
                {
                    myChart.Axes[0].HasTitle = true;
                    myChart.Axes[0].Title.Caption = XTitle; //横轴名称 
                    myChart.Axes[0].Title.Font.Size = 9; //横轴名称的字体大小
                    myChart.Axes[0].Title.Font.Color = "black"; //横轴名称字体的颜色  
                }
                if (!string.IsNullOrEmpty(YTitle))
                {
                    myChart.Axes[1].HasTitle = true;
                    myChart.Axes[1].Title.Caption = YTitle; //纵轴名称 
                    myChart.Axes[1].Title.Font.Size = 9;
                    myChart.Axes[1].Title.Font.Color = "black";
                }
                myChart.Axes[0].Font.Size = 12;
                myChart.Axes[1].Font.Size = 12;
                myChart.Axes[1].HasMajorGridlines = true;//设置Y轴的主网格线为True
                myChart.Axes[1].MajorGridlines.Line.DashStyle = ChartLineDashStyleEnum.chLineLongDash;//设置网格线的样式为虚线形式
                myChart.Axes[1].MajorGridlines.Line.Color = "Gray";//设置网格线的颜色为灰色         
                //添加一个series(序列) 
                myChart.SeriesCollection.Add(0);
                //给定series的名字 
                //myChart.SeriesCollection[0].SetData(ChartDimensionsEnum.chDimSeriesNames, (int)ChartSpecialDataSourcesEnum.chDataLiteral, "购买");
                //给定series的分类 
                myChart.SeriesCollection[0].SetData(ChartDimensionsEnum.chDimCategories, (int)ChartSpecialDataSourcesEnum.chDataLiteral, strValue);
                //给定具体值 
                myChart.SeriesCollection[0].SetData(ChartDimensionsEnum.chDimValues, (int)ChartSpecialDataSourcesEnum.chDataLiteral, strCateory);

                //设置折线的颜色
                myChart.SeriesCollection[0].Line.Color = "#a5d2b5";

                //设置折线的样式
                myChart.SeriesCollection[0].Line.DashStyle = ChartLineDashStyleEnum.chLineSolid;
                //设置折线的weight值（粗细）
                myChart.SeriesCollection[0].Line.set_Weight(LineWeightEnum.owcLineWeightThin);

                //加入数据标签
                myChart.SeriesCollection[0].DataLabelsCollection.Add();
                //设置数据标签的显示为true                
                myChart.SeriesCollection[0].DataLabelsCollection[0].HasValue = true;
                //设置数据标签的样式为正方形
                myChart.SeriesCollection[0].Marker.Style = ChartMarkerStyleEnum.chMarkerStyleSquare;
                //设置数据标签形体的大小
                myChart.SeriesCollection[0].Marker.Size = 0;
                //设置数据标签形体的背景色
                myChart.SeriesCollection[0].Interior.Color = "#83abed";
                //输出成GIF文件 
                string OutPutPath = "../dataImg/";
                string fileName = ReportTitle + "zdtj" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".gif";
                string strAbsolutePath = System.Web.HttpContext.Current.Server.MapPath(OutPutPath) + fileName;
                string filePath = OutPutPath + fileName;
                mySpace.ExportPicture(strAbsolutePath, "GIF", ImgWidth, ImgHeight); //输出图表 
                return filePath;
            }
            catch(Exception e) {
                //LogWriter.WriteErrLog("生成趋势图出错：" + e.ToString());
                return "";
            }            
        }
    }
}
