namespace FacebookTransactionParser
{
    using FacebookTransactionParser.Contracts;
    using FacebookTransactionParser.Entities;
    using FacebookTransactionParser.Implementations;
    using Microsoft.Extensions.Options;
    using Serilog;

    public class ProgramServiceHandler
    {
        private ILogger logger;
        private IStatementParser parser;
        private IStatementProcessor processor;
        private IEmailService emailService;
        private IOptions<AppConfig> config;

        public ProgramServiceHandler(ILogger logger, IOptions<AppConfig> config, IStatementParser parser, IStatementProcessor processor, IEmailService emailService)
        {
            this.logger = logger;
            this.parser = parser;
            this.processor = processor;
            this.emailService = emailService;
            this.config = config;
        }

        public void BeginProcessing()
        {
            this.logger.Information($"Processing has began at {DateTime.Now}");

            // download attachments from emails
            this.emailService.DownloadAttachmentsFromInbox();

            // parse files
            var parsedEntities = this.ParseFiles(this.ReadFilesFromDirectory(this.config.Value.AttachmentDownloadPath));

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