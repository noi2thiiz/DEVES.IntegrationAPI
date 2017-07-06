using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.MakeAppointment
{
    public class MakeAppointmentOutputModel : BaseContentJsonProxyOutputModel
    {
        public MakeAppointmentDataModel data { get; set; }
    }

    public class MakeAppointmentDataModel
    {
        public string appointmentId { get; set; }
    }
}
