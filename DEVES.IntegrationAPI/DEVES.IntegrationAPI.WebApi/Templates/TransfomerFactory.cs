using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using Model= DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.WebApi.Logic;
//using DEVES.IntegrationAPI.Model.InquiryClientMaster;
using DEVES.IntegrationAPI.Model;


namespace DEVES.IntegrationAPI.WebApi.Templates
{
    public static class TransformerFactory
    {
        public static BaseDataModel TransformModel(BaseDataModel source ,BaseDataModel target)
        {
            BaseTransformer transformer = GetTransformer(source.GetType(), target.GetType());
            return transformer.TransformModel(source, target);
        }

        public static BaseTransformer GetTransformer( Type inputType , Type outputType )
        {
            BaseTransformer t = new NullTransformer();
            if (inputType == typeof(Model.InquiryClientMaster.InquiryClientMasterInputModel))
            {
                if (outputType == typeof(Model.CLS.CLSInquiryPersonalClientInputModel))
                {
                    t = new TransformCRMInquiryCRMClientMasterInput_to_CLSInquiryCLSPersonalClientInput();
                }
                else if (outputType == typeof(Model.CLS.CLSInquiryCorporateClientInputModel))
                {
                    t = new TransformCRMInquiryCRMClientMasterInput_to_CLSInquiryCorporateClientInputModel();
                }
                else if (outputType == typeof(Model.Polisy400.COMPInquiryClientMasterInputModel))
                {
                    t = new TransformCRMInquiryCRMClientMasterInput_to_COMPInquiryClientMasterInput();
                }

            }

            else if(outputType == typeof(Model.InquiryClientMaster.CRMInquiryClientContentOutputModel))
            {
                if (inputType == typeof(Model.CLS.CLSInquiryPersonalClientContentOutputModel) )
                {
                    t = new TransformCLSInquiryPersonalClientContentOut_to_CrmInquiryClientMasterContentOut();
                }
                else if (inputType == typeof(Model.Polisy400.EWIResCOMPInquiryClientMasterContentModel) )
                {
                    t = new TransformCOMPInquiryClientMasterContentOutputModel_to_CrmInquiryClientMasterContentOut();
                }
                else if (inputType == typeof(Model.CLS.CLSInquiryCorporateClientContentOutputModel) )
                {
                    t = new TransformCLSInquiryCorporateClientContentOut_to_CrmInquiryClientMasterContentOut();
                }                                                

            }

            return t;
        }
    }
}