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
using DEVES.IntegrationAPI.WebApi.Logic;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class RequestCleansingIdController:BaseApiController
    {

        OrganizationServiceProxy _serviceProxy;
        private Guid _accountId;
        [HttpPost]
        public IHttpActionResult Post([FromBody]CRMRequestCleansingIdDataInputModel input)
        {
            if (!ModelState.IsValid)
            {
                //invalid input
                return Ok(new OutputGenericDataModel<object>
                {
                    code = AppConst.CODE_INVALID_INPUT,
                    message = AppConst.MESSAGE_INVALID_INPUT,
                    transactionDateTime = DateTime.Now,
                    transactionId = GetTransactionId()
                });
            }


            // search personal form cls
            var cleansingId = CleansingClientService.Instance.GetCleansingId(input);
            if (!string.IsNullOrEmpty(cleansingId))
            {
                var cmd = new buzReqClsId();
                var success = cmd.Execute(input, cleansingId);
                if (success)
                {
                    return Ok(new OutputGenericDataModel<object>
                    {
                        code = AppConst.CODE_SUCCESS,
                        message = AppConst.MESSAGE_SUCCESS,
                        transactionDateTime = DateTime.Now,
                        transactionId = GetTransactionId(),
                        data = new
                        {
                            input,
                            cleansingId
                        }
                    });
                }
            }
            else
            {
                return Ok(new OutputGenericDataModel<object>
                {
                    code = AppConst.CODE_FAILED,
                    message = "Cannot create cleansing ",
                    transactionDateTime = DateTime.Now,
                    transactionId = GetTransactionId()
                });
            }


            //error
            return Ok(new OutputGenericDataModel<object>
            {
                code = AppConst.CODE_FAILED,
                message = AppConst.MESSAGE_INTERNAL_ERROR,
                transactionDateTime = DateTime.Now,
                transactionId = GetTransactionId()
            });
        }

    }
}