using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Demo.DAL.SQLEntity;
using System.Data;

namespace Demo.BLL
{
    /*文件注释
     *修改时间：2012-08-07
     *修改内容:
     *+++++++++++++++++++++++++++++++++++++++++++++++++
     *扩展DataTable方法
     *扩展方法ToJson(this DataTable dt,bool backBone)
     *扩展方法适应Bockbone的数据格式
     *++++++++++++++++++++++++++++++++++++++++++++++++
     */

    public static class EventsFacade
    {
        private static readonly EventsEntity.EventsDAO eventsdao = new EventsEntity.EventsDAO();
        private static readonly EventClueEntity.EventClueDAO eventcluedao = new EventClueEntity.EventClueDAO();
        private static readonly EventImgEntity.EventImgDAO eventimgdao = new EventImgEntity.EventImgDAO();
        private static readonly EventTopicEntity.EventTopicDAO eventtopicdao = new EventTopicEntity.EventTopicDAO();
        public static EventsEntity GetSingleEventByID(int id)
        {
            return eventsdao.FindById(id);
        }
        public static DataSet GetEventClueByEventID(int id)
        {
            return eventcluedao.GetDataSet(" EventId=" + id, null);
        }
        public static DataSet GetEventImgByEventID(int id)
        {
            return eventimgdao.GetDataSet(" EventId=" + id, null);
        }
        public static DataSet GetEventTopicByEventID(int id)
        {
            return eventtopicdao.GetDataSet(" EventId=" + id, null);
        }
        public static string ToJson(this DataTable dt)
        {
            StringBuilder jsonstr = new StringBuilder();
            int count = 1;
            string[] captions = new string[dt.Columns.Count];
            for (int i = 0; i != dt.Columns.Count; i++)
            {
                captions[i] = dt.Columns[i].Caption;
            }
            jsonstr.Append("{");
            foreach (DataRow row in dt.Rows)
            {
                jsonstr.AppendFormat("\"entity_{0}\":", count);
                jsonstr.Append("{");
                for (int i = 0; i != captions.Length; i++)
                {
                    jsonstr.AppendFormat("\"{0}\":\"{1}\",", captions[i], Demo.Util.EncodeByEscape.GetEscapeStr(row[captions[i]].ToString()));
                }
                jsonstr.Length = jsonstr.Length - 1;
                jsonstr.Append("},");
                count++;
            }
            jsonstr.Append("\"success\":1}");
            return jsonstr.ToString();
        }
        public static string ToJson(this DataTable dt, bool bockbone)
        {
            StringBuilder jsonstr = new StringBuilder();
            int count = 1;
            string[] captions = new string[dt.Columns.Count];
            for (int i = 0; i != dt.Columns.Count; i++)
            {
                captions[i] = dt.Columns[i].Caption;
            }
            jsonstr.Append("[");
            foreach (DataRow row in dt.Rows)
            {
                jsonstr.Append("{");
                for (int i = 0; i != captions.Length; i++)
                {
                    jsonstr.AppendFormat("\"{0}\":\"{1}\",", captions[i], Demo.Util.EncodeByEscape.GetEscapeStr(row[captions[i]].ToString()));
                }
                jsonstr.Length = jsonstr.Length - 1;
                jsonstr.Append("},");
                count++;
            }
            jsonstr.Remove(jsonstr.Length - 1, 1);
            jsonstr.Append("]");
            return jsonstr.ToString();
        }
    }

}
