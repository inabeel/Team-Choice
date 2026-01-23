using TeamChoice.WebApis.Domain.Models.DTOs;

namespace TeamChoice.WebApis.Utils
{
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

    public static class RemittanceSql
    {
        public const string CALL_PROCEDURE = "YOUR_STORED_PROCEDURE_NAME";
    }

    public static class CancellationSql
    {
        public const string CALL_PROCEDURE = "YOUR_CANCEL_PROCEDURE_NAME";
    }
}
