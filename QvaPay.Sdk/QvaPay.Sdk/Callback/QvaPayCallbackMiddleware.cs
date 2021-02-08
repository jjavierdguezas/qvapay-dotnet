using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace QvaPay.Sdk.Callback
{
    public class QvaPayCallbackMiddleware
    {
        private readonly RequestDelegate _next;

        public QvaPayCallbackMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IQvaPayCallbackHander handler)
        {
            var invoiceId = context.Request.Query.TryGetValue("id", out var idVal) && Guid.TryParse(idVal, out var id) ? id : Guid.Empty;
            var remoteId = context.Request.Query.TryGetValue("remote_id", out var remoteIdVal) ? remoteIdVal.ToString() : null;

            var response = await handler.HandleCallback(invoiceId, remoteId);

            context.Response.StatusCode = (int)response.StatusCode;
            await context.Response.WriteAsync(response.Message);

            // we're all done, so don't invoke next middleware
        }
    }
}
