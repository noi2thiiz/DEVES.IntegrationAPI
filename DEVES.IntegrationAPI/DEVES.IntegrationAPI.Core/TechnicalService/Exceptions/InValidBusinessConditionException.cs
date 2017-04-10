using DEVES.IntegrationAPI.Core.TechnicalService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Core.TechnicalService.Exceptions
{
    //[Serializable]
    public class BuzInValidBusinessConditionException : Exception
    {
        public ErrorMessage ErrorMessage { get; set; }

        public BuzInValidBusinessConditionException()
        {
        }

        public BuzInValidBusinessConditionException(ErrorMessage errorMessage)
        {
            this.ErrorMessage = errorMessage;
        }

        public BuzInValidBusinessConditionException(string message) : base(message)
        {
        }

        public BuzInValidBusinessConditionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BuzInValidBusinessConditionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
