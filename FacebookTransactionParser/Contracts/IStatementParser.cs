namespace FacebookTransactionParser.Contracts
{
    using FacebookTransactionParser.Entities;

    internal interface IStatementParser
    {
        public StatementEntity? ParseTransactionFile(string filePath);
    }
}
