using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.InquiryClientMaster;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic.Commands.Client
{
    /// <summary>
    /// Api Command Class
    /// </summary>
    public class BuzCrmInquiryClientMaster:BaseCommand
    {


        /// <summary>
        /// สำหรับสืบค้นข้อมูล CLient โดยจะแยก role เป็นแบบ General กับ Asrh
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override BaseDataModel Execute(object input)
        {
            InquiryClientMasterInputModel contentModel = (InquiryClientMasterInputModel)input;
            BaseCommand cmd;

            if (contentModel.conditionHeader.roleCode == "G")
            {
                cmd = new BuzInquiryCrmGeneralClient
                {
                    ControllerName = ControllerName,
                    TransactionId = TransactionId
                };
            }
            else 
            {
                cmd = new BuzInquiryCrmAsrhClientMaster
                {
                    ControllerName = ControllerName,
                    TransactionId = TransactionId
                };
            }
            
            return  cmd.Execute(input);
          
        }
        /*
           public override BaseDataModel Execute(object input)
           {

               InquiryClientMasterInputModel contentModel = (InquiryClientMasterInputModel)input;
               BaseCommand cmd = new NullCommand();
               switch (contentModel.conditionHeader.clientType)
               {
                   case "P":
                       cmd = new buzCrmInquiryPersonalClientMaster();
                       cmd.TransactionId = TransactionId;
                       break;
                   case "C":
                       cmd = new buzCrmInquiryCorporateClientMaster();
                       cmd.TransactionId = TransactionId;
                       break;
                   default:
                       break;
               }


               return cmd.Execute(input);
           }
              */
    }
}