using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEVES.IntegrationAPI.WebApi.Logic.DataBaseContracts
{
    public class SpQueryGarageAssessmentFromLocus : BaseDataBaseContracts<GarageAssessmentFromLocusEntity>
    {
        private static SpQueryGarageAssessmentFromLocus _instance;
       

        public static SpQueryGarageAssessmentFromLocus Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = new SpQueryGarageAssessmentFromLocus("sp_Query_GarageAssessmentFromLocus", "Rest");
                return _instance;
            }
        }

        public SpQueryGarageAssessmentFromLocus(string storeName) : base(storeName)
        {

        }

        public SpQueryGarageAssessmentFromLocus(string storeName, string readerName) : this(storeName)
        {
            this.readerName = readerName;
        }
    }

    public class GarageAssessmentFromLocusEntity
    {
        public string Id { get; set; }
        public string PolicyNumber { get; set; }
        public string DriverGuid { get; set; }
        public string DriverFullname { get; set; }
        public string DriverMobile { get; set; }
        public string CurrentRegNum { get; set; }
        public string CurrentRegNumProv { get; set; }
        public string CustomerClientNumber { get; set; }
        public int DateDiffFromCurrentDate { get; set; }
        public string AssessmentRefCode { get; set; }

        public string TicketNumber { get; set; }
        public string ClaimNotiNumber { get; set; }
        public string QuestionGuid { get; set; }
        public int QuestionType { get; set; }

        public string AssesseeCode { get; set; }
        public string AssesseeName { get; set; }
        public string AssessmentOwnerGuid { get; set; }
        public int SurveySmsOption { get; set; }
        


    }
}