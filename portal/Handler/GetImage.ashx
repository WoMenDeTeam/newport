<%@ WebHandler Language="C#" Class="GetImage" %>

using System;
using System.IO;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Configuration;

public class GetImage : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        string action = context.Request.QueryString["action"];
        string SourceJobname = context.Request.QueryString["SourceJobname"];
        string enddate = context.Request.QueryString["enddate"];
        string idolServer = ConfigurationManager.AppSettings["IdolACIPort"];
        
        string requestUrl = string.Format("{2}/action={0}&SourceJobname={1}", action, SourceJobname,idolServer);
        if (!string.IsNullOrEmpty(enddate))
        {
            requestUrl += "&enddate=" + enddate;
        }

        context.Response.ContentType = "image/Png";
        MemoryStream ms = new MemoryStream();
        GetMemoryStreamFromImage(ms, requestUrl);
        if (ms != null)
        {
            context.Response.BinaryWrite(ms.ToArray());
            ms.Dispose();
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    private static void GetMemoryStreamFromImage(MemoryStream MemStream, string requestUrl)
    {
        try
        {
            var myRequest = System.Net.WebRequest.Create(requestUrl);
            myRequest.Timeout = 1000000;
            var myResponse = myRequest.GetResponse();
            var myResponseStream = myResponse.GetResponseStream();

            var img = new Bitmap(myResponseStream);

            //建立一个输出位图
            var imgOutput = new Bitmap(512, 512, PixelFormat.Format16bppRgb555);

            //根据以上位图建立一个新图象；
            var graph = Graphics.FromImage(imgOutput);
            graph.SmoothingMode = SmoothingMode.AntiAlias;

            //按指定大小和位置绘制
            graph.DrawImage(img, new Rectangle(0, 0, 512, 512), new Rectangle(0, 0, img.Width, img.Height), GraphicsUnit.Pixel);

            //存入内存流
            imgOutput.Save(MemStream, ImageFormat.Png);

            graph.Dispose();
            img.Dispose();
            imgOutput.Dispose();
        }
        catch (Exception exec)
        {
            //throw exec;
        }
    }
}