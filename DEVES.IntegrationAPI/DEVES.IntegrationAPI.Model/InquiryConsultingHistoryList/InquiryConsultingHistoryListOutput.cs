using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DEVES.IntegrationAPI.Model.InquiryConsultingHistoryList
{
    public class InquiryConsultingHistoryListOutput
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
