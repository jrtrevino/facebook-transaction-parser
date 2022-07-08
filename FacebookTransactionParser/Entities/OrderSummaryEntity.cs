namespace FacebookTransactionParser.Entities
{
    public class OrderSummaryEntity
    {
        public string? Filename { get; set; }

        public IEnumerable<TransactionEntity>? TransactionEntities { get; set; }
    }
}