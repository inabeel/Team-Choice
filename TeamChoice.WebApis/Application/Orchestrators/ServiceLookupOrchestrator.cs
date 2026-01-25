using TeamChoice.WebApis.Application.Policies;
using TeamChoice.WebApis.Contracts.DTOs;
using TeamChoice.WebApis.Infrastructure.Clients;

namespace TeamChoice.WebApis.Application.Orchestrators;

public interface IServiceLookupOrchestrator
{
    Task<IReadOnlyCollection<ServiceDetailDto>> GetAvailableServicesAsync();
}

/// <summary>
/// Coordinates fetching services and applying business rules.
/// </summary>
public sealed class ServiceLookupOrchestrator : IServiceLookupOrchestrator
{
    private readonly IServiceCatalogClient _serviceCatalogClient;
    private readonly IServiceLookupPolicy _serviceLookupPolicy;
    private readonly ILogger<ServiceLookupOrchestrator> _logger;

    public ServiceLookupOrchestrator(
        IServiceCatalogClient serviceCatalogClient,
        IServiceLookupPolicy serviceLookupPolicy,
        ILogger<ServiceLookupOrchestrator> logger)
    {
        _serviceCatalogClient = serviceCatalogClient;
        _serviceLookupPolicy = serviceLookupPolicy;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<ServiceDetailDto>> GetAvailableServicesAsync()
    {
        _logger.LogDebug("🔍 Orchestrating service lookup");

        var services = await _serviceCatalogClient.FetchServicesAsync();

        var filtered = _serviceLookupPolicy.FilterActiveServices(services);

        if (filtered.Count == 0)
            throw new InvalidOperationException("No services available");

        _logger.LogInformation("✅ {Count} services found", filtered.Count);

        return filtered;
    }
}

