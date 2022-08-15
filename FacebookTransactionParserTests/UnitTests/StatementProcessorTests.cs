namespace FacebookTransactionParserTests.UnitTests
{
    using FacebookTransactionParser.Entities;
    using Stubs;
    using NUnit.Framework;
    using FacebookTransactionParser.Implementations;

    public class Tests
    {
        public StatementEntity entity;

        [SetUp]
        public void Setup()
        {
            entity = StatementEntityStub.ValidEntity;
        }

        [Test]
        public void TestStatementProcessingValues()
        {
            // Arrange
            var parser = new StatementProcessor(entity);

            // Act
            parser.ProcessOrderSummary();
            var data = parser.GetStatementSummary();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(parser, Is.Not.Null);
                Assert.That(data, Is.Not.Null);
            });
            Assert.Multiple(() =>
            {
                Assert.That(data.ContainsKey("revenue"), Is.True);
                Assert.That(data.ContainsKey("profit"), Is.True);
                Assert.That(data.ContainsKey("tax"), Is.True);
                Assert.That(data.ContainsKey("shipping"), Is.True);
                Assert.That(data.ContainsKey("fee"), Is.True);
            });
            Assert.Multiple(() =>
            {
                Assert.That(data["revenue"], Is.EqualTo(694.21M));
                Assert.That(data["tax"], Is.EqualTo(75.52M));
                Assert.That(data["fee"], Is.EqualTo(77.383));
                Assert.That(data["shipping"], Is.EqualTo(6.00M));
            });
        }
    }
}