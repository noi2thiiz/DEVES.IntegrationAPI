<%@ WebHandler Language="C#" Class="proxyLink" %>

/*
 * DotNet proxy client.
 *
 * Version 1.1.1-beta
 * See https://github.com/Esri/resource-proxy for more information.
 *
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


        WebClient objWebClient = new WebClient();
        String strURL = @"http://192.168.10.33/NewProduct.asp?token=INi9SdhXRvpZO7mYI0ViUlflmqc9I9GO";
        String strRequest;

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
