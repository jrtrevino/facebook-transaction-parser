namespace FacebookTransactionParser.Contracts
{
    using MailKit.Net.Imap;

    public interface IEmailClientFactory
    {
        public ImapClient? GetImapClient();
    }
}
