using Microsoft.VisualStudio.TestTools.UnitTesting;
using RobinhoodCli.CommandParsers;
using RobinhoodCli.Commands;
using RobinhoodCli.Models;

namespace RobinhoodCli.Tests.CommandParsers
{
    [TestClass]
    public class OrderCommandParserTests
    {

        private OrderCommandParser commandParser;

        [TestInitialize]
        public void Init()
        {
            commandParser = new OrderCommandParser();
        }

        [TestMethod]
        public void ParseCommandTokens_ShouldReturnNull_WithMissingSymbol()
        {
            // Act
            var order = commandParser.ParseCommandTokens(OrderType.Unknown, new[] { "unknown" });

            // Assert
            Assert.IsNull(order);
            Assert.AreEqual(OrderCommandParser.Error_EmptySymbol, commandParser.LastError);
        }

        [TestMethod]
        public void ParseCommandTokens_ShouldReturnNull_WithBuyTypeAndMissingSize()
        {
            // Act
            var order = commandParser.ParseCommandTokens(OrderType.Buy, new[] { "buy", "hood" });

            // Assert
            Assert.IsNull(order);
            Assert.AreEqual(OrderCommandParser.Error_EmptySizeParameter, commandParser.LastError);
        }

        [TestMethod]
        public void ParseCommandTokens_ShouldReturnOrder_WithSellTypeAndMissingSize()
        {
            // Act
            var order = commandParser.ParseCommandTokens(OrderType.Sell, new[] { "sell", "hood" });

            // Assert
            Assert.IsNotNull(order);
            Assert.AreEqual(OrderType.Sell, order.Type);
            Assert.AreEqual("hood", order.Symbol);
            Assert.IsNull(order.Size);
            Assert.IsNull(order.LimitPrice);
        }

        [TestMethod]
        public void ParseCommandTokens_ShouldReturnNull_WithBadSize()
        {
            // Act
            var order = commandParser.ParseCommandTokens(OrderType.Unknown, new[] { "unknown", "hood", "one" });

            // Assert
            Assert.IsNull(order);
            Assert.AreEqual(
                string.Format(OrderCommandParser.Error_BadSizeParameter, "one"),
                commandParser.LastError);
        }

        [TestMethod]
        public void ParseCommandTokens_ShouldReturnOrder_WithMissingLimitPrice()
        {
            // Act
            var order = commandParser.ParseCommandTokens(OrderType.Unknown, new[] { "unknown", "hood", "123" });

            // Assert
            Assert.IsNotNull(order);
            Assert.AreEqual(OrderType.Unknown, order.Type);
            Assert.AreEqual("hood", order.Symbol);
            Assert.AreEqual(123, order.Size);
            Assert.IsNull(order.LimitPrice);
        }

        [TestMethod]
        public void ParseCommandTokens_ShouldReturnNull_WithBadLimitPrice()
        {
            // Act
            var order = commandParser.ParseCommandTokens(OrderType.Unknown, new[] { "unknown", "hood", "123", "1-23" });

            // Assert
            Assert.IsNull(order);
            Assert.AreEqual(
                string.Format(OrderCommandParser.Error_BadLimitPriceParameter, "1-23"),
                commandParser.LastError);
        }

        [TestMethod]
        public void ParseCommandTokens_ShouldReturnOrder()
        {
            // Act
            var order = commandParser.ParseCommandTokens(OrderType.Unknown, new[] { "unknown", "hood", "123", "1.23" });

            // Assert
            Assert.IsNotNull(order);
            Assert.AreEqual(OrderType.Unknown, order.Type);
            Assert.AreEqual("hood", order.Symbol);
            Assert.AreEqual(123, order.Size);
            Assert.AreEqual(1.23m, order.LimitPrice);
        }

        [TestMethod]
        public void Parse_ShouldReturnBuyOrder_WithBuyTypeParameter()
        {
            // Act
            var buyOrder = commandParser.Parse(new[] { "buy", "hood", "1" }) as OrderCommand;
            var bOrder = commandParser.Parse(new[] { "b", "hood", "1" }) as OrderCommand;

            // Assert
            Assert.IsNotNull(buyOrder);
            Assert.AreEqual(OrderType.Buy, buyOrder.Type);
            Assert.AreEqual(1, buyOrder.Size);
            Assert.IsNull(buyOrder.LimitPrice);

            Assert.IsNotNull(bOrder);
            Assert.AreEqual(OrderType.Buy, bOrder.Type);
            Assert.AreEqual(1, bOrder.Size);
            Assert.IsNull(bOrder.LimitPrice);
        }

        [TestMethod]
        public void Parse_ShouldReturnSellOrder_WithSellTypeParameter()
        {
            // Act
            var sellOrder = commandParser.Parse(new[] { "sell", "hood" }) as OrderCommand;
            var sOrder = commandParser.Parse(new[] { "s", "hood" }) as OrderCommand;

            // Assert
            Assert.IsNotNull(sellOrder);
            Assert.AreEqual(OrderType.Sell, sellOrder.Type);
            Assert.IsNull(sellOrder.Size);
            Assert.IsNull(sellOrder.LimitPrice);

            Assert.IsNotNull(sOrder);
            Assert.AreEqual(OrderType.Sell, sOrder.Type);
            Assert.IsNull(sOrder.Size);
            Assert.IsNull(sOrder.LimitPrice);
        }

    }
}