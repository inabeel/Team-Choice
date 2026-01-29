using Newtonsoft.Json;

namespace TeamChoice.WebApis.Contracts.DTOs
{
    public class TplusLookupReq
    {
        [JsonProperty("account")]
        public string Account { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }

    // Response DTOs
    public class TplusApiResponse
    {
        public List<TplusAccount> TPlusAccValidate { get; set; }
        public string StatusCode { get; set; }
        public string Status { get; set; }
        public DateTime TimeStamp { get; set; }
    }

    public class TplusAccount
    {
        [JsonProperty("accountType")]
        public string AccountType { get; set; }

        [JsonProperty("accountName")]
        public string AccountName { get; set; }

        [JsonProperty("accountNo")]
        public string AccountNo { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }
    }
}
