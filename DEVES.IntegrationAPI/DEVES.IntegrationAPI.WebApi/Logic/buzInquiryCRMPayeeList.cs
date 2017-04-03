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

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzInquiryCRMPayeeList : BaseCommand
    {
        public override BaseDataModel Execute(object input)
        {
            bool bFoundIn_APAR_or_Master = false;

            CRMInquiryPayeeContentOutputModel crmInqPayeeOut = new CRMInquiryPayeeContentOutputModel();
            crmInqPayeeOut.data = new List<InquiryCrmPayeeListDataModel>();
            crmInqPayeeOut.transactionId = TransactionId;
            crmInqPayeeOut.transactionDateTime = DateTime.Now;
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
                }
                #endregion inqCrmPayeeListIn.roleCode == {A,S,R,H} -> Master.InquiryMasterASRH

                #region Search in Cleansing(CLS) then search in Polisy400
                if (!bFoundIn_APAR_or_Master)
                {
                    foreach(InquiryCrmPayeeListDataModel data in crmInqPayeeOut.data )
                    {

                        /*

                        List<string> crmData = SearchCrmContactClientId(data.cleansingId);
                        if (crmData != null && crmData.Count == 1)
                        {
                            data.generalHeader.crmClientId = crmData.First();
                        }
                        else
                        {
                            #region Call COMP_Inquiry through ServiceProxy
                            COMPInquiryClientMasterInputModel compInqClientInput = new COMPInquiryClientMasterInputModel();
                            compInqClientInput = (COMPInquiryClientMasterInputModel)TransformerFactory.TransformModel(contentModel, compInqClientInput);

                            //+ Call CLS_InquiryCLSPersonalClient through ServiceProxy
                            EWIResCOMPInquiryClientMasterContentModel retCOMPInqClient = CallDevesServiceProxy<COMPInquiryClientMasterOutputModel, EWIResCOMPInquiryClientMasterContentModel>
                                                                                                    (CommonConstant.ewiEndpointKeyCOMPInquiryClient, compInqClientInput);

                            //Found in Polisy400
                            if (retCOMPInqClient.clientListCollection != null)
                            {
                                crmInqContent = (CRMInquiryClientContentOutputModel)TransformerFactory.TransformModel(retCOMPInqClient, crmInqContent);
                            }


                            #endregion Call COMP_Inquiry through ServiceProxy
                        }


                        */
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
           
            return crmInqPayeeOut;
        }

      
     
    }
}