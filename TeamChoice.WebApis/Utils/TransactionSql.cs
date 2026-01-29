namespace TeamChoice.WebApis.Utils;

/// <summary>
/// Centralized SQL / Stored Procedure names for transaction-related operations.
/// Names reflect intent, not implementation.
/// </summary>
public static class TransactionSql
{
    // ===============================
    // Transaction validation / lookup
    // ===============================

    /// <summary>
    /// Checks whether a transaction already exists for a given partner reference.
    /// Java equivalent: agentTransactionFacade.validateTransaction(...)
    /// </summary>
    public const string VALIDATE_DUPLICATE_TRANSACTION =
        "sp_validate_transaction";

    /// <summary>
    /// Retrieves a transaction by its internal reference.
    /// </summary>
    public const string GET_TRANSACTION_BY_REFERENCE =
        "sp_get_transaction_by_reference";

    // ===============================
    // Transaction persistence
    // ===============================

    /// <summary>
    /// Inserts a new transaction record.
    /// </summary>
    public const string INSERT_TRANSACTION =
        "sp_insert_transaction";

    /// <summary>
    /// Updates the status of an existing transaction.
    /// </summary>
    public const string UPDATE_TRANSACTION_STATUS =
        "sp_update_transaction_status";

    // ===============================
    // Service / location resolution
    // ===============================

    /// <summary>
    /// Resolves service type using bank service code.
    /// Java equivalent: findServiceCodeUsingBankServiceType(...)
    /// </summary>
    public const string RESOLVE_SERVICE_TYPE =
        "sp_resolve_service_type";

    /// <summary>
    /// Resolves recipient location code.
    /// Java equivalent: getRecLocCode(...)
    /// </summary>
    public const string RESOLVE_RECIPIENT_LOCATION =
        "sp_get_recipient_location_code";

    public const string VALIDATE_TRANSACTION_STATUS = """
        SELECT 
            D.DESCRIPTION AS TrnsStatus
        FROM sMT_TRANSACTION T
        INNER JOIN sMT_STATUS_DESCRIPTION D
            ON D.STATUS + D.SUBSTATUS = T.TrnsStatus + T.TrnsSubStatus
        WHERE T.TrnsCode = @trnsCode
        """;
}
