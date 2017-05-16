using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using DEVES.IntegrationAPI.Model.EWI;
using DEVES.IntegrationAPI.Model.Polisy400;
using DEVES.IntegrationAPI.WebApi.Core.DataAdepter;
using DEVES.IntegrationAPI.WebApi.Templates;
using Microsoft.IdentityModel.Protocols.WSIdentity;

namespace DEVES.IntegrationAPI.WebApi.Logic.Services
{
    public class CleansingClientService:BaseProxyService
    {
        #region Singleton
        private static CleansingClientService _instance;

        private CleansingClientService()
        {
           
        }

        public static CleansingClientService Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = new CleansingClientService();

                return _instance;
            }
        }


        #endregion
        public BaseEWIResponseModel RemoveByCleansingId(string cleansingClientId, string clientType = "P")
        {
            
            var input = new 
            {
                cleansing_id = cleansingClientId
            };
            Console.WriteLine(input.ToJson());

            string endpointKey ;
            if (clientType == "P")
            {
                endpointKey = "EWI_ENDPOINT_CLSDeleteCLSPersonalClient";
                this.serviceName = "CLSDeleteCLSPersonalClient";
            }
            else
            {
                endpointKey = "EWI_ENDPOINT_CLSDeleteCLSCorporateClient";
                this.serviceName = "CLSDeleteCLSCorporateClient";
            }
            string endpoint = "https://crmapp.deves.co.th/proxy/xml.ashx?" + AppConfig.Instance.Get(endpointKey);


            var result = SendRequest(input, endpoint);
            if (result.StatusCode != HttpStatusCode.OK)
            {
                throw new InternalErrorException(result.Message);
            }
            var jss = new JavaScriptSerializer();
            var contentObj = jss.Deserialize<BaseEWIResponseModel>(result.Content);
            return contentObj;
        }
    }
}