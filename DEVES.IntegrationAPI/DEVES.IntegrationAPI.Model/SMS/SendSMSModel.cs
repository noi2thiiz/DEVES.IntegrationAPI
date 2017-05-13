using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.SMS
{
    public class SendSMSInputModel : BaseDataModel
    {
        /// <summary>
        ///  ข้อความที่ต้องการส่ง.
        /// </summary>

        public string message { get; set; }
        /// <summary>
        /// AD Account
        /// </summary>
        //[Required(ErrorMessage = "global ID  is required")]
        public string uid { get; set; }
        /// <summary>
        /// หมายเลขที่ต้องการส่งถึง
        /// </summary>

        public string mobileNumber { get; set; }

    }

    public class SendSMSOutputModel : BaseContentJsonProxyOutputModel
    {
        public SendSMSOutputDataModel data { set; get; } = new SendSMSOutputDataModel();
    }

    public class SendSMSOutputDataModel
    {
        public string code { get; set; }
        public string message { get; set; }
       
    }


}
