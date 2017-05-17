using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class RequestCleansingIdController:BaseApiController
    {

      
        public IHttpActionResult RequestCleansingId(CLSInquiryCorporateClientInputModel input)
        {
            if (!ModelState.IsValid)
            {

                // search personal form cls


                // update  CleansingId to crm
                string cleansingId = "";
                updateCleansingIdToCRM(input, cleansingId);


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

        

        public void updateCleansingIdToCRM(CLSInquiryCorporateClientInputModel input,string cleansingId)
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