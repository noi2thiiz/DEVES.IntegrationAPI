using System.Collections.Generic;
using System.Linq;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.SAP;
using DEVES.IntegrationAPI.Model.InquiryClientMaster;
using DEVES.IntegrationAPI.Model.InquiryCRMPayeeList;
using CLS=DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TransformSAPInquiryVendorOutputModel_to_InquiryCRMPayeeListDataOutputModel: BaseTransformer
    {
        public override BaseDataModel TransformModel(Model.BaseDataModel input, BaseDataModel output)
        {
            EWIResSAPInquiryVendorContentModel srcContent = (EWIResSAPInquiryVendorContentModel)input;
            CRMInquiryPayeeContentOutputModel trgtContent = (CRMInquiryPayeeContentOutputModel)output;

             //@todo Process Data

            return trgtContent;
        }
    }
}