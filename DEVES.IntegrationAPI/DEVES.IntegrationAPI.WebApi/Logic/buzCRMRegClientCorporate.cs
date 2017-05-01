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
using DEVES.IntegrationAPI.WebApi.DataAccessService.MasterData;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzCRMRegClientCorporate : BaseCommand
    {

        public RegClientCorporateOutputModel_Fail regFail { get; set; } = new RegClientCorporateOutputModel_Fail();

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

                // Validate Master Data before sending to other services
                regFail  = new RegClientCorporateOutputModel_Fail();
                regFail.data = new RegClientCorporateDataOutputModel_Fail();
                regFail.data.fieldErrors = new List<RegClientCorporateFieldErrors>();



               Console.WriteLine(regClientCorporateInput.profileHeader.countryOrigin);
                NationalityEntity master_countryorigin = NationalityMasterData.Instance.FindByCode(regClientCorporateInput.profileHeader.countryOrigin, "00203");
                Console.WriteLine(master_countryorigin.ToJson());
                if (master_countryorigin == null)
                {

                      regFail.data.fieldErrors.Add( new RegClientCorporateFieldErrors("profileHeader.countryOrigin",
                          "NationalityMasterData is invalid"));

                }
                else
                {
                    //Console.WriteLine("set countryOrigin ");
                    regClientCorporateInput.profileHeader.countryOrigin = master_countryorigin.PolisyCode;
                }






                if (regFail.data.fieldErrors.Count > 0)
                {
                    Console.WriteLine("regFail.data.fieldErrors.Count > 0");
                    throw new FieldValidationException();
                }

                ///////////////////////////////////////////////////////////////////////////////////////////////////

                if (IsASRHValid (regClientCorporateInput) )
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
                    //กรณีมาจากหน้าจอ CRM  notCreatePolisyClientFlag == Y ไม่ต้องไปสร้างใน polisyClientId
                    if (regClientCorporateInput.generalHeader.notCreatePolisyClientFlag != "Y")
                    {

                        if (!string.IsNullOrEmpty(regClientCorporateInput.generalHeader.polisyClientId))
                        {
                            if ((regClientCorporateInput.generalHeader.assessorFlag == "Y"
                                 || regClientCorporateInput.generalHeader.solicitorFlag == "Y"
                                 || regClientCorporateInput.generalHeader.repairerFlag == "Y"
                                 || regClientCorporateInput.generalHeader.hospitalFlag == "Y"))
                            {
                                try
                                {
                                    COMPInquiryClientMasterInputModel compInqClientInput =
                                        new COMPInquiryClientMasterInputModel();
                                    compInqClientInput =
                                        (COMPInquiryClientMasterInputModel) TransformerFactory.TransformModel(
                                            regClientCorporateInput, compInqClientInput);

                                    EWIResCOMPInquiryClientMasterContentModel retCOMPInqClient =
                                        CallDevesServiceProxy<COMPInquiryClientMasterOutputModel,
                                                EWIResCOMPInquiryClientMasterContentModel>
                                            (CommonConstant.ewiEndpointKeyCOMPInquiryClient, compInqClientInput);
                                    //Found in Polisy400
                                    if (retCOMPInqClient.clientListCollection != null &&
                                        retCOMPInqClient.clientListCollection.Count == 1)
                                    {
                                        COMPInquiryClientMasterContentClientListModel inqClientPolisy400Out =
                                            retCOMPInqClient.clientListCollection.First();

                                        CLIENTUpdateCorporateClientAndAdditionalInfoInputModel updateClientPolisy400In =
                                            (CLIENTUpdateCorporateClientAndAdditionalInfoInputModel) DataModelFactory
                                                .GetModel(
                                                    typeof(CLIENTUpdateCorporateClientAndAdditionalInfoInputModel));

                                        if (inqClientPolisy400Out.clientList.additionalExistFlag == "Y")
                                        {
                                            updateClientPolisy400In =
                                                (CLIENTUpdateCorporateClientAndAdditionalInfoInputModel)
                                                TransformerFactory.TransformModel(inqClientPolisy400Out,
                                                    updateClientPolisy400In);
                                            updateClientPolisy400In.checkFlag = "UPDATE";
                                        }
                                        else if (inqClientPolisy400Out.clientList.additionalExistFlag == "N")
                                        {
                                            updateClientPolisy400In =
                                                (CLIENTUpdateCorporateClientAndAdditionalInfoInputModel)
                                                TransformerFactory.TransformModel(regClientCorporateInput,
                                                    updateClientPolisy400In);
                                            updateClientPolisy400In.checkFlag = "CREATE";
                                        }
                                        updateClientPolisy400In.assessorFlag =
                                            string.IsNullOrEmpty(regClientCorporateInput.generalHeader.assessorFlag)
                                                ? updateClientPolisy400In.assessorFlag
                                                : regClientCorporateInput.generalHeader.assessorFlag;
                                        updateClientPolisy400In.solicitorFlag =
                                            string.IsNullOrEmpty(regClientCorporateInput.generalHeader.solicitorFlag)
                                                ? updateClientPolisy400In.solicitorFlag
                                                : regClientCorporateInput.generalHeader.solicitorFlag;
                                        updateClientPolisy400In.repairerFlag =
                                            string.IsNullOrEmpty(regClientCorporateInput.generalHeader.repairerFlag)
                                                ? updateClientPolisy400In.repairerFlag
                                                : regClientCorporateInput.generalHeader.repairerFlag;
                                        updateClientPolisy400In.hospitalFlag =
                                            string.IsNullOrEmpty(regClientCorporateInput.generalHeader.hospitalFlag)
                                                ? updateClientPolisy400In.hospitalFlag
                                                : regClientCorporateInput.generalHeader.hospitalFlag;

                                        CallDevesServiceProxy<CLIENTUpdateCorporateClientAndAdditionalInfoOutputModel,
                                            CLIENTUpdateCorporateClientAndAdditionalInfoContentModel>(
                                            CommonConstant.ewiEndpointKeyCLIENTUpdateCorporateClient,
                                            updateClientPolisy400In);

                                        //CLIENTUpdateCorporateClientAndAdditionalInfoContentModel updateClientPolisy400Out = CallDevesServiceProxy<CLIENTUpdateCorporateClientAndAdditionalInfoOutputModel, CLIENTUpdateCorporateClientAndAdditionalInfoContentModel>(CommonConstant.ewiEndpointKeyCLIENTUpdateCorporateClient, updateClientPolisy400In);
                                    }
                                }
                                catch (Exception e)
                                {
                                    regClientCorporateOutput.code = CONST_CODE_FAILED;
                                    regClientCorporateOutput.message = e.Message;
                                    regClientCorporateOutput.description = e.StackTrace;

                                    RegClientCorporateDataOutputModel_Fail dataOutFail =
                                        new RegClientCorporateDataOutputModel_Fail();
                                    regClientCorporateOutput.data.Add(dataOutFail);
                                }
                            }

                            buzCreateCrmClientCorporate cmdCreateCrmClient = new buzCreateCrmClientCorporate();
                            CreateCrmCorporateInfoOutputModel crmContentOutput =
                                (CreateCrmCorporateInfoOutputModel) cmdCreateCrmClient.Execute(regClientCorporateInput);

                            if (crmContentOutput.code == CONST_CODE_SUCCESS)
                            {
                                regClientCorporateOutput.code = CONST_CODE_SUCCESS;
                                regClientCorporateOutput.message = "SUCCESS";
                                RegClientCorporateDataOutputModel_Pass dataOutPass =
                                    new RegClientCorporateDataOutputModel_Pass();
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
                }
                else
                {
                    regClientCorporateOutput.code = CONST_CODE_FAILED;
                    regClientCorporateOutput.message = "There is some conflicts among the arguments";
                    regClientCorporateOutput.description = "Look between roleCode and {assessorFlag ,solicitorFlag ,repairerFlag or hospitalFlag}";
                }

            }
            catch (FieldValidationException e)
            {
                Console.WriteLine("FieldValidationException");
                regFail.code = CONST_CODE_INVALID_INPUT;
                regFail.message = CONST_MESSAGE_INVALID_INPUT;
                regFail.description = e.StackTrace;
                regFail.transactionId = TransactionId;
                regFail.transactionDateTime = DateTime.Now.ToString();

                return regFail;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception");
                regClientCorporateOutput.code = CONST_CODE_FAILED;
                regClientCorporateOutput.message = e.Message;
                regClientCorporateOutput.description = e.StackTrace;

                RegClientCorporateDataOutputModel_Fail dataOutFail = new RegClientCorporateDataOutputModel_Fail();
                regClientCorporateOutput.data = new List<RegClientCorporateDataOutputModel>();
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