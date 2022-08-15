namespace FacebookTransactionParserTests.UnitTests
{
    using NUnit.Framework;
    using FacebookTransactionParser.Entities;
    using FacebookTransactionParser.Implementations;
    using Serilog;
    using Moq;

    public class StatementParserTests
    {
        StatementParser parser;

        [SetUp]
        public void SetUp()
        {
            var mock = new Mock<ILogger>().Object;
            parser = new StatementParser(mock);
        }

        [Test]
        public void TestEntityReturnFromFilePathMercari()
        {
            // Arrange
            var mercariFilePath = "Custom-sales-report_blah";

            // Act
            var response = StatementParser.GetTransactionTypeFromFilePath(mercariFilePath);

            // Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.GetType(), Is.EqualTo(typeof(MercariTransactionEntity)));
        }

        [Test]
        public void TestEntityReturnFromFilePathFacebook()
        {
            // Arrange
            var mercariFilePath = "C2C_Order_History_Report";

            // Act
            var response = StatementParser.GetTransactionTypeFromFilePath(mercariFilePath);

            // Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.GetType(), Is.EqualTo(typeof(FacebookTransactionEntity)));
        }
    }
}
