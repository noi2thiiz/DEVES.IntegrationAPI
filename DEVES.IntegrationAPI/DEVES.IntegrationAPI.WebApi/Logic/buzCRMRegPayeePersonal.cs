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
using DEVES.IntegrationAPI.WebApi.Templates.Exceptions;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzCRMRegPayeePersonal: BuzCommand
    {

      
        private RegPayeePersonalInputModel RegPayeePersonalInput { get; set; }
        private RegPayeePersonalDataOutputModel_Pass outputPass { get; set; }= new RegPayeePersonalDataOutputModel_Pass();
        
        // Validate Master Data before sending to other services
        public void TranFormInput(RegPayeePersonalInputModel regPayeePersonalInput)
        {
            // ป้องกันปัญหา locus ส่ง json มาไม่ครบ
            if (regPayeePersonalInput.addressInfo == null)
            {
                regPayeePersonalInput.addressInfo = new AddressInfoModel();
            }
            if (regPayeePersonalInput.contactInfo == null)
            {
                regPayeePersonalInput.contactInfo = new ContactInfoModel();
            }
            if (regPayeePersonalInput.generalHeader == null)
            {
                regPayeePersonalInput.generalHeader = new GeneralHeaderModel();
            }
            if (regPayeePersonalInput.profileInfo ==null)
            {
                regPayeePersonalInput.profileInfo = new ProfileInfoModel();
            }
            if (regPayeePersonalInput.sapVendorInfo == null)
            {
                regPayeePersonalInput.sapVendorInfo = new SapVendorInfoModel();
            }

            Console.WriteLine("27");;

           
            var fieldErrorData = new OutputModelFailData();
            Console.WriteLine("31"); ;

            var masterSalutation = PersonalTitleMasterData.Instance.FindByCode(regPayeePersonalInput?.profileInfo?.salutation,"0001");
            if (masterSalutation == null)
            {
                var message =
                    MessageBuilder.Instance.GetInvalidMasterMessage("Salutation",
                        regPayeePersonalInput.profileInfo.salutation);

                fieldErrorData.AddFieldError("profileInfo.salutation", message);
            }
            else
            {
                regPayeePersonalInput.profileInfo.salutation = masterSalutation.PolisyCode;
            }
            Console.WriteLine("46"); ;
            var masterNationality = NationalityMasterData.Instance.FindByCode(regPayeePersonalInput?.profileInfo?.nationality, "00203");
            if (masterNationality == null)
            {
                var message =
                    MessageBuilder.Instance.GetInvalidMasterMessage("Nationality",
                        regPayeePersonalInput.profileInfo.nationality);
              
                fieldErrorData.AddFieldError("profileInfo.nationality", message);
            }
            else
            {
                regPayeePersonalInput.profileInfo.nationality = masterNationality.PolisyCode;
            }
            Console.WriteLine("60"); ;
            var masterOccupation = OccupationMasterData.Instance.FindByCode(regPayeePersonalInput?.profileInfo?.occupation, "00023");
            if (masterOccupation == null)
            {
                var message =
                    MessageBuilder.Instance.GetInvalidMasterMessage("Occupation",
                        regPayeePersonalInput.profileInfo.occupation);

                fieldErrorData.AddFieldError("profileInfo.occupation", message);

               
            }
            else
            {
                regPayeePersonalInput.profileInfo.occupation = masterOccupation.PolisyCode;
            }
            Console.WriteLine("76"); ;
            var masterCountry = CountryMasterData.Instance.FindByCode(regPayeePersonalInput?.addressInfo?.country, "00220");
            if (masterCountry == null)
            {
                var message =
                    MessageBuilder.Instance.GetInvalidMasterMessage("Country",
                        regPayeePersonalInput.addressInfo.country);
                fieldErrorData.AddFieldError("addressInfo.country", message);
               
            }
            else
            {
                regPayeePersonalInput.addressInfo.country = masterCountry.PolisyCode;
            }
            Console.WriteLine("90"); ;

            if (fieldErrorData.fieldErrors.Any())
            {
                throw new FieldValidationException(fieldErrorData);
            }

      
          
        }
        public override BaseDataModel ExecuteInput(object input)
        {
            RegPayeePersonalInput = (RegPayeePersonalInputModel)input;

            var regPayeePersonalOutput =
                new RegPayeePersonalContentOutputModel
                {
                    data = new List<RegPayeePersonalDataOutputModel>(),
                    code = CONST_CODE_SUCCESS,
                    message = CONST_MESSAGE_SUCCESS,
                    transactionDateTime = DateTime.Now,
                    transactionId = TransactionId
                };



            TranFormInput(RegPayeePersonalInput);


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

                regPayeePersonalOutput.data.Add(outputPass);
            
           
            return regPayeePersonalOutput;

        }
    }
}