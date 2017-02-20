using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.WebApi.Models.WebDialer
{
    public interface IServiceResult
    {
         string Code { get; set; }
         string Description { get; set; }
         string Content { get; set; }
         object Data { get; set; }
        
    }
}
