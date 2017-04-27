using System;
using System.ComponentModel.DataAnnotations;

namespace DEVES.IntegrationAPI.WebApi.Core.Attributes
{
    public class NotNullOrEmpty : ValidationAttribute
    {
        public override object TypeId { get; } = new object();

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var required = new RequiredAttribute();
            if ( !required.IsValid(Convert.ToString(value).Trim()))
            {
                return new ValidationResult(
                    "NotNullOrEmpty"
                );
            }

            return null;
        }
    }

}