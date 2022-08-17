using FacebookTransactionParser.Entities;
using FacebookTransactionParser.Implementations;
using Moq;
using NUnit.Framework;
using Serilog;

namespace FacebookTransactionParserTests.IntegrationTests
{
    internal class StatementParserTests
    {
        StatementParser parser;

        [SetUp]
        public void SetUp()
        {
            var mock = new Mock<ILogger>().Object;
            parser = new StatementParser(mock);
        }


        [Test]
        public void ReadFromMercariFile()
        {
            // Arrange 
            var filePath = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Data\Custom-sales-report_070122-073122.csv");

            // Act
            var response = this.parser.ParseTransactionFile(filePath);
            var transactions = response.GetTransactions().ToList();

            // Assert
            Assert.That(response, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(response.GetFileName, Is.EqualTo(Path.GetFileName(filePath)));
                Assert.That(transactions.Count(), Is.EqualTo(3));
                Assert.That(transactions[0].Price, Is.EqualTo("31.00"));
                Assert.That(transactions[0].GetVendorFeeFromPrice("$31.00"), Is.EqualTo(3.10M));
            });
        }

        [Test]
        public void ReadFromFacebookFile()
        {
            // Arrange 
            var filePath = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Data\C2C_Order_History_Report_Lisa-Ruiz_07012022_07312022.csv");

            // Act
            var response = this.parser.ParseTransactionFile(filePath);
            var transactions = response.GetTransactions().ToList();

            // Assert
            Assert.That(response, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(response.GetFileName, Is.EqualTo(Path.GetFileName(filePath)));
                Assert.That(transactions.Count(), Is.EqualTo(3));
                Assert.That(transactions[0].Price, Is.EqualTo("$32.00"));
                Assert.That(transactions[0].GetVendorFeeFromPrice("$32.00"), Is.EqualTo(32 * 0.10M));
            });
        }
    }
}
