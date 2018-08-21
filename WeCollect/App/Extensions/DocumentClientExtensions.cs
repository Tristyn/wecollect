using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WeCollect.App;

namespace System.Linq
{
    public static class DocumentClientExtensions
    {
        private static readonly ILogger _log = Logger.GetLogger(nameof(DocumentClientExtensions));

        public static async Task<T> SingleOrLog<T>(this IQueryable<T> source)
        {
            IDocumentQuery<T> result = source.AsDocumentQuery();
            
            T first = default;

            while (result.HasMoreResults)
            {
                FeedResponse<T> batch = await result.ExecuteNextAsync<T>();
                foreach (T item in batch)
                {
                    if (first == default)
                    {
                        first = item;
                        continue;
                    }

                    _log.LogWarning("Single or default yielded multiple results. {0}, {1}.", first, item);
                    break;
                }

                break;
            }

            if (first == default)
            {
                throw new InvalidOperationException();
            }

            return first;
        }

        public static async Task<T> SingleOrLog<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate)
        {
            IDocumentQuery<T> result = source.Where(predicate).AsDocumentQuery();

            T first = default;

            do
            {
                FeedResponse<T> batch = await result.ExecuteNextAsync<T>();
                foreach (T item in batch)
                {
                    if (first == default)
                    {
                        first = item;
                        continue;
                    }

                    _log.LogWarning("Single or default yielded multiple results. {0}, {1}.", first, item);
                    break;
                }

                break;
            } while (result.HasMoreResults);

            if (first == default)
            {
                throw new InvalidOperationException();
            }

            return first;
        }

        public static async Task<bool> Exists<T>(this IQueryable<T> source)
        {
            IDocumentQuery<T> result = source.AsDocumentQuery();

            T first = default;

            while (result.HasMoreResults)
            {
                FeedResponse<T> batch = await result.ExecuteNextAsync<T>();
                foreach (T item in batch)
                {
                    if (first == default)
                    {
                        first = item;
                        continue;
                    }

                    _log.LogWarning("Linq single yielded multiple results. {0}, {1}.", first, item);
                    break;
                }

                break;
            }

            return first != default;
        }

        public static async Task<bool> Exists<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate)
        {
            IDocumentQuery<T> result = source.Where(predicate).AsDocumentQuery();

            T first = default;

            while (result.HasMoreResults)
            {
                FeedResponse<T> batch = await result.ExecuteNextAsync<T>();
                foreach (T item in batch)
                {
                    if (first == default)
                    {
                        first = item;
                        continue;
                    }

                    _log.LogWarning("Linq single yielded multiple results. {0}, {1}.", first, item);
                    break;
                }

                return true;
            }

            return first != default;
        }

        public static async Task<IEnumerable<T>> ToListAsync<T>(this IQueryable<T> query)
        {
            IDocumentQuery<T> result = query.AsDocumentQuery();
            IEnumerable<T> ret = null;
            while (result.HasMoreResults)
            {
                FeedResponse<T> batch = await result.ExecuteNextAsync<T>();
                if (ret == null)
                {
                    ret = batch;
                }
                else
                {
                    ret = ret.Concat(batch);
                }
            }
            return ret;
        }
    }
}
