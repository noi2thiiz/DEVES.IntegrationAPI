using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.Polisy400;
using DEVES.IntegrationAPI.Model.RegPayeeCorporate;
using DEVES.IntegrationAPI.Model.SAP;
using DEVES.IntegrationAPI.WebApi.Templates;
using Microsoft.IdentityModel.Protocols.WSIdentity;

namespace DEVES.IntegrationAPI.WebApi.Logic.Services
{
    public class SAPCreateVendor : BaseEwiServiceProxy
    {
        public SAPCreateVendor(string globalTransactionID, string controllerName = "") : base(globalTransactionID, controllerName)
        {
            serviceName = "COMP_SAPCreateVendor";
            systemName = "SAP";
            serviceEndpoint = AppConfig.Instance.Get(CommonConstant.ewiEndpointKeySAPCreateVendor);
        }
        public SAPCreateVendorContentOutputModel Execute(SAPCreateVendorInputModel input)
        {

            var result = SendRequest(input, serviceEndpoint);

            var jss = new JavaScriptSerializer();
            var contentObj = jss.Deserialize<SAPCreateVendorOutputModel>(result.Content);
            return contentObj?.content;
        }

         public SAPCreateVendorContentOutputModel Execute(RegPayeeCorporateInputModel input)
       
        {
            Model.SAP.SAPCreateVendorInputModel SAPCreateVendorIn = new Model.SAP.SAPCreateVendorInputModel();
            SAPCreateVendorIn =
                (Model.SAP.SAPCreateVendorInputModel)TransformerFactory.TransformModel(input,
                    SAPCreateVendorIn);

          
            return Execute(SAPCreateVendorIn);
        }
    }
}