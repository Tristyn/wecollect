using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using WeCollect.App;

namespace System.Linq
{
    public static class EnumerableExtensions
    {

        private static readonly ILogger _log = Log.GetLogger(nameof(EnumerableExtensions));

        public static T SingleOrLog<T>(this IEnumerable<T> source)
        {
            T first = default;
            foreach(var item in source)
            {
                if (EqualityComparer<T>.Default.Equals(first, default(T)))
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

                if (EqualityComparer<T>.Default.Equals(first, default(T)))
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
