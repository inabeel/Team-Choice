using TeamChoice.WebApis.Contracts.DTOs;
using TeamChoice.WebApis.Contracts.Exchanges;

namespace TeamChoice.WebApis.Application.Interfaces.Repositories;

public interface IRateRepository
{
    Task<CommissionResultDTO> CalculateExternalPartnerCommissionAsync(ExchangePayloadDto exchangePayload);
}