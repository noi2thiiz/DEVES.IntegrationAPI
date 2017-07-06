using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.MakeAppointment
{
    public class MakeAppointmentInputModel
    {
        public GeneralHeaderModel generalHeader { get; set; }
        public AttendeeModel attendee { get; set; }
        public AppointmentInfoModel appointmentInfo { get; set; }
        public ContactInfoModel contactInfo { get; set; }
    }

    public class GeneralHeaderModel
    {
        public string requester { get; set; }
    }

    public class AttendeeModel
    {
        public string attendeeType { get; set; }
        public string attendeeId { get; set; }
    }

    public class AppointmentInfoModel : BaseDataModel
    {
        public string subject { get; set; }
        public string startDateTime
        {
            get
            {
                string s = "";
                if (dtStartDateTime != null)
                {
                    CultureInfo enUS = new CultureInfo("en-US");
                    s = dtStartDateTime.Value.ToString(DateTimeCustomFormat, enUS);

                }
                return s;
            }
        }
        public DateTime? dtStartDateTime { get; set; }

        public int duration { get; set; }
        public string allDayEventFlag { get; set; }
        public string description { get; set; }
    }

    public class ContactInfoModel
    {
        public string fullName { get; set; }
        public string email { get; set; }
        public string mobilePhone { get; set; }
        public string address { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
    }
}
