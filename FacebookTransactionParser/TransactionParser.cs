namespace FacebookTransactionParser
{
    using System.Globalization;
    using CsvHelper;
    using FacebookTransactionParser.Entities;

    public static class TransactionParser
    {
        public static OrderSummaryEntity? ParseTransactionFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                return null;
            }

            var entities = Parse(filePath);

            if (entities == null || !entities.Any())
            {
                return null;
            }

            // Create return object
            return new OrderSummaryEntity()
            {
                Filename = Path.GetFileName(filePath),
                TransactionEntities = entities,
            };
        }

        private static IEnumerable<TransactionEntity>? Parse(string filePath)
        {
            try
            {
                var reader = new StreamReader(filePath);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                var records = csv.GetRecords<TransactionEntity>();
                return records;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error reading file. Reason: {e.Message}");
            }

            return null;
        }
    }
}