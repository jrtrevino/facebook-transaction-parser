namespace FacebookTransactionParser.Implementations
{
    using FacebookTransactionParser.Contracts;
    using FacebookTransactionParser.Entities;
    using MailKit;
    using MailKit.Search;
    using Microsoft.Extensions.Options;
    using MimeKit;
    using MimeKit.Text;
    using Serilog;
    using System.Globalization;

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
                    var vendor = StatementParser.GetVendorNameFromFilePath(fileName) ?? "vendor";
                    var subject = $"Your {vendor} Transaction Summary.";
                    if (this.attachmentFilePathBySender[sender].Contains(fileName))
                    {
                        this.SendEmail(sender, Constants.SenderEmail, subject, CreateMessageBody(metricsByFileName[fileName], vendor, fileName));
                    }
                }
            }
        }

        private static string CreateMessageBody(Dictionary<string, decimal> metrics, string vendor, string fileName)
        {
            var statementEndDate = DateTime.Now.AddDays(-1).Date;
            var statementBeginDate = new DateTime(statementEndDate.Year, statementEndDate.Month, 1);
            var statementRange = $"{statementBeginDate:yyyyMMdd} - {statementEndDate:yyyyMMdd}";
            return string.Format(
                Constants.EmailTemplate,
                vendor,
                statementRange,
                metrics["revenue"].ToString("C2", CultureInfo.CurrentCulture),
                metrics["tax"].ToString("C2", CultureInfo.CurrentCulture),
                metrics["shipping"].ToString("C2", CultureInfo.CurrentCulture),
                vendor,
                metrics["fee"].ToString("C2", CultureInfo.CurrentCulture),
                metrics["profit"].ToString("C2", CultureInfo.CurrentCulture),
                fileName);
        }

        private void SendEmail(string to, string from, string subject, string body)
        {
            var client = this.emailClientFactory.GetSmtpClient();
            if (client == null)
            {
                this.logger.Error("Error creating SmtpClient.");
                return;
            }

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = body };
            client.Send(email);
            client.Disconnect(quit: true);
            this.logger.Information($"Email successfully sent to {to}.");
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

                if (!this.attachmentFilePathBySender.ContainsKey(sender))
                {
                    var attachmentList = new List<string>()
                    {
                        fileName,
                    };

                    this.attachmentFilePathBySender.Add(sender, attachmentList);
                }
                else
                {
                    this.attachmentFilePathBySender[sender].Add(fileName);
                }

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