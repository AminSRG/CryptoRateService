using System.Text.Json.Serialization;

namespace CryptoRateService.Integrations.Model
{
    public class CoinMarketCapResponse
    {
        [JsonPropertyName("status")]
        public StatusInfo? Status { get; set; }

        [JsonPropertyName("data")]
        public Dictionary<string, CoinDetails>? Data { get; set; }
    }

    public class StatusInfo
    {
        [JsonPropertyName("timestamp")]
        public string? Timestamp { get; set; }

        [JsonPropertyName("error_code")]
        public int ErrorCode { get; set; }

        [JsonPropertyName("error_message")]
        public string? ErrorMessage { get; set; }

        [JsonPropertyName("elapsed")]
        public int Elapsed { get; set; }

        [JsonPropertyName("credit_count")]
        public int CreditCount { get; set; }
    }

    public class CoinDetails
    {
        [JsonPropertyName("quote")]
        public Dictionary<string, CurrencyDetails>? Quote { get; set; }
    }

    public class CurrencyDetails
    {
        [JsonPropertyName("price")]
        public double Price { get; set; }
    }
}