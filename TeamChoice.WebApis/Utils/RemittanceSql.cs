namespace TeamChoice.WebApis.Utils
{
    public static class RemittanceSql
    {
        public const string CALL_PROCEDURE = @"
            EXEC SmartInboundService
                @Id = @Id,
                @AgtRefNo = @AgtRefNo,
                @TrnsSrvType = @TrnsSrvType,
                @TrnsSrvCode = @TrnsSrvCode,
                @SndAgtCode = @SndAgtCode,
                @SndLocCode = @SndLocCode,
                @RecAgtCode = @RecAgtCode,
                @RecLocCode = @RecLocCode,
                @RecCurCde = @RecCurCde,
                @RecConCode = @RecConCode,
                @RemFirstName = @RemFirstName,
                @RemMiddleName = @RemMiddleName,
                @RemLastName = @RemLastName,
                @RemConCode = @RemConCode,
                @RemNatCode = @RemNatCode,
                @RemPhone = @RemPhone,
                @RemMobile = @RemMobile,
                @RemAddr1 = @RemAddr1,
                @RemDOB = @RemDOB,
                @RemcityText = @RemcityText,
                @BenFirstName = @BenFirstName,
                @BenMiddleName = @BenMiddleName,
                @BenLastName = @BenLastName,
                @BenConCode = @BenConCode,
                @BenNatCode = @BenNatCode,
                @BenPhone = @BenPhone,
                @BenMobile = @BenMobile,
                @PayMode = @PayMode,
                @FXAmount = @FXAmount,
                @LCYAmount = @LCYAmount,
                @TrnsComm = @TrnsComm,
                @LCYTotAmount = @LCYTotAmount,
                @TrnsRate = @TrnsRate,
                @TrnsRateDiv = @TrnsRateDiv,
                @TrnsUser = @TrnsUser,
                @receiptno = @receiptno,
                @errorsource = @errorsource,
                @PaymentMode = @PaymentMode
            ";
    }
}