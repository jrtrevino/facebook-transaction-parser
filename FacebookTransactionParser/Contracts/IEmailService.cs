namespace FacebookTransactionParser.Contracts
{
    public interface IEmailService
    {
        public void DownloadAttachmentsFromInbox();

        void SendEmailWithMetrics(Dictionary<string, Dictionary<string, decimal>> metricsByFileName);
    }
}
