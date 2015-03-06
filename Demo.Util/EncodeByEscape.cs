using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.JScript;

namespace Demo.Util
{
    public static class EncodeByEscape
    {
        public static string GetEscapeStr(string Str)
        {
            if (!string.IsNullOrEmpty(Str))
            {
                return GlobalObject.escape(Str);
            }
            else
            {
                return "";
            }
        }
        public static string GetUnEscapeStr(string Str)
        {
            if (!string.IsNullOrEmpty(Str))
            {
                return GlobalObject.unescape(Str);
            }
            else {
                return "";
            }
        }
        
        public static string GetOtherEscapeStr(string str)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byStr = System.Text.Encoding.UTF8.GetBytes(str); //默认是System.Text.Encoding.Default.GetBytes(str)
            for (int i = 0; i < byStr.Length; i++)
            {
                int ch = byStr[i];
                if ('A' <= ch && ch <= 'Z')
                {// 'A'..'Z' : as it was        
                    sb.Append((char)ch);
                }
                else if ('a' <= ch && ch <= 'z')
                {// 'a'..'z' : as it was          
                    sb.Append((char)ch);
                }
                else if ('0' <= ch && ch <= '9')
                {// '0'..'9' : as it was          
                    sb.Append((char)ch);
                }
                else
                {
                    sb.Append(@"%" + System.Convert.ToString(byStr[i], 16));
                }
            }
            return sb.ToString();
        }
    }
}
