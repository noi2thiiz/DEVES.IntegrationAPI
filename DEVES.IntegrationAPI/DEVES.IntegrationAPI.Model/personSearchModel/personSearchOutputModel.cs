using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.personSearchModel
{
    public class personSearchOutputModel : BaseDataModel
    {
        public string code { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string transactionId { get; set; }
        public DateTime transactionDateTime { get; set; }
        public List<personSearchDataOutput> data { get; set; }
    }

    public class personSearchDataOutput : BaseDataModel
    {
        public string fullName { get; set; }
        public string clsId { get; set; }
        public string phoneNum { get; set; }
        public string citizenId { get; set; }
    }
}
