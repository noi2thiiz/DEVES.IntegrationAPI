using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.SMS;
using DEVES.IntegrationAPI.WebApi.Logic.Services;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzSendSMS:BuzCommand
    {
        public override BaseDataModel ExecuteInput(object input)
        {
            var smsInput = (SendSMSInputModel) input;
            var result = SendSmsService.Instance.SendMessage(smsInput.message, smsInput.mobileNumber);

            var output = new SendSMSOutputModel();
            if (result.success)
            {
                output.data = new SendSMSOutputDataModel
                {
                    code = "1",
                    message = "Delivered"
                };
            }
            else
            {
                output.data = new SendSMSOutputDataModel
                {
                    code = "0",
                    message = "Failed"
                };

               
            }

            output.transactionDateTime = DateTime.Now;
            output.transactionId = TransactionId;
            output.code = CommonConstant.CODE_SUCCESS;
            output.message = "Success";
            output.description = "The server successfully processed the request";
           
            return output;
        }
    }
}