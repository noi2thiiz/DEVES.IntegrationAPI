﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DEVES.IntegrationAPI.Model.InquiryConsultingHistory
{

    public class InquiryConsultingHistoryOutputModel
    {
        public int code { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string transactionId { get; set; }
        public DateTime? transactionDateTime { get; set; }
        public InquiryConsultingHistoryDataOutputModel data { get; set; }
    }

    public class InquiryConsultingHistoryDataOutputModel
    {
        public string ticketNo { get; set; }
        public string claimNotiNo { get; set; }
        public string claimNo { get; set; }
        public string crmChannel { get; set; }
        public string crmNote { get; set; }
        public DateTime? crmCreatedOn { get; set; }
        public string crmCreatedBy { get; set; }
    }
}

