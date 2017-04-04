using System;
using System.Web.UI;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.ClaimRegistration;
using DEVES.IntegrationAPI.Model.RegClaimRequestFromRVP;
using DEVES.IntegrationAPI.WebApi.TechnicalService;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class BuzRegClaimRequestFromRVPCommand: BaseCommand
    {

        public override BaseDataModel Execute(object input)
        {
            CrmRegClaimRequestFromRVPDataOutputModel output = new CrmRegClaimRequestFromRVPDataOutputModel();

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
                    throw new Exception("The above query returns multiple  result sets");
            }

            if (result.Count == 0)
            {
                try
                {
                    clientDataGateway.Create();
                }
                catch (Exception e)
                {
                    throw new Exception("Error On Create Client");
                }
            }




            return output;
        }

        protected bool isValidPolicyAndPolicyAditional()
        {
            return true;
        }
    }
}