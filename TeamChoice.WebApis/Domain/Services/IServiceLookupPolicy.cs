using TeamChoice.WebApis.Domain.Models;
using TeamChoice.WebApis.Models.DTOs.Services;

namespace TeamChoice.WebApis.Domain.Services;

public interface IServiceLookupPolicy
{
    IReadOnlyList<ServiceDetail> FilterActiveServices(
        IReadOnlyList<ServiceDetail> services);
}

public sealed class ServiceLookupPolicy : IServiceLookupPolicy
{
    /// <summary>
    /// Applies domain-level business filtering.
    /// </summary>
    public IReadOnlyList<ServiceDetail> FilterActiveServices(
        IReadOnlyList<ServiceDetail> services)
    {
        return services
            .Where(s => s.IsActive)
            .ToList();
    }
}