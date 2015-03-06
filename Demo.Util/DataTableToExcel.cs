using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using Microsoft.Office.Interop.Excel;

namespace Demo.Util
{
    public class DataTableToExcel
    {
        public static void ToExcel(System.Data.DataTable dt, string filepth)
        {
            // 1. 创建Excel应用程序对象的一个实例，相当于我们从开始菜单打开Excel应用程序。
            ApplicationClass xlsApp = new ApplicationClass();

            if (xlsApp == null)
            {
                //对此实例进行验证，如果为null则表示运行此代码的机器可能未安装Excel
            }
        }
        public static void ExportExcel(string title, System.Data.DataTable dt, Dictionary<string, string> cloms, string path, int c1, int c2, int c3, int c4)
        {
            if (dt == null || dt.Rows.Count == 0) return;
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            if (xlApp == null)
            {
                return;
            }
            System.Globalization.CultureInfo CurrentCI = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            Microsoft.Office.Interop.Excel.Workbooks workbooks = xlApp.Workbooks;
            Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
            Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];
            Microsoft.Office.Interop.Excel.Range range;
            long totalCount = dt.Rows.Count;
            long rowRead = 0;
            float percent = 0;
            worksheet.Name = title.Length > 31 ? title.Substring(0, 30) : title;
            range = worksheet.get_Range(worksheet.Cells[c1, c2], worksheet.Cells[c3, c4]);
            range.Merge(false);
            range.Value2 = title;
            range.Interior.ColorIndex = 16;
            range.Font.Bold = false;
            //表头列文字设置
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                string cName = dt.Columns[i].ColumnName;
                worksheet.Cells[c3 + 1, i + 1] = cName;
                range = (Microsoft.Office.Interop.Excel.Range)worksheet.Cells[c3 + 1, i + 1];
                range.Interior.ColorIndex = 15;
                range.Font.Bold = true;
            }

            //列表类容设置
            c3++;
            for (int r = 0; r < dt.Rows.Count; r++)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    string cName = dt.Columns[i].ColumnName;
                    worksheet.Cells[r + c3 + 1, i + 1] = dt.Rows[r][i].ToString().Replace("•", ".");
                }
                rowRead++;
                percent = ((float)(100 * rowRead)) / totalCount;
            }
            worksheet.Columns.EntireColumn.AutoFit();
            workbook.Saved = true;
            workbook.SaveCopyAs(string.Format("{0}\\{1}.xlsx", path, title));
            //xlApp.Visible = true;
            workbook.Close(true, Type.Missing, Type.Missing);
            workbook = null;
            xlApp.Quit();
            xlApp = null;
        }
    }
}
