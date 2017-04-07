﻿using System;
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

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzCRMRegPayeePersonal: BaseCommand
    {

        public override BaseDataModel Execute(object input)
        {

            RegPayeePersonalContentOutputModel regPayeePersonalOutput = new RegPayeePersonalContentOutputModel();
            regPayeePersonalOutput.data = new List<RegPayeePersonalDataOutputModel>();
            RegPayeePersonalDataOutputModel_Pass outputPass = new RegPayeePersonalDataOutputModel_Pass();
            try
            {
                RegPayeePersonalInputModel regPayeePersonalInput = (RegPayeePersonalInputModel)input;

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
                BaseDataModel SAPInqVendorIn = DataModelFactory.GetModel(typeof(Model.SAP.SAPInquiryVendorInputModel));
                SAPInqVendorIn = TransformerFactory.TransformModel(regPayeePersonalInput, SAPInqVendorIn);
                var SAPInqVendorContentOut = CallDevesServiceProxy<Model.SAP.SAPInquiryVendorOutputModel, Model.SAP.SAPInquiryVendorContentVendorInfoModel>(CommonConstant.ewiEndpointKeySAPInquiryVendor, SAPInqVendorIn);
                #endregion Search Payee in SAP


                if (string.IsNullOrEmpty(SAPInqVendorContentOut?.VCODE))
                {
                    #region Create Payee in SAP
                    BaseDataModel SAPCreateVendorIn = DataModelFactory.GetModel(typeof(Model.SAP.SAPCreateVendorInputModel));
                    SAPCreateVendorIn = TransformerFactory.TransformModel(regPayeePersonalInput, SAPCreateVendorIn);
                    var SAPCreateVendorContentOut = CallDevesServiceProxy<Model.SAP.SAPCreateVendorOutputModel, Model.SAP.SAPCreateVendorContentOutputModel>(CommonConstant.ewiEndpointKeySAPCreateVendor, SAPCreateVendorIn);
                    regPayeePersonalInput.sapVendorInfo.sapVendorCode = SAPCreateVendorContentOut.VCODE;
                    #endregion Create Payee in SAP

                    #region Create payee in CRM
                    buzCreateCrmClientPersonal cmdCreateCrmPayee = new buzCreateCrmClientPersonal();
                    CreateCrmPersonInfoOutputModel crmContentOutput = (CreateCrmPersonInfoOutputModel)cmdCreateCrmPayee.Execute(regPayeePersonalInput);

                    if (crmContentOutput.code == CONST_CODE_SUCCESS)
                    {
                        regPayeePersonalOutput.code = CONST_CODE_SUCCESS;
                        regPayeePersonalOutput.message = "SUCCESS";
                        RegPayeePersonalDataOutputModel_Pass dataOutPass = new RegPayeePersonalDataOutputModel_Pass();
                        dataOutPass.polisyClientId = regPayeePersonalInput.generalHeader.polisyClientId;
                        dataOutPass.sapVendorCode = regPayeePersonalInput.sapVendorInfo.sapVendorCode;
                        dataOutPass.sapVendorGroupCode = regPayeePersonalInput.sapVendorInfo.sapVendorGroupCode;
                        dataOutPass.personalName = regPayeePersonalInput.profileInfo.personalName;
                        dataOutPass.personalSurname = regPayeePersonalInput.profileInfo.personalSurname;
                        //dataOutPass.corporateBranch = regPayeePersonalInput.profileInfo.corporateBranch;

                        regPayeePersonalOutput.data.Add(dataOutPass);
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
                    regPayeePersonalInput.sapVendorInfo.sapVendorCode = SAPInqVendorContentOut.VCODE;
                }

            }
            catch (Exception e)
            {
                regPayeePersonalOutput.code = CONST_CODE_FAILED;
                regPayeePersonalOutput.message = e.Message;
                regPayeePersonalOutput.description = e.StackTrace;

                RegPayeePersonalDataOutputModel_Fail dataOutFail = new RegPayeePersonalDataOutputModel_Fail();
                regPayeePersonalOutput.data.Add(dataOutFail);
            }
            regPayeePersonalOutput.transactionDateTime = DateTime.Now;
            regPayeePersonalOutput.transactionId = TransactionId;
            return regPayeePersonalOutput;

        }
    }
}