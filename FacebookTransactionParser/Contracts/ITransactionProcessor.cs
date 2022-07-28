namespace FacebookTransactionParser.Contracts
{
    public interface ITransactionProcessor
    {
        public Dictionary<string, decimal> GetStatementSummary();

        public void ProcessOrderSummary();
    }
}