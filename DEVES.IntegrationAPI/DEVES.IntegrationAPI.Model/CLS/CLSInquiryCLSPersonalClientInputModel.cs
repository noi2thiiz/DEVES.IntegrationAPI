﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Model.CLS
{
    public enum ENUM_CLIENT_ROLE
    {
        G,
        A,
        S,
        R,
        H
    }

    public class CLSInquiryPersonalClientInputModel : BaseDataModel
    {
        public String clientId { set; get; }
        public string roleCode { set; get; }
        public String personalFullName { set; get; }
        public String idCitizen { set; get; }
        public String telephone { set; get; }
        public String emailAddress { set; get; }
    }
}