namespace FacebookTransactionParser.Entities
{
    public class OrderSummaryEntity
    {
        private readonly string fileName;

        private readonly IEnumerable<TransactionEntity> transactionEntities;

        public OrderSummaryEntity(string fileName, IEnumerable<TransactionEntity> transactionEntities)
        {
            this.fileName = fileName;
            this.transactionEntities = transactionEntities;
        }

        public IEnumerable<TransactionEntity> GetTransactions()
        {
            return this.transactionEntities.ToList();
        }

        public string GetFileName()
        {
            return this.fileName;
        }
    }
}