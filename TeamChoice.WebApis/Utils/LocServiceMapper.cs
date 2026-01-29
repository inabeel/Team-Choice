using TeamChoice.WebApis.Contracts.DTOs;
using TeamChoice.WebApis.Domain.Entities;

namespace TeamChoice.WebApis.Utils
{
    public static class LocServiceMapper
    {
        public static LocServiceRes ToResponse(LocServiceEntity entity)
        {
            if (entity == null) return null;

            return new LocServiceRes
            {
                ServiceName = entity.ServiceName,
                ServiceCode = entity.ServiceCode
            };
        }

        public static LocServiceRes ToFullResponse(LocServiceEntity entity)
        {
            if (entity is null)
                return null;

            return new LocServiceRes
            {
                ServiceID = entity.ServiceID,
                ProviderName = entity.ProviderName,
                ServiceType = entity.ServiceType,
                ServiceName = entity.ServiceName,
                ServiceCode = entity.ServiceCode,
                MinAmount = entity.MinAmount,
                MaxAmount = entity.MaxAmount,
                CurrencyCode = entity.CurrencyCode,
                CountryCode = entity.CountryCode,
                Description = entity.Description,

                // common DB pattern: Y/N or 1/0
                Active = entity.Active
            };
        }
    }
}
