using CryptoRateService.Integrations.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CryptoRateService.Integrations.ExternalClients
{
    public class CoinMarketCapClient(IConfiguration configuration, HttpClient httpClient)
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly string _baseUrl = configuration["CoinMarketCap:BaseUrl"] ?? throw new ArgumentNullException("CoinMarketCap:BaseUrl", "Base URL for CoinMarketCap is not configured.");
        private readonly string _cryptoQuotesEndpoint = configuration["CoinMarketCap:CryptoQuotesEndpoint"] ?? throw new ArgumentNullException("CoinMarketCap:CryptoQuotesEndpoint", "Crypto quotes endpoint is not configured.");
        private readonly string _apiKey = configuration["CoinMarketCap:ApiKey"] ?? throw new ArgumentNullException("CoinMarketCap:ApiKey", "API key for CoinMarketCap is not configured.");

        public async Task<CoinMarketCapResponse> GetCryptoQuoteAsync(string symbol, string convert)
        {
            var url = _getCryptoQuoteApiUrl() + $"?symbol={symbol}&convert={convert}";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("X-CMC_PRO_API_KEY", _apiKey);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new($"Error: {response.StatusCode}, {response.ReasonPhrase}");


            var jsonResponse = await response.Content.ReadAsStringAsync();

            var coinMarketCapResponse = JsonConvert.DeserializeObject<CoinMarketCapResponse>(jsonResponse) ?? throw new("Failed to deserialize the response.");

            if (coinMarketCapResponse.Status?.ErrorCode != 0)
                throw new($"Error Code: {coinMarketCapResponse.Status?.ErrorCode}, " +
                                     $"Message: {coinMarketCapResponse.Status?.ErrorMessage}");


            return coinMarketCapResponse;
        }

        private string _getCryptoQuoteApiUrl() => $"{_baseUrl}{_cryptoQuotesEndpoint}";
    }
}
