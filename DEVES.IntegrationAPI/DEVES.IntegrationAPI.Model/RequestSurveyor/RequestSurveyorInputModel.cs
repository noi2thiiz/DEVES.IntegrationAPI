using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.RequestSurveyor
{
    public class RequestSurveyorInputModel
    {
        public string EventCode { get; set; }
        public string claimnotirefer { get; set; }
        public string InsureID { get; set; }
        public string RSKNo { get; set; }
        public string TranNo { get; set; }
        public string NotifyName { get; set; }
        public string Mobile { get; set; }
        public string Driver { get; set; }
        public string DriverTel { get; set; }
        public string current_VehicleLicence { get; set; }
        public string current_Province { get; set; }
        public string EventDate { get; set; }
        public string ActivityDate { get; set; }
        public string EventDetail { get; set; }
        public string isCasualty { get; set; }
        public string EventLocation { get; set; }
        public string accidentLocation { get; set; }
        public double accidentLat { get; set; }
        public double accidentLng { get; set; }
        public string IsVIP { get; set; }
        public string Remark { get; set; }
        public int ClameTypeID { get; set; }
        public int SubClameTypeID { get; set; }
        public string informBy { get; set; }
        public double appointLat { get; set; }
        public double appointLong { get; set; }
        public string appointLocation { get; set; }
        public string appointDate { get; set; }
        public string appointName { get; set; }
        public string appointPhone { get; set; }
        public string contractName { get; set; }
        public string contractPhone { get; set; }
    }
}
