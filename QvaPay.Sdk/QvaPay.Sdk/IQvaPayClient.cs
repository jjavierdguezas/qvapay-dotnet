using QvaPay.Sdk.Models;
using System.Threading.Tasks;

namespace QvaPay.Sdk
{
    public interface IQvaPayClient
    {
        Task<QvaPayResponse<QvaPayInvoiceInfo>> CreateInvoice(double amount, string description, string remoteId = null, bool signed = true);
        Task<QvaPayResponse<QvaPayAppInfo>> GetAppInfo();
        Task<QvaPayResponse<double>> GetBalance();
        Task<QvaPayResponse<QvaPayTransactionsPage>> GetTransactions(int? page = null);
    }
}