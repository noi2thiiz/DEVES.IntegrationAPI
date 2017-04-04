using System.Collections.Generic;
using DEVES.IntegrationAPI.WebApi.DataAccessService;
using Microsoft.Crm.Sdk.Messages;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class ClientMasterDataGateway
    {
        public DataResult<dynamic> fetchAll()
        {
            var result = new List<dynamic>();
            return new DataResult<dynamic>(result);
        }

        public void Create()
        {
            throw new System.NotImplementedException();
        }
    }
}