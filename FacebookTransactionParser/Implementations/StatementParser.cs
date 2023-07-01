namespace FacebookTransactionParser.Implementations
{
    using System.Globalization;
    using CsvHelper;
    using FacebookTransactionParser.Contracts;
    using FacebookTransactionParser.Entities;
    using Serilog;

    public class StatementParser : IStatementParser
    {
        private readonly ILogger logger;

        public StatementParser(ILogger logger)
        {
            this.logger = logger;
        }

        public StatementEntity? ParseTransactionFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                this.logger.Error($"The provided filePath does not exist: {filePath}");
                return null;
            }

            this.logger.Information($"Begin Parsing of file: {filePath}");

            var entities = this.GetRecordsFromFile(filePath);

            if (entities == null || !entities.Any())
            {
                this.logger.Information($"No records were parsed from the file.");
                return null;
            }

            this.logger.Information("Parsing has complete");

            return new StatementEntity(Path.GetFileName(filePath), entities);
        }

        internal static ITransactionEntity? GetTransactionTypeFromFilePath(string filePath)
        {
            if (Path.GetFileName(filePath).Contains(Constants.FacebookFileName))
            {
                return new FacebookTransactionEntity();
            }

            if (Path.GetFileName(filePath).Contains(Constants.MercariFileName))
            {
                return new MercariTransactionEntity();
            }

            return null;
        }

        internal static string? GetVendorNameFromFilePath(string filePath)
        {
            if (Path.GetFileName(filePath).Contains(Constants.FacebookFileName))
            {
                return Constants.Facebook;
            }

            if (Path.GetFileName(filePath).Contains(Constants.MercariFileName))
            {
                return Constants.Mercari;
            }

            return null;
        }

        private IEnumerable<ITransactionEntity>? GetRecordsFromFile(string filePath)
        {
            this.logger.Information($"Begin parsing of file: {filePath}");

            try
            {
                var reader = new StreamReader(filePath);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                switch (GetTransactionTypeFromFilePath(filePath))
                {
                    case FacebookTransactionEntity facebookEntity:
                        var faceboookRecords = csv.GetRecords<FacebookTransactionEntity>().ToList();
                        return faceboookRecords;

                    case MercariTransactionEntity mercariEntity:
                        var mercariRecords = csv.GetRecords<MercariTransactionEntity>().ToList();
                        return mercariRecords;

                    default:
                        this.logger.Error("Could not determine type of transaction file.");
                        break;
                }
            }
            catch (Exception e)
            {
                this.logger.Error($"Error reading file. Reason: {e.Message}");
            }

            return new List<ITransactionEntity>();
        }
    }
}