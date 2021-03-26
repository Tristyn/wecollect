using System.Net;

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
