namespace FacebookTransactionParser
{
    using FacebookTransactionParser.Implementations;
    using Serilog;

    public class Program
    {
        public static void Main()
        {
            var logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
            var parser = new StatementParser(logger);
            var statementEntity = parser.ParseTransactionFile(@"C:\Code\facebook-transaction-parser\Data\C2C_Order_History_Report_Lisa-Ruiz_07012022_07312022.csv");
            var statementProcessor = new StatementProcessor(statementEntity);
            statementProcessor.ProcessOrderSummary();
            var metrics = statementProcessor.GetStatementSummary();
        }
    }
}