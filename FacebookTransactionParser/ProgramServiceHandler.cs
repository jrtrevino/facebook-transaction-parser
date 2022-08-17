namespace FacebookTransactionParser
{
    using FacebookTransactionParser.Contracts;
    using FacebookTransactionParser.Entities;
    using Serilog;

    public class ProgramServiceHandler
    {
        private ILogger logger;
        private IStatementParser parser;

        public ProgramServiceHandler(ILogger logger, IStatementParser parser)
        {
            this.logger = logger;
            this.parser = parser;
        }

        public async Task BeginProcessing()
        {
            // read email

            // parse files
            var parsedEntities = this.ParseFiles(this.ReadFilesFromDirectory(@"C:\Code\facebook-transaction-parser\Data"));

            foreach (var entity in parsedEntities)
            {
                this.logger.Information($"Filename: {entity.GetFileName()}");
            }
        }

        internal void ReadEmails()
        {
            return;
        }

        internal IEnumerable<string> ReadFilesFromDirectory(string directoryPath)
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

        internal List<StatementEntity> ParseFiles(IEnumerable<string> filesToParse)
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