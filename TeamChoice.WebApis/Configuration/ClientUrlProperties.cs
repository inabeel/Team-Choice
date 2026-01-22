namespace TeamChoice.WebApis.Configuration
{
    public class ClientUrlProperties
    {
        // Configuration section name for binding (matches prefix = "notification.client")
        public const string SectionName = "notification:client";

        public int IdCorresponsal { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string UserCred { get; set; }
        public string Loccode { get; set; }
        public string TeamMessage { get; set; }
        public string SndAgtCode { get; set; }
        public string IdTeller { get; set; }
        public string CallbackUrl { get; set; }
        public string NotificationUrl { get; set; }
        public string PaymentReportUrl { get; set; }
        public string PaymentReportUrlCallBack { get; set; }
        public string InfoUrl { get; set; }
        public string Remit0Url { get; set; }
        public string ApiKey { get; set; }
        public string AppId { get; set; }
    }
}
