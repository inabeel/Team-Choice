using TeamChoice.WebApis.Contracts.Exchanges;
using TeamChoice.WebApis.Domain.Configuration;

namespace TeamChoice.WebApis.Application.Services
{
    public interface ISendTeamsMessage
    {
        Task<string> SendTeamMessageAsync(TeamsNotificationRequest sendTeamsMessage);
    }

    public class SendTeamsMessage : ISendTeamsMessage
    {
        private readonly HttpClient _httpClient;
        private readonly ClientUrlProperties _clientUrlProperties;
        private readonly ILogger<SendTeamsMessage> _logger;

        public SendTeamsMessage(
            HttpClient httpClient,
            ClientUrlProperties clientUrlProperties,
            ILogger<SendTeamsMessage> logger)
        {
            _httpClient = httpClient;
            _clientUrlProperties = clientUrlProperties;
            _logger = logger;
        }

        public async Task<string> SendTeamMessageAsync(TeamsNotificationRequest sendTeamsMessage)
        {
            _logger.LogInformation("🚀 Initiating transaction forwarding for transactionCode={Desc} and partnerRef={Title}",
                sendTeamsMessage.Description, sendTeamsMessage.Title);

            // Matches logic: clientUrlProperties.getTeamMessage()
            string teamsNotificationUrl = _clientUrlProperties.TeamMessage;

            try
            {
                var response = await _httpClient.PostAsJsonAsync(teamsNotificationUrl, sendTeamsMessage);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("✅ Teams notification sent successfully");
                    return "Teams notification sent successfully";
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("❌ Failed to send Teams notification: {Status} {Content}", response.StatusCode, errorContent);
                    // Throwing to catch below and wrap exception, matching Java's onErrorResume behavior somewhat
                    throw new HttpRequestException($"Request failed with status {response.StatusCode}");
                }
            }
            catch (Exception e)
            {
                // Matches onErrorResume logic
                _logger.LogError(e, "❌ Failed to send Teams notification: {Message}", e.Message);
                throw new Exception("Failed to send Teams notification: " + e.Message, e);
            }
        }
    }
}
