using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model
{
   public interface IServiceOutput
    {
        string code { get; set; }

        /// <summary>
        /// Service’s Response Description
        /// </summary>
        string message { get; set; }

        string description { get; set; }
        string transactionId { get; set; }

        /// <summary>
        /// yyyy-MM-dd HH:mm:ss
        /// </summary>
        DateTime transactionDateTime { get; set; }
    }
}
