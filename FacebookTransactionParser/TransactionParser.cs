namespace FacebookTransactionParser
{
    using FacebookTransactionParser.Entities;

    public static class TransactionParser
    {
        public static IEnumerable<TransactionEntity>? ParseTransactionFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return null;
            }

            if (!File.Exists(filePath))
            {
                return null;
            }

            var entities = Parse(filePath);

            return entities;
        }

        private static IEnumerable<TransactionEntity>? Parse(string filePath)
        {
            return null;
        }

        private static bool CheckFileHeader(string header)
        {
            return false;
        }
    }
}