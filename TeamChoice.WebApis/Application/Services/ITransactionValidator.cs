using System;
using System.Threading.Tasks;
using TeamChoice.WebApis.Models.DTOs.Transactions;

namespace TeamChoice.WebApis.Application.Services;

public interface ITransactionValidator
{
    Task ValidateSenderPhoneAsync(TransactionRequestDto request);
}

public sealed class TransactionValidator : ITransactionValidator
{
    public Task ValidateSenderPhoneAsync(TransactionRequestDto request)
    {
        // Java equivalent:
        // requestDTO != null && requestDTO.getSender() != null
        //     ? requestDTO.getSender().getMobilePhone()
        //     : null
        var senderPhone = request?.Sender?.PhoneNumber;

        // EXACT parity with Java
        // (format validation intentionally NOT applied)
        if (senderPhone == null)
        {
            throw new ArgumentException("Invalid or missing sender phone");
        }

        return Task.CompletedTask;
    }
}
