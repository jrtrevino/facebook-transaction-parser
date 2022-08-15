namespace FacebookTransactionParser.Entities
{
    using FacebookTransactionParser.Contracts;

    public class StatementEntity
    {
        private readonly string fileName;

        private readonly IEnumerable<ITransactionEntity> transactionEntities;

        internal StatementEntity(string fileName, IEnumerable<ITransactionEntity> transactionEntities)
        {
            this.fileName = fileName;
            this.transactionEntities = transactionEntities;
        }

        internal IEnumerable<ITransactionEntity> GetTransactions()
        {
            return this.transactionEntities.ToList();
        }

        internal string GetFileName()
        {
            return this.fileName;
        }
    }
}