namespace FacebookTransactionParser.Entities
{
    public class StatementEntity
    {
        private readonly string fileName;

        private readonly IEnumerable<TransactionEntity> transactionEntities;

        public StatementEntity(string fileName, IEnumerable<TransactionEntity> transactionEntities)
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