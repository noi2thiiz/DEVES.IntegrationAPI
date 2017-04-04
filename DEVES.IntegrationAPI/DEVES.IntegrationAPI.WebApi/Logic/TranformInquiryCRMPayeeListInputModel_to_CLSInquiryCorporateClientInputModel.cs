using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.InquiryCRMPayeeList;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TranformInquiryCRMPayeeListInputModel_to_CLSInquiryCorporateClientInputModel : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            InquiryCRMPayeeListInputModel src = (InquiryCRMPayeeListInputModel)input;
            CLSInquiryCorporateClientInputModel trgt = (CLSInquiryCorporateClientInputModel)output;

            trgt.clientId = ""+src.polisyClientId;
            trgt.roleCode = ""+src.roleCode;

            trgt.corporateFullName = ""+src.fullname;
            trgt.taxNo = ""+src.taxNo;
            trgt.backDay = "30";
             trgt.telephone = "";
            trgt.emailAddress= "";

            return trgt;
        
        }

    }
}