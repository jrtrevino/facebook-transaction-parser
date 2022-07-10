namespace FacebookTransactionParser
{
    public class Program
    {
        public static void Main()
        {
            var path = "/Users/jtrevino/Projects/email-program/data/C2C_Order_History_Report_Lisa-Ruiz_03012022_03312022.csv";
            var parsedTransaction = TransactionParser.ParseTransactionFile(path);
        }
    }
}