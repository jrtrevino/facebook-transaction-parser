namespace FacebookTransactionParser.Entities
{
    public class OrderSummaryEntity
    {
        public OrderSummaryEntity(string filePath, IEnumerable<TransactionEntity> transactions)
        {
            this.Filename = filePath;
            this.TransactionEntities = transactions;
        }

        public string Filename { get; }

        public IEnumerable<TransactionEntity> TransactionEntities { get; }
    }
}