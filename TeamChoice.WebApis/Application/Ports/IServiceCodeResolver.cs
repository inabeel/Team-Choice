namespace TeamChoice.WebApis.Application.Ports;

public interface IServiceCodeResolver
{
    /// <summary>
    /// Resolves an incoming service code into a canonical service type.
    /// </summary>
    /// <exception cref="InvalidServiceCodeException">
    /// Thrown when the service code is invalid or cannot be resolved.
    /// </exception>
    Task<string> ResolveAsync(string serviceCode, CancellationToken cancellationToken);
}