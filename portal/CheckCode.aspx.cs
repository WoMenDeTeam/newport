using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

public partial class CheckCode : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["CheckCode"] = null;
        CreateCheckCodeImage(CreateRandomCode(5));
    }
    private string CreateRandomCode(int codeCount)
    {
        string allChar = "2,3,4,5,6,7,8,9,0,1";
        string[] allCharArray = allChar.Split(',');
        string randomCode = "";
        int temp = -1;
        Random rand = new Random();
        for (int i = 0; i < codeCount; i++)
        {
            if (temp != -1)
            {
                rand = new Random(i * temp * ((int)DateTime.Now.Ticks));
            }
            int t = rand.Next(10);
            if (temp == t)
            {
                return CreateRandomCode(codeCount);
            }
            temp = t;
            randomCode += allCharArray[t];
        }
        Session["CheckCode"] = randomCode;
        return randomCode;
    }

    //private string GenerateCheckCode(int length)
    //{
    //    char code;
    //    string checkCode = String.Empty;

    //    Random random = new Random();

    //    for (int i = 0; i < length; i++)
    //    {
    //        int number = random.Next();

    //        if (number % 3 == 0)
    //            code = (char)('0' + (char)(number % 10));
    //        else if (number % 3 == 1)
    //            code = (char)('A' + (char)(number % 26));
    //        else
    //            code = (char)('a' + (char)(number % 26));
    //        checkCode += code.ToString();
    //    }
    //    Session.Add("CheckCode", checkCode.ToLower());
    //    return checkCode;
    //}

    private void CreateCheckCodeImage(string checkCode)
    {
        if (checkCode == null || checkCode.Trim() == String.Empty)
            return;

        System.Drawing.Bitmap image = new System.Drawing.Bitmap((int)Math.Ceiling((checkCode.Length * 16.0)), 25);
        Graphics g = Graphics.FromImage(image);

        try
        {
            //生成随机生成器
            Random random = new Random();

            //清空图片背景色
            g.Clear(Color.White);

            //画图片的背景噪音线
            for (int i = 0; i < 3; i++)
            {
                int x1 = random.Next(image.Width);
                int x2 = random.Next(image.Width);
                int y1 = random.Next(image.Height);
                int y2 = random.Next(image.Height);

                g.DrawLine(new Pen(Color.Black), x1, y1, x2, y2);
            }

            Font font = new System.Drawing.Font("Arial", 15, (System.Drawing.FontStyle.Bold), GraphicsUnit.Point);
            System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);
            g.DrawString(checkCode, font, brush, 5, 1, StringFormat.GenericTypographic);

            //画图片的前景噪音点
            for (int i = 0; i < 100; i++)
            {
                int x = random.Next(image.Width);
                int y = random.Next(image.Height);

                image.SetPixel(x, y, Color.FromArgb(random.Next(255), random.Next(255), random.Next(255)));
            }

            //画图片的边框线
            g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            Response.ClearContent();
            Response.ContentType = "image/Gif";
            Response.BinaryWrite(ms.ToArray());
        }
        finally
        {
            g.Dispose();
            image.Dispose();
        }
    }
}
