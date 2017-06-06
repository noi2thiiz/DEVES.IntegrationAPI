using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.WebApi.Templates;
using Newtonsoft.Json;
using DEVES.IntegrationAPI.WebApi;
using DEVES.IntegrationAPI.Model.RegPayeeCorporate;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.Polisy400;
using DEVES.IntegrationAPI.Model.RegPayeePersonal;
using DEVES.IntegrationAPI.WebApi.DataAccessService.MasterData;
using DEVES.IntegrationAPI.WebApi.Logic.Services;
using DEVES.IntegrationAPI.WebApi.Logic.Validator;
using DEVES.IntegrationAPI.WebApi.Templates.Exceptions;
using SapVendorInfoModel = DEVES.IntegrationAPI.Model.RegPayeeCorporate.SapVendorInfoModel;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzCRMRegPayeeCorporate : BuzCommand
    {
        protected string newCleansingId;
        protected string newPolisyClientId;
        protected string newCrmClientId;
        protected string newSapVendorCode;
        protected string newSapVendorGroupCode;

        protected bool ignoreSap = false;
        protected bool ignoreCrm = false;

        public RegPayeeCorporateOutputModel_Fail regFail { get; set; } = new RegPayeeCorporateOutputModel_Fail();
        protected RegPayeeCorporateInputModel regPayeeCorporateInput { get; set; }

        public void TranFormInput()
        {
            // ป้องกัน null reference จากการส่ง json มาไม่ครบ
            if (regPayeeCorporateInput.profileHeader == null)
            {
                regPayeeCorporateInput.profileHeader = new ProfileHeaderModel();
            }


              if (regPayeeCorporateInput.addressHeader == null)
            {
                regPayeeCorporateInput.addressHeader = new AddressHeaderModel();
            }
            if (regPayeeCorporateInput.contactHeader == null)
            {
                regPayeeCorporateInput.contactHeader = new ContactHeaderModel();
            }
            if (regPayeeCorporateInput.sapVendorInfo == null)
            {
                regPayeeCorporateInput.sapVendorInfo = new SapVendorInfoModel();
            }
            // Validate Master Data before sending to other services
            var validator = new MasterDataValidator();




            //"profileHeader.countryOrigin"
            regPayeeCorporateInput.profileHeader.countryOrigin = validator.TryConvertNationalityCode(
                "profileHeader.countryOrigin",
                regPayeeCorporateInput?.profileHeader?.countryOrigin);

            //"addressHeader.country"
            regPayeeCorporateInput.addressHeader.country = validator.TryConvertCountryCode(
                "addressHeader.country",
                regPayeeCorporateInput?.addressHeader?.country);

            //"addressHeader.provinceCode"
            regPayeeCorporateInput.addressHeader.provinceCode = validator.TryConvertProvinceCode(
                "addressHeader.provinceCode",
                regPayeeCorporateInput?.addressHeader?.provinceCode,
                regPayeeCorporateInput?.addressHeader?.country);

            //"addressHeader.districtCode"
            regPayeeCorporateInput.addressHeader.districtCode = validator.TryConvertDistrictCode(
                    "addressInfo.districtCode",
                    regPayeeCorporateInput?.addressHeader?.districtCode,
                    regPayeeCorporateInput?.addressHeader?.provinceCode)
                ;

            //"addressHeader.subDistrictCode"
            regPayeeCorporateInput.addressHeader.subDistrictCode = validator.TryConvertSubDistrictCode(
                "addressInfo.subDistrictCode",
                regPayeeCorporateInput?.addressHeader?.subDistrictCode,
                regPayeeCorporateInput?.addressHeader?.districtCode,
                regPayeeCorporateInput?.addressHeader?.provinceCode
            );
            //"addressHeader.addressType"
            regPayeeCorporateInput.addressHeader.addressType = validator.TryConvertAddressTypeCode(
                "addressInfo.addressType",
                regPayeeCorporateInput?.addressHeader?.addressType
            );

            //"addressHeader.addressType"
            regPayeeCorporateInput.addressHeader.addressType = validator.TryConvertAddressTypeCode(
                "addressInfo.addressType",
                regPayeeCorporateInput?.addressHeader?.addressType
            );

            //"profileHeader.econActivity"
            regPayeeCorporateInput.profileHeader.econActivity = validator.TryConvertEconActivityCode(
                "profileHeader.econActivity",
                regPayeeCorporateInput?.profileHeader?.econActivity
            );

          
            if (validator.Invalid())
            {
                throw new FieldValidationException(validator.GetFieldErrorData());
            }
        }

        public override BaseDataModel ExecuteInput(object input)
        {
            RegPayeeCorporateContentOutputModel regPayeeCorporateOutput = new RegPayeeCorporateContentOutputModel();
            regPayeeCorporateOutput.data = new List<RegPayeeCorporateDataOutputModel>();
            regPayeeCorporateOutput.code = AppConst.CODE_SUCCESS;
            regPayeeCorporateOutput.message = AppConst.MESSAGE_SUCCESS;
            regPayeeCorporateOutput.description = "";
            regPayeeCorporateOutput.transactionDateTime = DateTime.Now;
            regPayeeCorporateOutput.transactionId = TransactionId;

            RegPayeeCorporateDataOutputModel_Pass outputPass = new RegPayeeCorporateDataOutputModel_Pass();
            regPayeeCorporateInput = (RegPayeeCorporateInputModel) input;
            TranFormInput();


            ///////////////////////////////////////////////////////////////////////////////////////////////////
            
            //ถ้าส่ง polisyClientId มาจะข้ามไปสร้าง SAP เลย  ไม่ต้องส่ง Cleansing มาก็ได้ แต่ user ไม่ควรทำได้เอง ระบบควรทำให้
            if (string.IsNullOrEmpty(regPayeeCorporateInput?.generalHeader?.polisyClientId))
            {
                if (string.IsNullOrEmpty(regPayeeCorporateInput?.generalHeader?.cleansingId))
                {   //เก็บค่า newCleansingId ไว้ลบ
                    #region Create Payee in Cleansing

                    BaseDataModel clsCreateCorporateIn =
                        DataModelFactory.GetModel(typeof(CLSCreateCorporateClientInputModel));
                    clsCreateCorporateIn =
                        TransformerFactory.TransformModel(regPayeeCorporateInput, clsCreateCorporateIn);

                    AddDebugInfo("Create Payee in Cleansing", clsCreateCorporateIn);

                    CLSCreateCorporateClientContentOutputModel clsCreatePayeeContent =
                        CallDevesServiceProxy<CLSCreateCorporateClientOutputModel,
                                CLSCreateCorporateClientContentOutputModel>
                            (CommonConstant.ewiEndpointKeyCLSCreateCorporateClient, clsCreateCorporateIn);
                    //regPayeeCorporateInput = (RegPayeeCorporateInputModel)TransformerFactory.TransformModel(clsCreatePayeeContent, regPayeeCorporateInput);
                    AddDebugInfo("Create Payee in Cleansing Output ", clsCreatePayeeContent);
                    if (clsCreatePayeeContent?.code == CONST_CODE_SUCCESS)
                    {
                        //เก็บค่าไว้ลบ กรณีสร้างใหม่
                        newCleansingId = clsCreatePayeeContent?.data?.cleansingId;
                        regPayeeCorporateInput.generalHeader.cleansingId =
                            clsCreatePayeeContent?.data?.cleansingId ?? "";

                        outputPass.polisyClientId = clsCreatePayeeContent.data?.clientId;
                        outputPass.corporateName1 = clsCreatePayeeContent.data?.corporateName1;
                        outputPass.corporateName2 = clsCreatePayeeContent.data?.corporateName2;
                    }
                    else
                    {
                        regPayeeCorporateOutput.code = clsCreatePayeeContent?.code??AppConst.CODE_FAILED;
                        regPayeeCorporateOutput.message = $"CLS Error:{clsCreatePayeeContent?.message}";
                        regPayeeCorporateOutput.description = clsCreatePayeeContent?.description;
                        outputPass.cleansingId = clsCreatePayeeContent?.data?.cleansingId ?? "";
                        outputPass.polisyClientId = clsCreatePayeeContent?.data?.clientId??"";
                        outputPass.corporateName1 = clsCreatePayeeContent?.data?.corporateName1??"";
                        outputPass.corporateName2 = clsCreatePayeeContent?.data?.corporateName2??"";
                        outputPass.corporateBranch = "";
                        outputPass.sapVendorCode = "";
                        outputPass.sapVendorGroupCode = "";
                      

                        regPayeeCorporateOutput.data.Add(outputPass);
                        throw  new BuzErrorException(regPayeeCorporateOutput,"CLS");
                    }

                    #endregion Create Payee in Cleansing
                }
                //implement rollback
                #region Create Payee in Polisy400

                try
                {
                    BaseDataModel polCreateCorporateIn =
                        DataModelFactory.GetModel(typeof(CLIENTCreateCorporateClientAndAdditionalInfoInputModel));
                    polCreateCorporateIn = TransformerFactory.TransformModel(regPayeeCorporateInput, polCreateCorporateIn);

                    AddDebugInfo("Create Payee in Polisy400", polCreateCorporateIn);

                    CLIENTCreateCorporateClientAndAdditionalInfoContentModel polCreatePayeeContent =
                        CallDevesServiceProxy<CLIENTCreateCorporateClientAndAdditionalInfoOutputModel
                                , CLIENTCreateCorporateClientAndAdditionalInfoContentModel>
                            (CommonConstant.ewiEndpointKeyCLIENTCreateCorporateClient, polCreateCorporateIn);
                    //regPayeeCorporateInput = (RegPayeeCorporateInputModel)TransformerFactory.TransformModel(polCreatePayeeContent, regPayeeCorporateInput);

                    AddDebugInfo("Create Payee in Polisy400 Output ", polCreatePayeeContent);

                    if (polCreatePayeeContent != null)
                    {
                        newPolisyClientId = polCreatePayeeContent?.clientID??"";

                        regPayeeCorporateInput.generalHeader.polisyClientId = polCreatePayeeContent.clientID;

                        outputPass.polisyClientId = polCreatePayeeContent.clientID;
                    }
                }
                catch (Exception)
                {
                    //@TODO adHoc fix Please fill recipient type  มัน return success เลยถ้าไม่ได้ดักไว้ 
                    if(!string.IsNullOrEmpty(newCleansingId))
                    {
                        AddDebugInfo("244:try rollback" + newCleansingId);
                        var deleteResult = CleansingClientService.Instance.RemoveByCleansingId(newCleansingId, "C");
                    }
                   
                    throw;
                }
                #endregion Create Payee in Polisy400
            }

            #region Search Payee in SAP

            Model.SAP.SAPInquiryVendorInputModel SAPInqVendorIn =
                (Model.SAP.SAPInquiryVendorInputModel) DataModelFactory.GetModel(
                    typeof(Model.SAP.SAPInquiryVendorInputModel));
            AddDebugInfo("Search Payee in SAP", SAPInqVendorIn);
            //SAPInqVendorIn = TransformerFactory.TransformModel(regPayeeCorporateInput, SAPInqVendorIn);

            SAPInqVendorIn.TAX3 = regPayeeCorporateInput.profileHeader.idTax ?? "";
        
            SAPInqVendorIn.TAX4 = regPayeeCorporateInput.profileHeader.corporateBranch ?? "";
            SAPInqVendorIn.PREVACC = regPayeeCorporateInput.sapVendorInfo.sapVendorCode ?? "";
            SAPInqVendorIn.VCODE = regPayeeCorporateInput.generalHeader.polisyClientId ?? "";


            var SAPInqVendorContentOut =
                CallDevesServiceProxy<Model.SAP.SAPInquiryVendorOutputModel,
                    Model.SAP.EWIResSAPInquiryVendorContentModel>(CommonConstant.ewiEndpointKeySAPInquiryVendor,
                    SAPInqVendorIn);
            AddDebugInfo("Search Payee  in SAP Output", SAPInqVendorContentOut);
            #endregion Search Payee in SAP

            var sapInfo = SAPInqVendorContentOut?.VendorInfo?.FirstOrDefault();

            if (string.IsNullOrEmpty(sapInfo?.VCODE))
            {
                //error
                //implement rollback
                #region Create Payee in SAP

                try
                {


                    if (!ignoreSap)
                    {
                        //BaseDataModel SAPCreateVendorIn = DataModelFactory.GetModel(typeof(Model.SAP.SAPCreateVendorInputModel));
                        Model.SAP.SAPCreateVendorInputModel SAPCreateVendorIn = new Model.SAP.SAPCreateVendorInputModel();
                        SAPCreateVendorIn =
                            (Model.SAP.SAPCreateVendorInputModel)TransformerFactory.TransformModel(regPayeeCorporateInput,
                                SAPCreateVendorIn);

                        var SAPCreateVendorContentOut =
                            CallDevesServiceProxy<Model.SAP.SAPCreateVendorOutputModel,
                                Model.SAP.SAPCreateVendorContentOutputModel>(CommonConstant.ewiEndpointKeySAPCreateVendor,
                                SAPCreateVendorIn);
                        if (string.IsNullOrEmpty(SAPCreateVendorContentOut?.VCODE))
                        {
                            throw new Exception(SAPCreateVendorContentOut?.Message);
                        }

                        regPayeeCorporateInput.sapVendorInfo.sapVendorCode = SAPCreateVendorContentOut?.VCODE;
                        outputPass.sapVendorCode = SAPCreateVendorContentOut?.VCODE;

                        newSapVendorCode = SAPCreateVendorContentOut?.VCODE;
                    }
                  
                }
                catch (Exception e)
                {
                    //@TODO adHoc fix Please fill recipient type  มัน return success เลยถ้าไม่ได้ดักไว้ 
                    if (!string.IsNullOrEmpty(newCleansingId))
                    {
                        AddDebugInfo("273:try rollback" + newCleansingId);
                        var deleteResult = CleansingClientService.Instance.RemoveByCleansingId(newCleansingId, "C");
                    }
                    

                    List<OutputModelFailDataFieldErrors> fieldError = MessageBuilder.Instance.ExtractSapCreateVendorFieldError<RegPayeeCorporateInputModel>(e.Message, regPayeeCorporateInput);
                    if (fieldError.Any())
                    {
                        AddDebugInfo("FieldValidationException" + "SAP Error:" + e.Message);
                       
                     
                         throw new FieldValidationException(fieldError, "Cannot create SAP Vendor:" + MessageBuilder.Instance.ConvertMessageSapToCrm(e.Message), ""); 
                        

                    }
                    else
                    {
                        throw new FieldValidationException(fieldError, "SAP Error:" + MessageBuilder.Instance.ConvertMessageSapToCrm(e.Message), "");
                      
                    }
                }
                #endregion Create Payee in SAP

                #region Create payee in CRM

                try
                {
                    if (!ignoreCrm && !string.IsNullOrEmpty(regPayeeCorporateInput.generalHeader.cleansingId))
                    {
                        AddDebugInfo("Create payee in CRM");
                        buzCreateCrmPayeeCorporate cmdCreateCrmPayee = new buzCreateCrmPayeeCorporate();
                        CreateCrmCorporateInfoOutputModel crmContentOutput =
                            (CreateCrmCorporateInfoOutputModel)cmdCreateCrmPayee.Execute(regPayeeCorporateInput);

                        if (crmContentOutput.code == CONST_CODE_SUCCESS)
                        {
                            //RegPayeeCorporateDataOutputModel_Pass dataOutPass = new RegPayeeCorporateDataOutputModel_Pass();
                            //dataOutPass.polisyClientId = regPayeeCorporateInput.generalHeader.polisyClientId;
                            //dataOutPass.sapVendorCode = regPayeeCorporateInput.sapVendorInfo.sapVendorCode;
                            //dataOutPass.sapVendorGroupCode = regPayeeCorporateInput.sapVendorInfo.sapVendorGroupCode;
                            //dataOutPass.personalName = regPayeeCorporateInput.profileInfo.personalName;
                            //dataOutPass.personalSurname = regPayeeCorporateInput.profileInfo.personalSurname;
                            outputPass.corporateBranch = regPayeeCorporateInput.profileHeader.corporateBranch;
                            outputPass.sapVendorGroupCode = regPayeeCorporateInput.sapVendorInfo.sapVendorGroupCode;
                            //regPayeeCorporateOutput.data.Add(dataOutPass);
                        }
                        else
                        {
                            regPayeeCorporateOutput.code = CONST_CODE_FAILED;
                            regPayeeCorporateOutput.message = crmContentOutput.message;
                            regPayeeCorporateOutput.description = crmContentOutput.description;
                        }
                    }
                }
                catch (Exception e)
                {
                    //do not thing
                    AddDebugInfo("Cannot create Client in CRM :" + e.Message, e.StackTrace);
                }
               

                #endregion Create payee in CRM
            }
            else
            {   
                //regPayeeCorporateInput.sapVendorInfo.sapVendorCode = SAPInqVendorContentOut.VCODE;
                outputPass.sapVendorCode = sapInfo?.VCODE;
                outputPass.sapVendorGroupCode = sapInfo?.VGROUP;
                outputPass.corporateName1 = sapInfo?.NAME1;
                outputPass.corporateName2 = sapInfo?.NAME2;
                newSapVendorCode= sapInfo?.VCODE;
                newSapVendorGroupCode = sapInfo?.VGROUP;


            }

            //@TODO adHoc fix if fullname in null
            // Tranform จน งง ว่าข้อมูลไปยังไงมายังไง เลยต้องมา check อีกที
           
            if (!string.IsNullOrEmpty(newPolisyClientId))
            {
                outputPass.polisyClientId = newPolisyClientId;
            }

            if (!string.IsNullOrEmpty(newCleansingId))
            {
                outputPass.cleansingId = newCleansingId;
            }

            if (!string.IsNullOrEmpty(newSapVendorCode))
            {
                outputPass.sapVendorCode = newSapVendorCode;
            }


            // ถ้ายังไม่มีค่าเอา input มา output
            if (string.IsNullOrEmpty(outputPass.polisyClientId))
            {
                outputPass.polisyClientId = regPayeeCorporateInput.generalHeader.polisyClientId;
            }


            if (string.IsNullOrEmpty(outputPass.cleansingId))
            {
                outputPass.cleansingId = regPayeeCorporateInput.generalHeader.cleansingId;
            }

            if (string.IsNullOrEmpty(outputPass.corporateName1))
            {
                outputPass.corporateName1 = regPayeeCorporateInput.profileHeader.corporateName1;
                outputPass.corporateName2 = regPayeeCorporateInput.profileHeader.corporateName2;
            }




            outputPass.sapVendorGroupCode = !string.IsNullOrEmpty(newSapVendorGroupCode)? newSapVendorGroupCode: regPayeeCorporateInput.sapVendorInfo.sapVendorGroupCode; ;
             outputPass.corporateBranch = regPayeeCorporateInput.profileHeader.corporateBranch;


            regPayeeCorporateOutput.data.Add(outputPass);

            //add debug info to model
         
            return regPayeeCorporateOutput;
        }

        public BaseDataModel XExecuteX(object input)
        {
            RegPayeeCorporateContentOutputModel regPayeeCorporateOutput = new RegPayeeCorporateContentOutputModel();
            regPayeeCorporateOutput.data = new List<RegPayeeCorporateDataOutputModel>();
            try
            {
                RegPayeeCorporateInputModel regPayeeCorporateInput = (RegPayeeCorporateInputModel) input;

                if (string.IsNullOrEmpty(regPayeeCorporateInput.generalHeader.cleansingId))
                {
                    BaseDataModel clsCreateCorporateIn =
                        DataModelFactory.GetModel(typeof(CLSCreateCorporateClientInputModel));
                    clsCreateCorporateIn =
                        TransformerFactory.TransformModel(regPayeeCorporateInput, clsCreateCorporateIn);
                    CLSCreateCorporateClientContentOutputModel clsCreatePayeeContent =
                        CallDevesServiceProxy<CLSCreateCorporateClientOutputModel,
                                CLSCreateCorporateClientContentOutputModel>
                            (CommonConstant.ewiEndpointKeyCLSCreateCorporateClient, clsCreateCorporateIn);
                    regPayeeCorporateInput =
                        (RegPayeeCorporateInputModel) TransformerFactory.TransformModel(clsCreatePayeeContent,
                            regPayeeCorporateInput);
                }

                if (string.IsNullOrEmpty(regPayeeCorporateInput.generalHeader.polisyClientId))
                {
                    BaseDataModel polCreateCorporateIn =
                        DataModelFactory.GetModel(typeof(CLIENTCreateCorporateClientAndAdditionalInfoInputModel));
                    polCreateCorporateIn =
                        TransformerFactory.TransformModel(regPayeeCorporateInput, polCreateCorporateIn);
                    CLIENTCreateCorporateClientAndAdditionalInfoContentModel polCreatePayeeContent =
                        CallDevesServiceProxy<CLIENTCreateCorporateClientAndAdditionalInfoOutputModel
                                , CLIENTCreateCorporateClientAndAdditionalInfoContentModel>
                            (CommonConstant.ewiEndpointKeyCLIENTCreateCorporateClient, polCreateCorporateIn);
                    regPayeeCorporateInput =
                        (RegPayeeCorporateInputModel) TransformerFactory.TransformModel(polCreatePayeeContent,
                            regPayeeCorporateInput);
                }

                BaseDataModel SAPInqVendorIn = DataModelFactory.GetModel(typeof(Model.SAP.SAPInquiryVendorInputModel));
                SAPInqVendorIn = TransformerFactory.TransformModel(regPayeeCorporateInput, SAPInqVendorIn);
                var SAPInqVendorContentOut =
                    CallDevesServiceProxy<Model.SAP.SAPInquiryVendorOutputModel,
                        Model.SAP.SAPInquiryVendorContentVendorInfoModel>(CommonConstant.ewiEndpointKeySAPInquiryVendor,
                        SAPInqVendorIn);
                if (SAPInqVendorContentOut != null && !string.IsNullOrEmpty(SAPInqVendorContentOut.VCODE))
                {
                    regPayeeCorporateInput.sapVendorInfo.sapVendorCode = SAPInqVendorContentOut.VCODE;
                }
                else
                {
                    BaseDataModel SAPCreateVendorIn =
                        DataModelFactory.GetModel(typeof(Model.SAP.SAPCreateVendorInputModel));
                    SAPCreateVendorIn = TransformerFactory.TransformModel(regPayeeCorporateInput, SAPCreateVendorIn);
                    var SAPCreateVendorContentOut =
                        CallDevesServiceProxy<Model.SAP.SAPCreateVendorOutputModel,
                            Model.SAP.SAPCreateVendorContentOutputModel>(CommonConstant.ewiEndpointKeySAPCreateVendor,
                            SAPCreateVendorIn);
                    regPayeeCorporateInput.sapVendorInfo.sapVendorCode = SAPCreateVendorContentOut.VCODE;
                }
                try
                {
                    buzCreateCrmClientCorporate cmdCreateCrmPayee = new buzCreateCrmClientCorporate();
                    CreateCrmCorporateInfoOutputModel crmContentOutput =
                        (CreateCrmCorporateInfoOutputModel) cmdCreateCrmPayee.Execute(regPayeeCorporateInput);
                }
                catch (Exception )
                {
                    //do notthing
                }
               

                //if (crmContentOutput.code == CONST_CODE_SUCCESS)
                //{
                    regPayeeCorporateOutput.code = AppConst.CODE_SUCCESS;
                    regPayeeCorporateOutput.message = AppConst.MESSAGE_SUCCESS;
                    RegPayeeCorporateDataOutputModel_Pass dataOutPass = new RegPayeeCorporateDataOutputModel_Pass();
                    dataOutPass.polisyClientId = regPayeeCorporateInput.generalHeader.polisyClientId;
                    dataOutPass.sapVendorCode = regPayeeCorporateInput.sapVendorInfo.sapVendorCode;
                    dataOutPass.sapVendorGroupCode = regPayeeCorporateInput.sapVendorInfo.sapVendorGroupCode;
                    dataOutPass.corporateName1 = regPayeeCorporateInput.profileHeader.corporateName1;
                    dataOutPass.corporateName2 = regPayeeCorporateInput.profileHeader.corporateName2;
                    dataOutPass.corporateBranch = regPayeeCorporateInput.profileHeader.corporateBranch;

                    regPayeeCorporateOutput.data.Add(dataOutPass);
                //}
                //else
               // {
                 //   regPayeeCorporateOutput.code = AppConst.CODE_FAILED;
                 //   regPayeeCorporateOutput.message = crmContentOutput.message;
                //    regPayeeCorporateOutput.description = crmContentOutput.description;
               // }
            }
            catch (FieldValidationException e)
            {
                regFail.code = AppConst.CODE_INVALID_INPUT;
                regFail.message = AppConst.MESSAGE_INVALID_INPUT;
                regFail.description = AppConst.DESC_INVALID_INPUT;
                regFail.transactionId = TransactionId;
                regFail.transactionDateTime = DateTime.Now;

                return regFail;
            }
            catch (Exception e)
            {
                regPayeeCorporateOutput.code = AppConst.MESSAGE_INTERNAL_ERROR;
                regPayeeCorporateOutput.message = AppConst.MESSAGE_INTERNAL_ERROR;
                regPayeeCorporateOutput.description = e.StackTrace;

                RegPayeeCorporateDataOutputModel_Fail dataOutFail = new RegPayeeCorporateDataOutputModel_Fail();
                // Object reference not set to an instance of an object.
                regPayeeCorporateOutput.data = new List<RegPayeeCorporateDataOutputModel>();
                regPayeeCorporateOutput.data.Add(dataOutFail);
            }
            regPayeeCorporateOutput.transactionDateTime = DateTime.Now;
            regPayeeCorporateOutput.transactionId = TransactionId;

           

            return regPayeeCorporateOutput;
        }
    }
}