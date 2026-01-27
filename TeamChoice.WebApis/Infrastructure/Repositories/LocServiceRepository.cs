using TeamChoice.WebApis.Domain.Entities;
using TeamChoice.WebApis.Infrastructure.Persistence;
using TeamChoice.WebApis.Utils;

namespace TeamChoice.WebApis.Infrastructure.Repositories
{
    public interface ILocServiceRepository
    {
        Task<IEnumerable<LocServiceEntity>> FindByLocIdAsync(string locId);
    }
    public class LocServiceRepository : ILocServiceRepository
    {
        private readonly IDatabaseService _databaseService;
        private readonly ILogger<LocServiceRepository> _logger;

        public LocServiceRepository(IDatabaseService databaseService, ILogger<LocServiceRepository> logger)
        {
            _databaseService = databaseService;
            _logger = logger;
        }

        public async Task<IEnumerable<LocServiceEntity>> FindByLocIdAsync(string locId)
        {
            var parameters = new Dictionary<string, object> { { "@locId", locId } };

            // Assuming QueryAsync returns a list/enumerable. 
            // Since IDatabaseService.QueryOneAsync returns T, we assume QueryAsync or similar exists for lists,
            // or we use a stored procedure execution that returns a list.
            // Here I will implement mapping logic assuming IDatabaseService has a method for collections 
            // or I will use ExecuteStoredProcedureAsync if it supports lists.

            // Note: If IDatabaseService only has QueryOneAsync, you might need to add QueryAsync<T> to it.
            // For now, I'll use a hypothetical QueryAsync similar to QueryOneAsync but returning IEnumerable.

            return await _databaseService.QueryAsync(LocServiceSqlQueries.FIND_BY_LOC_ID, parameters,
                reader => new LocServiceEntity
                {
                    // Mapping fields based on LocServiceRes structure as Entity usually mirrors DB
                    ServiceID = reader["ServiceID"] as string,
                    ProviderName = reader["ProviderName"] as string,
                    ServiceType = reader["ServiceType"] as string,
                    ServiceName = reader["ServiceName"] as string,
                    ServiceCode = reader["ServiceCode"] as string,
                    MinAmount = reader["MinAmount"] as string,
                    MaxAmount = reader["MaxAmount"] as string,
                    CurrencyCode = reader["CurrencyCode"] as string,
                    CountryCode = reader["CountryCode"] as string,
                    Description = reader["Description"] as string,
                    Active = reader["Active"] != DBNull.Value && Convert.ToBoolean(reader["Active"])
                }
            );
        }
    }
}
