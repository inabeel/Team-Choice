namespace TeamChoice.WebApis.Contracts.DTOs
{
    public class LocServiceReq
    {
        // Empty request body as per Java source
    }

    public class LocServiceRes
    {
        public string ServiceID { get; set; }
        public string ProviderName { get; set; }
        public string ServiceType { get; set; }
        public string ServiceName { get; set; }
        public string ServiceCode { get; set; }
        public string MinAmount { get; set; }
        public string MaxAmount { get; set; }
        public string CurrencyCode { get; set; }
        public string CountryCode { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
    }
}
