using TeamChoice.WebApis.Contracts;
using TeamChoice.WebApis.Contracts.DTOs;

namespace TeamChoice.WebApis.Application.Interfaces.Services;

public interface ITplusLookupService
{
    Task<ServiceLookupResponse> LookupAsync(AccountsLookupRequest request);
}