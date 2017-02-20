using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEVES.IntegrationAPI.WebApi.Models.WebDialer
{
    public class MakeCallResultModel:IServiceResult
    {
       
        public string Code { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public object Data { get;  set; }
        public string CodeRef { get;  set; }
    }
}