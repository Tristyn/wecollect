using Microsoft.Azure.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Microsoft.Azure.Documents
{
    public static class DocumentClientExceptionExtensions
    {
        public static bool IsNotFound(this DocumentClientException ex)
        {
            return ex.StatusCode == HttpStatusCode.NotFound;
        }

        public static bool IsTooManyRequests(this DocumentClientException ex)
        {
            return ex.StatusCode == HttpStatusCode.TooManyRequests;
        }

        public static bool IsPreconditionFailed(this DocumentClientException ex)
        {
            return ex.StatusCode == HttpStatusCode.PreconditionFailed;
        }
    }
}
