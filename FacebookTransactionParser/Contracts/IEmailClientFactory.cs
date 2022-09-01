namespace FacebookTransactionParser.Contracts
{
    using MailKit.Net.Imap;
    using MailKit.Net.Smtp;

    public interface IEmailClientFactory
    {
        public ImapClient? GetImapClient();

        public SmtpClient? GetSmtpClient();

    }
}
