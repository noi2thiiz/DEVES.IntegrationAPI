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

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzReqClsId 
    {

        OrganizationServiceProxy _serviceProxy;
        private Guid _accountId;

        public bool Execute(CRMRequestCleansingIdDataInputModel input, string cleansingId)
        {
            try
            {
                var connection = new CrmServiceClient(ConfigurationManager.ConnectionStrings["CRM_DEVES"].ConnectionString);

                bool check = connection.IsReady;
                if (!check) // check is ready = true
                {
                    return false; // should return something that notice system about CRM is not connect properly
                }

                _serviceProxy = connection.OrganizationServiceProxy;
                ServiceContext svcContext = new ServiceContext(_serviceProxy);

                var query = from c in svcContext.ContactSet
                            where (c.pfc_cleansing_cusormer_profile_code == null || c.pfc_cleansing_cusormer_profile_code == "")
                        && (c.FirstName == null || c.FirstName == input.firstname)
                        && (c.LastName == null || c.LastName == input.lastname)
                        && (c.pfc_citizen_id == null || c.pfc_citizen_id == input.citizenId)
                        && (c.pfc_telephone1 == null || c.pfc_telephone1 == input.telephone1)
                        && (c.pfc_telephone2 == null || c.pfc_telephone2 == input.telephone2)
                        && (c.pfc_telephone3 == null || c.pfc_telephone3 == input.telephone3)
                        && (c.pfc_fax == null || c.pfc_fax == input.fax)
                        && (c.pfc_MobilePhone1 == null || c.pfc_MobilePhone1 == input.telephone1)
                        && (c.EMailAddress1 == null || c.EMailAddress1 == input.emailaddress1)
                        && (c.GenderCode == new Microsoft.Xrm.Sdk.OptionSetValue(Int32.Parse(input.gendercode)))
                        && (c.Salutation == null || c.Salutation == input.titlePersonalname)
                            select c;

                Contact personal = query.FirstOrDefault<Contact>();
                _accountId = new Guid(personal.Id.ToString());

                Contact retrievedIncident = (Contact)_serviceProxy.Retrieve(Contact.EntityLogicalName, _accountId, new Microsoft.Xrm.Sdk.Query.ColumnSet(true));
                try
                {
                    retrievedIncident.pfc_cleansing_cusormer_profile_code = cleansingId;

                    _serviceProxy.Update(retrievedIncident);
                }
                catch (Exception e)
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