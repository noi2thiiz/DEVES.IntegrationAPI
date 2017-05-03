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

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzCRMRegPayeePersonal: BaseCommand
    {

        RegPayeePersonalOutputModel_Fail regFail = new RegPayeePersonalOutputModel_Fail();

        public override BaseDataModel Execute(object input)
        {
            RegPayeePersonalContentOutputModel regPayeePersonalOutput = new RegPayeePersonalContentOutputModel();
            regPayeePersonalOutput.data = new List<RegPayeePersonalDataOutputModel>();
            regPayeePersonalOutput.code = CONST_CODE_SUCCESS;
            regPayeePersonalOutput.message = CONST_MESSAGE_SUCCESS;
            regPayeePersonalOutput.transactionDateTime = DateTime.Now;
            regPayeePersonalOutput.transactionId = TransactionId;
            try
            {
                RegPayeePersonalDataOutputModel_Pass outputPass = new RegPayeePersonalDataOutputModel_Pass();
                RegPayeePersonalInputModel regPayeePersonalInput = (RegPayeePersonalInputModel)input;

                // Validate Master Data before sending to other services
                regFail.data = new RegPayeePersonalDataOutputModel_Fail();
                regFail.data.fieldErrors = new List<RegPayeePersonalFieldErrors>();

                var master_salutation = PersonalTitleMasterData.Instance.FindByCode(regPayeePersonalInput.profileInfo.salutation);
                if (master_salutation == null)
                {
                    var message =
                        MessageBuilder.Instance.GetInvalidMasterMessage("Salutation",
                            regPayeePersonalInput.profileInfo.salutation);
                    regFail.data.fieldErrors.Add(new RegPayeePersonalFieldErrors("profileInfo.salutation",message ));
                }
                else
                {
                    regPayeePersonalInput.profileInfo.salutation = master_salutation.PolisyCode;
                }

                var master_nationality = NationalityMasterData.Instance.FindByCode(regPayeePersonalInput.profileInfo.nationality, "00203");
                if (master_nationality == null)
                {
                    var message =
                        MessageBuilder.Instance.GetInvalidMasterMessage("Nationality",
                            regPayeePersonalInput.profileInfo.nationality);
                    regFail.data.fieldErrors.Add(new RegPayeePersonalFieldErrors("profileInfo.nationality", message));
                }
                else
                {
                    regPayeePersonalInput.profileInfo.nationality = master_nationality.PolisyCode;
                }

                var master_occupation = OccupationMasterData.Instance.FindByCode(regPayeePersonalInput.profileInfo.occupation, "00023");
                if (master_occupation == null)
                {
                    var message =
                        MessageBuilder.Instance.GetInvalidMasterMessage("Occupation",
                            regPayeePersonalInput.profileInfo.occupation);
                    regFail.data.fieldErrors.Add(new RegPayeePersonalFieldErrors("profileInfo.occupation", message));
                }
                else
                {
                    regPayeePersonalInput.profileInfo.occupation = master_occupation.PolisyCode;
                }

                var master_country = CountryMasterData.Instance.FindByCode(regPayeePersonalInput.addressInfo.country, "00220");
                if (master_country == null)
                {
                    var message =
                        MessageBuilder.Instance.GetInvalidMasterMessage("Country",
                            regPayeePersonalInput.addressInfo.country);
                    regFail.data.fieldErrors.Add(new RegPayeePersonalFieldErrors("addressInfo.country", message));
                }
                else
                {
                    regPayeePersonalInput.addressInfo.country = master_country.PolisyCode;
                }

                if (regFail.data.fieldErrors.Count > 0)
                {
                    throw new FieldValidationException();
                }

                ///////////////////////////////////////////////////////////////////////////////////////////////////

                if (string.IsNullOrEmpty(regPayeePersonalInput?.generalHeader?.polisyClientId))
                {
                    if (string.IsNullOrEmpty(regPayeePersonalInput?.generalHeader?.cleansingId))
                    {
                        #region Create Payee in Cleansing
                        BaseDataModel clsCreatePersonalIn = DataModelFactory.GetModel(typeof(CLSCreatePersonalClientInputModel));
                        clsCreatePersonalIn = TransformerFactory.TransformModel(regPayeePersonalInput, clsCreatePersonalIn);
                        CLSCreatePersonalClientContentOutputModel clsCreatePayeeContent = CallDevesServiceProxy<CLSCreatePersonalClientOutputModel, CLSCreatePersonalClientContentOutputModel>
                                                                                            (CommonConstant.ewiEndpointKeyCLSCreatePersonalClient, clsCreatePersonalIn);
                        //regPayeePersonalInput = (RegPayeePersonalInputModel)TransformerFactory.TransformModel(clsCreatePayeeContent, regPayeePersonalInput);
                        if (clsCreatePayeeContent?.code == CONST_CODE_SUCCESS)
                        {
                            regPayeePersonalInput.generalHeader.cleansingId = clsCreatePayeeContent.data?.cleansingId ?? "";

                            outputPass.polisyClientId = clsCreatePayeeContent.data?.clientId;
                            outputPass.personalName = clsCreatePayeeContent.data?.personalName;
                            outputPass.personalSurname = clsCreatePayeeContent.data?.personalSurname;
                        }
                        #endregion Create Payee in Cleansing
                    }

                    #region Create Payee in Polisy400
                    BaseDataModel polCreatePersonalIn = DataModelFactory.GetModel(typeof(CLIENTCreatePersonalClientAndAdditionalInfoInputModel));
                    polCreatePersonalIn = TransformerFactory.TransformModel(regPayeePersonalInput, polCreatePersonalIn);
                    CLIENTCreatePersonalClientAndAdditionalInfoContentModel polCreatePayeeContent = CallDevesServiceProxy<CLIENTCreatePersonalClientAndAdditionalInfoOutputModel
                                                                                                        , CLIENTCreatePersonalClientAndAdditionalInfoContentModel>
                                                                                                        (CommonConstant.ewiEndpointKeyCLIENTCreatePersonalClient, polCreatePersonalIn);
                    //regPayeePersonalInput = (RegPayeePersonalInputModel)TransformerFactory.TransformModel(polCreatePayeeContent, regPayeePersonalInput);

                    if (polCreatePayeeContent != null)
                    {
                        regPayeePersonalInput.generalHeader.polisyClientId = polCreatePayeeContent.clientID; 

                        outputPass.polisyClientId = polCreatePayeeContent.clientID;
                    }
                             
                    #endregion Create Payee in Polisy400
                }

                #region Search Payee in SAP
                Model.SAP.SAPInquiryVendorInputModel SAPInqVendorIn = (Model.SAP.SAPInquiryVendorInputModel)DataModelFactory.GetModel(typeof(Model.SAP.SAPInquiryVendorInputModel));
                //SAPInqVendorIn = TransformerFactory.TransformModel(regPayeePersonalInput, SAPInqVendorIn);

                SAPInqVendorIn.TAX3 = regPayeePersonalInput.profileInfo.idCitizen??"";
                SAPInqVendorIn.TAX4 = "";
                SAPInqVendorIn.PREVACC = regPayeePersonalInput.sapVendorInfo.sapVendorCode ?? "";
                SAPInqVendorIn.VCODE = regPayeePersonalInput.generalHeader.polisyClientId ?? "";

                var SAPInqVendorContentOut = CallDevesServiceProxy<Model.SAP.SAPInquiryVendorOutputModel, Model.SAP.EWIResSAPInquiryVendorContentModel>(CommonConstant.ewiEndpointKeySAPInquiryVendor, SAPInqVendorIn);
                #endregion Search Payee in SAP

                var sapInfo = SAPInqVendorContentOut?.VendorInfo?.FirstOrDefault();

                if (string.IsNullOrEmpty(sapInfo?.VCODE))
                {
                    #region Create Payee in SAP
                    //BaseDataModel SAPCreateVendorIn = DataModelFactory.GetModel(typeof(Model.SAP.SAPCreateVendorInputModel));
                    Model.SAP.SAPCreateVendorInputModel SAPCreateVendorIn = new Model.SAP.SAPCreateVendorInputModel();
                    SAPCreateVendorIn = (Model.SAP.SAPCreateVendorInputModel)TransformerFactory.TransformModel(regPayeePersonalInput, SAPCreateVendorIn);

                    var SAPCreateVendorContentOut = CallDevesServiceProxy<Model.SAP.SAPCreateVendorOutputModel, Model.SAP.SAPCreateVendorContentOutputModel>(CommonConstant.ewiEndpointKeySAPCreateVendor, SAPCreateVendorIn);

                    regPayeePersonalInput.sapVendorInfo.sapVendorCode = SAPCreateVendorContentOut?.VCODE;
                    outputPass.sapVendorCode = SAPCreateVendorContentOut?.VCODE;
                    outputPass.sapVendorGroupCode = regPayeePersonalInput?.sapVendorInfo?.sapVendorGroupCode;


                    #endregion Create Payee in SAP

                    #region Create payee in CRM
                    buzCreateCrmPayeePersonal cmdCreateCrmPayee = new buzCreateCrmPayeePersonal();
                    CreateCrmPersonInfoOutputModel crmContentOutput = (CreateCrmPersonInfoOutputModel)cmdCreateCrmPayee.Execute(regPayeePersonalInput);

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
                    outputPass.sapVendorGroupCode = regPayeePersonalInput?.sapVendorInfo?.sapVendorGroupCode;
                    //sapInfo?.VGROUP ?? regPayeePersonalInput?.sapVendorInfo?.sapVendorGroupCode ?? "";
                }
                regPayeePersonalOutput.data.Add(outputPass);
            }
            catch (FieldValidationException e)
            {

                regFail.code = CONST_CODE_INVALID_INPUT;
                regFail.message = CONST_MESSAGE_INVALID_INPUT;
                regFail.description = CONST_DESC_INVALID_INPUT;
                regFail.transactionId = TransactionId;
                regFail.transactionDateTime = DateTime.Now;

                return regFail;
            }
            catch (Exception e)
            {
                regPayeePersonalOutput.code = CONST_CODE_FAILED;
                regPayeePersonalOutput.message = CONST_MESSAGE_INTERNAL_ERROR;
                regPayeePersonalOutput.description = e.StackTrace;
                regPayeePersonalOutput.transactionId = TransactionId;
                regPayeePersonalOutput.transactionDateTime= DateTime.Now;

                RegPayeePersonalDataOutputModel_Fail dataOutFail = new RegPayeePersonalDataOutputModel_Fail();
                regPayeePersonalOutput.data.Add(dataOutFail);
            }
            return regPayeePersonalOutput;

        }
    }
}