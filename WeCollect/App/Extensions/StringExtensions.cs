using System.Linq;

namespace System
{
    public static class StringExtensions
    {
        public static string ToUriSafeString(this string str)
        {
            return new string(str.Select(c => char.IsLetterOrDigit(c) ? char.ToLower(c) : '-').ToArray());
        }

    }
}
