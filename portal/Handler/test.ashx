﻿<%@ WebHandler Language="C#" Class="test" %>

using System;
using System.Web;

public class test : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        context.Response.Write("Hello World");
        context.Response.Close();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}