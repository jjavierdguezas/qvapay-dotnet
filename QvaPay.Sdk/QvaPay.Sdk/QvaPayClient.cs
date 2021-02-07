using QvaPay.Sdk.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.AspNetCore.WebUtilities;

namespace QvaPay.Sdk
{
    public class QvaPayClient : IQvaPayClient
    {
        private readonly Uri _baseUrl = new Uri("https://qvapay.com/api/v1/");
        private readonly QvaPayAuthConfiguration _config;
        protected readonly HttpClient _httpClient;

        public QvaPayClient(QvaPayAuthConfiguration config, HttpClient httpClient)
        {
            _config = config;
            _httpClient = httpClient;
            _httpClient.BaseAddress = _baseUrl;
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private string BuildUrl(string action, Dictionary<string, string> qs = null)
        {
            var fullUrl = new Uri(_baseUrl, action);

            var queryDict = qs ?? new Dictionary<string, string>();

            queryDict["app_id"] = _config.AppId;
            queryDict["app_secret"] = _config.AppSecret;

            return QueryHelpers.AddQueryString(fullUrl.ToString(), queryDict);
        }

        private async Task<QvaPayResponse<T>> GetResponse<T>(HttpResponseMessage httpResponse)
        {
            var responseContentString = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

            var response = new QvaPayResponse<T>
            {
                StatusCode = httpResponse.StatusCode,
            };

            if (!httpResponse.IsSuccessStatusCode)
            {
                var msg = string.IsNullOrWhiteSpace(httpResponse.ReasonPhrase) ? "Error" : httpResponse.ReasonPhrase;

                if (!string.IsNullOrWhiteSpace(responseContentString))
                    msg = $"{msg}\n{responseContentString}";

                response.Success = false;
                response.Message = msg;
            }
            else
            {
                var data = JsonConvert.DeserializeObject<T>(responseContentString);
                response.Success = true;
                response.Data = data;
            }

            return response;
        }

        public async Task<QvaPayResponse<QvaPayAppInfo>> GetAppInfo()
        {
            var url = BuildUrl("info");
            var httpResponse = await _httpClient.GetAsync(url).ConfigureAwait(false);

            return await GetResponse<QvaPayAppInfo>(httpResponse);
        }

        public async Task<QvaPayResponse<QvaPayInvoiceInfo>> CreateInvoice(double amount, string description, string remoteId = null, bool signed = true)
        {
            var url = BuildUrl("create_invoice", new Dictionary<string, string>
            {
                ["amount"] = amount.ToString("0.00"),
                ["description"] = description,
                ["remote_id"] = remoteId,
                ["signed"] = signed ? "1" : "0",
            });

            var httpResponse = await _httpClient.GetAsync(url).ConfigureAwait(false);

            return await GetResponse<QvaPayInvoiceInfo>(httpResponse);
        }

        public async Task<QvaPayResponse<QvaPayTransactionsPage>> GetTransactions(int? page = null)
        {
            if (page.HasValue && page <= 0) page = null;
            var url = BuildUrl("transactions", page.HasValue ? new Dictionary<string, string> { ["page"] = page.Value.ToString() } : null);
            var httpResponse = await _httpClient.GetAsync(url).ConfigureAwait(false);

            return await GetResponse<QvaPayTransactionsPage>(httpResponse);
        }

        public async Task<QvaPayResponse<double>> GetBalance()
        {
            var url = BuildUrl("balance");
            var httpResponse = await _httpClient.GetAsync(url).ConfigureAwait(false);

            return await GetResponse<double>(httpResponse);
        }
    }
}
