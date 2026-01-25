using Newtonsoft.Json;

namespace TeamChoice.WebApis.Contracts.DTOs
{
    public class MMTAccount
    {
        [JsonProperty("AccountName")]
        public string AccountName { get; set; }
    }
}
