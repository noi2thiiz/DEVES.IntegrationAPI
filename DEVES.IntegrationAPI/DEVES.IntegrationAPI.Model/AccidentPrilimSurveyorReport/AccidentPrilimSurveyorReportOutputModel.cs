using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DEVES.IntegrationAPI.Model.AccidentPrilimSurveyorReport
{
    class AccidentPrilimSurveyorReportOutputModel
    {

    }
    public class AccidentPrilimSurveyorReportOutputModel_Pass
    {
        public string code { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string transactionId { get; set; }
        public string transactionDateTime { get; set; }
        public AccidentPrilimSurveyorReportDataOutputModel_Pass data { get; set; }
    }

    public class AccidentPrilimSurveyorReportDataOutputModel_Pass
    {
        public string message { get; set; }
    }

    public class AccidentPrilimSurveyorReportOutputModel_Fail
    {
        public string code { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string transactionId { get; set; }
        public string transactionDateTime { get; set; }
        public AccidentPrilimSurveyorReportDataOutputModel_Fail data { get; set; }
    }

    public class AccidentPrilimSurveyorReportDataOutputModel_Fail
    {
        public List<AccidentPrilimSurveyorReportFieldErrorsModel> fieldErrors { get; set; }
    }

    public class AccidentPrilimSurveyorReportFieldErrorsModel
    {
        public string name { get; set; }
        public string message { get; set; }

        public AccidentPrilimSurveyorReportFieldErrorsModel(string n, string m)
        {
            name = n;
            message = m;
        }
    }
}
