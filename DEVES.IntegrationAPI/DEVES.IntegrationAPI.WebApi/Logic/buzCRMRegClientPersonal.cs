using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;
using DEVES.IntegrationAPI.WebApi;
using DEVES.IntegrationAPI.WebApi.Templates;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.RegClientPersonal;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.Polisy400;


namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzCRMRegClientPersonal : BaseCommand
    {
        public override BaseDataModel Execute(object input)
        {
            RegClientPersonalContentOutputModel regClientPersonOutput = new RegClientPersonalContentOutputModel();
            regClientPersonOutput.data = new List<RegClientPersonalDataOutputModel>();
            try
            {

                RegClientPersonalInputModel regClientPersonalInput = (RegClientPersonalInputModel)input;

                if (string.IsNullOrEmpty(regClientPersonalInput.generalHeader.cleansingId))
                {
                    BaseDataModel clsCreatePersonIn = DataModelFactory.GetModel(typeof(CLSCreatePersonalClientInputModel));
                    clsCreatePersonIn = TransformerFactory.TransformModel(regClientPersonalInput, clsCreatePersonIn);
                    CLSCreatePersonalClientContentOutputModel clsCreateClientContent = CallDevesServiceProxy<CLSCreatePersonalClientOutputModel, CLSCreatePersonalClientContentOutputModel>
                                                                                        (CommonConstant.ewiEndpointKeyCLSCreatePersonalClient, clsCreatePersonIn);
                    regClientPersonalInput = (RegClientPersonalInputModel)TransformerFactory.TransformModel(clsCreateClientContent, regClientPersonalInput);

                }

                if (string.IsNullOrEmpty(regClientPersonalInput.generalHeader.polisyClientId))
                {
                    BaseDataModel polCreatePersonIn = DataModelFactory.GetModel(typeof(CLIENTCreatePersonalClientAndAdditionalInfoInputModel));
                    polCreatePersonIn = TransformerFactory.TransformModel(regClientPersonalInput, polCreatePersonIn);
                    CLIENTCreatePersonalClientAndAdditionalInfoContentModel polCreateClientContent = CallDevesServiceProxy<CLIENTCreatePersonalClientAndAdditionalInfoOutputModel
                                                                                                        , CLIENTCreatePersonalClientAndAdditionalInfoContentModel>
                                                                                                        (CommonConstant.ewiEndpointKeyCLIENTCreatePersonalClient, polCreatePersonIn);
                    regClientPersonalInput = (RegClientPersonalInputModel)TransformerFactory.TransformModel(polCreateClientContent, regClientPersonalInput);
                }

                buzCreateCrmClientPersonal cmdCreateCrmClient = new buzCreateCrmClientPersonal();
                CreateCrmPresonInfoOutputModel crmContentOutput = (CreateCrmPresonInfoOutputModel)cmdCreateCrmClient.Execute(regClientPersonalInput);

                if (crmContentOutput.code == CONST_CODE_SUCCESS)
                {
                    regClientPersonOutput.code = CONST_CODE_SUCCESS;
                    regClientPersonOutput.message = "SUCCESS";
                    RegClientPersonalDataOutputModel_Pass dataOutPass = new RegClientPersonalDataOutputModel_Pass();
                    dataOutPass.cleansingId = regClientPersonalInput.generalHeader.cleansingId;
                    dataOutPass.polisyClientId = regClientPersonalInput.generalHeader.polisyClientId;
                    dataOutPass.crmClientId = crmContentOutput.crmClientId;
                    regClientPersonOutput.data.Add(dataOutPass);
                }
                else
                {
                    regClientPersonOutput.code = CONST_CODE_FAILED;
                    regClientPersonOutput.message = crmContentOutput.message;
                    regClientPersonOutput.description = crmContentOutput.description;
                }
            }
            catch (Exception e)
            {
                regClientPersonOutput.code = CONST_CODE_FAILED;
                regClientPersonOutput.message = e.Message;
                regClientPersonOutput.description = e.StackTrace;
                RegClientPersonalDataOutputModel_Fail dataOutFail = new RegClientPersonalDataOutputModel_Fail();
                regClientPersonOutput.data.Add(dataOutFail);
            }

            return regClientPersonOutput;

        }
    }
}