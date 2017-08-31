using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DEVES.IntegrationAPI.Model.EWI;

namespace DEVES.IntegrationAPI.Model.CLS
{
    public class EWIResCLSInquiryPersonalClient : BaseEWIResponseModel
    {
        [JsonProperty(Order = 10)]
        public CLSInquiryPersonalClientContentOutputModel content { set; get; }
    }

    public class CLSInquiryPersonalClientContentOutputModel : BaseContentJsonProxyOutputModel
    {
        [JsonProperty(Order = 20)]
        public bool success { set; get; }

        [JsonProperty(Order = 21)]
        public List<CLSInquiryPersonalClientOutputModel> data { set; get; }
    }
    public class CLSInquiryPersonalClientOutputModel : BaseDataModel
    {
        //Extra Field
        public String sourceData { set; get; } = "CLS";
        public String clientType { set; get; } = "P";

        //cls field

        public String cleansing_id { set; get; }
        public String clntnum { set; get; }
        public String secuityno { set; get; }
        public String cls_citizen_id_new { set; get; }
        public String lgivname { set; get; }
        public String lsurname { set; get; }
        public String cls_full_name { set; get; }
        public String salutl { set; get; }
        public String cltsex { set; get; }
        public String cls_sex { set; get; }
        public String cltphone01 { set; get; }
        public String cltphone02 { set; get; }
        public String cls_fax { set; get; }
        public String cls_display_phone { set; get; }
        public String email_1 { set; get; }
        public String email_2 { set; get; }
        public String email_3 { set; get; }
        public String vip { set; get; }
        public String cls_vip { set; get; }
        public String cls_tier { set; get; }
        public String cls_occpcode { set; get; }
        public DateTime? cls_start_date { set; get; }
        public String crm_ref_code1 { set; get; }

        public String corporate_staff_no { set; get; }

        public String cltstat { get; set; } = "";
        public String cls_stat_desc { get; set; } = "";


        public List<CLSAddressListsCollectionModel> addressListsCollection { set; get; }
    }
    public class CLSAddressListsCollectionModel : BaseDataModel
    {
        public String ref_cleansing_id { set; get; }
        public String address_id { set; get; }
        public String address_type_code { set; get; }
        public int sequence_id { set; get; }
        public String address_1 { set; get; }
        public String address_2 { set; get; }
        public String address_3 { set; get; }
        public String ref_province_code { set; get; }
        public String province_text { set; get; }
        public String province_display { set; get; }
        public String ref_district_code { set; get; }
        public String district_text { set; get; }
        public String district_display { set; get; }
        public String ref_sub_district_code { set; get; }
        public String sub_district_text { set; get; }
        public String sub_district_display { set; get; }
        public String postal_code { set; get; }
        public String lattitude { set; get; }
        public String longitude { set; get; }
        
        
        public String full_original_address { set; get; }
        public String cltaddr01 { set; get; }
        public String cltaddr02 { set; get; }
        public String cltaddr03 { set; get; }
        public String cltaddr04 { set; get; }
        public String cltaddr05 { set; get; }
        public String cltpcode { set; get; }
        public String ctrycode { set; get; }
        public String cls_ctrycode_text { set; get; }
    }


}
