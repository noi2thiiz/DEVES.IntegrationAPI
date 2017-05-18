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
        /// <param name="refCode"></param>
        /// <param name="clientType"> C or P</param>
        /// <param name="cleansingId"> </param>
        /// <returns></returns>
        public bool Execute(string guid , string refCode, string clientType, string cleansingId)
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

                if (string.IsNullOrEmpty(guid))
                {
                    if (string.IsNullOrEmpty(refCode))
                    {
                        return false;

                    }
                    guid = RetrieveClientGuid(refCode, clientType);
                }
               

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
                        Contact retrievedPersonal = (Contact)_serviceProxy.Retrieve(Contact.EntityLogicalName, _accountId, new Microsoft.Xrm.Sdk.Query.ColumnSet(true));
                        if (null != retrievedPersonal)
                        {
                            retrievedPersonal.pfc_cleansing_cusormer_profile_code = cleansingId;
                           _serviceProxy.Update(retrievedPersonal);
                        }
                        
                    }
                    else if (clientType == "C")
                    {
                        _accountId = new Guid(guid);
                        Account retrievedAccount = (Account) _serviceProxy.Retrieve(Account.EntityLogicalName,
                            _accountId, new Microsoft.Xrm.Sdk.Query.ColumnSet(true));
                        if (null != retrievedAccount)
                        {
                            retrievedAccount.pfc_cleansing_cusormer_profile_code = cleansingId;
                            retrievedAccount.pfc_temp_ref_code = "";
                            _serviceProxy.Update(retrievedAccount);
                        }
                        

                       
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

        public string RetrieveClientGuid(string refCode,string clientType) 
        {
            var connection = new CrmServiceClient(ConfigurationManager.ConnectionStrings["CRM_DEVES"]
                .ConnectionString);

            _serviceProxy = connection.OrganizationServiceProxy;
            ServiceContext svcContext = new ServiceContext(_serviceProxy);

            if (clientType=="P")
            {
                var query = from c in svcContext.ContactSet
                    where (c.pfc_cleansing_cusormer_profile_code == null || c.pfc_cleansing_cusormer_profile_code == "")
                          && (c.ContactId == new Guid(""))

                    select c;
                Contact personal = query.FirstOrDefault<Contact>();
                return personal?.ContactId.ToString();
            }
            if(clientType == "C") 
            {
                var query = from c in svcContext.AccountSet
                    where (c.pfc_cleansing_cusormer_profile_code == null || c.pfc_cleansing_cusormer_profile_code == "")
                          && (c.pfc_temp_ref_code == refCode)

                    select c;
                Account corporate = query.FirstOrDefault<Account>();
                return corporate?.AccountId.ToString();
            }

            return "";

        }
    }
}