using TeamChoice.WebApis.Application.Interfaces.Services;
using TeamChoice.WebApis.Contracts.Exchanges;
using TeamChoice.WebApis.Domain.Configuration;
using TeamChoice.WebApis.Domain.Constants;
using TeamChoice.WebApis.Infrastructure.Persistence;

namespace TeamChoice.WebApis.Application.Services
{
    public interface IRemittanceService
    {
        Task<RemittanceResultDTO> SendRemittanceAsync(RemittanceRequest req);
    }

    public class RemittanceService : IRemittanceService
    {
        private readonly IDatabaseService _databaseService;
        private readonly ISendTeamsMessage _teamsNotifier;
        private readonly ClientUrlProperties _clientUrlProperties;
        private readonly IAgentTransactionFacade _agentTransactionFacade;
        private readonly ILogger<RemittanceService> _logger;

        public RemittanceService(
            IDatabaseService databaseService,
            ISendTeamsMessage teamsNotifier,
            ClientUrlProperties clientUrlProperties,
            IAgentTransactionFacade agentTransactionFacade,
            ILogger<RemittanceService> logger)
        {
            _databaseService = databaseService;
            _teamsNotifier = teamsNotifier;
            _clientUrlProperties = clientUrlProperties;
            _agentTransactionFacade = agentTransactionFacade;
            _logger = logger;
        }

        public async Task<RemittanceResultDTO> SendRemittanceAsync(RemittanceRequest req)
        {
            req.Id = Guid.NewGuid().ToString();

            _logger.LogInformation("📤 Sending remittance request with ID {Id}", req.Id);

            string recLoc;
            try
            {
                // Matches logic: agentTransactionFacade.getRecLocCode(...)
                recLoc = await _agentTransactionFacade.GetRecLocCodeAsync(req.RecLocCode, req.TrnsSrvCode);
                _logger.LogInformation("🔍 Retrieved recipient location code: {RecLoc}", recLoc);
            }
            catch (Exception e)
            {
                // Matches outer onErrorResume
                throw new InvalidOperationException("❌ Error while finding service code" + e.Message, e);
            }

            try
            {
                // Matches logic: "2".equals(req.getTrnsSrvCode()) ? "00003" : req.getTrnsSrvCode()
                var resolvedTrnsSrvCode = "2".Equals(req.TrnsSrvCode) ? "00003" : req.TrnsSrvCode;

                // Matches logic: recLoc.substring(0, 3)
                var recAgtCode = !string.IsNullOrEmpty(recLoc) && recLoc.Length >= 3
                    ? recLoc.Substring(0, 3)
                    : recLoc;

                var parameters = new Dictionary<string, object>
                {
                    { "@Id", req.Id },
                    { "@AgtRefNo", req.AgtRefNo },
                    { "@TrnsSrvType", req.TrnsSrvType },
                    { "@TrnsSrvCode", resolvedTrnsSrvCode },
                    { "@SndAgtCode", _clientUrlProperties.SndAgtCode },
                    { "@SndLocCode", _clientUrlProperties.Loccode },
                    { "@RecAgtCode", recAgtCode },
                    { "@RecLocCode", recLoc },
                    { "@RecCurCde", RemittanceConstants.CURRENCY_CODE },
                    { "@RecConCode", RemittanceConstants.REC_COUNTRY_CODE },
                    { "@RemFirstName", req.RemFirstName },
                    { "@RemMiddleName", req.RemMiddleName },
                    { "@RemLastName", req.RemLastName },
                    { "@RemConCode", RemittanceConstants.REM_COUNTRY_CODE },
                    { "@RemNatCode", req.RemNatCode },
                    { "@RemPhone", req.RemPhone?.Replace("+", "") },
                    { "@RemMobile", req.RemMobile?.Replace("+", "") },
                    { "@RemAddr1", req.RemcityText }, // Mapped from RemcityText per Java code
                    { "@RemDOB", req.RemDob },
                    { "@RemcityText", req.RemcityText },
                    { "@BenFirstName", req.BenFirstName },
                    { "@BenMiddleName", req.BenMiddleName },
                    { "@BenLastName", req.BenLastName },
                    { "@BenConCode", RemittanceConstants.BEN_COUNTRY_CODE },
                    { "@BenNatCode", req.BenNatCode },
                    { "@BenPhone", req.BenMobile?.Replace("+", "") }, // Mapped from BenMobile per Java code
                    { "@BenMobile", req.BenMobile?.Replace("+", "") },
                    { "@PayMode", req.PayMode },
                    { "@FXAmount", req.FxAmount },
                    { "@LCYAmount", req.FxAmount }, // Same as FX per Java context
                    { "@TrnsComm", req.TrnsComm },
                    { "@LCYTotAmount", RemittanceConstants.TOTAL_AMOUNT },
                    { "@TrnsRate", RemittanceConstants.RATE },
                    { "@TrnsRateDiv", RemittanceConstants.RATE_DIV },
                    { "@TrnsUser", _clientUrlProperties.UserCred },
                    { "@receiptno", RemittanceConstants.DEFAULT_RECEIPT_NO },
                    { "@errorsource", RemittanceConstants.ERROR_SOURCE },
                    { "@PaymentMode", req.PaymentMode }
                };

                // Execute Query and Map Result
                return await _databaseService.QueryOneAsync(
                    Utils.RemittanceSql.CALL_PROCEDURE,
                    parameters,
                    reader => new RemittanceResultDTO
                    {
                        ProfileId = reader["ProfileID"]?.ToString(),
                        ReceiptNo = reader["ReceiptNo"]?.ToString(),
                        Id = reader["ID"]?.ToString(),
                        BenfCode = reader["Benfcode"]?.ToString(),
                        Crlmt = reader["crlmt"] != DBNull.Value ? Convert.ToDecimal(reader["crlmt"]) : 0,
                        TrnsPin = reader["trnspin"]?.ToString(),
                        Reference = reader["Reference"]?.ToString(),
                        CustCode = reader["CustCode"]?.ToString()
                    }
                );
            }
            catch (Exception e)
            {
                // Matches inner doOnError logic
                _logger.LogError(e, "❌ Failed to send remittance: {Message}", e.Message);

                // Fire and forget Teams notification
                _ = _teamsNotifier.SendTeamMessageAsync(GetTeamsNotificationRequest());

                // Matches inner onErrorResume logic: Return DTO with error reference
                return new RemittanceResultDTO
                {
                    Reference = "Error: " + e.Message
                };
            }
        }

        private TeamsNotificationRequest GetTeamsNotificationRequest()
        {
            return new TeamsNotificationRequest
            {
                Title = "500 Internal Server Error - " + _clientUrlProperties.Loccode,
                Description = "System Monitor - store procedure issue",
                ProfileImageUrl = "https://cdn-icons-png.flaticon.com/512/847/847969.png",
                CreatedUtc = DateTime.UtcNow.ToString("EEE d MMMM yyyy"),
                ViewUrl = "https://logs.tawakal.net/transactions/" + "null",
                Properties = new List<Dictionary<string, string>>
                {
                    new Dictionary<string, string> { { "key", "Transaction Code" }, { "value", "null" } },
                    new Dictionary<string, string> { { "key", "Status" }, { "value", "Error" } },
                    new Dictionary<string, string> { { "key", "Environment" }, { "value", "Production" } }
                }
            };
        }
    }
}
