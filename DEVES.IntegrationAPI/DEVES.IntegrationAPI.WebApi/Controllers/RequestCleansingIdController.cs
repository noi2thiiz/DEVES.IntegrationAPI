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

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class RequestCleansingIdController:BaseApiController
    {

      
        public IHttpActionResult RequestCleansingId(CRMRequestCleansingIdDataInputModel input)
        {
            if (ModelState.IsValid)
            {

                // search personal form cls
                var  result = CleansingClientService.Instance.GetCleansingId(input);
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
                    code = AppConst.CODE_INVALID_INPUT,
                    message = AppConst.MESSAGE_INVALID_INPUT,
                    transactionDateTime = DateTime.Now,
                    transactionId = GetTransactionId()

                });
            }
            return Ok(new OutputGenericDataModel<object>
            {
                code = AppConst.CODE_SUCCESS,
                message = AppConst.MESSAGE_SUCCESS,
                transactionDateTime = DateTime.Now,
                transactionId = GetTransactionId()

            });

        }

        



        public void updateCleansingIdToCRM(CRMRequestCleansingIdDataInputModel input,string cleansingId)
        {
            
              //pfc_title_personalid
              //  firstname
              //lastname
              //  gendercode
              //pfc_citizen_id
              //  pfc_telephone1
              //pfc_telephone2
              //  pfc_telephone3
              //pfc_fax
              //  pfc_moblie_phone1
              //emailaddress1
        }
    }
}