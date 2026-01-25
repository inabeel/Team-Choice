using Newtonsoft.Json;
using System.Security.Principal;

namespace TeamChoice.WebApis.Contracts.DTOs
{
    public class MMTApiResponse
    {
        [JsonProperty("Validate")]
        public List<MMTAccount> Validate { get; set; }

        [JsonProperty("StatusCode")]
        public string StatusCode { get; set; }

        [JsonProperty("StatusMessage")]
        public string StatusMessage { get; set; }
    }
}
