<%@ WebHandler Language="C#" Class="proxyLink" %>

/*
 * DotNet proxy client.

 */

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
using System.Text;
using System.Xml;

public class proxyLink : IHttpHandler {

    public void ProcessRequest(HttpContext context)
    {
        
        string remoteHost = "http://192.168.10.33";

        WebClient objWebClient = new WebClient();

        var linkType = context.Request.QueryString["linkType"];
        var env = context.Request.QueryString["env"];
        var token = "INi9SdhXRvpZO7mYI0ViUlflmqc9I9GO";//context.Request.QueryString["token"];

        if (env=="pro")
        {
            remoteHost = "http://192.168.10.3";
        }
        //if (linkType=="Information")
        //{
        //    remoteHost = "http://192.168.10.33";
        //}
        var strURL = remoteHost+"/"+linkType+".asp?token="+token;
        string strRequest;

        UTF8Encoding objUTF8 = new UTF8Encoding ();
        Encoding objEncoding = Encoding .GetEncoding( "Windows-874" );

        Byte[] byteRequest = objWebClient.DownloadData(strURL);
        strRequest = objEncoding.GetString(byteRequest);

        HttpResponse response = context.Response;
        response.AppendHeader("Access-Control-Allow-Origin", "*");
        response.ContentType = "text/html;charset=UTF-8";
        response.ContentEncoding=Encoding.GetEncoding("UTF-8");
        //;charset=windows-874
        response.StatusCode = 200;
        //custom status description for when the rate limit has been exceeded

        //this displays our customized error messages instead of IIS's custom errors
        response.TrySkipIisCustomErrors = true;
        response.Write(strRequest);
        response.Flush();





        context.ApplicationInstance.CompleteRequest();
    }

    public bool IsReusable {
        get { return true; }
    }

}
