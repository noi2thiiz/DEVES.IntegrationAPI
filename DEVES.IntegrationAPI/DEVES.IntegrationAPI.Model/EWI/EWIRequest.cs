﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DEVES.IntegrationAPI.Model.EWI
{
    public class EWIRequest
    {
        public string username { get; set; }
        /// <summary>
        /// Base64 encryption
        /// </summary>
        public string password { get; set; }
        public string uid { get; set; }
        public string gid { get; set; }
        public string token { get; set; }
        public object content { get; set; }
    }
}
