using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.WebApi.Templates;
using DEVES.IntegrationAPI.Model.CRM;
using DEVES.IntegrationAPI.WebApi.Logic.Services;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Tooling.Connector;
using System.Configuration;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class RequestCleansingIdController:BaseApiController
    {

        OrganizationServiceProxy _serviceProxy;
        private Guid _accountId;

        public IHttpActionResult Post([FromBody]CRMRequestCleansingIdDataInputModel input)
        {
            string clsId = "C2017-7357";
            updateCleansingIdToCRM(input, clsId);
            if (ModelState.IsValid)
            {

                // search personal form cls
                var result = CleansingClientService.Instance.GetCleansingId(input);
                if (result.success)
                {
                    foreach (CLSInquiryPersonalClientOutputModel item in result.content.data)
                    {
                        // update  CleansingId to crm
                        string cleansingId = item.cleansing_id;
                        updateCleansingIdToCRM(input, cleansingId);
                    }
                }



                //สำเร็จไม่สำเร็จก็จะ return ok 
                return Ok(new OutputGenericDataModel<object>
                {
                    code = AppConst.CODE_SUCCESS,
                    message = AppConst.MESSAGE_SUCCESS,
                    transactionDateTime = DateTime.Now,
                    transactionId = GetTransactionId()

                });
            }
            else
            {
                return Ok(new OutputGenericDataModel<object>
                {
                    code = AppConst.CODE_INVALID_INPUT,
                    message = AppConst.MESSAGE_INVALID_INPUT,
                    transactionDateTime = DateTime.Now,
                    transactionId = GetTransactionId()

                });
            }
            

        }





        public void updateCleansingIdToCRM(CRMRequestCleansingIdDataInputModel input, string cleansingId)
        {
            try
            {
                var connection = new CrmServiceClient(ConfigurationManager.ConnectionStrings["CRM_DEVES"].ConnectionString);

                bool check = connection.IsReady;
                if (!check) // check is ready = true
                {
                    return;// should return something that notice system about CRM is not connect properly
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
                        && (c.GenderCode == null || c.GenderCode == new Microsoft.Xrm.Sdk.OptionSetValue(Int32.Parse(string.IsNullOrEmpty(input.gendercode)? "100000000": input.gendercode)))
                        && (c.Salutation == null || c.Salutation == input.titlePersonalname)
                            select c;

                Contact personal = query.FirstOrDefault<Contact>();
                try
                {
                    _accountId = new Guid(personal.Id.ToString());
                }
                catch (Exception e)
                {
                    // หาข้อมูลไม่พบ
                }

                Contact retrievedIncident = (Contact)_serviceProxy.Retrieve(Contact.EntityLogicalName, _accountId, new Microsoft.Xrm.Sdk.Query.ColumnSet(true));
                try
                {
                    retrievedIncident.pfc_cleansing_cusormer_profile_code = cleansingId;

                    _serviceProxy.Update(retrievedIncident);
                }
                catch (Exception e)
                {

                }

            }
            catch (Exception e)
            {

            }

        }

    }
}