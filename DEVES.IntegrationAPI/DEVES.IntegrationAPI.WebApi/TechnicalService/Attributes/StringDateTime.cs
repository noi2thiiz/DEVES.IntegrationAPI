using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Resources;
using System.Globalization;

namespace DEVES.IntegrationAPI.WebApi.Services.Core.Attributes
{
    public class StringDateTimeAttribute:RegularExpressionAttribute
    {

        public StringDateTimeAttribute()
            : base(GetRegex())
        { }


        private static string GetRegex()
        {
            // TODO: Go off and get your RegEx here
            return @"^([0-9]{4})(-(?:1[0-2]|0?[1-9])(-(?:3[01]|[12][0-9]|0[1-9])?)?)([ ]+((?:2[0-3]|[01][0-9])(:(?:[0-5][0-9])(:(?:[0-5][0-9])?)?)?)$)?";
        }

    }
}