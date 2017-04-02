using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model = DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.InquiryClientMaster;
using CLS = DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.APAR;

namespace DEVES.IntegrationAPI.Model
{
    public static class DataModelFactory
    {
        public static BaseDataModel GetModel(Type t)
        {
            BaseDataModel o = new NullDataModel();
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
