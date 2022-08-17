namespace FacebookTransactionParser.Implementations
{
    using System.Globalization;
    using FacebookTransactionParser.Contracts;
    using FacebookTransactionParser.Entities;

    public class StatementProcessor : IStatementProcessor
    {
        private decimal TotalRevenue { get; set; } = 0;

        private decimal TotalShippingCost { get; set; } = 0;

        private decimal TotalTaxPrice { get; set; } = 0;

        private decimal TotalProfit { get; set; } = 0;

        private decimal TotalSellerFee { get; set; } = 0;

        public Dictionary<string, decimal> GetStatementSummary()
        {
            return new Dictionary<string, decimal>
        {
            { "revenue", this.TotalRevenue },
            { "profit", this.TotalProfit },
            { "tax", this.TotalTaxPrice },
            { "shipping", this.TotalShippingCost },
            { "fee", this.TotalSellerFee },
        };
        }

        public void ProcessOrderSummary(StatementEntity unprocessedEntity)
        {
            var transactionList = unprocessedEntity.GetTransactions();

            foreach (var transaction in transactionList)
            {
                // convert values to decimals
                var revenue = ConvertPriceToDecimal(transaction.Price);
                var tax = ConvertPriceToDecimal(transaction.Tax);
                var shipping = ConvertPriceToDecimal(transaction.ShippingCost);
                var combinedPrice = revenue + tax + shipping;

                // Calculate seller fee
                var fee = transaction.GetVendorFeeFromPrice(combinedPrice.ToString());

                // Update prices
                this.TotalRevenue += revenue;
                this.TotalTaxPrice += tax;
                this.TotalShippingCost += shipping;
                this.TotalSellerFee += fee;
            }

            // Set total profit after iterating through each transaction.
            this.TotalProfit = this.TotalRevenue - this.TotalSellerFee;
        }

        // Converts a string representation of a currency price to a decimal.
        private static decimal ConvertPriceToDecimal(string? price)
        {
            if (string.IsNullOrEmpty(price))
            {
                return 0;
            }

            try
            {
                return decimal.Parse(price, NumberStyles.Currency);
            }
            catch
            {
                return 0;
            }
        }
    }
}