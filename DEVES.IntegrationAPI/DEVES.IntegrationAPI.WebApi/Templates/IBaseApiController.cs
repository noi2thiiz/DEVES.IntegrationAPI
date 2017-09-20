using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.WebApi.Templates
{
    interface IBaseApiController
    {
        object Post(object value);
    }
}
