using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.SubmitSurveyAssessmentResult
{
    public class SubmitSurveyAssessmentResultInputModel
    {
        public int assessmentType { get; set; }
        public string assessmentrefcode { get; set; }
        public int assessmentClaimNotiScore { get; set; }
        public string assessmentClaimNotiComment { get; set; }
        public int assessmentSurveyScore { get; set; }
        public int assessmentSurveySpeedScore { get; set; }
        public int assessmentSurveyTimeUsageScore { get; set; }
        public string assessmentSurveyComment { get; set; }
        public string assessmentSurveyByUserid { get; set; }
        public int assessmentGarageServiceScore { get; set; }
        public int assessmentGarageCommitScore { get; set; }
        public int assessmentGarageRepairScore { get; set; }
        public string assessmentGarageComment { get; set; }
        public string assessmentGarageByUserid { get; set; }


    }
}
