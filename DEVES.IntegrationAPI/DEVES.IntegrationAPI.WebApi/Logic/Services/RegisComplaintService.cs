using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using DEVES.IntegrationAPI.Model.EWI;
using DEVES.IntegrationAPI.Model.RegComplaint;

namespace DEVES.IntegrationAPI.WebApi.Logic.Services
{
    public class RegisComplaintService : BaseEwiServiceProxy
    {


        public RegisComplaintService(string globalTransactionID, string controllerName = "") : base(globalTransactionID,
            controllerName)
        {
            serviceName = "RegisComplaintService";
            systemName = "EWI";
          
        }

        public EWIResponseContent Execute(Request_RegComplaintModel input)
        {
            serviceEndpoint = AppConfig.Instance.Get(CommonConstant.EWI_ENDPOINT_RequestRegComplaint);
            Console.WriteLine("serviceEndpoint=" + serviceEndpoint);
            var result = SendRequest(input, serviceEndpoint);

            var jss = new JavaScriptSerializer();
            var contentObj = jss.Deserialize<EWIResponse>(result.Content);
            return contentObj?.content;
        }
    }
}