using System.Text.Json.Serialization;

namespace Infrastructure.Models
{
    public class CashOrder
    {
        [JsonPropertyName("request_id")]
        public int Id { get; set; }

        [JsonPropertyName("client_id")]
        public string UserId { get; set; }

        [JsonPropertyName("departemnt_address")]
        public string OfficeAddress { get; set; }

        [JsonPropertyName("amount")]
        public double Amount { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }
        public string IpAddress { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}