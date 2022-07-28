namespace FacebookTransactionParser
{
    using System.Globalization;
    using FacebookTransactionParser.Contracts;
    using FacebookTransactionParser.Entities;

    public class StatementProcessor : ITransactionProcessor
    {
        private readonly StatementEntity unprocessedEntity;

        public StatementProcessor(StatementEntity unprocessedEntity)
        {
            this.unprocessedEntity = unprocessedEntity;
        }

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

        public void ProcessOrderSummary()
        {
            var transactionList = this.unprocessedEntity.GetTransactions();

            foreach (var transaction in transactionList)
            {
                // convert values to decimals
                var revenue = ConvertPriceToDecimal(transaction.Price);
                var tax = ConvertPriceToDecimal(transaction.Tax);
                var shipping = ConvertPriceToDecimal(transaction.ShippingCost);

                // Calculate seller fee
                var fee = CalculateFacebookFee(revenue + tax + shipping);

                // Update prices
                this.TotalRevenue += revenue;
                this.TotalTaxPrice += tax;
                this.TotalShippingCost += shipping;
                this.TotalSellerFee += fee;
            }

            // Set total profit after iterating through each transaction.
            this.TotalProfit = this.TotalRevenue - this.TotalSellerFee;
        }

        // Calculates Facebook's selling fee for a single transaction's price.
        // As of 2022, this is $0.40 for sales $8.00 or less. For sales over $8, this is a 5% fee.
        // Total price should be a decimal representing the sum of an item's sale price, shipping price, and tax.
        private static decimal CalculateFacebookFee(decimal totalPrice)
        {
            if (totalPrice <= 8)
            {
                return 0.40M;
            }

            return totalPrice * 0.10M;
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