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
        EWI0000I,
        /// <summary>
        /// Password Expired
        /// </summary>
        EWI3001E,
        /// <summary>
        /// Signon Failed
        /// </summary>
        EWI3000E,
        /// <summary>
        /// Invalid Token
        /// </summary>
        EWI3002E,
        /// <summary>
        /// Invalid Credential
        /// </summary>
        EWI3003E,
        /// <summary>
        /// Bad Request
        /// </summary>
        EWI3004E,
        ETC,
    }
}
