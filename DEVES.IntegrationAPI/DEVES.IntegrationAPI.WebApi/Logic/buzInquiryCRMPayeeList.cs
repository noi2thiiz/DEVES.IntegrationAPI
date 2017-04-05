using System;
using System.Collections.Generic;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.APAR;
using DEVES.IntegrationAPI.Model.SAP;
using DEVES.IntegrationAPI.Model.Polisy400;
using DEVES.IntegrationAPI.Model.MASTER;
using DEVES.IntegrationAPI.Model.InquiryCRMPayeeList;
using DEVES.IntegrationAPI.WebApi.Templates;
using System.Linq;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzInquiryCRMPayeeList : BaseCommand
    {
        public override BaseDataModel Execute(object input)
        {
            bool bFoundIn_APAR_or_Master = false;

            CRMInquiryPayeeContentOutputModel crmInqPayeeOut = new CRMInquiryPayeeContentOutputModel();
            crmInqPayeeOut.data = new List<InquiryCrmPayeeListDataModel>();
            
            try
            {
                Console.WriteLine(input.ToString());

                InquiryCRMPayeeListInputModel inqCrmPayeeListIn = (InquiryCRMPayeeListInputModel)input;

                #region IF inqCrmPayeeListIn.roleCode == "G" -> APAR.InquiryAPARPayeeList
                if (inqCrmPayeeListIn.roleCode.ToUpper() == ENUM_CLIENT_ROLE.G.ToString())
                {
                    InquiryAPARPayeeListInputModel inqAPARIn = (InquiryAPARPayeeListInputModel)DataModelFactory.GetModel(typeof(InquiryAPARPayeeListInputModel));
                    inqAPARIn = (InquiryAPARPayeeListInputModel)TransformerFactory.TransformModel(inqCrmPayeeListIn, inqAPARIn);

                    InquiryAPARPayeeContentModel inqAPAROut = CallDevesServiceProxy<InquiryAPARPayeeOutputModel, InquiryAPARPayeeContentModel>(CommonConstant.ewiEndpointKeyAPARInquiryPayeeList, inqAPARIn);

                    if (inqAPAROut != null && inqAPAROut.aparPayeeListCollection != null)
                    {
                        if (inqAPAROut.aparPayeeListCollection.Count > 0)
                        {

                            crmInqPayeeOut = (CRMInquiryPayeeContentOutputModel)TransformerFactory.TransformModel(inqAPAROut, crmInqPayeeOut);
                            bFoundIn_APAR_or_Master = true;
                        }
                    }



                }


                #endregion inqCrmPayeeListIn.roleCode == "G" -> APAR.InquiryAPARPayeeList

                #region IF inqCrmPayeeListIn.roleCode == {A,S,R,H} -> Master.InquiryMasterASRH
                else
                {
                    // Search in MASTER: MOTOR_InquiryMasterASRH()
                    InquiryMasterASRHDataInputModel inqASRHIn = (InquiryMasterASRHDataInputModel)DataModelFactory.GetModel(typeof(InquiryMasterASRHDataInputModel));
                    inqASRHIn = (InquiryMasterASRHDataInputModel)TransformerFactory.TransformModel(inqCrmPayeeListIn, inqASRHIn);
                    InquiryMasterASRHContentModel inqASRHOut = CallDevesServiceProxy<InquiryMasterASRHOutputModel, InquiryMasterASRHContentModel>(CommonConstant.ewiEndpointKeyMOTORInquiryMasterASRH, inqASRHIn);

                    if (inqASRHOut != null && inqASRHOut.ASRHListCollection != null)
                    {
                        if (inqASRHOut.ASRHListCollection.Count > 0)
                        {
                            crmInqPayeeOut = (CRMInquiryPayeeContentOutputModel)TransformerFactory.TransformModel(inqASRHOut, crmInqPayeeOut);
                            bFoundIn_APAR_or_Master = true;
                        }
                    }


                    Console.WriteLine(crmInqPayeeOut.ToJson());
                }
                #endregion inqCrmPayeeListIn.roleCode == {A,S,R,H} -> Master.InquiryMasterASRH

                #region Search in Cleansing(CLS) then search in Polisy400
                if (!bFoundIn_APAR_or_Master)
                {
                    bool bFound_Cleansing = false;

                    switch (inqCrmPayeeListIn.clientType.ToUpper())
                    {
                        case "P":
                            #region Search Client from Cleansing CLS_InquiryCLSPersonalClient
                            CLSInquiryPersonalClientInputModel clsPersonalInput = new CLSInquiryPersonalClientInputModel();
                            clsPersonalInput = (CLSInquiryPersonalClientInputModel)TransformerFactory.TransformModel(inqCrmPayeeListIn, clsPersonalInput);

                            //++ Call CLS_InquiryCLSPersonalClient through ServiceProxy
                            CLSInquiryPersonalClientContentOutputModel retCLSInqPersClient = CallDevesServiceProxy<EWIResCLSInquiryPersonalClient, CLSInquiryPersonalClientContentOutputModel>
                                                                                                    (CommonConstant.ewiEndpointKeyCLSInquiryPersonalClient, clsPersonalInput);

                            //++ If Found records in Cleansing(CLS) then pour the data from Cleansing to contentOutputModel
                            if ((retCLSInqPersClient.success | IsOutputSuccess(retCLSInqPersClient)) & (retCLSInqPersClient.data.Count == 1))
                            {
                                bFound_Cleansing = true;
                                //crmInqPayeeOut = (CRMInquiryPayeeContentOutputModel)TransformerFactory.TransformModel(retCLSInqPersClient, crmInqPayeeOut);
                                inqCrmPayeeListIn.polisyClientId = retCLSInqPersClient.data[0].clntnum;
                            }
                            #endregion Search Client from Cleansing
                            break;

                        case "C":
                            #region Call CLS_InquiryCLSCorporateClient through ServiceProxy
                            CLSInquiryCorporateClientInputModel clsCorpInput = new CLSInquiryCorporateClientInputModel();
                            clsCorpInput = (CLSInquiryCorporateClientInputModel)TransformerFactory.TransformModel(inqCrmPayeeListIn, clsCorpInput);


                            CLSInquiryCorporateClientContentOutputModel retCLSInqCorpClient = (CLSInquiryCorporateClientContentOutputModel)CallDevesJsonProxy<EWIResCLSInquiryCorporateClient>
                                                                                                    (CommonConstant.ewiEndpointKeyCLSInquiryCorporateClient, clsCorpInput);

                             //+ If Success then pour the data from Cleansing to contentOutputModel

                            if (retCLSInqCorpClient?.data != null)
                            {

                                if ((retCLSInqCorpClient.success | IsOutputSuccess(retCLSInqCorpClient)) & (retCLSInqCorpClient.data.Count ==1))
                                {
                                    bFound_Cleansing = true;
                                    //crmInqPayeeOut = (CRMInquiryPayeeContentOutputModel)TransformerFactory.TransformModel(retCLSInqCorpClient, crmInqPayeeOut);
                                    inqCrmPayeeListIn.polisyClientId = retCLSInqCorpClient.data[0].clntnum;
                                }
                            }

                            #endregion Call CLS_InquiryCLSCorporateClient through ServiceProxy
                            break;
                                                    
                        default:

                            break;
                    }

                    if (!bFound_Cleansing)
                    {
                        #region Call COMP_Inquiry through ServiceProxy
                        COMPInquiryClientMasterInputModel compInqClientInput = new COMPInquiryClientMasterInputModel();
                        compInqClientInput = (COMPInquiryClientMasterInputModel)TransformerFactory.TransformModel(inqCrmPayeeListIn, compInqClientInput);

                        //+ Call CLS_InquiryCLSPersonalClient through ServiceProxy
                        var retCOMPInqClient = CallDevesServiceProxy<COMPInquiryClientMasterOutputModel, EWIResCOMPInquiryClientMasterContentModel>
                                                                                                (CommonConstant.ewiEndpointKeyCOMPInquiryClient, compInqClientInput);

                        //Found in Polisy400
                        if (retCOMPInqClient?.clientListCollection != null)
                        {
                            crmInqPayeeOut = (CRMInquiryPayeeContentOutputModel)TransformerFactory.TransformModel(retCOMPInqClient, crmInqPayeeOut);
                        }


                        #endregion Call COMP_Inquiry through ServiceProxy
                    }
                }
                #endregion Search in Cleansing(CLS) then search in Polisy400

                #region Search In SAP: SAP_InquiryVendor()
                SAPInquiryVendorInputModel inqSAPVendorIn = (SAPInquiryVendorInputModel)DataModelFactory.GetModel(typeof(SAPInquiryVendorInputModel));
                inqSAPVendorIn = (SAPInquiryVendorInputModel)TransformerFactory.TransformModel(inqCrmPayeeListIn, inqSAPVendorIn);

                //InquiryAPARPayeeContentOutputModel inqAPAROut = CallDevesServiceProxy<InquiryAPARPayeeModel, InquiryAPARPayeeContentOutputModel>(CommonConstant.ewiEndpointKeyClaimRegistration, inqAPARIn);
                EWIResSAPInquiryVendorContentModel inqSAPVendorContentOut = CallDevesServiceProxy<SAPInquiryVendorOutputModel , EWIResSAPInquiryVendorContentModel>(CommonConstant.ewiEndpointKeySAPInquiryVendor, inqSAPVendorIn);




                if (inqSAPVendorContentOut != null)
                {
                    crmInqPayeeOut = (CRMInquiryPayeeContentOutputModel) TransformerFactory.TransformModel(inqSAPVendorContentOut, crmInqPayeeOut);
                }


                #endregion Search In SAP: SAP_InquiryVendor()

                crmInqPayeeOut.code = CONST_CODE_SUCCESS;
                
                crmInqPayeeOut.message = "SUCCESS";
            }
            catch (Exception e)
            {
                crmInqPayeeOut.code = CONST_CODE_FAILED;
                crmInqPayeeOut.message = e.Message;
                crmInqPayeeOut.description = e.StackTrace;
            }
            crmInqPayeeOut.transactionId = TransactionId;
            crmInqPayeeOut.transactionDateTime = DateTime.Now;

            return crmInqPayeeOut;
        }

      
     
    }
}