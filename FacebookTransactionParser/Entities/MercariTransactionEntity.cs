using CsvHelper.Configuration.Attributes;
using FacebookTransactionParser.Contracts;
using System.Globalization;

namespace FacebookTransactionParser.Entities
{
    internal class MercariTransactionEntity : ITransactionEntity
    {
        [Name("Item Id")]
        public string? Id {get; set; }

        [Name("Sold Date")]
        public string? Date { get; set; }

        [Name("Canceled Date")]
        public string? CanceledDate { get; set; }

        [Name("Completed Date")]
        public string? CompletedDate { get; set; }

        [Name("Item Title")]
        public string? ItemTitle { get; set; }

        [Name("Order Status")]
        public string? OrderStatus { get; set; }

        [Name("Shipped to State")]
        public string? FromState { get; set; }

        [Name("Shipped from State")]
        public string? ToState { get; set; }

        [Name("Item Price")]
        public string? Price { get; set; }

        [Name("Buyer Shipping Fee")]
        public string? BuyerShippingFee { get; set; }

        [Name("Seller Shipping Fee")]
        public string? ShippingCost { get; set; }

        [Name("Mercari Selling Fee")]
        public string? VendorFee { get; set; }

        [Name("Payment Processing Fee")]
        public string? PaymentProcessingFee { get; set; }

        [Name("Shipping Adjustment Fee")]
        public string? ShippingAdjustmentFee { get; set; }

        [Name("Net Seller Proceeds")]
        public string? Revenue { get; set; }

        [Name("Sales Tax Charged to Buyer")]
        public string? Tax { get; set; }

        [Name("Merchant Fees Charged to Buyer")]
        public string? MerchantFeesForBuyer { get; set; }

        public decimal GetVendorFeeFromPrice(string price)
        {
            if (decimal.TryParse(this.VendorFee, NumberStyles.Currency, CultureInfo.CurrentCulture, out var parsedPrice))
            {
                return parsedPrice;
            }

            return 0;
        }
    }
}
