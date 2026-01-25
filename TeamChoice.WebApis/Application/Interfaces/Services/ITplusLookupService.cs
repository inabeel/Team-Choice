using TeamChoice.WebApis.Domain.Models;
using TeamChoice.WebApis.Domain.Models.DTOs;

namespace TeamChoice.WebApis.Application.Interfaces.Services;

public interface ITplusLookupService
{
    Task<ServiceLookupResponse> LookupAsync(AccountsLookupRequest request);
}