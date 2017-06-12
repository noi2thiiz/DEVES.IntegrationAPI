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
        public Guid assessmentquestionnireid { get; set; }
        public string assessmentrefcode { get; set; }
        public int assessmentScore1 { get; set; }
        public int assessmentScore2 { get; set; }
        public int assessmentScore3 { get; set; }
        public int assessmentScore4 { get; set; }
        public int assessmentScore5 { get; set; }
        public int assessmentScore6 { get; set; }
        public int assessmentScore7 { get; set; }
        public int assessmentScore8 { get; set; }
        public int assessmentScore9 { get; set; }
        public int assessmentScore10 { get; set; }
        public string assessmentComment { get; set; }
        
    }
}
