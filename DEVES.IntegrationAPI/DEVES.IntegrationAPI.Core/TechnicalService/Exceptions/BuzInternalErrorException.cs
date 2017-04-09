using DEVES.IntegrationAPI.Core.TechnicalService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Core.TechnicalService.Exceptions
{
    [Serializable]
    public class BuzInternalErrorException : Exception
    {
        private ErrorMessage errorMessage;

        public BuzInternalErrorException()
        {
        }

        public BuzInternalErrorException(ErrorMessage errorMessage)
        {
            this.errorMessage = errorMessage;
        }

        public BuzInternalErrorException(string message) : base(message)
        {
        }

        public BuzInternalErrorException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BuzInternalErrorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
