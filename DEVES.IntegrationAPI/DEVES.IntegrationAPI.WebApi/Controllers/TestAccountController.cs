using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using System.Configuration;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;

using DEVES.IntegrationAPI.Model.CRM;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class TestAccountController : ApiController
    {
        public object Post([FromBody]object value)
        {
            using (var connection = new CrmServiceClient(ConfigurationManager.ConnectionStrings["CRM_DEVES"].ConnectionString))
            {
                using (var serviceProxy = connection.OrganizationServiceProxy)
                {
                    serviceProxy.EnableProxyTypes();
                    ServiceContext sc = new ServiceContext(serviceProxy);

                    var countries = (from ctry in sc.pfc_master_countriesSet
                                     where ctry.statecode == pfc_master_countriesState.Active
                                     select new CRM_MasterCountry { Id=  ctry.Id
                                     , Code = ctry.pfc_master_countries_code
                                     , Name = ctry.pfc_master_countries_name
                                     , ctryPolisy = ctry.pfc_ref_polisy_descpf_t3645
                                     , ctrySAP = ctry.pfc_ref_sap }).ToList<CRM_MasterCountry>();
                    return Ok(countries);


                    //var account = (from a in sc.AccountSet
                    //               where a.Name.Contains("เจริญ") && a.pfc_tax_no
                    //               select new Account { Name = a.Name }).FirstOrDefault();
                    //return Ok(account);
                }
            }

        }
    }
}