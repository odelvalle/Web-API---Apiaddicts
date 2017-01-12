namespace ASP.NET.ApiAddits.RESTFul.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrTrimIsEmpty(this string value)
        {
            return string.IsNullOrEmpty(value) || value.Trim().Length == 0;
        }
    }
}