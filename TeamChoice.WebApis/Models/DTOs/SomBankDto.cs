using Newtonsoft.Json;

namespace TeamChoice.WebApis.Models.DTOs
{
    public class SomBankLookupReq
    {
        [JsonProperty("credentials")]
        public SomBankCredentialsReq Credentials { get; set; }

        [JsonProperty("selectionCriteria")]
        public SomBankCriteriaReq SelectionCriteria { get; set; }
    }

    public class SomBankCredentialsReq
    {
        [JsonProperty("apiKey")]
        public string ApiKey { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }

    public class SomBankCriteriaReq
    {
        [JsonProperty("phoneNo")]
        public string PhoneNo { get; set; }
    }

    // Responses
    public class SomBankApiResponse
    {
        [JsonProperty("customerDetailsList")]
        public List<SomBankCustomerDetail> CustomerDetailsList { get; set; }
    }

    public class SomBankCustomerDetail
    {
        [JsonProperty("accountDetailsList")]
        public List<SomBankAccountDetail> AccountDetailsList { get; set; }

        [JsonProperty("shortName")]
        public string ShortName { get; set; }
    }

    public class SomBankAccountDetail
    {
        [JsonProperty("accountNo")]
        public string AccountNo { get; set; }
    }
}
