namespace FacebookTransactionParser.Contracts
{
    internal interface ITransactionEntity
    {
        public string? Id { get; set; }

        public string? Price { get; set; }

        public string? Tax { get; set; }

        public string? ShippingCost { get; set; }

        public string? OrderStatus { get; set; }

        public decimal GetVendorFeeFromPrice(string price);
    }
}
