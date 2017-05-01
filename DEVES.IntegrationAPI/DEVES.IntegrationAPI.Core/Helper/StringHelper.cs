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

    }
}