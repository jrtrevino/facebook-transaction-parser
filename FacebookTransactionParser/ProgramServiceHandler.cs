namespace FacebookTransactionParser
{
    using FacebookTransactionParser.Contracts;
    using FacebookTransactionParser.Entities;
    using FacebookTransactionParser.Implementations;
    using Microsoft.Extensions.Options;
    using Serilog;

    public class ProgramServiceHandler
    {
        private readonly ILogger logger;
        private readonly IStatementParser parser;
        private readonly IStatementProcessor processor;
        private readonly IEmailService emailService;
        private readonly IOptions<AppConfig> config;

        public ProgramServiceHandler(
            ILogger logger,
            IOptions<AppConfig> config,
            IStatementParser parser,
            IStatementProcessor processor,
            IEmailService emailService)
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

            if (!this.IsCreateDirectorySuccessful() || !this.TryDeleteFilesInDirectory())
            {
                return;
            }

            var metricsByFileName = new Dictionary<string, Dictionary<string, decimal>>();

            this.emailService.DownloadAttachmentsFromInbox();

            var parsedEntities = this.ParseFiles(this.ReadFilesFromDirectory(this.config.Value.AttachmentDownloadPath));

            if (parsedEntities.Count == 0)
            {
                this.logger.Error("No files to process.");
                return;
            }

            foreach (var entity in parsedEntities)
            {
                this.logger.Information($"Processing entity with filename: {entity.GetFileName()}");
                this.processor.ProcessOrderSummary(entity);
                var metrics = this.processor.GetStatementSummary();
                this.logger.Information($"Processed file. Total profit: {metrics["profit"]}");
                metricsByFileName.Add(entity.GetFileName(), metrics);
            }

            this.emailService.SendEmailWithMetrics(metricsByFileName);
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

        private bool IsCreateDirectorySuccessful()
        {
            try
            {
                var directory = Directory.CreateDirectory(this.config.Value.AttachmentDownloadPath);
                return true;
            }
            catch (Exception exception)
            {
                this.logger.Error($"Error creating attachment directory: {exception.Message}");
            }

            return false;
        }

        private bool TryDeleteFilesInDirectory()
        {
            try
            {
                var files = Directory.EnumerateFiles(this.config.Value.AttachmentDownloadPath);

                foreach (var file in files)
                {
                    File.Delete(file);
                }

                return true;
            }
            catch (Exception exception)
            {
                this.logger.Error($"Files detected in attachment download path. Error when trying to delete: {exception.Message}");
            }

            return false;
        }
    }
}