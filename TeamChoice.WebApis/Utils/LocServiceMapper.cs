using TeamChoice.WebApis.Domain.Entities;
using TeamChoice.WebApis.Models.DTOs;

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
