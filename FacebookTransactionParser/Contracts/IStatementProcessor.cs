namespace FacebookTransactionParser.Contracts
{
    using FacebookTransactionParser.Entities;

    public interface IStatementProcessor
    {
        public Dictionary<string, decimal> GetStatementSummary();

        public void ProcessOrderSummary(StatementEntity entity);
    }
}