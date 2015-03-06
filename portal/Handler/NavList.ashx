<%@ WebHandler Language="C#" Class="NavList" %>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Demo.BLL;
using Demo.DAL;
using Demo.Util;

public class NavList : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}


