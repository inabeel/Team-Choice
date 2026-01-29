using System.Text.RegularExpressions;
using TeamChoice.WebApis.Application.Ports;

namespace TeamChoice.WebApis.Infrastructure.Services;

public sealed class ServiceCodeResolver : IServiceCodeResolver
{
	private static readonly Regex SixDigitCode =
		new(@"^\d{6}$", RegexOptions.Compiled);

	private readonly IAgentTransactionGateway _agentGateway;

	public ServiceCodeResolver(IAgentTransactionGateway agentGateway)
	{
		_agentGateway = agentGateway
			?? throw new ArgumentNullException(nameof(agentGateway));
	}

	public async Task<string> ResolveAsync(
		string serviceCode,
		CancellationToken cancellationToken)
	{
		if (string.IsNullOrWhiteSpace(serviceCode))
		{
			throw new InvalidServiceCodeException(
				"Service code must be provided.");
		}

		var normalized = serviceCode.Trim();

		// Rule 1: 6-digit numeric → external lookup
		if (SixDigitCode.IsMatch(normalized))
		{
			var resolved = await _agentGateway
				.ResolveServiceTypeAsync(normalized, cancellationToken);

			if (string.IsNullOrWhiteSpace(resolved))
			{
				throw new InvalidServiceCodeException(
					$"Service type not found for code '{normalized}'.");
			}

			return resolved;
		}

		// Rule 2: already a canonical service type
		return normalized;
	}
}
