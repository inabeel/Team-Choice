
using TeamChoice.WebApis.Models.DTOs;
using TeamChoice.WebApis.Utils;

namespace TeamChoice.WebApis.Application.Services
{
    public interface ICancellationService
    {
        Task<RemittanceResultDTO> CancelTransactionAsync(CancelReceiveRequest req);
    }

    public class CancellationService : ICancellationService
    {
        private readonly IDatabaseService _databaseService;
        private readonly ILogger<CancellationService> _logger;

        public CancellationService(IDatabaseService databaseService, ILogger<CancellationService> logger)
        {
            _databaseService = databaseService;
            _logger = logger;
        }

        public async Task<RemittanceResultDTO> CancelTransactionAsync(CancelReceiveRequest req)
        {
            _logger.LogInformation("Cancelling transaction with refno: {Refno}", req.Refno);

            // Map all properties to SQL parameters
            var parameters = new Dictionary<string, object>
            {
                { "@refno", req.Refno },
                { "@reason", req.Reason },
                { "@agtcode", req.Agtcode },
                { "@subcode", req.Subcode },
                { "@loccode", req.Loccode },
                { "@rqstuserid", req.Rqstuserid },
                { "@rqstdate", req.Rqstdate },
                { "@agtaprvduser", req.Agtaprvduser },
                { "@agtaprvddate", req.Agtaprvddate },
                { "@smtaprvduser", req.Smtaprvduser },
                { "@smtaprvddate", req.Smtaprvddate },
                { "@refundrate", req.Refundrate },
                { "@recagtcode", req.Recagtcode },
                { "@refundamt", req.Refundamt },
                { "@refundackflg", req.Refundackflg },
                { "@refundackuser", req.Refundackuser },
                { "@refundackdate", req.Refundackdate },
                { "@refundfrom", req.Refundfrom },
                { "@module", req.Module },
                { "@rateoption", req.Rateoption },
                { "@commoption", req.Commoption },
                { "@commdesc", req.Commdesc },
                { "@trnsstatus", req.Trnsstatus },
                { "@trnssubstatus", req.Trnssubstatus },
                { "@agenttype", req.Agenttype },
                { "@action", req.Action },
                { "@bmapruser", req.Bmapruser },
                { "@errorsource", req.Errorsource },
                { "@trnssndmode", req.Trnssndmode }
            };

            try
            {
                // Execute Stored Procedure
                // Java implementation maps the result to a new empty DTO
                var result = await _databaseService.ExecuteStoredProcedureAsync(
                    CancellationSql.CALL_PROCEDURE,
                    parameters,
                    reader => new RemittanceResultDTO() // Mapping logic mirroring Java's empty object creation
                );

                return result ?? new RemittanceResultDTO();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to send remittance: {Message}", e.Message);
                return new RemittanceResultDTO
                {
                    Reference = "Error: " + e.Message
                };
            }
        }
    }
}
