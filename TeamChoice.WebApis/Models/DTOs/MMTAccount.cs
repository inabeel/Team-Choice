using Newtonsoft.Json;

namespace TeamChoice.WebApis.Models.DTOs
{
    public class MMTAccount
    {
        [JsonProperty("AccountName")]
        public string AccountName { get; set; }
    }
}
