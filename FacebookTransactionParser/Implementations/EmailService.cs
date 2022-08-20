namespace FacebookTransactionParser.Implementations
{
    using FacebookTransactionParser.Contracts;
    using FacebookTransactionParser.Entities;
    using MailKit;
    using MailKit.Search;
    using Microsoft.Extensions.Options;
    using MimeKit;
    using Serilog;

    public class EmailService : IEmailService
    {
        private readonly IOptions<AppConfig> config;
        private readonly IEmailClientFactory emailClientFactory;
        private readonly ILogger logger;

        public EmailService(IOptions<AppConfig> config, IEmailClientFactory emailClientFactory, ILogger logger)
        {
            this.config = config;
            this.emailClientFactory = emailClientFactory;
            this.logger = logger;
        }

        public void DownloadAttachmentsFromInbox()
        {
            this.logger.Information("Connecting to email client.");
            var client = this.emailClientFactory.GetImapClient();

            if (client == null)
            {
                this.logger.Error("Error with generating email client. Please check credentials.");
                return;
            }

            client.Inbox.Open(FolderAccess.ReadWrite);
            this.logger.Information("Connection successful. Searching inbox for attachments.");
            IList<UniqueId> uids = client.Inbox.Search(SearchQuery.All);

            foreach (UniqueId uid in uids)
            {
                MimeMessage message = client.Inbox.GetMessage(uid);

                foreach (MimeEntity attachment in message.Attachments)
                {
                    var fileName = attachment.ContentDisposition?.FileName ?? attachment.ContentType.Name;
                    var filePath = Path.Combine(this.config.Value.AttachmentDownloadPath, fileName);

                    using (var stream = File.Create(filePath))
                    {
                        if (attachment is MessagePart messagePart)
                        {
                            messagePart.Message.WriteTo(stream);
                        }
                        else
                        {
                            var part = (MimePart)attachment;

                            part.Content.DecodeTo(stream);
                        }
                    }
                }
            }
        }
    }
}