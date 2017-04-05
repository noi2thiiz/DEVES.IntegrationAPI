using System.Collections.Generic;
namespace DEVES.IntegrationAPI.WebApi.DataAccessService
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