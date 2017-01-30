using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.EWI
{
    public enum EWIResponseCode
    {
        /// <summary>
        /// Success
        /// </summary>
        EWI0000I = 0,
        /// <summary>
        /// Password Expired
        /// </summary>
        EWI3001E = 3001,
        /// <summary>
        /// Signon Failed
        /// </summary>
        EWI3000E = 3000,
        /// <summary>
        /// Invalid Token
        /// </summary>
        EWI3002E = 3002,
        /// <summary>
        /// Invalid Credential
        /// </summary>
        EWI3003E = 3003,
        /// <summary>
        /// Bad Request
        /// </summary>
        EWI3004E = 3004,
        ETC = 9999,
    }
}
