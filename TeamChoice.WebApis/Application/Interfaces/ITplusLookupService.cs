using TeamChoice.WebApis.Domain.Models;
using TeamChoice.WebApis.Domain.Models.DTOs;

namespace TeamChoice.WebApis.Application.Interfaces;

public interface ITplusLookupService
{
    Task<ServiceLookupResponse> LookupAsync(AccountsLookupRequest request);
}