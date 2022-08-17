namespace FacebookTransactionParser
{
    using FacebookTransactionParser.Contracts;
    using FacebookTransactionParser.Entities;
    using FacebookTransactionParser.Implementations;
    using Serilog;

    public class ProgramServiceHandler
    {
        private ILogger logger;
        private IStatementParser parser;
        private IStatementProcessor processor;

        public ProgramServiceHandler(ILogger logger, IStatementParser parser, IStatementProcessor processor)
        {
            this.logger = logger;
            this.parser = parser;
            this.processor = processor;
        }

        public void BeginProcessing()
        {
            this.logger.Information($"Processing has began at {DateTime.Now}");

            // read email

            // parse files
            var parsedEntities = this.ParseFiles(this.ReadFilesFromDirectory(@"C:\Code\facebook-transaction-parser\Data"));

            // process files
            foreach (var entity in parsedEntities)
            {
                this.logger.Information($"Processing entity with filename: {entity.GetFileName()}");
                this.processor.ProcessOrderSummary(entity);
                var metrics = this.processor.GetStatementSummary();
                this.logger.Information($"Processed file. Total profit: {metrics["profit"]}");
            }

            // send email with metrics
        }

        private void ReadEmails()
        {
            return;
        }

        private IEnumerable<string> ReadFilesFromDirectory(string directoryPath)
        {
            try
            {
                return Directory.EnumerateFiles(directoryPath);
            }
            catch (Exception exception)
            {
                this.logger.Error($"Could not enumerate files. Reason: {exception.Message}");
            }

            return new List<string>();
        }

        private List<StatementEntity> ParseFiles(IEnumerable<string> filesToParse)
        {
            var parsedEntities = new List<StatementEntity>();

            foreach (var file in filesToParse)
            {
                var entity = this.parser.ParseTransactionFile(file);

                if (entity != null)
                {
                    parsedEntities.Add(entity);
                }
            }

            return parsedEntities;
        }
    }
}