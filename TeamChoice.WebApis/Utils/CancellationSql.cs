namespace TeamChoice.WebApis.Utils
{
    public static class CancellationSql
    {
        public const string CALL_PROCEDURE = @"
            EXEC iSP_Cancel_Receive
                @refno = @refno,
                @reason = @reason,
                @agtcode = @agtcode,
                @subcode = @subcode,
                @loccode = @loccode,
                @rqstuserid = @rqstuserid,
                @rqstdate = @rqstdate,
                @agtaprvduser = @agtaprvduser,
                @agtaprvddate = @agtaprvddate,
                @smtaprvduser = @smtaprvduser,
                @smtaprvddate = @smtaprvddate,
                @refundrate = @refundrate,
                @recagtcode = @recagtcode,
                @refundamt = @refundamt,
                @refundackflg = @refundackflg,
                @refundackuser = @refundackuser,
                @refundackdate = @refundackdate,
                @refundfrom = @refundfrom,
                @module = @module,
                @rateoption = @rateoption,
                @commoption = @commoption,
                @commdesc = @commdesc,
                @trnsstatus = @trnsstatus,
                @trnssubstatus = @trnssubstatus,
                @agenttype = @agenttype,
                @action = @action,
                @bmapruser = @bmapruser,
                @errorsource = @errorsource,
                @trnssndmode = @trnssndmode
        ";
    }
}