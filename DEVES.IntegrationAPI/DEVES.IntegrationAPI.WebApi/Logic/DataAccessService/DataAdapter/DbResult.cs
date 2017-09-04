using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.DataAdapter
{
    public class DbResult
    {
        public Dictionary<string, object> ReqParams { get; set; }
        internal List<object> FieldInfo { get; set; }
        public string Code { get; set; }

        public string Message { get; set; }
        public int Total { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; }

        public List<object> Data { get; set; }

        public int Fieldcount { get; set; }
        public bool Success { get; set; }
        public int Count { get; set; }

    }
}