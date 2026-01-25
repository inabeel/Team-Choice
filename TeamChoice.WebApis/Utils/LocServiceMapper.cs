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
    }
}
