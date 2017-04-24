﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;
using DEVES.IntegrationAPI.WebApi;
using DEVES.IntegrationAPI.WebApi.Templates;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.RegClientCorporate;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.Polisy400;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzCRMRegClientCorporate : BaseCommand
    {
        public override BaseDataModel Execute(object input)
        {
            RegClientCorporateContentOutputModel regClientCorporateOutput = new RegClientCorporateContentOutputModel();
            regClientCorporateOutput.data = new List<RegClientCorporateDataOutputModel>();
            regClientCorporateOutput.transactionDateTime = DateTime.Now;
            regClientCorporateOutput.transactionId = TransactionId;
            regClientCorporateOutput.code = CONST_CODE_SUCCESS;

            try
            {

                RegClientCorporateInputModel regClientCorporateInput = (RegClientCorporateInputModel)input;

                if(IsASRHValid (regClientCorporateInput) )
                {
                    if (string.IsNullOrEmpty(regClientCorporateInput.generalHeader.cleansingId))
                    {
                        BaseDataModel clsCreateCorporateIn = DataModelFactory.GetModel(typeof(CLSCreateCorporateClientInputModel));
                        clsCreateCorporateIn = TransformerFactory.TransformModel(regClientCorporateInput, clsCreateCorporateIn);
                        CLSCreateCorporateClientContentOutputModel clsCreateClientContent = CallDevesServiceProxy<CLSCreateCorporateClientOutputModel, CLSCreateCorporateClientContentOutputModel>
                                                                                            (CommonConstant.ewiEndpointKeyCLSCreateCorporateClient, clsCreateCorporateIn);
                        regClientCorporateInput = (RegClientCorporateInputModel)TransformerFactory.TransformModel(clsCreateClientContent, regClientCorporateInput);

                    }

                    if (string.IsNullOrEmpty(regClientCorporateInput.generalHeader.polisyClientId) && !string.IsNullOrEmpty(regClientCorporateInput.generalHeader.cleansingId) )
                    {
                        BaseDataModel polCreateCorporateIn = DataModelFactory.GetModel(typeof(CLIENTCreateCorporateClientAndAdditionalInfoInputModel));
                        polCreateCorporateIn = TransformerFactory.TransformModel(regClientCorporateInput, polCreateCorporateIn);
                        CLIENTCreateCorporateClientAndAdditionalInfoContentModel polCreateClientContent = CallDevesServiceProxy<CLIENTCreateCorporateClientAndAdditionalInfoOutputModel
                                                                                                            , CLIENTCreateCorporateClientAndAdditionalInfoContentModel>
                                                                                                            (CommonConstant.ewiEndpointKeyCLIENTCreateCorporateClient, polCreateCorporateIn);
                        regClientCorporateInput = (RegClientCorporateInputModel)TransformerFactory.TransformModel(polCreateClientContent, regClientCorporateInput);
                    }

                    if( !string.IsNullOrEmpty(regClientCorporateInput.generalHeader.polisyClientId) )
                    {
                        if ((regClientCorporateInput.generalHeader.assessorFlag == "Y"
                            || regClientCorporateInput.generalHeader.solicitorFlag == "Y"
                            || regClientCorporateInput.generalHeader.repairerFlag == "Y"
                            || regClientCorporateInput.generalHeader.hospitalFlag == "Y"))
                        {
                            try
                            {
                                COMPInquiryClientMasterInputModel compInqClientInput = new COMPInquiryClientMasterInputModel();
                                compInqClientInput = (COMPInquiryClientMasterInputModel)TransformerFactory.TransformModel(regClientCorporateInput, compInqClientInput);

                                EWIResCOMPInquiryClientMasterContentModel retCOMPInqClient = CallDevesServiceProxy<COMPInquiryClientMasterOutputModel, EWIResCOMPInquiryClientMasterContentModel>
                                                                                                        (CommonConstant.ewiEndpointKeyCOMPInquiryClient, compInqClientInput);
                                //Found in Polisy400
                                if (retCOMPInqClient.clientListCollection != null && retCOMPInqClient.clientListCollection.Count == 1)
                                {
                                    COMPInquiryClientMasterContentClientListModel inqClientPolisy400Out = retCOMPInqClient.clientListCollection.First();

                                    CLIENTUpdateCorporateClientAndAdditionalInfoInputModel updateClientPolisy400In = (CLIENTUpdateCorporateClientAndAdditionalInfoInputModel) DataModelFactory.GetModel(typeof(CLIENTUpdateCorporateClientAndAdditionalInfoInputModel));
                                    updateClientPolisy400In = (CLIENTUpdateCorporateClientAndAdditionalInfoInputModel)TransformerFactory.TransformModel(inqClientPolisy400Out, updateClientPolisy400In);

                                    updateClientPolisy400In.assessorFlag = string.IsNullOrEmpty(regClientCorporateInput.generalHeader.assessorFlag) ? updateClientPolisy400In.assessorFlag : regClientCorporateInput.generalHeader.assessorFlag;
                                    updateClientPolisy400In.solicitorFlag = string.IsNullOrEmpty(regClientCorporateInput.generalHeader.solicitorFlag) ? updateClientPolisy400In.solicitorFlag : regClientCorporateInput.generalHeader.solicitorFlag;
                                    updateClientPolisy400In.repairerFlag = string.IsNullOrEmpty(regClientCorporateInput.generalHeader.repairerFlag) ? updateClientPolisy400In.repairerFlag : regClientCorporateInput.generalHeader.repairerFlag;
                                    updateClientPolisy400In.hospitalFlag = string.IsNullOrEmpty( regClientCorporateInput.generalHeader.hospitalFlag )? updateClientPolisy400In.hospitalFlag : regClientCorporateInput.generalHeader.hospitalFlag;

                                    if (regClientCorporateInput.generalHeader.clientAdditionalExistFlag == "Y")
                                    {
                                        updateClientPolisy400In.checkFlag = "UPDATE";
                                    }
                                    else if(regClientCorporateInput.generalHeader.clientAdditionalExistFlag == "N")
                                    {
                                        updateClientPolisy400In.checkFlag = "CREATE";
                                    }

                                    CallDevesServiceProxy<CLIENTUpdateCorporateClientAndAdditionalInfoOutputModel, CLIENTUpdateCorporateClientAndAdditionalInfoContentModel>(CommonConstant.ewiEndpointKeyCLIENTUpdateCorporateClient, updateClientPolisy400In);

                                    //CLIENTUpdateCorporateClientAndAdditionalInfoContentModel updateClientPolisy400Out = CallDevesServiceProxy<CLIENTUpdateCorporateClientAndAdditionalInfoOutputModel, CLIENTUpdateCorporateClientAndAdditionalInfoContentModel>(CommonConstant.ewiEndpointKeyCLIENTUpdateCorporateClient, updateClientPolisy400In);
                                }
                            }
                            catch (Exception e)
                            {
                                regClientCorporateOutput.code = CONST_CODE_FAILED;
                                regClientCorporateOutput.message = e.Message;
                                regClientCorporateOutput.description = e.StackTrace;

                                RegClientCorporateDataOutputModel_Fail dataOutFail = new RegClientCorporateDataOutputModel_Fail();
                                regClientCorporateOutput.data.Add(dataOutFail);
                            }
                        }

                        buzCreateCrmClientCorporate cmdCreateCrmClient = new buzCreateCrmClientCorporate();
                        CreateCrmCorporateInfoOutputModel crmContentOutput = (CreateCrmCorporateInfoOutputModel)cmdCreateCrmClient.Execute(regClientCorporateInput);

                        if (crmContentOutput.code == CONST_CODE_SUCCESS)
                        {
                            regClientCorporateOutput.code = CONST_CODE_SUCCESS;
                            regClientCorporateOutput.message = "SUCCESS";
                            RegClientCorporateDataOutputModel_Pass dataOutPass = new RegClientCorporateDataOutputModel_Pass();
                            dataOutPass.cleansingId = regClientCorporateInput.generalHeader.cleansingId;
                            dataOutPass.polisyClientId = regClientCorporateInput.generalHeader.polisyClientId;
                            dataOutPass.crmClientId = crmContentOutput.crmClientId;
                            dataOutPass.corporateName1 = regClientCorporateInput.profileHeader.corporateName1;
                            dataOutPass.corporateName2 = regClientCorporateInput.profileHeader.corporateName2;
                            dataOutPass.corporateBranch = regClientCorporateInput.profileHeader.corporateBranch;

                            regClientCorporateOutput.data.Add(dataOutPass);
                        }
                        else
                        {
                            regClientCorporateOutput.code = CONST_CODE_FAILED;
                            regClientCorporateOutput.message = crmContentOutput.message;
                            regClientCorporateOutput.description = crmContentOutput.description;
                        }

                    }
                }
                else
                {
                    regClientCorporateOutput.code = CONST_CODE_FAILED;
                    regClientCorporateOutput.message = "There is some conflicts among the arguments";
                    regClientCorporateOutput.description = "Look between roleCode and {assessorFlag ,solicitorFlag ,repairerFlag or hospitalFlag}";
                }

            }
            catch (Exception e)
            {
                regClientCorporateOutput.code = CONST_CODE_FAILED;
                regClientCorporateOutput.message = e.Message;
                regClientCorporateOutput.description = e.StackTrace;

                RegClientCorporateDataOutputModel_Fail dataOutFail = new RegClientCorporateDataOutputModel_Fail();
                regClientCorporateOutput.data.Add(dataOutFail);
            }
            return regClientCorporateOutput;

        }

        bool IsASRHValid(RegClientCorporateInputModel input)
        {
            bool validFlag = true;
            if (
                (input.generalHeader.roleCode == "A" && input.generalHeader.assessorFlag != "Y" ) ||
                (input.generalHeader.roleCode == "S" && input.generalHeader.solicitorFlag != "Y" ) ||
                (input.generalHeader.roleCode == "R" && input.generalHeader.repairerFlag != "Y" ) ||
                (input.generalHeader.roleCode == "H" && input.generalHeader.hospitalFlag != "Y" )
            )
            {
                validFlag = false;
            }
            return validFlag;
        }
    }
}