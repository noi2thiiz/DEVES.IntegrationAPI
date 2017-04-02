using System;
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
            try
            {

                RegClientCorporateInputModel regClientCorporateInput = (RegClientCorporateInputModel)input;

                if (string.IsNullOrEmpty(regClientCorporateInput.generalHeader.cleansingId))
                {
                    BaseDataModel clsCreateCorporateIn = DataModelFactory.GetModel(typeof(CLSCreateCorporateClientInputModel));
                    clsCreateCorporateIn = TransformerFactory.TransformModel(regClientCorporateInput, clsCreateCorporateIn);
                    CLSCreateCorporateClientContentOutputModel clsCreateClientContent = CallDevesServiceProxy<CLSCreateCorporateClientOutputModel, CLSCreateCorporateClientContentOutputModel>
                                                                                        (CommonConstant.ewiEndpointKeyCLSCreateCorporateClient, clsCreateCorporateIn);
                    regClientCorporateInput = (RegClientCorporateInputModel)TransformerFactory.TransformModel(clsCreateClientContent, regClientCorporateInput);

                }

                if (string.IsNullOrEmpty(regClientCorporateInput.generalHeader.polisyClientId))
                {
                    BaseDataModel polCreateCorporateIn = DataModelFactory.GetModel(typeof(CLIENTCreateCorporateClientAndAdditionalInfoInputModel));
                    polCreateCorporateIn = TransformerFactory.TransformModel(regClientCorporateInput, polCreateCorporateIn);
                    CLIENTCreateCorporateClientAndAdditionalInfoContentModel polCreateClientContent = CallDevesServiceProxy<CLIENTCreateCorporateClientAndAdditionalInfoOutputModel
                                                                                                        , CLIENTCreateCorporateClientAndAdditionalInfoContentModel>
                                                                                                        (CommonConstant.ewiEndpointKeyCLIENTCreateCorporateClient, polCreateCorporateIn);
                    regClientCorporateInput = (RegClientCorporateInputModel)TransformerFactory.TransformModel(polCreateClientContent, regClientCorporateInput);
                }

                if(regClientCorporateInput.generalHeader.assessorFlag == "Y" 
                    || regClientCorporateInput.generalHeader.solicitorFlag == "Y" 
                    || regClientCorporateInput.generalHeader.repairerFlag == "Y" 
                    || regClientCorporateInput.generalHeader.hospitalFlag == "Y" )
                {
                    COMPInquiryClientMasterInputModel compInqClientInput = new COMPInquiryClientMasterInputModel();
                    compInqClientInput = (COMPInquiryClientMasterInputModel)TransformerFactory.TransformModel(regClientCorporateInput, compInqClientInput);

                    //+ Call CLS_InquiryCLSPersonalClient through ServiceProxy
                    EWIResCOMPInquiryClientMasterContentModel retCOMPInqClient = CallDevesServiceProxy<COMPInquiryClientMasterOutputModel, EWIResCOMPInquiryClientMasterContentModel>
                                                                                            (CommonConstant.ewiEndpointKeyCOMPInquiryClient, compInqClientInput);

                    //Found in Polisy400
                    if (retCOMPInqClient.clientListCollection != null && retCOMPInqClient.clientListCollection.Count == 1)
                    {
                        COMPInquiryClientMasterContentClientListModel inqClientPolisy400Out = retCOMPInqClient.clientListCollection.First();

                        BaseDataModel updateClientPolisy400In = DataModelFactory.GetModel( typeof( CLIENTUpdateCorporateClientAndAdditionalInfoInputModel) );
                        updateClientPolisy400In = TransformerFactory.TransformModel(inqClientPolisy400Out, updateClientPolisy400In);

                        CallDevesServiceProxy<CLIENTUpdateCorporateClientAndAdditionalInfoOutputModel, CLIENTUpdateCorporateClientAndAdditionalInfoContentModel>(CommonConstant.ewiEndpointKeyCLIENTUpdateCorporateClient, updateClientPolisy400In);
                        
                        //CLIENTUpdateCorporateClientAndAdditionalInfoContentModel updateClientPolisy400Out = CallDevesServiceProxy<CLIENTUpdateCorporateClientAndAdditionalInfoOutputModel, CLIENTUpdateCorporateClientAndAdditionalInfoContentModel>(CommonConstant.ewiEndpointKeyCLIENTUpdateCorporateClient, updateClientPolisy400In);
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
                    regClientCorporateOutput.data.Add(dataOutPass);
                }
                else
                {
                    regClientCorporateOutput.code = CONST_CODE_FAILED;
                    regClientCorporateOutput.message = crmContentOutput.message;
                    regClientCorporateOutput.description = crmContentOutput.description;
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
    }
}