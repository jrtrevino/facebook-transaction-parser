namespace FacebookTransactionParser.Contracts
{
    public interface IStatementProcessor
    {
        public Dictionary<string, decimal> GetStatementSummary();

        public void ProcessOrderSummary();
    }
}