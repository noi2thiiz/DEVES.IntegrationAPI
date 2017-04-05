using System;
using DEVES.IntegrationAPI.Model.RegClaimRequestFromRVP;
using DEVES.IntegrationAPI.WebApi.DataAccessService;
namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class BuzRegClaimRequestFromRVPCommand
    {

        public CrmregClaimRequestFromRVPContentOutputModel Execute(object input)
        {
            CrmregClaimRequestFromRVPContentOutputModel outputContent = new CrmregClaimRequestFromRVPContentOutputModel();
            outputContent.data = new CrmRegClaimRequestFromRVPDataOutputModel()
            {
                claimNotiNo = "",
                ticketNo=""
            };



            // ตรวจสอบ  Policy และ PolicyAditional ถ้ามีมากกว่า 1 รายการ ให้ return error
            if (!isValidPolicyAndPolicyAditional())
            {
                    throw new Exception("invalidPolicyAndPolicyAditional");
            }
            ClientMasterDataGateway clientDataGateway = new ClientMasterDataGateway();

            // หาว่ามี client แล้วหรือยัง ถ้ามีมากกว่า 1 รายการให้ return error ถ้าไม่เจอให้สร้างใหม่
            var result = clientDataGateway.fetchAll();
            if (result.Count > 1)
            {
                    //throw new Exception("The above query returns multiple  result sets");
            }

            if (result.Count == 0)
            {
                try
                {
                    clientDataGateway.Create();
                }
                catch (Exception e)
                {
                    //throw new Exception("Error On Create Client");
                }
            }

            return outputContent;
        }

        protected bool isValidPolicyAndPolicyAditional()
        {
            return true;
        }
    }
}