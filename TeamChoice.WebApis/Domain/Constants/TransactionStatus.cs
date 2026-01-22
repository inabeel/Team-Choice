using System;
using System.Collections.Generic;
using TeamChoice.WebApis.Domain.Exceptions;

namespace TeamChoice.WebApis.Domain.Constants;

public static class TransactionStatus
{
    private static readonly IReadOnlyDictionary<string, string> CodeToLabelMap =
        new Dictionary<string, string>
        {
            ["3"] = "REM P",
            ["4"] = "REC C",
            ["11"] = "CAN Y"
        };

    /// <summary>
    /// Gets the transaction status label for a given code.
    /// </summary>
    /// <param name="code">Transaction status code.</param>
    /// <returns>Status label.</returns>
    /// <exception cref="InvalidTransactionStatusException">
    /// Thrown when the code is invalid.
    /// </exception>
    public static string FromCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new InvalidTransactionStatusException(code);
        }

        if (!CodeToLabelMap.TryGetValue(code, out var label))
        {
            throw new InvalidTransactionStatusException(code);
        }

        return label;
    }
}
