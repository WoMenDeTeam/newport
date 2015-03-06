using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Demo.DAL;
using Demo.Util;
using IdolACINet;
using System.Configuration;

namespace Demo.BLL
{
    public abstract class query
    {
        public string CategoryID;
        public string Text;
        public string MinDate;
        public string MaxDate;
        public string Sentiment;
        public string SortType;
        public int MinScore;
        public int Start;
        public int PageSize;
        public string QueryText;
        public Connection conn =  new Connection(ConfigurationManager.AppSettings["IdolHttp"], 9000);
        public IdolNewsEntity.IdolNewsDao Dao = new IdolNewsEntity.IdolNewsDao();
        public abstract IList<IdolNewsEntity> DoQuery();

        public string GetHtmlStr(string timeStr)
        {
            DateTime time = Convert.ToDateTime(timeStr);
            string str = time.ToString("dd-MM-yyyy").Replace("-", "/");
            return str;
        }
    }

    public class CategoryQuery : query
    {
        public override IList<IdolNewsEntity> DoQuery()
        {
            IList<IdolNewsEntity> entitList = new List<IdolNewsEntity>();
            try
            {
                CategoryQueryCommand query = new CategoryQueryCommand();
                query.Category = Convert.ToInt64(CategoryID);
                if (!string.IsNullOrEmpty(Sentiment))
                {
                    if (Sentiment == "n")
                    {
                        query.SetParam(QueryParams.FieldText, "STRINGALL{" + Sentiment + "}:C1");
                    }
                    else
                    {
                        query.SetParam(QueryParams.FieldText, "STRINGALL{" + Sentiment + "}:C1");
                    }                    
                }
                if (!string.IsNullOrEmpty(QueryText))
                {
                    query.SetParam("QueryText", QueryText);
                }
                query.Databases = ConfigurationManager.AppSettings["DATABASE"];
                query.SetParam("params", "Sort,MinScore,Combine");
                query.SetParam("values", SortType + "," + MinScore + ",DRETITLE");
                query.TotalResults = true;
                query.Start = Start;
                int End = PageSize;               
                query.NumResults = End;
                query.SetParam("Predict", "false");
                query.PrintFields = "MYSITENAME,DREDATE";
                XmlDocument xmldoc = conn.Execute(query).Data;
                if (xmldoc != null)
                {
                    entitList = Dao.GetNewsList(xmldoc);
                }
            }
            catch (Exception ex)
            {
                //LogWriter.WriteErrLog("category查询出错：" + ex.ToString());                
            }
            return entitList;
        }
    }

    public class CommonQuery : query
    {
        public override IList<IdolNewsEntity> DoQuery()
        {
            IList<IdolNewsEntity> entityList = new List<IdolNewsEntity>();
            try
            {
                QueryCommand query = new QueryCommand();
                query.Text = Text;
                if (Sentiment == "n")
                {
                    query.SetParam(QueryParams.FieldText, "STRINGALL{" + Sentiment + "}:C1");
                }
                else
                {
                    query.SetParam(QueryParams.FieldText, "STRINGALL{" + Sentiment + "}:C1");
                }
                query.SetParam(QueryParams.DatabaseMatch, ConfigurationManager.AppSettings["DATABASE"]);
                query.Start = Start;
                query.MaxResults = Start + PageSize - 1;
                //query.MinDate = Convert.ToDateTime(MinDate);
                //query.MaxDate = Convert.ToDateTime(MaxDate);
                query.SetParam(QueryParams.Sort, SortType);
                query.SetParam(QueryParams.Print, QueryParamValue.Fields);
                query.SetParam(QueryParams.PrintFields, "MYSITENAME,DREDATE,AUTHOR,WEBSITE,WEBSITENAME");
                query.SetParam(QueryParams.Combine, "DRETITLE");
                query.SetParam(QueryParams.TotalResults, QueryParamValue.True);
                query.SetParam("summary", "context");
                query.SetParam("Characters", "152");
                query.MinScore = MinScore;
                XmlDocument xmldoc = conn.Execute(query).Data;
                if (xmldoc != null)
                {
                    entityList = Dao.GetNewsList(xmldoc);
                }
            }
            catch (Exception ex)
            {
                //LogWriter.WriteErrLog("commonquery出错：" + ex.ToString());
            }
            return entityList;
        }
    }

    public class SpecialQuery : query
    {
        public override IList<IdolNewsEntity> DoQuery()
        {
            IList<IdolNewsEntity> entityList = new List<IdolNewsEntity>();
            try
            {
                QueryCommand query = new QueryCommand();
                query.Text = "*";
                if (Sentiment == "n")
                {
                    query.SetParam(QueryParams.FieldText, "(STRINGALL{" + Text + "}:MYKEYWORD + AND + STRINGALL{" + Sentiment + "}:SENTIMENT)");
                }
                else
                {
                    query.SetParam(QueryParams.FieldText, "(STRINGALL{" + Text + "}:MYKEYWORD + AND + STRINGALL{" + Sentiment + "}:MYSRCTYPE)");
                } 
                query.SetParam(QueryParams.DatabaseMatch, "shandong");
                query.Start = Start;
                query.MaxResults = Start + PageSize - 1;
                //query.MinDate = Convert.ToDateTime(MinDate);
                //query.MaxDate = Convert.ToDateTime(MaxDate);
                query.SetParam(QueryParams.Sort, "date");               
                query.SetParam(QueryParams.Print, QueryParamValue.Fields);
                query.SetParam(QueryParams.PrintFields, "MYSITENAME,DREDATE");
                query.SetParam(QueryParams.Combine, "DRETITLE");
                query.SetParam(QueryParams.TotalResults, QueryParamValue.True);
                query.MinScore = MinScore;
                XmlDocument xmldoc = conn.Execute(query).Data;
                if (xmldoc != null)
                {
                    entityList = Dao.GetNewsList(xmldoc);
                }
            }
            catch (Exception ex)
            {
                //LogWriter.WriteErrLog("specialquery出错：" + ex.ToString());
            }
            return entityList;
        }
    }

    public class QueryFactory
    {
        public static query GetQueryStyle(string Type)
        {
            switch (Type)
            {
                case "categoryquery":
                    return new CategoryQuery();
                case "commonquery":
                    return new CommonQuery();
                case "specialquery":
                    return new SpecialQuery();
                default:
                    return null;
            }
        }
    }
}
