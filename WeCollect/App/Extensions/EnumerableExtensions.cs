using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using WeCollect.App;

namespace System.Linq
{
    public static class EnumerableExtensions
    {

        private static readonly ILogger _log = Logger.GetLogger(nameof(EnumerableExtensions));

        public static T SingleOrLog<T>(this IEnumerable<T> source)
        {
            T first = default;
            foreach(var item in source)
            {
                if (first == default)
                {
                    first = item;
                    continue;
                }

                _log.LogWarning("Single or default yielded multiple results. {0}, {1}.", first, item);
                break;
            }

            return first;
        }

        public static T SingleOrLog<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            T first = default;
            foreach (var item in source)
            {
                if (!predicate(item))
                    continue;

                if (first == default)
                {
                    first = item;
                    continue;
                }

                _log.LogWarning("Single or default yielded multiple results. {0}, {1}.", first, item);
                break;
            }

            return first;
        }
    }
}
