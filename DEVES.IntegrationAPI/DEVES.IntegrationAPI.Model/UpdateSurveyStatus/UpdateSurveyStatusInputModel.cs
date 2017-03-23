using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DEVES.IntegrationAPI.Model.UpdateSurveyStatus
{
    public class UpdateSurveyStatusInputModel
    {
        public string ticketNo { get; set; } // string 
        public string claimNotiNo { get; set; } // string 
        public string iSurveyStatus { get; set; } // enum ??
        public string iSurveyStatusOn { get; set; } // datetime yyyy-MM-dd HH:mm:ss format
    }
}
