using System;
using System.Collections.Generic;
using DEVES.IntegrationAPI.Model.APAR;
using DEVES.IntegrationAPI.Model.InquiryClientMaster;
using DEVES.IntegrationAPI.Model.MASTER;
using DEVES.IntegrationAPI.Model.Polisy400;
using DEVES.IntegrationAPI.Model.SAP;

namespace DEVES.IntegrationAPI.Model
{
    public enum ENUM_CLIENT_ROLE
    {
        G,
        A,
        S,
        R,
        H
    }


    public static class DataModelFactory
    {

        public static BaseDataModel GetModel(Type t)
        {
            BaseDataModel o = new NullDataModel();
            #region Model.InquiryClientMaster.CRMInquiryClientContentOutputModel
            if (t == typeof(Model.InquiryClientMaster.CRMInquiryClientContentOutputModel))
            {
                CRMInquiryClientContentOutputModel crmInqClient = new CRMInquiryClientContentOutputModel();
                crmInqClient.data = new List<CRMInquiryClientOutputDataModel>();
                //CRMInquiryClientOutputDataModel data = new CRMInquiryClientOutputDataModel();
                //data.addressInfo = new CRMInquiryClientAddressInfoModel();
                //data.asrhHeader = new CRMInquiryClientAsrhHeaderModel();
                //data.contactInfo = new CRMInquiryClientContactInfoModel();
                //data.generalHeader = new CRMInquiryClientGeneralHeaderModel();
                //data.profileInfo = new CRMInquiryClientProfileInfoModel();

                //crmInqClient.data.Add(data);
                o = crmInqClient;
            }
            #endregion Model.InquiryClientMaster.CRMInquiryClientContentOutputModel
            #region InquiryAPARPayeeListInputModel
            else if (t == typeof(InquiryAPARPayeeListInputModel))
            {
                InquiryAPARPayeeListInputModel model = new InquiryAPARPayeeListInputModel();


                o = model;
            }
            #endregion InquiryAPARPayeeListInputModel
            #region Model.SAP.SAPInquiryVendorInputModel

            else if (t == typeof(Model.SAP.SAPInquiryVendorInputModel))
            {
                SAPInquiryVendorInputModel input = new SAPInquiryVendorInputModel();

                //crmInqClient.data.Add(data);
                o = input;
            }
            #endregion Model.SAP.SAPInquiryVendorInputModel
            #region Model.MASTER.InquiryMasterASRHDataInputModel
            else if (t == typeof(Model.MASTER.InquiryMasterASRHDataInputModel))
            {
                InquiryMasterASRHDataInputModel input = new InquiryMasterASRHDataInputModel();

                //crmInqClient.data.Add(data);
                o = input;
            }
            #endregion Model.MASTER.InquiryMasterASRHDataInputModel
            #region Model.MASTER.InquiryMasterASRHContentModel
            else if (t == typeof(Model.MASTER.InquiryMasterASRHContentModel))
            {

                var modelContent = new InquiryAPARPayeeContentModel();
                modelContent.aparPayeeListCollection = new List<InquiryAPARPayeeContentAparPayeeListCollectionDataModel>();
                var colData = new InquiryAPARPayeeContentAparPayeeListCollectionDataModel();
                colData.aparPayeeList = new InquiryAPARPayeeListModel();

                o = modelContent;
            }
            #endregion Model.MASTER.InquiryMasterASRHContentModel
            #region EWIResCOMPInquiryClientMasterContentModel
            else if (t == typeof(EWIResCOMPInquiryClientMasterContentModel))
            {

                var modelContent = new EWIResCOMPInquiryClientMasterContentModel();
                modelContent.clientListCollection = new List<COMPInquiryClientMasterContentClientListModel>();


                o = modelContent;
            }
            #endregion EWIResCOMPInquiryClientMasterContentModel
            else
            {
                throw new NotImplementedException("GetModel(T) for type<T>:" + t.Name);
            }


            return o;
        }
    }

    public class NullDataModel: BaseDataModel
    { }
}
