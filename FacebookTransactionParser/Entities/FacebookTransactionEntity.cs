namespace FacebookTransactionParser.Entities
{
    using CsvHelper.Configuration.Attributes;
    using FacebookTransactionParser.Contracts;
    using System.Globalization;

    internal class FacebookTransactionEntity : ITransactionEntity
    {
        [Name("Order ID")]
        public string? Id { get; set; }

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

        [Name("Order status")]
        public string? OrderStatus { get; set; }

        public decimal GetVendorFeeFromPrice(string price)
        {
            if (decimal.TryParse(price, NumberStyles.Currency, CultureInfo.CurrentCulture, out var parsedPrice))
            {
                if (parsedPrice <= 8)
                {
                    return 0.40M;
                }

                return parsedPrice * 0.10M;
            }

            return 0;
        }
    }
}