namespace TeamChoice.WebApis.Utils
{
    public static class LocServiceSqlQueries
    {
        // Replace with actual table name and column selection
        public const string FIND_BY_LOC_ID = @"
            SELECT ServiceID, ProviderName, ServiceType, ServiceName, ServiceCode, 
                   MinAmount, MaxAmount, CurrencyCode, CountryCode, Description, Active 
            FROM LocationServiceEntity 
            WHERE LocId = @locId";
    }
}
