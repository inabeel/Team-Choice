using TeamChoice.WebApis.Contracts.DTOs;
using TeamChoice.WebApis.Contracts.Exchanges;
using TeamChoice.WebApis.Domain.Models;

namespace TeamChoice.WebApis.Application.Mappers;

public static class TransactionPayloadMapper
{
    public static TransactionRequest ToTransactionRequest(TransactionRequestDTOCopy dto, decimal rcvComm, string trnsCode, decimal smtComm, OutboundProviderCredential cred, string callbackUrl)
    {
        return new TransactionRequest();
    }
}

public static class TransactionUtil
{
    public static bool ValidateSenderLocation(string locId) => true;
}

public static class TransactionMapperUtil
{
    public static Transaction ToTransaction(TransactionRequestDto dto) => new Transaction();
}

public static class TransactionSmtMapperUtil
{
    public static SmtTransaction ToSmtTransaction(Transaction txn) => new SmtTransaction();
}

public static class RemittanceMapperUtil
{
    public static RemittanceRequest ToRemittanceRequest(SmtTransaction txn) => new RemittanceRequest();
}

