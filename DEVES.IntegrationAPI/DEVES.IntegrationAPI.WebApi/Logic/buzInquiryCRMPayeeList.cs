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
            CRMInquiryPayeeContentOutputModel crmInqPayeeOut = new CRMInquiryPayeeContentOutputModel();
            try
            {

                InquiryCRMPayeeListInputModel inqCrmPayeeListIn = (InquiryCRMPayeeListInputModel)input;
                InquiryAPARPayeeListInputModel inqAPARIn = (InquiryAPARPayeeListInputModel)DataModelFactory.GetModel(typeof(InquiryAPARPayeeListInputModel));
                inqAPARIn = (InquiryAPARPayeeListInputModel)TransformerFactory.TransformModel(inqCrmPayeeListIn, inqAPARIn);

                InquiryAPARPayeeContentOutputModel inqAPAROut = CallDevesServiceProxy<InquiryAPARPayeeModel, InquiryAPARPayeeContentOutputModel>(CommonConstant.ewiEndpointKeyClaimRegistration, inqAPARIn);
                if (inqAPAROut != null)
                {
                    if (inqAPAROut.aparPayeeListCollection.Count > 0)
                    {

                    }
                }


                buzCrmInquiryClientMaster searchCleansing = new buzCrmInquiryClientMaster();
                BaseContentJsonProxyOutputModel contentSearchCleansing = (BaseContentJsonProxyOutputModel)searchCleansing.Execute(input);

                contentSearchCleansing.code = CONST_CODE_SUCCESS;

            }
            catch (Exception e)
            {

            }

            return contentSearchCleansing;
        }
    }
}