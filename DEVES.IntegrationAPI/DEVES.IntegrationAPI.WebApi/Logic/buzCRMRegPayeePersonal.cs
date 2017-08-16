using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.WebApi.Templates;

using Newtonsoft.Json;
using DEVES.IntegrationAPI.WebApi;
using DEVES.IntegrationAPI.Model.RegPayeePersonal;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.Polisy400;
using DEVES.IntegrationAPI.Model.SAP;
using DEVES.IntegrationAPI.WebApi.DataAccessService.MasterData;
using DEVES.IntegrationAPI.WebApi.Logic.DataBaseContracts;
using DEVES.IntegrationAPI.WebApi.Logic.Services;
using DEVES.IntegrationAPI.WebApi.Logic.Validator;
using DEVES.IntegrationAPI.WebApi.Templates.Exceptions;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    
    public class buzCRMRegPayeePersonal: BuzCommand
    {
       

        protected string newCleansingId;
        protected string newPolisyClientId;
        protected string newCrmClientId;
        protected string newSapVendorCode;
        protected string newSapVendorGroupCode;

        //for test
        protected bool ignoreSap = false;
        protected bool ignoreCrm = false;

        private RegPayeePersonalInputModel RegPayeePersonalInput { get; set; }
        private RegPayeePersonalDataOutputModel_Pass outputPass { get; set; }= new RegPayeePersonalDataOutputModel_Pass();
        
        // Validate Master Data before sending to other services
        public void TranFormInput()
        {
            // ป้องกันปัญหา locus ส่ง json มาไม่ครบ
            if (RegPayeePersonalInput.addressInfo == null)
            {
                RegPayeePersonalInput.addressInfo = new AddressInfoModel();
            }
            if (RegPayeePersonalInput.contactInfo == null)
            {
                RegPayeePersonalInput.contactInfo = new ContactInfoModel();
            }
            if (RegPayeePersonalInput.generalHeader == null)
            {
                RegPayeePersonalInput.generalHeader = new GeneralHeaderModel();
            }
            if (RegPayeePersonalInput.profileInfo ==null)
            {
                RegPayeePersonalInput.profileInfo = new ProfileInfoModel();
            }
            if (RegPayeePersonalInput.sapVendorInfo == null)
            {
                RegPayeePersonalInput.sapVendorInfo = new SapVendorInfoModel();
            }

            // Validate Master Data before sending to other services
            var validator = new MasterDataValidator();


            //profileInfo.salutation
            RegPayeePersonalInput.profileInfo.salutation = validator.TryConvertSalutationCode(
                "profileInfo.salutation",
                RegPayeePersonalInput?.profileInfo?.salutation);

            //"profileInfo.nationality"
            RegPayeePersonalInput.profileInfo.nationality = validator.TryConvertNationalityCode(
                "profileInfo.nationality",
                RegPayeePersonalInput?.profileInfo?.nationality);

            //"profileInfo.occupation"
            RegPayeePersonalInput.profileInfo.occupation = validator.TryConvertOccupationCode(
                "profileInfo.occupation",
                RegPayeePersonalInput?.profileInfo?.occupation);


            //"profileInfo.country"
            RegPayeePersonalInput.addressInfo.country = validator.TryConvertCountryCode(
                "addressInfo.country",
                RegPayeePersonalInput?.addressInfo?.country);

            //"profileInfo.provinceCode"
            RegPayeePersonalInput.addressInfo.provinceCode = validator.TryConvertProvinceCode(
                "addressInfo.provinceCode",
                RegPayeePersonalInput?.addressInfo?.provinceCode,
                RegPayeePersonalInput?.addressInfo?.country);

            //"profileInfo.districtCode"
            RegPayeePersonalInput.addressInfo.districtCode = validator.TryConvertDistrictCode(
                    "addressInfo.districtCode",
                    RegPayeePersonalInput?.addressInfo?.districtCode,
                    RegPayeePersonalInput?.addressInfo?.provinceCode)
                ;

            //"profileInfo.subDistrictCode"
            RegPayeePersonalInput.addressInfo.subDistrictCode = validator.TryConvertSubDistrictCode(
                "addressInfo.subDistrictCode",
                RegPayeePersonalInput?.addressInfo?.subDistrictCode,
                RegPayeePersonalInput?.addressInfo?.districtCode,
                RegPayeePersonalInput?.addressInfo?.provinceCode
            );
            //"profileInfo.addressType"
            RegPayeePersonalInput.addressInfo.addressType = validator.TryConvertAddressTypeCode(
                "addressInfo.addressType",
                RegPayeePersonalInput?.addressInfo?.addressType
            );




            if (validator.Invalid())
            {
                throw new FieldValidationException(validator.GetFieldErrorData());
            }
        }
        public override BaseDataModel ExecuteInput(object input)
        {
            AddDebugInfo("call ExecuteInput", input);
            RegPayeePersonalInput = (RegPayeePersonalInputModel)input;
            TranFormInput();

            var regPayeePersonalOutput =
                new RegPayeePersonalContentOutputModel
                {
                    data = new List<RegPayeePersonalDataOutputModel>(),
                    code = CONST_CODE_SUCCESS,
                    message = CONST_MESSAGE_SUCCESS,
                    transactionDateTime = DateTime.Now,
                    transactionId = TransactionId
                };


               

            //ถ้าส่ง polisyClientId มาจะข้ามไปสร้าง SAP เลย  ไม่ต้องส่ง Cleansing มาก็ได้ แต่ user ไม่ควรทำได้เอง ระบบควรทำให้
            if (string.IsNullOrEmpty(RegPayeePersonalInput?.generalHeader?.polisyClientId))
            {
                if (string.IsNullOrEmpty(RegPayeePersonalInput?.generalHeader?.cleansingId))
                {
                    #region Create Payee in Cleansing

                    CreatePayeeInCleansing(regPayeePersonalOutput);

                    #endregion Create Payee in Cleansing
                }

                //implement rollback
                
                #region Create Payee in Polisy400
               
                //regPayeePersonalInput = (RegPayeePersonalInputModel)TransformerFactory.TransformModel(polCreatePayeeContent, regPayeePersonalInput);
                CreatePayeeInPolisy400();

                #endregion Create Payee in Polisy400
            }



            //Adhoc
            newPolisyClientId = "" + RegPayeePersonalInput?.generalHeader?.polisyClientId;
            newCleansingId = "" + RegPayeePersonalInput?.generalHeader?.cleansingId;

            AddDebugInfo(" newPolisyClientId", newPolisyClientId);
            AddDebugInfo(" newCleansingId", newCleansingId);



            #region Search Payee in SAP

            var sapInfo = SearchPayeeInSap();

            if (string.IsNullOrEmpty(sapInfo?.VCODE))
                {
                    //implement rollback
                #region Create Payee in SAP
                //BaseDataModel SAPCreateVendorIn = DataModelFactory.GetModel(typeof(Model.SAP.SAPCreateVendorInputModel));
                    CreatePayeeInSap();

                    #endregion Create Payee in SAP

                try
                {
                    #region Create payee in CRM

                    if (!ignoreCrm && !string.IsNullOrEmpty(RegPayeePersonalInput?.generalHeader?.cleansingId))
                    {

                        if (false == SpApiChkCustomerClient.Instance.CheckByCleansingId(RegPayeePersonalInput.generalHeader.cleansingId))
                        {
                            AddDebugInfo("Create payee in CRM ");
                            buzCreateCrmPayeePersonal cmdCreateCrmPayee = new buzCreateCrmPayeePersonal();
                            CreateCrmPersonInfoOutputModel crmContentOutput = (CreateCrmPersonInfoOutputModel)cmdCreateCrmPayee.Execute(RegPayeePersonalInput);
                            if (crmContentOutput.code == CONST_CODE_SUCCESS)
                            {
                                outputPass.crmClientId = crmContentOutput?.crmClientId ?? "";
                            }
                            else
                            {
                                AddDebugInfo("Cannot create Client in CRM :" + crmContentOutput.message, crmContentOutput);
                                //regPayeePersonalOutput.code = CONST_CODE_FAILED;
                                //regPayeePersonalOutput.message = crmContentOutput.message;
                                //regPayeePersonalOutput.description = crmContentOutput.description;
                            }
                        }
                    }

                    
                    #endregion Create payee in CRM
                }
                catch (Exception e)
                {
                    //do not thing
                    AddDebugInfo("Cannot create Client in CRM :" + e.Message, e.StackTrace);
                }
                }
                else
                {
                    //return success with existing SAP Vendor
                    AddDebugInfo(" found existing SAP Vendor ", sapInfo);
                   //regPayeePersonalInput.sapVendorInfo.sapVendorCode = SAPInqVendorContentOut.VCODE;
                    outputPass.sapVendorCode = sapInfo?.VCODE;
                    outputPass.sapVendorGroupCode = sapInfo?.VGROUP;
                    newSapVendorCode = sapInfo?.VCODE;
                    newSapVendorGroupCode = sapInfo?.VGROUP;
                    //sapInfo?.VGROUP ?? regPayeePersonalInput?.sapVendorInfo?.sapVendorGroupCode ?? "";
                }

            //@TODO adHoc fix if fullname in null

            FinalizeOurput();


            // RegPayeePersonalInput?.sapVendorInfo?.sapVendorGroupCode;

            regPayeePersonalOutput.data.Add(outputPass);
            AddDebugInfo(" output ", outputPass);
           
            return regPayeePersonalOutput;

        }

        private void FinalizeOurput()
        {
            if (!string.IsNullOrEmpty(newCleansingId))
            {
                outputPass.cleansingId = newCleansingId;
            }

            if (!string.IsNullOrEmpty(newPolisyClientId))
            {
                outputPass.polisyClientId = newPolisyClientId;

                AddDebugInfo(" outputPass.polisyClientId 1", outputPass.polisyClientId);
            }

            if (!string.IsNullOrEmpty(newSapVendorCode))
            {
                outputPass.sapVendorCode = newSapVendorCode;
            }

            if (!string.IsNullOrEmpty(newSapVendorGroupCode))
            {
                outputPass.sapVendorGroupCode = newSapVendorGroupCode;
            }

            // ถ้า output ยัง null

            if (string.IsNullOrEmpty(outputPass.personalName))
            {
                outputPass.personalName = RegPayeePersonalInput?.profileInfo.personalName;
                outputPass.personalSurname = RegPayeePersonalInput?.profileInfo.personalSurname;
            }

            if (string.IsNullOrEmpty(outputPass.polisyClientId))
            {
                outputPass.polisyClientId = RegPayeePersonalInput?.generalHeader?.polisyClientId;
                AddDebugInfo(" RegPayeePersonalInput?.generalHeader?.polisyClientId", outputPass.polisyClientId);
            }

            if (string.IsNullOrEmpty(outputPass.cleansingId))
            {
                outputPass.cleansingId = RegPayeePersonalInput?.generalHeader?.cleansingId;
            }
        }

        private SAPInquiryVendorContentVendorInfoModel SearchPayeeInSap()
        {
            Model.SAP.SAPInquiryVendorInputModel SAPInqVendorIn =
                (Model.SAP.SAPInquiryVendorInputModel) DataModelFactory.GetModel(typeof(Model.SAP.SAPInquiryVendorInputModel));
            //SAPInqVendorIn = TransformerFactory.TransformModel(regPayeePersonalInput, SAPInqVendorIn);

            SAPInqVendorIn.TAX3 = RegPayeePersonalInput.profileInfo.idCitizen ?? "";
            SAPInqVendorIn.TAX4 = "";
            SAPInqVendorIn.PREVACC = RegPayeePersonalInput.generalHeader.polisyClientId ?? ""; 
            SAPInqVendorIn.VCODE   = RegPayeePersonalInput.sapVendorInfo.sapVendorCode ?? "";


            AddDebugInfo("Search Payee in SAP", SAPInqVendorIn);

            var SAPInqVendorContentOut =
                CallDevesServiceProxy<Model.SAP.SAPInquiryVendorOutputModel, Model.SAP.EWIResSAPInquiryVendorContentModel>(
                    CommonConstant.ewiEndpointKeySAPInquiryVendor, SAPInqVendorIn);

            #endregion Search Payee in SAP

            var sapInfo = SAPInqVendorContentOut?.VendorInfo?.FirstOrDefault();
            return sapInfo;
        }

        private void CreatePayeeInSap()
        {
            try
            {
                if (!ignoreSap)
                {
                    Model.SAP.SAPCreateVendorInputModel SAPCreateVendorIn =
                        new Model.SAP.SAPCreateVendorInputModel();
                    SAPCreateVendorIn =
                        (Model.SAP.SAPCreateVendorInputModel) TransformerFactory.TransformModel(
                            RegPayeePersonalInput, SAPCreateVendorIn);

                    AddDebugInfo(" create SAP Vendor ", SAPCreateVendorIn);

                    var SAPCreateVendorContentOut =
                        CallDevesServiceProxy<Model.SAP.SAPCreateVendorOutputModel,
                            Model.SAP.SAPCreateVendorContentOutputModel>(
                            CommonConstant.ewiEndpointKeySAPCreateVendor, SAPCreateVendorIn);

                    if (string.IsNullOrEmpty(SAPCreateVendorContentOut?.VCODE))
                    {
                        throw new Exception(SAPCreateVendorContentOut?.Message);
                    }

                    RegPayeePersonalInput.sapVendorInfo.sapVendorCode = SAPCreateVendorContentOut?.VCODE;
                    outputPass.sapVendorCode = SAPCreateVendorContentOut?.VCODE;
                    outputPass.sapVendorGroupCode = RegPayeePersonalInput?.sapVendorInfo?.sapVendorGroupCode;

                    newSapVendorCode = SAPCreateVendorContentOut?.VCODE;
                    newSapVendorGroupCode = RegPayeePersonalInput?.sapVendorInfo?.sapVendorGroupCode;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error On Create Sap =" + e.Message);
                AddDebugInfo("Error On Create Sap =" + e.Message + e.StackTrace);
                //@TODO adHoc fix Please fill recipient type  มัน return success เลยถ้าไม่ได้ดักไว้ 
                if (!string.IsNullOrEmpty(newCleansingId))
                {
                    AddDebugInfo("rollback newCleansingId=" + newCleansingId);

                    var deleteResult = CleansingClientService.Instance.RemoveByCleansingId(newCleansingId, "P");
                }

                List<OutputModelFailDataFieldErrors> fieldError =
                    MessageBuilder.Instance.ExtractSapCreateVendorFieldError<RegPayeePersonalInputModel>(e.Message,
                        RegPayeePersonalInput);
                if (fieldError.Any())
                {
                    AddDebugInfo("FieldValidationException" + "SAP Error:" + e.Message);


                    throw new FieldValidationException(fieldError,
                        "Cannot create SAP Vendor:" + MessageBuilder.Instance.ConvertMessageSapToCrm(e.Message), "");
                }
                else
                {
                    throw new FieldValidationException(fieldError,
                        "SAP Error:" + MessageBuilder.Instance.ConvertMessageSapToCrm(e.Message), "");
                }
            }
        }

        private void CreatePayeeInCleansing(RegPayeePersonalContentOutputModel regPayeePersonalOutput)
        {
            BaseDataModel clsCreatePersonalIn = DataModelFactory.GetModel(typeof(CLSCreatePersonalClientInputModel));
            clsCreatePersonalIn = TransformerFactory.TransformModel(RegPayeePersonalInput, clsCreatePersonalIn);
            CLSCreatePersonalClientContentOutputModel clsCreatePayeeContent =
                CallDevesServiceProxy<CLSCreatePersonalClientOutputModel, CLSCreatePersonalClientContentOutputModel>
                    (CommonConstant.ewiEndpointKeyCLSCreatePersonalClient, clsCreatePersonalIn);
            //regPayeePersonalInput = (RegPayeePersonalInputModel)TransformerFactory.TransformModel(clsCreatePayeeContent, regPayeePersonalInput);
            if (clsCreatePayeeContent?.code == CONST_CODE_SUCCESS)
            {
                newCleansingId = clsCreatePayeeContent.data?.cleansingId;
                RegPayeePersonalInput.generalHeader.cleansingId = clsCreatePayeeContent.data?.cleansingId ?? "";

                outputPass.cleansingId = clsCreatePayeeContent.data?.cleansingId;
                outputPass.polisyClientId = clsCreatePayeeContent.data?.clientId;
                outputPass.personalName = clsCreatePayeeContent.data?.personalName;
                outputPass.personalSurname = clsCreatePayeeContent.data?.personalSurname;
            }
            else
            {
                regPayeePersonalOutput.code = clsCreatePayeeContent?.code ?? AppConst.CODE_FAILED;
                regPayeePersonalOutput.message = $"CLS Error:{clsCreatePayeeContent?.message}";
                regPayeePersonalOutput.description = clsCreatePayeeContent?.description;
                outputPass.cleansingId = clsCreatePayeeContent?.data?.cleansingId ?? "";
                outputPass.polisyClientId = clsCreatePayeeContent?.data?.clientId ?? "";
                outputPass.personalName = clsCreatePayeeContent?.data?.personalName ?? "";
                outputPass.personalSurname = clsCreatePayeeContent?.data?.personalSurname ?? "";
                outputPass.sapVendorCode = "";
                outputPass.sapVendorGroupCode = "";


                regPayeePersonalOutput.data.Add(outputPass);
                throw new BuzErrorException(regPayeePersonalOutput, "CLS");
            }
        }

        private void CreatePayeeInPolisy400()
        {
            try
            {
                BaseDataModel polCreatePersonalIn =
                    DataModelFactory.GetModel(typeof(CLIENTCreatePersonalClientAndAdditionalInfoInputModel));
                polCreatePersonalIn = TransformerFactory.TransformModel(RegPayeePersonalInput, polCreatePersonalIn);
                CLIENTCreatePersonalClientAndAdditionalInfoContentModel polCreatePayeeContent =
                    CallDevesServiceProxy<CLIENTCreatePersonalClientAndAdditionalInfoOutputModel
                            , CLIENTCreatePersonalClientAndAdditionalInfoContentModel>
                        (CommonConstant.ewiEndpointKeyCLIENTCreatePersonalClient, polCreatePersonalIn);

                if (polCreatePayeeContent != null)
                {
                    RegPayeePersonalInput.generalHeader.polisyClientId = polCreatePayeeContent.clientID;

                    newPolisyClientId = polCreatePayeeContent?.clientID ?? "";
                    outputPass.polisyClientId = polCreatePayeeContent.clientID;
                }
                else
                {
                    //เมื่อเกิด error ใด ๆ ใน service อื่นให้ลบ
                    if (!string.IsNullOrEmpty(newCleansingId))
                    {
                        AddDebugInfo("try rollback" + newCleansingId);
                        var deleteResult = CleansingClientService.Instance.RemoveByCleansingId(newCleansingId, "P");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error On Create 400 =" + e.Message);
                AddDebugInfo("Error On Create 400" + e.Message, e.StackTrace);
                //เมื่อเกิด error ใด ๆ ใน service อื่นให้ลบ
                if (!string.IsNullOrEmpty(newCleansingId))
                {
                    AddDebugInfo("try rollback" + newCleansingId);
                    var deleteResult = CleansingClientService.Instance.RemoveByCleansingId(newCleansingId, "P");
                }

                throw;
            }
        }
    }
}