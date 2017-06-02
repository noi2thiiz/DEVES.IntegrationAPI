using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.SubmitSurveyAssessmentResult
{
    public class SubmitSurveyAssessmentResultInputModel
    {
        public string assessmentrefcode { get; set; }
        public int assessmentClaimNotiScore { get; set; }
        public string assessmentClaimNotiComment { get; set; }
        public int assessmentSurveyScore { get; set; }
        public int assessmentSurveySpeedScore { get; set; }
        public string assessmentSurveyComment { get; set; }
        public string assessmentSurveyByUserid { get; set; }


    }
}
