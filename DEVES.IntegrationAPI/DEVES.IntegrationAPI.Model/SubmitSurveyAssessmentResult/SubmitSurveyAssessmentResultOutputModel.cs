using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.SubmitSurveyAssessmentResult
{
    public class SubmitSurveyAssessmentResultOutputModel_Pass
    {
        public string code { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string transactionId { get; set; }
        public string transactionDateTime { get; set; }
        public SubmitSurveyAssessmentResultDataModel_Pass data { get; set; }
    }

    public class SubmitSurveyAssessmentResultDataModel_Pass
    {
        public string message { get; set; }
    }


    public class SubmitSurveyAssessmentResultOutputModel_Fail
    {
        public string code { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string transactionId { get; set; }
        public string transactionDateTime { get; set; }
        public SubmitSurveyAssessmentResultDataModel_Fail data { get; set; }
    }

    public class SubmitSurveyAssessmentResultDataModel_Fail
    {
        public List<SubmitSurveyAssessmentResultFieldErrorsModel> fieldErrors { get; set; }
    }

    public class SubmitSurveyAssessmentResultFieldErrorsModel
    {
        public string name { get; set; }
        public string message { get; set; }

        public SubmitSurveyAssessmentResultFieldErrorsModel(string n, string m)
        {
            name = n;
            message = m;
        }
    }

}
