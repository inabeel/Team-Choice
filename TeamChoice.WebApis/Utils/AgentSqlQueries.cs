namespace TeamChoice.WebApis.Utils
{
    public static class AgentSqlQueries
    {
        public const string FIND_RECEIVE_COMMISSION_BY_AGENT_CODE = @"
            SELECT AMOUNT
            FROM sMT_IndividualCommission
            WHERE LOC = @agtCode AND COMMTYPE = 'R'";

        public const string FIND_OUTBOUND_CREDENTIALS_BY_SERVICE_CODE = @"
            SELECT p.ProviderId, pr.ProviderName, p.ServiceCode, p.ServiceName,
                   c.Username, c.Password, c.SecretKey, c.BaseURL
            FROM PayOutServices p
            JOIN Credentials c ON p.credentials = c.credentials
            JOIN Providers pr ON pr.ProviderId = p.ProviderId
            WHERE p.ServiceCode = @serviceCode";

        public const string GET_EXCHANGE_RATE = @"
            SELECT buyRate AS etbIr, sellRate AS usd
            FROM smt_agtrate
            WHERE curCode = @curcode AND agtcode = @agtcode AND loccode = @loccode";

        public const string FIND_SMT_COMMISSION_BY_TRNS_CODE = @"
            SELECT SMT_TOTCOM
            FROM sMT_TRANSACTION_COMMISSION_RATES c
            INNER JOIN sMT_TRANSACTION t ON t.TrnsCode = c.TrnsCode
            WHERE t.TrnsCode = @agtCode"; // Parameter name matches Java usage, though query implies TrnsCode

        public const string FIND_BANK_SERVICETYPE = @"
            SELECT DeliveryTypeID
            FROM DeliveryServices WHERE ServiceCode = @serviceCode";

        public const string FIND_TRNS_CODE_FROM_TRANSACTIONS = @"
            SELECT TrnsCode FROM sMT_TRANSACTION WHERE TrnsCode = @authNumber";

        public const string VALIDATE_TRANSACTION_BY_PARTNER_REFERENCE = @"
            SELECT partnerCode FROM PartnerTxnLog WHERE partnerReference = @partnerReference";

        public const string VALIDATE_TRANSACTION_STATUS = @"
            SELECT D.DESCRIPTION TrnsStatus 
            FROM sMT_TRANSACTION T 
            INNER JOIN sMT_STATUS_DESCRIPTION D ON D.STATUS+D.SUBSTATUS = TrnsStatus+TrnsSubStatus 
            WHERE TrnsCode = @trnsCode";

        public const string INSERT_PARTNER_TRANSACTION = @"
            INSERT INTO PartnerTxnLog (partnerReference, TransactionDate, Status, Payload, partnerCode)
            VALUES (@partnerReference, @transactionDate, @status, @payload, @partnerCode)";

        public const string UPDATE_TRANSACTION_STATUS = @"
            UPDATE OE_REMITTANCE
            SET id_status = @trnsStatus
            WHERE autho_number = @agtRefNo";

        public const string GET_USER_BY_LOC_CODE = @"
            SELECT * FROM sMT_MST_MPUSER WHERE LOCCODE = @loccode";

        public const string GET_REGLOCCODE = @"
            SELECT ExternalLocId FROM Location WHERE UsageLocation = @country AND (Name = 'TAWAKALPAY' )";

        public const string GET_LOCID_BY_SERVICE_CODE = @"
            SELECT TOP (1) LocId FROM LocationService WHERE ServiceCode = @serviceCode";
    }
}
