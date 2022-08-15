namespace FacebookTransactionParser
{
    using System.Globalization;
    using CsvHelper;
    using FacebookTransactionParser.Entities;
    using Microsoft.Extensions.Logging;

    public class TransactionParser
    {
        private readonly ILogger logger;

        public TransactionParser(ILogger logger)
        {
            this.logger = logger;
        }

        public StatementEntity? ParseTransactionFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                this.logger.LogError($"The provided filePath does not exist: {filePath}");
                return null;
            }

            this.logger.LogInformation($"File path received: {filePath}");

            var entities = this.GetRecordsFromFile(filePath);

            if (entities == null || !entities.Any())
            {
                this.logger.LogInformation($"No records were parsed from the file.");
                return null;
            }

            return new StatementEntity(Path.GetFileName(filePath), entities);
        }

        private IEnumerable<TransactionEntity>? GetRecordsFromFile(string filePath)
        {
            this.logger.LogInformation($"Begin parsing of file: {filePath}");
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