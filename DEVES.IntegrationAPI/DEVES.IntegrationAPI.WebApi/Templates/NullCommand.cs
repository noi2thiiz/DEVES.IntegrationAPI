using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;

namespace DEVES.IntegrationAPI.WebApi.Templates
{
    public class NullCommand : BaseCommand
    {
        public override BaseContentOutputModel Execute(object input)
        {
            throw new NotImplementedException();
        }
    }
}