using TeamChoice.WebApis.Domain.Models.DTOs;
using TeamChoice.WebApis.Domain.Models.DTOs.Exchanges;

namespace TeamChoice.WebApis.Application.Interfaces.Repositories;

public interface IRateRepository
{
    Task<CommissionResultDTO> CalculateExternalPartnerCommissionAsync(ExchangeRatePayloadDto exchangePayload);
}