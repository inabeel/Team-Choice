namespace TeamChoice.WebApis.Application.Commands.Transactions;

public sealed record CreateTransactionCommand(
    string PartnerReference,
    decimal Amount,
    string Currency,
    string ServiceCode,
    SenderInfo Sender,
    ReceiverInfo Receiver
);

public sealed record SenderInfo(
    string Name,
    string PhoneNumber,
    string LocationId
);

public sealed record ReceiverInfo(
    string Name,
    string PhoneNumber
);

public sealed record ValidateTransactionStatusCommand(
    string TransactionReference
);

public sealed record CancelTransactionCommand(
    string TransactionReference,
    string LocationCode,
    string Reason
);