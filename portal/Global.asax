<%@ Application Language="C#" %>
<%@ Import Namespace="Demo.DAL" %>
<%@ Import Namespace="Demo.BLL" %>
<%@ Import Namespace="Demo.Util" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="log4net" %>


<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        log4net.Config.DOMConfigurator.Configure();
        try
        {
            TrieTree t = new TrieTree();
            string path = Server.MapPath("~/") + "ck.txt";
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            StreamReader read = new StreamReader(fs);
            read.BaseStream.Seek(0, SeekOrigin.Begin);
            string strLine = read.ReadLine();
            while (strLine != null)
            {
                string[] str = strLine.Split(' ');
                t.Insert(str[1], str[0]);
                strLine = read.ReadLine();
            }
            Application.Add("trieTree", t);


            Dictionary<string, string> dict = new Dictionary<string, string>();
            string Lpath = Server.MapPath("~/") + "name.txt";
            FileStream Lfs = new FileStream(Lpath, FileMode.Open, FileAccess.Read);
            StreamReader Lread = new StreamReader(Lfs);
            Lread.BaseStream.Seek(0, SeekOrigin.Begin);
            string LstrLine = Lread.ReadLine();
            while (LstrLine != null)
            {

                string[] str = LstrLine.Split(',');
                string key = str[1];
                string value = str[2];
                if (!dict.ContainsKey(key))
                {
                    dict.Add(key, value);
                }
                LstrLine = Lread.ReadLine();
            }
            Application.Add("ItemDict", dict);            
        }
        catch (Exception ex)
        {
            ILog logger = LogManager.GetLogger("InitTree");
            logger.Error(ex.Message);
        }
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //在应用程序关闭时运行的代码

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        //在出现未处理的错误时运行的代码
        
    }

    void Session_Start(object sender, EventArgs e) 
    {
        //在新会话启动时运行的代码

        Session.Timeout = 120;
    }

    void Session_End(object sender, EventArgs e) 
    {
        //在会话结束时运行的代码。 
        // 注意: 只有在 Web.config 文件中的 sessionstate 模式设置为
        // InProc 时，才会引发 Session_End 事件。如果会话模式 
        //设置为 StateServer 或 SQLServer，则不会引发该事件。

    }
       
</script>
