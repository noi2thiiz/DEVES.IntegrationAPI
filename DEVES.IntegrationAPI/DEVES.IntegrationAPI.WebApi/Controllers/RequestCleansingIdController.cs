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
using DEVES.IntegrationAPI.WebApi.DataAccessService;
using DEVES.IntegrationAPI.WebApi.Logic;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{

    public class RequestCleansingIdController : BaseApiController
    {
        
        OrganizationServiceProxy _serviceProxy;
        private Guid _accountId;
        /// <summary>
        /// ใช้สำหรับ Quick create บนหน้าจอ CRM 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        
        public IHttpActionResult RequestCleansingId([FromBody]CRMRequestCleansingIdDataInputModel input)
        {
            /*
             * รับข้อมูลจาก Quick create จากนั้นนำข้อมูลไปสร้างในระบบ CLS หากซ้ำ ก็เอา Cleansing Id ที่ CLS ให้กลับมา
             * แล้วนำไป update ใน CRM ตาม GUID ที่หน้าจอส่งมา
             * *
             */



            string cleansingId = "";
            try
            {
                if (!ModelState.IsValid)
                {
                    //invalid input
                    throw new Exception(AppConst.MESSAGE_INVALID_INPUT);

                }
                //กรณีสร้าง Personal
                if (input.clientType == "P")
                {
                   
                    var profileInfo = input?.profileInfo;
                    var clsPersonalInput = new CLSCreatePersonalClientInputModel
                    {
                        roleCode = "G",
                        personalName = profileInfo?.firstname ?? "",
                        personalSurname = profileInfo?.lastname ?? "",
                        telephone1 = profileInfo?.telephone1 ?? "",
                        telephone2 = profileInfo?.telephone2 ?? "",
                        mobilePhone = profileInfo?.mobilePhone1 ?? "",
                        emailAddress = profileInfo?.emailaddress1 ?? "",
                        fax = profileInfo?.fax,
                        idCitizen = profileInfo?.citizenId,
                        salutation = profileInfo?.personalCode?.ToUpper() ?? "0001",
                        sex = MasterDataConvertor.Instance.ConvertSexOptionSetValueToMasterCode(profileInfo?.gendercode),
                        

                    };

                    cleansingId = CleansingClientService.Instance.GetPersonalCleansingId(clsPersonalInput);

                }
                //กรณี สร้าง  Corporate
                else if (input.clientType == "C")
                {
                    var profileHeader = input?.profileHeader;
                    var clsCorporateInput = new CLSCreateCorporateClientInputModel
                    {
                        roleCode = input?.roleCode ?? "G",
                        corporateName1 = profileHeader?.name1 ?? "",
                        corporateName2 = profileHeader?.name2 ?? "",
                        corporateBranch = profileHeader?.taxbranch ?? "",
                        telephone1 = profileHeader?.telephone1 ?? "",
                        telephone2 = profileHeader?.telephone2 ?? "",
                        
                        mobilePhone = profileHeader?.mobilePhone1 ?? "",
                        idTax  = profileHeader?.taxno ?? "",
                        fax = profileHeader?.fax,
                        emailAddress = profileHeader.emailaddress1,


                    };
                    cleansingId = CleansingClientService.Instance.GetCorporateCleansingId(clsCorporateInput);
                }

                //update cleansingId to CRM
                if (!string.IsNullOrEmpty(cleansingId))
                {
                    var cmd = new buzReqClsId();
                    var success = cmd.Execute(input?.guid??"", input?.refCode??"", input.clientType, cleansingId);
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
                    throw new Exception("Cannot find Cleansing id ");

                }
            }
            catch (Exception e)
            {



                //error
                return Ok(new OutputGenericDataModel<object>
                {
                    code = AppConst.CODE_FAILED,
                    stackTrace = e.StackTrace,
                    message = e.Message,
                    transactionDateTime = DateTime.Now,
                    description = "",
                    transactionId = GetTransactionId(),
                    data = new
                    {
                        input,
                        cleansingId
                    }
                });
            }

            return InternalServerError();


        }


    }
}