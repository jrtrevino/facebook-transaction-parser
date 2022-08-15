using FacebookTransactionParser.Entities;

namespace FacebookTransactionParserTests.Stubs
{
    internal static class StatementEntityStub
    {
        public static IEnumerable<FacebookTransactionEntity> _transactions = new List<FacebookTransactionEntity>()
        {
            new FacebookTransactionEntity()
            {
                Buyer = "Joey Trevino",
                City = "San Diego",
                Date = "2022-07-30",
                Price = "$3.50",
                Tax = "$0.40",
                ShippingCost = "$2.00",
            },
             new FacebookTransactionEntity()
            {
                Buyer = "Jonny Depp",
                City = "San Diego",
                Date = "2022-07-30",
                Price = "$10.50",
                Tax = "$2.80",
                ShippingCost = "$2.00",
            },
             new FacebookTransactionEntity()
            {
                Buyer = "Joe Biden",
                City = "Washington, D.C.",
                Date = "2022-07-30",
                Price = "$680.21",
                Tax = "$72.32",
                ShippingCost = "$2.00",

            },
        };

        public static StatementEntity ValidEntity = new("validFilename.txt", _transactions);
    }
}
