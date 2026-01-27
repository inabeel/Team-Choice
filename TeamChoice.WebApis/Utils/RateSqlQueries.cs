namespace TeamChoice.WebApis.Utils
{
    public static class RateSqlQueries
    {
        public const string CALCULATE_EXTERNAL_PARTNER_COMMISSION = @"
            EXEC usp_CalculateExternalPartnerCommission 
                @SendingCountry = @SendingCountry, 
                @CurrencyCode = @CurrencyCode, 
                @ServiceCode = @ServiceCode, 
                @RecipientCountry = @RecipientCountry, 
                @SendingAmount = @SendingAmount
        ";
    }
}