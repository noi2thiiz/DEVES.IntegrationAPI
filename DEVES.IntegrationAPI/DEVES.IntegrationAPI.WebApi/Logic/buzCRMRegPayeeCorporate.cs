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
using DEVES.IntegrationAPI.WebApi.DataAccessService.MasterData;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzCRMRegPayeeCorporate : BaseCommand
    {

        public RegPayeeCorporateOutputModel_Fail regFail { get; set; } = new RegPayeeCorporateOutputModel_Fail();


        public override BaseDataModel Execute(object input)
        {
            RegPayeeCorporateContentOutputModel regPayeeCorporateOutput = new RegPayeeCorporateContentOutputModel();
            regPayeeCorporateOutput.data = new List<RegPayeeCorporateDataOutputModel>();
            regPayeeCorporateOutput.code = CONST_CODE_SUCCESS;
            regPayeeCorporateOutput.message = "SUCCESS";
            regPayeeCorporateOutput.description = "";
            regPayeeCorporateOutput.transactionDateTime = DateTime.Now;
            regPayeeCorporateOutput.transactionId = TransactionId;
            try
            {
                RegPayeeCorporateDataOutputModel_Pass outputPass = new RegPayeeCorporateDataOutputModel_Pass();
                RegPayeeCorporateInputModel regPayeeCorporateInput = (RegPayeeCorporateInputModel)input;

                // Validate Master Data before sending to other services

                regFail.data = new RegPayeeCorporateDataOutputModel_Fail();
                regFail.data.fieldErrors = new List<RegPayeeCorporateFieldErrors>();

                var countryOrigin = regPayeeCorporateInput.profileHeader.countryOrigin;

                var master_countryorigin = NationalityMasterData.Instance.FindByCode(countryOrigin, "00203");

                if (master_countryorigin == null)
                {
                    Console.WriteLine("NationalityMasterData is invalid");
                    regFail.data.fieldErrors.Add(new RegPayeeCorporateFieldErrors("profileHeader.countryOrigin", "NationalityMasterData is invalid"));
                }
                else
                {
                    Console.WriteLine("NationalityMasterData  invalid");
                    regPayeeCorporateInput.profileHeader.countryOrigin = master_countryorigin.PolisyCode;
                }

                var master_country = CountryMasterData.Instance.FindByCode(regPayeeCorporateInput.addressHeader.country, "00220");
                if (master_country == null)
                {
                    regFail.data.fieldErrors.Add(new RegPayeeCorporateFieldErrors("addressHeader.country", "CountryMasterData is invalid"));
                }
                else
                {
                    regPayeeCorporateInput.addressHeader.country = master_country.PolisyCode;
                }


                if (regFail.data.fieldErrors.Count > 0)
                {
                    throw new FieldValidationException();
                }

                ///////////////////////////////////////////////////////////////////////////////////////////////////

                if (string.IsNullOrEmpty(regPayeeCorporateInput?.generalHeader?.polisyClientId))
                {
                    if (string.IsNullOrEmpty(regPayeeCorporateInput?.generalHeader?.cleansingId))
                    {
                        #region Create Payee in Cleansing
                        BaseDataModel clsCreateCorporateIn = DataModelFactory.GetModel(typeof(CLSCreateCorporateClientInputModel));
                        clsCreateCorporateIn = TransformerFactory.TransformModel(regPayeeCorporateInput, clsCreateCorporateIn);
                        CLSCreateCorporateClientContentOutputModel clsCreatePayeeContent = CallDevesServiceProxy<CLSCreateCorporateClientOutputModel, CLSCreateCorporateClientContentOutputModel>
                                                                                            (CommonConstant.ewiEndpointKeyCLSCreateCorporateClient, clsCreateCorporateIn);
                        //regPayeeCorporateInput = (RegPayeeCorporateInputModel)TransformerFactory.TransformModel(clsCreatePayeeContent, regPayeeCorporateInput);
                        if (clsCreatePayeeContent?.code == CONST_CODE_SUCCESS )
                        {
                            regPayeeCorporateInput.generalHeader.cleansingId = clsCreatePayeeContent.data?.cleansingId ?? "";

                            outputPass.polisyClientId = clsCreatePayeeContent.data?.clientId;
                            outputPass.corporateName1 = clsCreatePayeeContent.data?.corporateName1;
                            outputPass.corporateName2 = clsCreatePayeeContent.data?.corporateName2;
                        }
                        #endregion Create Payee in Cleansing
                    }

                    #region Create Payee in Polisy400
                    BaseDataModel polCreateCorporateIn = DataModelFactory.GetModel(typeof(CLIENTCreateCorporateClientAndAdditionalInfoInputModel));
                    polCreateCorporateIn = TransformerFactory.TransformModel(regPayeeCorporateInput, polCreateCorporateIn);
                    CLIENTCreateCorporateClientAndAdditionalInfoContentModel polCreatePayeeContent = CallDevesServiceProxy<CLIENTCreateCorporateClientAndAdditionalInfoOutputModel
                                                                                                        , CLIENTCreateCorporateClientAndAdditionalInfoContentModel>
                                                                                                        (CommonConstant.ewiEndpointKeyCLIENTCreateCorporateClient, polCreateCorporateIn);
                    //regPayeeCorporateInput = (RegPayeeCorporateInputModel)TransformerFactory.TransformModel(polCreatePayeeContent, regPayeeCorporateInput);

                    if (polCreatePayeeContent != null)
                    {
                        regPayeeCorporateInput.generalHeader.polisyClientId = polCreatePayeeContent.clientID;

                        outputPass.polisyClientId = polCreatePayeeContent.clientID;
                    }

                    #endregion Create Payee in Polisy400
                }

                #region Search Payee in SAP
                Model.SAP.SAPInquiryVendorInputModel SAPInqVendorIn = (Model.SAP.SAPInquiryVendorInputModel)DataModelFactory.GetModel(typeof(Model.SAP.SAPInquiryVendorInputModel));
                //SAPInqVendorIn = TransformerFactory.TransformModel(regPayeeCorporateInput, SAPInqVendorIn);

                SAPInqVendorIn.TAX3 = regPayeeCorporateInput.profileHeader.idTax?? "";
                SAPInqVendorIn.TAX4 = regPayeeCorporateInput.profileHeader.corporateBranch?? "";
                SAPInqVendorIn.PREVACC = regPayeeCorporateInput.sapVendorInfo.sapVendorCode ?? "";
                SAPInqVendorIn.VCODE = regPayeeCorporateInput.generalHeader.polisyClientId ?? "";

                var SAPInqVendorContentOut = CallDevesServiceProxy<Model.SAP.SAPInquiryVendorOutputModel, Model.SAP.EWIResSAPInquiryVendorContentModel>(CommonConstant.ewiEndpointKeySAPInquiryVendor, SAPInqVendorIn);
                #endregion Search Payee in SAP

                var sapInfo = SAPInqVendorContentOut?.VendorInfo?.FirstOrDefault();

                if (string.IsNullOrEmpty(sapInfo?.VCODE))
                {
                    #region Create Payee in SAP
                    //BaseDataModel SAPCreateVendorIn = DataModelFactory.GetModel(typeof(Model.SAP.SAPCreateVendorInputModel));
                    Model.SAP.SAPCreateVendorInputModel SAPCreateVendorIn = new Model.SAP.SAPCreateVendorInputModel();
                    SAPCreateVendorIn = (Model.SAP.SAPCreateVendorInputModel)TransformerFactory.TransformModel(regPayeeCorporateInput, SAPCreateVendorIn);

                    var SAPCreateVendorContentOut = CallDevesServiceProxy<Model.SAP.SAPCreateVendorOutputModel, Model.SAP.SAPCreateVendorContentOutputModel>(CommonConstant.ewiEndpointKeySAPCreateVendor, SAPCreateVendorIn);

                    regPayeeCorporateInput.sapVendorInfo.sapVendorCode = SAPCreateVendorContentOut?.VCODE;
                    outputPass.sapVendorCode = SAPCreateVendorContentOut?.VCODE;

                    #endregion Create Payee in SAP

                    #region Create payee in CRM
                    buzCreateCrmPayeeCorporate cmdCreateCrmPayee = new buzCreateCrmPayeeCorporate();
                    CreateCrmCorporateInfoOutputModel crmContentOutput = (CreateCrmCorporateInfoOutputModel)cmdCreateCrmPayee.Execute(regPayeeCorporateInput);

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
                    #endregion Create payee in CRM
                }
                else
                {
                    //regPayeeCorporateInput.sapVendorInfo.sapVendorCode = SAPInqVendorContentOut.VCODE;
                    outputPass.sapVendorCode = sapInfo?.VCODE;
                    outputPass.sapVendorGroupCode = sapInfo?.VGROUP;
                }
                regPayeeCorporateOutput.data.Add(outputPass);
            }
            catch (FieldValidationException e)
            {

                regFail.code = CONST_CODE_FAILED;
                regFail.message = e.Message;
                regFail.description = e.StackTrace;
                regFail.transactionId = TransactionId;
                regFail.transactionDateTime = DateTime.Now.ToString();

                return regFail;
            }
            catch (Exception e)
            {
                regPayeeCorporateOutput.code = CONST_CODE_FAILED;
                regPayeeCorporateOutput.message = e.Message;
                regPayeeCorporateOutput.description = e.StackTrace;

                RegPayeeCorporateDataOutputModel_Fail dataOutFail = new RegPayeeCorporateDataOutputModel_Fail();
                regPayeeCorporateOutput.data.Add(dataOutFail);
            }
            return regPayeeCorporateOutput;

        }

        public BaseDataModel XExecuteX(object input)
        {
            
            RegPayeeCorporateContentOutputModel regPayeeCorporateOutput = new RegPayeeCorporateContentOutputModel();
            regPayeeCorporateOutput.data = new List<RegPayeeCorporateDataOutputModel>();
            try
            {

                RegPayeeCorporateInputModel regPayeeCorporateInput = (RegPayeeCorporateInputModel)input;

                if (string.IsNullOrEmpty(regPayeeCorporateInput.generalHeader.cleansingId))
                {
                    BaseDataModel clsCreateCorporateIn = DataModelFactory.GetModel(typeof(CLSCreateCorporateClientInputModel));
                    clsCreateCorporateIn = TransformerFactory.TransformModel(regPayeeCorporateInput, clsCreateCorporateIn);
                    CLSCreateCorporateClientContentOutputModel clsCreatePayeeContent = CallDevesServiceProxy<CLSCreateCorporateClientOutputModel, CLSCreateCorporateClientContentOutputModel>
                                                                                        (CommonConstant.ewiEndpointKeyCLSCreateCorporateClient, clsCreateCorporateIn);
                    regPayeeCorporateInput = (RegPayeeCorporateInputModel)TransformerFactory.TransformModel(clsCreatePayeeContent, regPayeeCorporateInput);

                }

                if (string.IsNullOrEmpty(regPayeeCorporateInput.generalHeader.polisyClientId))
                {
                    BaseDataModel polCreateCorporateIn = DataModelFactory.GetModel(typeof(CLIENTCreateCorporateClientAndAdditionalInfoInputModel));
                    polCreateCorporateIn = TransformerFactory.TransformModel(regPayeeCorporateInput, polCreateCorporateIn);
                    CLIENTCreateCorporateClientAndAdditionalInfoContentModel polCreatePayeeContent = CallDevesServiceProxy<CLIENTCreateCorporateClientAndAdditionalInfoOutputModel
                                                                                                        , CLIENTCreateCorporateClientAndAdditionalInfoContentModel>
                                                                                                        (CommonConstant.ewiEndpointKeyCLIENTCreateCorporateClient, polCreateCorporateIn);
                    regPayeeCorporateInput = (RegPayeeCorporateInputModel)TransformerFactory.TransformModel(polCreatePayeeContent, regPayeeCorporateInput);
                }

                BaseDataModel SAPInqVendorIn = DataModelFactory.GetModel(typeof(Model.SAP.SAPInquiryVendorInputModel));
                SAPInqVendorIn = TransformerFactory.TransformModel(regPayeeCorporateInput, SAPInqVendorIn);
                var SAPInqVendorContentOut = CallDevesServiceProxy<Model.SAP.SAPInquiryVendorOutputModel, Model.SAP.SAPInquiryVendorContentVendorInfoModel>( CommonConstant.ewiEndpointKeySAPInquiryVendor , SAPInqVendorIn );
                if (SAPInqVendorContentOut != null && !string.IsNullOrEmpty(SAPInqVendorContentOut.VCODE))
                {
                    regPayeeCorporateInput.sapVendorInfo.sapVendorCode = SAPInqVendorContentOut.VCODE;
                }
                else
                { 
                    BaseDataModel SAPCreateVendorIn = DataModelFactory.GetModel(typeof(Model.SAP.SAPCreateVendorInputModel));
                    SAPCreateVendorIn = TransformerFactory.TransformModel(regPayeeCorporateInput, SAPCreateVendorIn);
                    var SAPCreateVendorContentOut = CallDevesServiceProxy<Model.SAP.SAPCreateVendorOutputModel, Model.SAP.SAPCreateVendorContentOutputModel>(CommonConstant.ewiEndpointKeySAPCreateVendor, SAPCreateVendorIn);
                    regPayeeCorporateInput.sapVendorInfo.sapVendorCode = SAPCreateVendorContentOut.VCODE;
                }

                buzCreateCrmClientCorporate cmdCreateCrmPayee = new buzCreateCrmClientCorporate();
                CreateCrmCorporateInfoOutputModel crmContentOutput = (CreateCrmCorporateInfoOutputModel)cmdCreateCrmPayee.Execute(regPayeeCorporateInput);

                if (crmContentOutput.code == CONST_CODE_SUCCESS)
                {
                    regPayeeCorporateOutput.code = CONST_CODE_SUCCESS;
                    regPayeeCorporateOutput.message = "SUCCESS";
                    RegPayeeCorporateDataOutputModel_Pass dataOutPass = new RegPayeeCorporateDataOutputModel_Pass();
                    dataOutPass.polisyClientId = regPayeeCorporateInput.generalHeader.polisyClientId;
                    dataOutPass.sapVendorCode = regPayeeCorporateInput.sapVendorInfo.sapVendorCode;
                    dataOutPass.sapVendorGroupCode = regPayeeCorporateInput.sapVendorInfo.sapVendorGroupCode;
                    dataOutPass.corporateName1 = regPayeeCorporateInput.profileHeader.corporateName1;
                    dataOutPass.corporateName2 = regPayeeCorporateInput.profileHeader.corporateName2;
                    dataOutPass.corporateBranch = regPayeeCorporateInput.profileHeader.corporateBranch;

                    regPayeeCorporateOutput.data.Add(dataOutPass);
                }
                else
                {
                    regPayeeCorporateOutput.code = CONST_CODE_FAILED;
                    regPayeeCorporateOutput.message = crmContentOutput.message;
                    regPayeeCorporateOutput.description = crmContentOutput.description;
                }
            }
            catch (Exception e)
            {
                regPayeeCorporateOutput.code = CONST_CODE_FAILED;
                regPayeeCorporateOutput.message = e.Message;
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