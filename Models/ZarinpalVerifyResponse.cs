using System.Text.Json.Serialization;

namespace OnlineShop.Models
{
    public class ZarinpalVerifyResponse
    {
        [JsonPropertyName("data")]
        public ZarinpalVerifyData? Data { get; set; }


        [JsonPropertyName("errors")]
        public object? Errors { get; set; }
    }



    public class ZarinpalVerifyData
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }


        [JsonPropertyName("message")]
        public string? Message { get; set; }


        [JsonPropertyName("ref_id")]
        public long RefId { get; set; }


        [JsonPropertyName("card_pan")]
        public string? CardPan { get; set; }


        [JsonPropertyName("card_hash")]
        public string? CardHash { get; set; }


        [JsonPropertyName("fee")]
        public int Fee { get; set; }


        [JsonPropertyName("fee_type")]
        public string? FeeType { get; set; }
    }
}