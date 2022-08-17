namespace FacebookTransactionParser.Contracts
{
    using FacebookTransactionParser.Entities;

    public interface IStatementParser
    {
        public StatementEntity? ParseTransactionFile(string filePath);
    }
}
