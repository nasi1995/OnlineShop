using System.Text.Json;
using System.Text.Json.Serialization;

namespace OnlineShop.Models
{
    public class ZarinpalResponse
    {
        [JsonPropertyName("data")]
        public ZarinpalData? Data { get; set; }

        [JsonPropertyName("errors")]
        public JsonElement Errors { get; set; }
    }

    public class ZarinpalData
    {
        [JsonPropertyName("authority")]
        public string? Authority { get; set; }

        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("fee")]
        public int Fee { get; set; }

        [JsonPropertyName("fee_type")]
        public string? FeeType { get; set; }
    }
}