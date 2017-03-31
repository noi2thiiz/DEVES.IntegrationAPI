using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;

using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.EWI;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.APAR;
using DEVES.IntegrationAPI.Model.InquiryCRMPayeeList;
using DEVES.IntegrationAPI.Model.InquiryClientMaster;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzInquiryCRMPayeeList : BaseCommand
    {
        public override BaseDataModel Execute(object input)
        {
            bool bFoundIn_APAR_or_Master = false;

            CRMInquiryPayeeContentOutputModel crmInqPayeeOut = new CRMInquiryPayeeContentOutputModel();
            crmInqPayeeOut.transactionDateTime = DateTime.Now;
            crmInqPayeeOut.transactionId = Guid.NewGuid().ToString();
            try
            {

                InquiryCRMPayeeListInputModel inqCrmPayeeListIn = (InquiryCRMPayeeListInputModel)input;

                if (inqCrmPayeeListIn.roleCode.ToUpper() == ENUM_CLIENT_ROLE.G.ToString())
                {
                    InquiryAPARPayeeListInputModel inqAPARIn = (InquiryAPARPayeeListInputModel)DataModelFactory.GetModel(typeof(InquiryAPARPayeeListInputModel));
                    inqAPARIn = (InquiryAPARPayeeListInputModel)TransformerFactory.TransformModel(inqCrmPayeeListIn, inqAPARIn);

                    //InquiryAPARPayeeContentOutputModel inqAPAROut = CallDevesServiceProxy<InquiryAPARPayeeModel, InquiryAPARPayeeContentOutputModel>(CommonConstant.ewiEndpointKeyClaimRegistration, inqAPARIn);
                    InquiryAPARPayeeContentModel inqAPAROut = CallDevesServiceProxy<InquiryAPARPayeeOutputModel, InquiryAPARPayeeContentModel>(CommonConstant.ewiEndpointKeyClaimRegistration, inqAPARIn);
                    if (inqAPAROut != null)
                    {
                        if (inqAPAROut.aparPayeeListCollection.Count > 0)
                        {
                            crmInqPayeeOut = (CRMInquiryPayeeContentOutputModel)TransformerFactory.TransformModel(inqAPAROut, crmInqPayeeOut);
                            bFoundIn_APAR_or_Master = true;
                        }
                    }
                }
                else
                {
                    // Search in MASTER: MOTOR_InquiryMasterASRH()
                }

                if (!bFoundIn_APAR_or_Master)
                {
                    // Search in [CLS: CLS_InquiryPersonalClient or CLS_InquiryCorporateClient ] & Polisy400: COMP_InquiryClientMaster
                    buzCrmInquiryClientMaster searchCleansing = new buzCrmInquiryClientMaster();
                    BaseContentJsonProxyOutputModel contentSearchCleansing = (BaseContentJsonProxyOutputModel)searchCleansing.Execute(input);
                }

                //Search In SAP: SAP_InquiryVendor()

            }
            catch (Exception e)
            {

            }

            return crmInqPayeeOut;
        }
    }
}