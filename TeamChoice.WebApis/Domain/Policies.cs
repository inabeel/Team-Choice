using TeamChoice.WebApis.Models.DTOs.Services;

namespace TeamChoice.WebApis.Domain.Policies;

public interface IServiceLookupPolicy
{
    IReadOnlyCollection<ServiceDetailDto> FilterActiveServices(
        IEnumerable<ServiceDetailDto> services);
}

/// <summary>
/// Applies domain-level business filtering.
/// </summary>
public sealed class ServiceLookupPolicy : IServiceLookupPolicy
{
    public IReadOnlyCollection<ServiceDetailDto> FilterActiveServices(
        IEnumerable<ServiceDetailDto> services)
    {
        return services
            .Where(s => s.Active)
            .ToList();
    }
}