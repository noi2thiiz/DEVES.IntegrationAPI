<%@ WebHandler Language="C#" Class="redirectUrl" %>


#define TRACE
using System;
using System.IO;
using System.Web;
using System.Xml.Serialization;
using System.Web.Caching;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Net;

public class redirectUrl : IHttpHandler {

    public void ProcessRequest(HttpContext context)
    {
        context.Response.BufferOutput = true;
        //context.Response.ContentType = "text/html";
       // context.Response.Redirect("http://192.168.10.33/MainRedirect.asp?Menu=6&Target=3&Topic=32",true);
        //context.Response.Redirect(Sites[site].AbsoluteUri, true);
           // return Redirect("http://www.example.com");

        //string site = context.Request.QueryString["site"];
        var url = new Uri("http://192.168.10.33/MainRedirect.asp?Menu=6&Target=3&Topic=32");
       
            context.Response.Redirect(url.AbsoluteUri, true);
        

    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}
