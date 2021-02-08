using System;
using System.Threading.Tasks;

namespace QvaPay.Sdk.Callback
{
    public interface IQvaPayCallbackHander
    {
        Task<QvaPayCallbackResponse> HandleCallback(Guid invoiceId, string remoteId = null);
    }
}
