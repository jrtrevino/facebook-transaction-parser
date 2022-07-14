namespace FacebookTransactionParser
{
    using System.Globalization;
    using CsvHelper;
    using FacebookTransactionParser.Contracts;
    using FacebookTransactionParser.Entities;

    public class TransactionParser
    {
        private readonly ILogger logger;

        public TransactionParser(ILogger logger)
        {
            this.logger = logger;
        }

        public OrderSummaryEntity? ParseTransactionFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                return null;
            }

            this.logger.LogInfo($"File path received: {filePath}");

            var entities = this.Parse(filePath);

            if (entities == null || !entities.Any())
            {
                return null;
            }

            return new OrderSummaryEntity(Path.GetFileName(filePath), entities);
        }

        private IEnumerable<TransactionEntity>? Parse(string filePath)
        {
            this.logger.LogInfo($"Begin parsing of file: {filePath}");
            try
            {
                var reader = new StreamReader(filePath);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                var records = csv.GetRecords<TransactionEntity>();
                return records.ToList();
            }
            catch (Exception e)
            {
                this.logger.LogError($"Error reading file. Reason: {e.Message}");
            }

            return null;
        }
    }
}