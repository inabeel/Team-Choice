using TeamChoice.WebApis.Contracts.Exchanges;
using TeamChoice.WebApis.Domain.Models;
using TeamChoice.WebApis.Utils;

namespace TeamChoice.WebApis.Application.Mappers
{
    public interface ITransactionPayloadMapper
    {
        TransactionRequest ToTransactionRequest(
            TransactionRequestDTOCopy requestDTO,
            decimal share,
            string trnsCode,
            decimal smtComm,
            OutboundProviderCredential outbound,
            string callbackUrl
        );
    }
    public class TransactionPayloadMapperImpl : ITransactionPayloadMapper
    {
        private readonly ILogger<TransactionPayloadMapperImpl> _logger;

        public TransactionPayloadMapperImpl(ILogger<TransactionPayloadMapperImpl> logger)
        {
            _logger = logger;
        }

        public TransactionRequest ToTransactionRequest(
            TransactionRequestDTOCopy requestDTO,
            decimal share,
            string trnsCode,
            decimal smtComm,
            OutboundProviderCredential outbound,
            string callbackUrl)
        {
            // Delegating to static mapper helper as per Java implementation logic
            // Assuming SendAndTransactionMapper.ToTransactionRequest exists or logic is similar
            return SendAndTransactionMapper.ToTransactionRequest(requestDTO, share, trnsCode, smtComm, outbound, callbackUrl);
        }
    }
}
