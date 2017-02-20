using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.WebApi.Models.WebDialer
{
    public  interface IServiceRequestAble<T>
        where T : class
    {

        IServiceResult Execute(T req);
      
    }
}
