using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using DEVES.IntegrationAPI.Core.Helper;

namespace DEVES.IntegrationAPI.WebApi.Templates
{
    public class BaseApiController:ApiController
    {
        public string GetTransactionId()
        {
           
            if (string.IsNullOrEmpty(Request.Properties["TransactionID"].ToStringOrEmpty()))
            {
             
                Request.Properties["TransactionID"] = Guid.NewGuid().ToString();
            }

            return Request.Properties["TransactionID"].ToString();
            
        }

     

    }
}