<%@ WebHandler Language = "C#" Class="proxy" %>

#define TRACE
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


    /// <summary>
    /// Summary description for redirect
    /// </summary>
    public class redirect : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            context.Response.Redirect("http://www.microsoft.com/gohere/look_esp.htm");
            
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
