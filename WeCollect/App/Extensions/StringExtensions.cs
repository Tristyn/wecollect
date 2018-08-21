using System.Linq;

namespace WeCollect.App.Extensions
{
    public static class StringExtensions
    {
        public static string ToUriSafeString(this string str)
        {
            return new string(str.Select(c => char.IsLetterOrDigit(c) ? char.ToLower(c) : '-').ToArray());
        }

    }
}
