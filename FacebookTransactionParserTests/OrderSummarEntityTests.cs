namespace FacebookTransactionParserTests;

using FacebookTransactionParser;
using FacebookTransactionParser.Entities;

public class OrderSummaryTests
{
    OrderSummaryEntity? orderSummary;
    IEnumerable<TransactionEntity>? transactionEntities;

    [SetUp]
    public void Setup()
    {
        var fileName = "test.csv";
        var transaction1 = new TransactionEntity
        {
            Price = "$5.00",
            Tax = "$0.40",
            ShippingCost = "$2.00",
        };
        var transaction2 = new TransactionEntity
        {
            Price = "$3.50",
            Tax = "$0.40",
            ShippingCost = "$3.80",
        };
        var transaction3 = new TransactionEntity
        {
            Price = "$11.00",
            Tax = "$1.40",
            ShippingCost = "$1.80",
        };

        this.transactionEntities = new List<TransactionEntity> { transaction1, transaction2, transaction3 };
        this.orderSummary = new OrderSummaryEntity(fileName, transactionEntities);
    }

    [Test]
    public void TestPriceCalculationForTransactions()
    {
        // Act
        var response = new ProcessedOrderSummary(this.orderSummary);
        var metrics = response.GetTransactionSummary();

        // Assert
        Assert.That(response, Is.Not.Null);
        Assert.That(metrics, Is.Not.Null);
        Assert.That(metrics.Count(), Is.EqualTo(5));
        Assert.That(metrics.ContainsKey("revenue"), Is.True);
        Assert.That(metrics.ContainsKey("profit"), Is.True);
        Assert.That(metrics.ContainsKey("tax"), Is.True);
        Assert.That(metrics.ContainsKey("shipping"), Is.True);
        Assert.That(metrics.ContainsKey("fee"), Is.True);
        Assert.That(metrics["revenue"], Is.EqualTo(19.5M));
        Assert.That(metrics["profit"], Is.EqualTo(17.28M));
        Assert.That(metrics["tax"], Is.EqualTo(2.2M));
        Assert.That(metrics["shipping"], Is.EqualTo(7.6M));
        Assert.That(metrics["fee"], Is.EqualTo(2.22M));

    }
}
