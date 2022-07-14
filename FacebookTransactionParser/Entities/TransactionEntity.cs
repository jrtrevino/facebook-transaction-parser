namespace FacebookTransactionParser.Entities
{
    using CsvHelper.Configuration.Attributes;
    using FacebookTransactionParser.Contracts;

    public class TransactionEntity : ITransactionEntity
    {
        [Name("Order ID")]
        public long OrderId { get; set; }

        [Name("Date")]
        public string? Date { get; set; }

        [Name("Buyer")]
        public string? Buyer { get; set; }

        [Name("Price")]
        public string? Price { get; set; }

        [Name("Tax")]
        public string? Tax { get; set; }

        [Name("Shipping")]
        public string? ShippingCost { get; set; }

        [Name("Shipping Type")]
        public string? ShippingType { get; set; }

        [Name("Products")]
        public string? Product { get; set; }

        [Name("Quantity")]
        public int Quantity { get; set; }

        [Name("Street 1")]
        public string? Street1 { get; set; }

        [Name("Street 2")]
        public string? Street2 { get; set; }

        [Name("City")]
        public string? City { get; set; }

        [Name("State")]
        public string? State { get; set; }

        [Name("ZIP Code")]
        public string? ZipCode { get; set; }

        [Name("Order Status")]
        public string? OrderStatus { get; set; }
    }
}