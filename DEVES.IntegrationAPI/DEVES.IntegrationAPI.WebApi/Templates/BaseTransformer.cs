using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;

namespace DEVES.IntegrationAPI.WebApi.Templates
{
    public abstract class BaseTransformer
    {
        internal const string CONST_CONCAT = "|";
        public abstract BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output);
        //public abstract void TransformModel(BaseDataModel input,ref BaseDataModel output);

    }
}