using System.Net;

namespace QvaPay.Sdk.Callback
{
    public class QvaPayCallbackResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
    }
}
