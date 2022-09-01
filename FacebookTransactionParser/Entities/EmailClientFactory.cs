namespace FacebookTransactionParser.Entities
{
    using FacebookTransactionParser.Contracts;
    using MailKit.Net.Imap;
    using MailKit.Net.Smtp;
    using MailKit.Security;
    using Microsoft.Extensions.Options;

    public class EmailClientFactory : IEmailClientFactory
    {
        private readonly IOptions<AppConfig> config;

        public EmailClientFactory(IOptions<AppConfig> config)
        {
            this.config = config;
        }

        public ImapClient? GetImapClient()
        {
            try
            {
                var client = new ImapClient();
                client.Connect(Constants.GoogleImapHost, Constants.ImapPort, SecureSocketOptions.SslOnConnect);
                client.Authenticate(this.config.Value.EmailUserName, this.config.Value.AppPassword);
                return client;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public SmtpClient? GetSmtpClient()
        {
            try
            {
                var client = new SmtpClient();
                client.Connect(Constants.GoogleSmtpHost, Constants.SmtpPort, SecureSocketOptions.StartTls);
                client.Authenticate(this.config.Value.EmailUserName, this.config.Value.AppPassword);
                return client;
            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}
