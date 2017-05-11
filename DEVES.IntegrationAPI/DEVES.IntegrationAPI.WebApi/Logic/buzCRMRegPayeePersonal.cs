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
using DEVES.IntegrationAPI.WebApi.DataAccessService.MasterData;
using DEVES.IntegrationAPI.WebApi.Logic.Validator;
using DEVES.IntegrationAPI.WebApi.Templates.Exceptions;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzCRMRegPayeePersonal: BuzCommand
    {

      
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



          


            if (string.IsNullOrEmpty(RegPayeePersonalInput?.generalHeader?.polisyClientId))
                {
                    if (string.IsNullOrEmpty(RegPayeePersonalInput?.generalHeader?.cleansingId))
                    {
                        #region Create Payee in Cleansing
                        BaseDataModel clsCreatePersonalIn = DataModelFactory.GetModel(typeof(CLSCreatePersonalClientInputModel));
                        clsCreatePersonalIn = TransformerFactory.TransformModel(RegPayeePersonalInput, clsCreatePersonalIn);
                        CLSCreatePersonalClientContentOutputModel clsCreatePayeeContent = CallDevesServiceProxy<CLSCreatePersonalClientOutputModel, CLSCreatePersonalClientContentOutputModel>
                                                                                            (CommonConstant.ewiEndpointKeyCLSCreatePersonalClient, clsCreatePersonalIn);
                        //regPayeePersonalInput = (RegPayeePersonalInputModel)TransformerFactory.TransformModel(clsCreatePayeeContent, regPayeePersonalInput);
                        if (clsCreatePayeeContent?.code == CONST_CODE_SUCCESS)
                        {
                            RegPayeePersonalInput.generalHeader.cleansingId = clsCreatePayeeContent.data?.cleansingId ?? "";

                            outputPass.polisyClientId = clsCreatePayeeContent.data?.clientId;
                            outputPass.personalName = clsCreatePayeeContent.data?.personalName;
                            outputPass.personalSurname = clsCreatePayeeContent.data?.personalSurname;
                        }
                        else
                        {
                            regPayeePersonalOutput.code = AppConst.CODE_FAILED;
                            regPayeePersonalOutput.message = "Cannot create client in cleasing";
                            regPayeePersonalOutput.description = clsCreatePayeeContent?.message;
                            outputPass.cleansingId = clsCreatePayeeContent?.data?.cleansingId ?? "";
                            outputPass.polisyClientId = clsCreatePayeeContent?.data?.cleansingId ?? "";
                            outputPass.personalName = clsCreatePayeeContent?.data?.personalName??"";
                            outputPass.personalSurname = clsCreatePayeeContent?.data?.personalSurname??"";
                            outputPass.sapVendorCode = "";
                            outputPass.sapVendorGroupCode = "";
                         

                        regPayeePersonalOutput.data.Add(outputPass);
                            throw new BuzErrorException(regPayeePersonalOutput, "CLS");
                        }
                    #endregion Create Payee in Cleansing
                }
                    

                #region Create Payee in Polisy400
                BaseDataModel polCreatePersonalIn = DataModelFactory.GetModel(typeof(CLIENTCreatePersonalClientAndAdditionalInfoInputModel));
                    polCreatePersonalIn = TransformerFactory.TransformModel(RegPayeePersonalInput, polCreatePersonalIn);
                    CLIENTCreatePersonalClientAndAdditionalInfoContentModel polCreatePayeeContent = CallDevesServiceProxy<CLIENTCreatePersonalClientAndAdditionalInfoOutputModel
                                                                                                        , CLIENTCreatePersonalClientAndAdditionalInfoContentModel>
                                                                                                        (CommonConstant.ewiEndpointKeyCLIENTCreatePersonalClient, polCreatePersonalIn);
                    //regPayeePersonalInput = (RegPayeePersonalInputModel)TransformerFactory.TransformModel(polCreatePayeeContent, regPayeePersonalInput);

                    if (polCreatePayeeContent != null)
                    {
                        RegPayeePersonalInput.generalHeader.polisyClientId = polCreatePayeeContent.clientID; 

                        outputPass.polisyClientId = polCreatePayeeContent.clientID;
                    }
                             
                    #endregion Create Payee in Polisy400
                }

                #region Search Payee in SAP
                Model.SAP.SAPInquiryVendorInputModel SAPInqVendorIn = (Model.SAP.SAPInquiryVendorInputModel)DataModelFactory.GetModel(typeof(Model.SAP.SAPInquiryVendorInputModel));
                //SAPInqVendorIn = TransformerFactory.TransformModel(regPayeePersonalInput, SAPInqVendorIn);

                SAPInqVendorIn.TAX3 = RegPayeePersonalInput.profileInfo.idCitizen??"";
                SAPInqVendorIn.TAX4 = "";
                SAPInqVendorIn.PREVACC = RegPayeePersonalInput.sapVendorInfo.sapVendorCode ?? "";
                SAPInqVendorIn.VCODE = RegPayeePersonalInput.generalHeader.polisyClientId ?? "";

                var SAPInqVendorContentOut = CallDevesServiceProxy<Model.SAP.SAPInquiryVendorOutputModel, Model.SAP.EWIResSAPInquiryVendorContentModel>(CommonConstant.ewiEndpointKeySAPInquiryVendor, SAPInqVendorIn);
                #endregion Search Payee in SAP

                var sapInfo = SAPInqVendorContentOut?.VendorInfo?.FirstOrDefault();

                if (string.IsNullOrEmpty(sapInfo?.VCODE))
                {
                    #region Create Payee in SAP
                    //BaseDataModel SAPCreateVendorIn = DataModelFactory.GetModel(typeof(Model.SAP.SAPCreateVendorInputModel));
                    try
                    {
                        Model.SAP.SAPCreateVendorInputModel SAPCreateVendorIn =
                            new Model.SAP.SAPCreateVendorInputModel();
                        SAPCreateVendorIn =
                            (Model.SAP.SAPCreateVendorInputModel) TransformerFactory.TransformModel(
                                RegPayeePersonalInput, SAPCreateVendorIn);

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

                    }
                    catch (Exception e)
                    {
                    //@TODO adHoc fix Please fill recipient type  มัน return success เลยถ้าไม่ได้ดักไว้ 

                        List<OutputModelFailDataFieldErrors> fieldError = MessageBuilder.Instance.ExtractSapCreateVendorFieldError<RegPayeePersonalInputModel>(e.Message,RegPayeePersonalInput);
                        if (fieldError != null)
                        {
                             throw new FieldValidationException(fieldError, "Cannot create SAP Vendor", e.Message);

                       }else{
                            throw;
                        }
                    }


                    #endregion Create Payee in SAP

                    #region Create payee in CRM
                    buzCreateCrmPayeePersonal cmdCreateCrmPayee = new buzCreateCrmPayeePersonal();
                    CreateCrmPersonInfoOutputModel crmContentOutput = (CreateCrmPersonInfoOutputModel)cmdCreateCrmPayee.Execute(RegPayeePersonalInput);

                    if (crmContentOutput.code == CONST_CODE_SUCCESS)
                    {
                        //RegPayeePersonalDataOutputModel_Pass dataOutPass = new RegPayeePersonalDataOutputModel_Pass();
                        //dataOutPass.polisyClientId = regPayeePersonalInput.generalHeader.polisyClientId;
                        //dataOutPass.sapVendorCode = regPayeePersonalInput.sapVendorInfo.sapVendorCode;


                       //  dataOutPass.sapVendorGroupCode = regPayeePersonalInput.sapVendorInfo.sapVendorGroupCode;
                        //dataOutPass.personalName = regPayeePersonalInput.profileInfo.personalName;
                        //dataOutPass.personalSurname = regPayeePersonalInput.profileInfo.personalSurname;
                        ////dataOutPass.corporateBranch = regPayeePersonalInput.profileInfo.corporateBranch;
                        //regPayeePersonalOutput.data.Add(dataOutPass);
                    }
                    else
                    {
                        regPayeePersonalOutput.code = CONST_CODE_FAILED;
                        regPayeePersonalOutput.message = crmContentOutput.message;
                        regPayeePersonalOutput.description = crmContentOutput.description;
                    }
                    #endregion Create payee in CRM
                }
                else
                {
                    //regPayeePersonalInput.sapVendorInfo.sapVendorCode = SAPInqVendorContentOut.VCODE;
                    outputPass.sapVendorCode = sapInfo?.VCODE;
                    outputPass.sapVendorGroupCode = RegPayeePersonalInput?.sapVendorInfo?.sapVendorGroupCode;
                    //sapInfo?.VGROUP ?? regPayeePersonalInput?.sapVendorInfo?.sapVendorGroupCode ?? "";
                }

                //@TODO adHoc fix if fullname in null
                if (string.IsNullOrEmpty(outputPass.personalName))
                {
                    outputPass.personalName = RegPayeePersonalInput?.profileInfo.personalName;
                    outputPass.personalSurname = RegPayeePersonalInput?.profileInfo.personalSurname;
                }
                if (string.IsNullOrEmpty(outputPass.polisyClientId))
                {
                   outputPass.polisyClientId = RegPayeePersonalInput?.generalHeader?.polisyClientId;
                }
                if (string.IsNullOrEmpty(outputPass.cleansingId))
                {
                    outputPass.polisyClientId = RegPayeePersonalInput?.generalHeader?.cleansingId;
                }

            regPayeePersonalOutput.data.Add(outputPass);
            
           
            return regPayeePersonalOutput;

        }
    }
}