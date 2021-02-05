using System.Net;

namespace QvaPay.Sdk.Models
{
    public class QvaPayResponse
    {
        public bool Success { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
    }

    public class QvaPayResponse<T> : QvaPayResponse
    {
        public T Data { get; set; }
    }
}
