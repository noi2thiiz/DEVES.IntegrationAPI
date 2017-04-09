using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Core.TechnicalService.Models
{
    public class ErrorMessage
    {
        public string httpStatus { get; set; }
        public string code { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string fieldError { get; set; }
    }
}
