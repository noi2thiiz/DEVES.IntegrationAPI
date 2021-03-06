﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DEVES.IntegrationAPI.Model.EWI;

namespace DEVES.IntegrationAPI.Model.CLS
{
    public class EWIResCLSInquiryCorporateClient : BaseEWIResponseModel
    {
        [JsonProperty(Order = 10)]
        public CLSInquiryCorporateClientContentOutputModel content { set; get; }
    }

    public class CLSInquiryCorporateClientContentOutputModel : BaseContentJsonProxyOutputModel
    {
        [JsonProperty(Order = 16)]
        public bool success { set; get; }

        [JsonProperty(Order = 17)]
        public List<CLSInquiryCorporateClientOutputModel> data { set; get; } = new List<CLSInquiryCorporateClientOutputModel>();
    }
    public class CLSInquiryCorporateClientOutputModel : BaseDataModel
    {
        //Extra Field
        public String sourceData { set; get; } = "CLS";
        public String clientType { set; get; } = "C";

        //cls field
        public String cleansing_id { set; get; } = "";
        public String clntnum { set; get; } = "";
        public String secuityno { set; get; } = "";
        public String cls_tax_no_new { set; get; } = "";
        public String lgivname { set; get; } = "";
        public String lsurname { set; get; } = "";
        public String cls_full_name { set; get; } = "";
        public String salutl { set; get; } = "";
        public String cltsex { set; get; } = "";
        public String cls_sex { set; get; } = "";
        public String cltphone01 { set; get; } = "";
        public String cltphone02 { set; get; } = "";
        public String cls_fax { set; get; } = "";
        public String cls_display_phone { set; get; } = "";
        public String email_1 { set; get; } = "";
        public String email_2 { set; get; } = "";
        public String email_3 { set; get; } = "";
        public String vip { set; get; } = "";
        public String cls_vip { set; get; } = "";
        public String cls_tier { set; get; } = "";
        public String cls_occpcode { set; get; } = "";
        public DateTime? cls_start_date { set; get; } 
        public String crm_ref_code1 { set; get; } = "";
        public String corporate_staff_no { get; set; } = "";

        public String cltstat { get; set; } = "";
        public String cls_stat_desc { get; set; } = "";

        public String lattitude { get; set; } = "";
        public String longitude { get; set; } = "";
        
            

        public List<CLSAddressListsCollectionModel> addressListsCollection { set; get; } = new List<CLSAddressListsCollectionModel>();
    }


}

 
