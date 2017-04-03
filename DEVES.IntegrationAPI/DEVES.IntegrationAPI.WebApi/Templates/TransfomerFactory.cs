using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using Model= DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.WebApi.Logic;
//using DEVES.IntegrationAPI.Model.InquiryClientMaster;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.InquiryCRMPayeeList;

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

            #region API:InquiryClientMaster
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
            else if (outputType == typeof(Model.InquiryClientMaster.CRMInquiryClientContentOutputModel))
            {
                if (inputType == typeof(Model.CLS.CLSInquiryPersonalClientContentOutputModel))
                {
                    t = new TransformCLSInquiryPersonalClientContentOut_to_CrmInquiryClientMasterContentOut();
                }
                else if (inputType == typeof(Model.Polisy400.EWIResCOMPInquiryClientMasterContentModel))
                {
                    t = new TransformCOMPInquiryClientMasterContentOutputModel_to_CrmInquiryClientMasterContentOut();
                }
                else if (inputType == typeof(Model.CLS.CLSInquiryCorporateClientContentOutputModel))
                {
                    t = new TransformCLSInquiryCorporateClientContentOut_to_CrmInquiryClientMasterContentOut();
                }

            }
            #endregion API:InquiryClientMaster

            #region API:InquiryCRMPayeeListInputModel
            else if (inputType == typeof(Model.InquiryCRMPayeeList.InquiryCRMPayeeListInputModel))
            {
                if (outputType == typeof(Model.SAP.SAPInquiryVendorInputModel))
                {
                    t = new TransformInquiryCRMPayeeListInputModel_to_SAPInquiryVendorInputModel();
                }
                else if (outputType == typeof(Model.MASTER.InquiryMasterASRHDataInputModel))
                {
                    t = new TransformInquiryCRMPayeeListInputModel_to_InquiryMasterASRHDataInputModel ();
                }
                else if (outputType == typeof(Model.APAR.InquiryAPARPayeeListInputModel))
                {
                    t = new TransformInquiryCRMPayeeListInputModel_to_InquiryAPARPayeeListInputModel ();
                }
            }
            else if (outputType == typeof(CRMInquiryPayeeContentOutputModel))
            {
                if (inputType == typeof(Model.APAR.InquiryAPARPayeeContentModel))
                {
                    t = new TransformAPARInquiryAPARPayeeListContentOutputModel_to_InquiryCRMPayeeListDataOutputModel();
                }
                if (inputType == typeof(Model.MASTER.InquiryMasterASRHContentModel))
                {
                    t = new TransformInquiryMasterASRHContentOutputModel_to_InquiryCRMPayeeListDataOutputModel();
                }
                if (inputType == typeof(Model.SAP.EWIResSAPInquiryVendorContentModel))
                {
                    t = new TransformSAPInquiryVendorOutputModel_to_InquiryCRMPayeeListDataOutputModel();
                }
            }

            #endregion API:InquiryCRMPayeeListInputModel



            else if (inputType == typeof(Model.Polisy400.CLIENTCreateCorporateClientAndAdditionalInfoContentModel))
            {
                if (outputType == typeof(Model.RegClientCorporate.RegClientCorporateInputModel))
                {
                    t = new TranformCLIENTCreateCorporateClientAndAdditionalInfoContentModel_to_RegClientCorporateInputModel();
                }
                else if (outputType == typeof(Model.RegClientPersonal.RegClientPersonalInputModel))
                {
                    t = new TranformCLIENTCreatePersonalClientAndAdditionalInfoOutputModel_to_RegClientPersonalInputModel();
                }
            }

            else if (inputType == typeof(Model.CLS.CLSCreateCorporateClientContentOutputModel) && outputType == typeof(Model.RegClientCorporate.RegClientCorporateInputModel))
            {
                t = new TranformCLSCreateCorporateClientContentOutputModel_to_RegClientCorporateInputModel();
            }
            else if (inputType == typeof(Model.CLS.CLSCreatePersonalClientContentOutputModel) && outputType == typeof(Model.RegClientPersonal.RegClientPersonalInputModel))
            {
                t = new TranformCLSCreatePersonalClientOutputModel_to_RegClientPersonalInputModel();
            }
            else if (inputType == typeof(Model.RegClientPersonal.CRMRegClientPersonalOutputModel) && outputType == typeof(Model.RegClientPersonal.CRMRegClientPersonalOutputModel))
            {
                t = new TranformCRMRegClientPersonalOutputModel_to_CRMRegClientPersonalOutputModel();
            }
            else if (inputType == typeof(Model.Polisy400.EWIResCOMPInquiryClientMasterContentModel) && outputType == typeof(Model.Polisy400.CLIENTUpdateCorporateClientAndAdditionalInfoInputModel))
            {
                t = new TranformEWIResCOMPInquiryClientMasterContentModel_to_CLIENTUpdateCorporateClientAndAdditionalInfoInputModel();
            }
            else if (inputType == typeof(Model.RegClientPersonal.RegClientPersonalInputModel))
            {
                if (outputType == typeof(Model.Polisy400.CLIENTCreatePersonalClientAndAdditionalInfoInputModel))
                {
                    t = new TranformRegClientPersonalInputModel_to_CLIENTCreatePersonalClientAndAdditionalInfoInputModel();
                }
                if (outputType == typeof(Model.CLS.CLSCreatePersonalClientInputModel))
                {
                    t = new TranformRegClientPersonalInputModel_to_CLSCreatePersonalClientInputModel();
                }
            }
            else if (inputType == typeof(Model.RegClientCorporate.RegClientCorporateInputModel))
            {
                if (outputType == typeof(Model.Polisy400.CLIENTCreateCorporateClientAndAdditionalInfoContentModel))
                {
                    t = new TranformRegClientCorporateInputModel_to_CLIENTUpdateCorporateClientAndAdditionalInfoInputModel();
                }
                else if (outputType == typeof(Model.Polisy400.CLIENTCreateCorporateClientAndAdditionalInfoInputModel))
                {
                    t = new TranformRegClientCorporateInputModel_to_CLIENTCreateCorporateClientAndAdditionalInfoInputModel();
                }
                else if (outputType == typeof(Model.CLS.CLSCreateCorporateClientInputModel))
                {
                    t = new TranformRegClientCorporateInputModel_to_CLSCreateCorporateClientInputModel();
                }
                else if (outputType == typeof(Model.Polisy400.COMPInquiryClientMasterInputModel))
                {
                    t = new TranformRegClientCorporateInputModel_to_COMPInquiryClientMasterInputModel();
                }
            }
            
            return t;
        }
    }
}