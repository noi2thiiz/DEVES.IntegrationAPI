using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using DEVES.IntegrationAPI.WebApi.Core.DataAdepter;

namespace DEVES.IntegrationAPI.WebApi.Controllers.Tests
{
    public class TestControllerBase
    {
        protected string PostMessage(string apiName, string json)
        {

            string endpoint = "https://crmappdev.deves.co.th/proxy/xml.ashx?http://192.168.8.121/inquiryAPI/api/" + apiName;
            var client = new RESTClient(endpoint);
            var result = client.Execute(json);

            var jss = new JavaScriptSerializer();

            return result.Content;
        }
    }
}
