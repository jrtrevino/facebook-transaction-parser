namespace FacebookTransactionParserTests;

using FacebookTransactionParser;
using Microsoft.Extensions.Logging;

public class TransactionParserTests
{
    TransactionParser transactionParser;
    [SetUp]
    public void Setup()
    {
        var logger = new Logger<TransactionParser>();
        this.transactionParser = new TransactionParser();
    }

    [Test]
    public void TestInvalidFilePath()
    {
        // Arrange
        var filePath = "test";

        // Act
        var response = transactionParser.ParseTransactionFile(filePath);

        // Assert
        Assert.IsNull(response);
    }

    [Test]
    public void TestValidFilePath()
    {
        // Arrange
        var filePath = "/Users/jtrevino/Projects/email-program/data/C2C_Order_History_Report_Lisa-Ruiz_03012022_03312022.csv";

        // Act
        var response = transactionParser.ParseTransactionFile(filePath);

        // Assert
        Assert.IsNotNull(response);
        Assert.That(response.GetFileName(), Is.EqualTo(Path.GetFileName(filePath)));
        Assert.That(response.GetTransactions().Count(), Is.EqualTo(16));
    }
}
