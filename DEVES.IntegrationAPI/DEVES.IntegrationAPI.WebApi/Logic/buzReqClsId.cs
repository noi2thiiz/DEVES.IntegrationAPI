using DEVES.IntegrationAPI.WebApi.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.CRM;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Tooling.Connector;
using System.Configuration;
using Microsoft.Xrm.Sdk;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzReqClsId 
    {

        OrganizationServiceProxy _serviceProxy;
        private Guid _accountId;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="clientType"> C or P</param>
        /// <param name="cleansingId"> </param>
        /// <returns></returns>
        public bool Execute(string guid , string clientType, string cleansingId)
        {
            try
            {
                var connection = new CrmServiceClient(ConfigurationManager.ConnectionStrings["CRM_DEVES"]
                    .ConnectionString);

                bool check = connection.IsReady;
                if (!check) // check is ready = true
                {
                    return false; // should return something that notice system about CRM is not connect properly
                }


                _serviceProxy = connection.OrganizationServiceProxy;
                ServiceContext svcContext = new ServiceContext(_serviceProxy);
                /*
                var query = from c in svcContext.ContactSet
                            where (c.pfc_cleansing_cusormer_profile_code == null || c.pfc_cleansing_cusormer_profile_code == "")
                        && (c.ContactId == new Guid(guid))
                       
                            select c;
                             Contact personal = query.FirstOrDefault<Contact>();
                 */

            }
            catch (Exception e)
            {
                return false;
            }

            try
                {
                    if (clientType=="P")
                    {
                        _accountId = new Guid(guid);
                        Contact retrievedIncident = (Contact)_serviceProxy.Retrieve(Contact.EntityLogicalName, _accountId, new Microsoft.Xrm.Sdk.Query.ColumnSet(true));

                         retrievedIncident.pfc_cleansing_cusormer_profile_code = cleansingId;

                        _serviceProxy.Update(retrievedIncident);
                    }
                    else if (clientType == "C")
                    {
                        _accountId = new Guid(guid);
                        Account retrievedIncident = (Account) _serviceProxy.Retrieve(Account.EntityLogicalName,
                            _accountId, new Microsoft.Xrm.Sdk.Query.ColumnSet(true));
                        retrievedIncident.pfc_cleansing_cusormer_profile_code = cleansingId;

                        _serviceProxy.Update(retrievedIncident);
                    }
                    else
                    {
                        return false;
                    }




                   
                }
                catch (Exception e)
                {
                    return false;
                }

            
           

            return true;
        }
    }
}