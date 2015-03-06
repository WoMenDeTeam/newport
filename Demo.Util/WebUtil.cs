using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Demo.Util
{
    public static class WebUtil
    {
        public static string GetEncodeUrl(string url) {
            if (!string.IsNullOrEmpty(url))
            {
                return System.Web.HttpUtility.UrlEncode(url);
            }
            else {
                return "";
            }
        }
    }
}
