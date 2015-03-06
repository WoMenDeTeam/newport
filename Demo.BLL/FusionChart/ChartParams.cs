using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo.BLL
{
    public class ChartParams
    {
        /// <summary>
        /// Chart类型，默认为BrokenLine
        /// </summary>
        public string ChartType = "BrokenLine";
        /// <summary>
        /// 标题
        /// </summary>        
        public string Caption = string.Empty;
        /// <summary>
        /// X轴名称
        /// </summary>
        public string XAxisName = string.Empty;
        /// <summary>
        /// Y轴名称
        /// </summary>
        public string YAxisName = string.Empty;
        /// <summary>
        /// 小数位的位数，默认为0
        /// </summary>
        public string DecimalPrecision = "0";        
        /// <summary>
        /// 画布边框厚度，默认为1
        /// </summary>        
        public string CanvasBorderThickness = "1";        
        /// <summary>
        /// 画布边框颜色，默认为a5d1ec
        /// </summary>        
        public string CanvasBorderColor = "a5d1ec";       
        /// <summary>
        /// 图标字体大小，默认为12
        /// </summary>        
        public string BaseFontSize = "12";        
        /// <summary>
        /// 是否格式化数据（如3000为3K），默认为0(否)
        /// </summary>        
        public string FormatNumberScale = "0";        
        /// <summary>
        /// 是否显示横向坐标轴(x轴)标签名称，默认为1(是)
        /// </summary>        
        public string ShowNames = "1";        
        /// <summary>
        /// 是否在图表显示对应的数据值，默认为0（否）
        /// </summary>        
        public string ShowValues = "0";
        /// <summary>
        /// 是否在横向网格带交替的颜色，默认为1（是）
        /// </summary>        
        public string ShowalTernatehGridColor = "1";        
        /// <summary>
        /// 横向网格带交替的颜色，默认为ff5904
        /// </summary>        
        public string AlternatehGridColor = "ff5904"; 
        /// <summary>
        /// 水平分区线颜色，默认为ff5904
        /// </summary>
        public string DivLineColor = "ff5904";        
        /// <summary>
        /// 水平分区线透明度，默认为20
        /// </summary>        
        public string DivLineAlpha = "20";
        /// <summary>
        /// 横向网格带的透明度，默认为5
        /// </summary>        
        public string AlternatehGridAlpha = "5";   
        /// <summary>
        /// 折线节点半径，默认为0
        /// </summary>        
        public string AnchorRadius = "0";
        /// <summary>
        /// 是否逗号分隔数字，默认为不分隔
        /// </summary>   
        public string FormatNumber = "0";
        /// <summary>
        /// 显示数字后缀
        /// </summary>
        public string NumberSuffix = string.Empty;        
        /// <summary>
        /// 横轴数据显示方式，默认为竖型显示
        /// </summary>
        public string RotateNames = "1";
        /// <summary>
        /// 获取头部XML
        /// </summary>
        /// <returns></returns>
        public string GetHeadXMLStr()
        {
            StringBuilder headxmlstr = new  StringBuilder();
            headxmlstr.Append("<?xml version='1.0' encoding='gb2312'?>");
            headxmlstr.Append("<graph");
            if (!string.IsNullOrEmpty(Caption))
            {
                headxmlstr.AppendFormat(" caption='{0}' ", Caption);
            }
            if (!string.IsNullOrEmpty(XAxisName))
            {
                headxmlstr.AppendFormat(" xAxisName='{0}' ", XAxisName);
            }
            if (!string.IsNullOrEmpty(YAxisName))
            {
                headxmlstr.AppendFormat(" yAxisName='{0}' ", YAxisName);
            }
            if (!string.IsNullOrEmpty(DecimalPrecision))
            {
                headxmlstr.AppendFormat(" decimalPrecision='{0}' ", DecimalPrecision);
            }
            if (!string.IsNullOrEmpty(CanvasBorderThickness))
            {
                headxmlstr.AppendFormat(" canvasBorderThickness='{0}' ", CanvasBorderThickness);
            }
            if (!string.IsNullOrEmpty(CanvasBorderColor))
            {
                headxmlstr.AppendFormat(" canvasBorderColor='{0}' ", CanvasBorderColor);
            }
            if (!string.IsNullOrEmpty(BaseFontSize))
            {
                headxmlstr.AppendFormat(" baseFontSize='{0}' ", BaseFontSize);
            }
            if (!string.IsNullOrEmpty(FormatNumber)) {
                headxmlstr.AppendFormat(" formatNumber='{0}' ", FormatNumber);
            }
            if (!string.IsNullOrEmpty(FormatNumberScale))
            {
                headxmlstr.AppendFormat(" formatNumberScale='{0}' ", FormatNumberScale);
            }
            if (!string.IsNullOrEmpty(ShowNames))
            {
                headxmlstr.AppendFormat(" showNames='{0}' ", ShowNames);
            }
            if (!string.IsNullOrEmpty(ShowValues))
            {
                headxmlstr.AppendFormat(" showValues='{0}' ", ShowValues);
            }
            if (!string.IsNullOrEmpty(ShowalTernatehGridColor))
            {
                headxmlstr.AppendFormat(" showAlternateHGridColor='{0}' ", ShowalTernatehGridColor);
            }
            if (!string.IsNullOrEmpty(AlternatehGridColor))
            {
                headxmlstr.AppendFormat(" AlternateHGridColor='{0}' ", AlternatehGridColor);
            }
            if (!string.IsNullOrEmpty(DivLineColor))
            {
                headxmlstr.AppendFormat(" divLineColor='{0}' ", DivLineColor);
            }
            if (!string.IsNullOrEmpty(DivLineAlpha))
            {
                headxmlstr.AppendFormat(" divLineAlpha='{0}' ", DivLineAlpha);
            }
            if (!string.IsNullOrEmpty(AlternatehGridAlpha))
            {
                headxmlstr.AppendFormat(" alternateHGridAlpha='{0}' ", AlternatehGridAlpha);
            }
            if (!string.IsNullOrEmpty(NumberSuffix))
            {
                headxmlstr.AppendFormat(" numberSuffix='{0}' ", NumberSuffix);
            }
            if (!string.IsNullOrEmpty(RotateNames))
            {
                headxmlstr.AppendFormat(" rotateNames='{0}' ", RotateNames);
            }
            
            if (ChartType == "BrokenLine") {
                if (!string.IsNullOrEmpty(AnchorRadius))
                {
                    headxmlstr.AppendFormat(" anchorRadius='{0}' ", AnchorRadius);
                }
            }
            headxmlstr.Append(">");
            return headxmlstr.ToString();
        }
         
    }

    
}
