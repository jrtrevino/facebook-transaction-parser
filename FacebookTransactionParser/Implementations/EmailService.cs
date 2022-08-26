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
        private readonly Dictionary<string, List<string>> attachmentFilePathBySender = new ();

        public EmailService(IOptions<AppConfig> config, IEmailClientFactory emailClientFactory, ILogger logger)
        {
            this.config = config;
            this.emailClientFactory = emailClientFactory;
            this.logger = logger;
        }

        public void DownloadAttachmentsFromInbox()
        {
            this.logger.Debug("Connecting to email client.");
            var client = this.emailClientFactory.GetImapClient();

            if (client == null)
            {
                this.logger.Error("Error with generating email client. Please check credentials.");
                return;
            }

            client.Inbox.Open(FolderAccess.ReadWrite);
            this.logger.Debug("Connection successful. Searching inbox for attachments.");
            IList<UniqueId> uids = client.Inbox.Search(SearchQuery.All);

            foreach (UniqueId uid in uids)
            {
                try
                {
                    MimeMessage message = client.Inbox.GetMessage(uid);

                    if (message == null)
                    {
                        continue;
                    }

                    this.ProcessAttachments(message);
                }
                catch (Exception exception)
                {
                    this.logger.Error($"Error parsing attachment. {exception.Message}");
                }
            }

            client.Inbox.Close(true);
            this.MoveEmailsToProcessedFolder(uids);
            client.Disconnect(true);
        }

        public void MoveEmailsToProcessedFolder(IList<UniqueId> uids)
        {
            var client = this.emailClientFactory.GetImapClient();

            if (client == null)
            {
                return;
            }

            try
            {
                _ = client.Inbox.Open(FolderAccess.ReadWrite);
                var processedFolder = client.GetFolder(client.PersonalNamespaces.FirstOrDefault()).Create("Processed", true);
                foreach (var uid in uids)
                {
                    client.Inbox.MoveTo(uid, processedFolder);
                }

                client.Inbox.Close(false);
                client.Disconnect(true);
            }
            catch (Exception exception)
            {
                this.logger.Error($"Error moving files to processed inbox subfolder. {exception.Message}");
            }
        }

        public void SendEmailWithMetrics(Dictionary<string, Dictionary<string, decimal>> metricsByFileName)
        {
            foreach (var fileName in metricsByFileName.Keys)
            {
                foreach (var sender in this.attachmentFilePathBySender.Keys)
                {
                    if (this.attachmentFilePathBySender[sender].Contains(fileName))
                    {
                        this.SendEmail(sender, null, this.CreateMessageBody(metricsByFileName[fileName]));
                    }
                }
            }
        }

        private string CreateMessageBody(Dictionary<string, decimal> dictionary)
        {
            throw new NotImplementedException();
        }

        private void SendEmail(string to, string subject, string body)
        {

        }

        private void ProcessAttachments(MimeMessage message)
        {
            foreach (MimeEntity attachment in message.Attachments)
            {
                var sender = message?.From?.Mailboxes?.FirstOrDefault()?.Address;

                if (sender == null)
                {
                    continue;
                }

                var fileName = attachment.ContentDisposition?.FileName ?? attachment.ContentType.Name;
                var filePath = Path.Combine(this.config.Value.AttachmentDownloadPath, fileName);
                this.attachmentFilePathBySender[sender].Add(fileName);

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