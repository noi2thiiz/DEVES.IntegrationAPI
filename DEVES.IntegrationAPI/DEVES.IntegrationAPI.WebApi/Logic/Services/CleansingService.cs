using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using DEVES.IntegrationAPI.Model.Polisy400;
using DEVES.IntegrationAPI.WebApi.Core.DataAdepter;
using Microsoft.IdentityModel.Protocols.WSIdentity;

namespace DEVES.IntegrationAPI.WebApi.Logic.Services
{
    public class CleansingService
    {
        public COMPInquiryClientMasterOutputModel RemoveClientById(string cleansingClientId)
        {
            var input = new COMPInquiryClientMasterInputModel();
                input.cleansingId = cleansingClientId;
            var endpoint = AppConfig.Instance.Get("EWI_ENDPOINT_COMPInquiryClientMaster");
            var client = new RESTClient(endpoint);
            var result = client.Execute(input);
            if (result.StatusCode != HttpStatusCode.OK)
            {
                throw new InternalErrorException(result.Message);
            }
            var jss = new JavaScriptSerializer();
            var contentObj = jss.Deserialize<COMPInquiryClientMasterOutputModel>(result.Content);
            return contentObj;
        }
    }
}