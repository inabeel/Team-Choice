
using System.Globalization;
using TeamChoice.WebApis.Application.Commands.Transactions;
using TeamChoice.WebApis.Contracts.DTOs.Transactions;
using TeamChoice.WebApis.Contracts.Exchanges;

namespace TeamChoice.WebApis.Application.Mappers;

/// <summary>
/// Maps CancelTransactionDto into CancelReceiveRequest
/// used by the cancellation stored procedure.
/// </summary>
public static class CancelRequestMapper
{
    // Defaults (mirrors Java exactly)
    private const string DEFAULT_SUB_CODE = "0";
    private const string DEFAULT_USER = "caneast";
    private const string DEFAULT_REASON = "without com";
    private const string DEFAULT_TRNS_STATUS = "CAN";
    private const string DEFAULT_TRNS_SUB_STATUS = "C";
    private const string DEFAULT_AGENT_TYPE = "SMT";
    private const string DEFAULT_ACTION = "1";
    private const string DEFAULT_COMM_OPTION = "W";

    public static CancelReceiveRequest Map(CancelTransactionCommand dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var now = CurrentDateTime();

        return new CancelReceiveRequest
        {
            Refno = dto.TransactionReference,
            Agtcode = dto.LocationCode,
            Reason = dto.Reason,

            Subcode = DEFAULT_SUB_CODE,
            Loccode = string.Empty,

            Rqstuserid = DEFAULT_USER,
            Rqstdate = DateTime.UtcNow,

            Agtaprvduser = string.Empty,
            Agtaprvddate = DateTime.UtcNow,

            Smtaprvduser = string.Empty,
            Smtaprvddate = DateTime.UtcNow,

            Refundrate = 0,
            Recagtcode = string.Empty,
            Refundamt = 0,
            Refundackflg = string.Empty,
            Refundackuser = string.Empty,
            Refundackdate = DateTime.UtcNow,
            Refundfrom = string.Empty,

            Module = string.Empty,
            Rateoption = string.Empty,

            Commoption = DEFAULT_COMM_OPTION,
            Commdesc = DEFAULT_REASON,

            Trnsstatus = DEFAULT_TRNS_STATUS,
            Trnssubstatus = DEFAULT_TRNS_SUB_STATUS,

            Agenttype = DEFAULT_AGENT_TYPE,
            Action = DEFAULT_ACTION,

            Bmapruser = string.Empty,
            Errorsource = string.Empty,
            Trnssndmode = string.Empty
        };
    }

    private static string CurrentDateTime()
        => DateTime.Now.ToString(
            "yyyy-MM-dd HH:mm:ss",
            CultureInfo.InvariantCulture);
}
