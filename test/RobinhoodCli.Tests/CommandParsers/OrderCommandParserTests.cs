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
            var orderCommand = commandParser.ParseCommandTokens(OrderType.Unknown, new[] { "unknown" });

            // Assert
            Assert.IsNull(orderCommand);
            Assert.AreEqual(OrderCommandParser.Error_EmptySymbol, commandParser.LastError);
        }

        [TestMethod]
        public void ParseCommandTokens_ShouldReturnNull_WithBuyTypeAndMissingSize()
        {
            // Act
            var orderCommand = commandParser.ParseCommandTokens(OrderType.Buy, new[] { "buy", "hood" });

            // Assert
            Assert.IsNull(orderCommand);
            Assert.AreEqual(OrderCommandParser.Error_EmptySizeParameter, commandParser.LastError);
        }

        [TestMethod]
        public void ParseCommandTokens_ShouldReturnOrder_WithSellTypeAndMissingSize()
        {
            // Act
            var orderCommand = commandParser.ParseCommandTokens(OrderType.Sell, new[] { "sell", "hood" });

            // Assert
            Assert.IsNotNull(orderCommand);
            Assert.AreEqual(OrderType.Sell, orderCommand.Type);
            Assert.AreEqual("hood", orderCommand.Symbol);
            Assert.IsNull(orderCommand.Size);
            Assert.IsNull(orderCommand.LimitPrice);
        }

        [TestMethod]
        public void ParseCommandTokens_ShouldReturnNull_WithBadSize()
        {
            // Act
            var orderCommand = commandParser.ParseCommandTokens(OrderType.Unknown, new[] { "unknown", "hood", "one" });

            // Assert
            Assert.IsNull(orderCommand);
            Assert.AreEqual(
                string.Format(OrderCommandParser.Error_BadSizeParameter, "one"),
                commandParser.LastError);
        }

        [TestMethod]
        public void ParseCommandTokens_ShouldReturnOrder_WithMissingLimitPrice()
        {
            // Act
            var orderCommand = commandParser.ParseCommandTokens(OrderType.Unknown, new[] { "unknown", "hood", "123" });

            // Assert
            Assert.IsNotNull(orderCommand);
            Assert.AreEqual(OrderType.Unknown, orderCommand.Type);
            Assert.AreEqual("hood", orderCommand.Symbol);
            Assert.AreEqual(123, orderCommand.Size);
            Assert.IsNull(orderCommand.LimitPrice);
        }

        [TestMethod]
        public void ParseCommandTokens_ShouldReturnNull_WithBadLimitPrice()
        {
            // Act
            var orderCommand = commandParser.ParseCommandTokens(OrderType.Unknown, new[] { "unknown", "hood", "123", "1-23" });

            // Assert
            Assert.IsNull(orderCommand);
            Assert.AreEqual(
                string.Format(OrderCommandParser.Error_BadLimitPriceParameter, "1-23"),
                commandParser.LastError);
        }

        [TestMethod]
        public void ParseCommandTokens_ShouldReturnOrderCommand()
        {
            // Act
            var orderCommand = commandParser.ParseCommandTokens(OrderType.Unknown, new[] { "unknown", "hood", "123", "1.23" });

            // Assert
            Assert.IsNotNull(orderCommand);
            Assert.AreEqual(OrderType.Unknown, orderCommand.Type);
            Assert.AreEqual("hood", orderCommand.Symbol);
            Assert.AreEqual(123, orderCommand.Size);
            Assert.AreEqual(1.23m, orderCommand.LimitPrice);
        }

        [TestMethod]
        public void Parse_ShouldReturnBuyOrder_WithBuyTypeParameter()
        {
            // Act
            var buyOrderCommand = commandParser.Parse(new[] { "buy", "hood", "1" }) as PrepareOrderCommand;
            var bOrderCommand = commandParser.Parse(new[] { "b", "hood", "1" }) as PrepareOrderCommand;

            // Assert
            Assert.IsNotNull(buyOrderCommand);
            Assert.AreEqual(OrderType.Buy, buyOrderCommand.Type);
            Assert.AreEqual(1, buyOrderCommand.Size);
            Assert.IsNull(buyOrderCommand.LimitPrice);

            Assert.IsNotNull(bOrderCommand);
            Assert.AreEqual(OrderType.Buy, bOrderCommand.Type);
            Assert.AreEqual(1, bOrderCommand.Size);
            Assert.IsNull(bOrderCommand.LimitPrice);
        }

        [TestMethod]
        public void Parse_ShouldReturnSellOrder_WithSellTypeParameter()
        {
            // Act
            var sellOrderCommand = commandParser.Parse(new[] { "sell", "hood" }) as PrepareOrderCommand;
            var sOrderCommand = commandParser.Parse(new[] { "s", "hood" }) as PrepareOrderCommand;

            // Assert
            Assert.IsNotNull(sellOrderCommand);
            Assert.AreEqual(OrderType.Sell, sellOrderCommand.Type);
            Assert.IsNull(sellOrderCommand.Size);
            Assert.IsNull(sellOrderCommand.LimitPrice);

            Assert.IsNotNull(sOrderCommand);
            Assert.AreEqual(OrderType.Sell, sOrderCommand.Type);
            Assert.IsNull(sOrderCommand.Size);
            Assert.IsNull(sOrderCommand.LimitPrice);
        }

    }
}