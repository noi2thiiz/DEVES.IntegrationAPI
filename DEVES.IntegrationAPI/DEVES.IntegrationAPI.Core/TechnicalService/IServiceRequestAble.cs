using DEVES.IntegrationAPI.WebApi.Core;

namespace DEVES.IntegrationAPI.WebApi.Services.Core
{
    public interface IServiceRequestAble<RequestModelType>
        where RequestModelType : class
        
    {

       IServiceResult  Execute(RequestModelType req);

    }
}
