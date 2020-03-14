namespace Fordere.RestService.Extensions
{
    public static class StringExtensions
    {
        public static string PrePostFix(this string targetString, string prePostFix)
        {
            return prePostFix + targetString + prePostFix;
        }
    }
}
