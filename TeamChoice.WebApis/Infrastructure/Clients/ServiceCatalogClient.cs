using Microsoft.Extensions.Options;
using TeamChoice.WebApis.Contracts.DTOs;

namespace TeamChoice.WebApis.Infrastructure.Clients;

public interface IServiceCatalogClient
{
    Task<IReadOnlyCollection<ServiceDetailDto>> FetchServicesAsync();
}

public sealed class ServiceCatalogClient : IServiceCatalogClient
{
    private readonly HttpClient _httpClient;
    private readonly ServiceCatalogOptions _options;
    private readonly ILogger<ServiceCatalogClient> _logger;

    public ServiceCatalogClient(
        HttpClient httpClient,
        IOptions<ServiceCatalogOptions> options,
        ILogger<ServiceCatalogClient> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<ServiceDetailDto>> FetchServicesAsync()
    {
        _logger.LogInformation("📡 Fetching services from catalog");

        var response = await _httpClient.GetAsync(_options.ServicesEndpoint);

        response.EnsureSuccessStatusCode();

        var services = await response.Content
            .ReadFromJsonAsync<List<ServiceDetailDto>>();

        if (services == null || services.Count == 0)
        {
            _logger.LogWarning("⚠ No services returned from catalog");
            return Array.Empty<ServiceDetailDto>();
        }

        _logger.LogInformation("✅ Retrieved {Count} services", services.Count);

        return services;
    }
}

public sealed class ServiceCatalogOptions
{
    public string BaseUrl { get; init; } = default!;
    public string ServicesEndpoint { get; init; } = default!;
}

