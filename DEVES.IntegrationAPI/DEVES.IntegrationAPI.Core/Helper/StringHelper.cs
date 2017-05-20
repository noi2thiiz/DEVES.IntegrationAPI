using System.Text.RegularExpressions;

namespace DEVES.IntegrationAPI.Core.Helper
{
    /// <summary>
    /// String helper methods
    /// </summary>
    public static class StringHelper
    {
        public static string ToUpperIgnoreNull(this string value)
        {
            if (value != null)
            {
                value = value.ToUpper();
            }
            return value;
        }
        public static string ReplaceMultiplSpacesWithSingleSpace(this string value)
        {
            if (value != null)
            {
               
                RegexOptions options = RegexOptions.None;
                Regex regex = new Regex("[ ]{2,}", options);
                value = regex.Replace(value, " ")?.Trim();
             
            }
            return value;
        }


       
    }
}