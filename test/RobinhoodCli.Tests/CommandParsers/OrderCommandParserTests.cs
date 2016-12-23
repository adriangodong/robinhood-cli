using Microsoft.VisualStudio.TestTools.UnitTesting;
using RobinhoodCli.CommandParsers;
using RobinhoodCli.Commands;
using RobinhoodCli.Models;

namespace RobinhoodCli.Tests.CommandParsers
{
    [TestClass]
    public class OrderCommandParserTests
    {

        private OrderCommandParser orderCommandParser;

        [TestInitialize]
        public void Init()
        {
            orderCommandParser = new OrderCommandParser();
        }

        [TestMethod]
        public void ParsePrepareOrderCommand_ShouldReturnNull_WithMissingSymbol()
        {
            // Act
            var orderCommand = orderCommandParser.ParsePrepareOrderCommand(OrderType.Unknown, new[] { "unknown" });

            // Assert
            Assert.IsNull(orderCommand);
            Assert.AreEqual(OrderCommandParser.Error_MissingSymbolParameter, orderCommandParser.LastError);
        }

        [TestMethod]
        public void ParsePrepareOrderCommand_ShouldReturnNull_WithBuyTypeAndMissingSize()
        {
            // Act
            var orderCommand = orderCommandParser.ParsePrepareOrderCommand(OrderType.Buy, new[] { "buy", "hood" });

            // Assert
            Assert.IsNull(orderCommand);
            Assert.AreEqual(OrderCommandParser.Error_EmptySizeParameter, orderCommandParser.LastError);
        }

        [TestMethod]
        public void ParsePrepareOrderCommand_ShouldReturnOrder_WithSellTypeAndMissingSize()
        {
            // Act
            var orderCommand = orderCommandParser.ParsePrepareOrderCommand(OrderType.Sell, new[] { "sell", "hood" });

            // Assert
            Assert.IsNotNull(orderCommand);
            Assert.AreEqual(OrderType.Sell, orderCommand.Type);
            Assert.AreEqual("hood", orderCommand.Symbol);
            Assert.IsNull(orderCommand.Size);
            Assert.IsNull(orderCommand.LimitPrice);
        }

        [TestMethod]
        public void ParsePrepareOrderCommand_ShouldReturnNull_WithBadSize()
        {
            // Act
            var orderCommand = orderCommandParser.ParsePrepareOrderCommand(OrderType.Unknown, new[] { "unknown", "hood", "one" });

            // Assert
            Assert.IsNull(orderCommand);
            Assert.AreEqual(
                string.Format(OrderCommandParser.Error_BadSizeParameter, "one"),
                orderCommandParser.LastError);
        }

        [TestMethod]
        public void ParsePrepareOrderCommand_ShouldReturnOrder_WithMissingLimitPrice()
        {
            // Act
            var orderCommand = orderCommandParser.ParsePrepareOrderCommand(OrderType.Unknown, new[] { "unknown", "hood", "123" });

            // Assert
            Assert.IsNotNull(orderCommand);
            Assert.AreEqual(OrderType.Unknown, orderCommand.Type);
            Assert.AreEqual("hood", orderCommand.Symbol);
            Assert.AreEqual(123, orderCommand.Size);
            Assert.IsNull(orderCommand.LimitPrice);
        }

        [TestMethod]
        public void ParsePrepareOrderCommand_ShouldReturnNull_WithBadLimitPrice()
        {
            // Act
            var orderCommand = orderCommandParser.ParsePrepareOrderCommand(OrderType.Unknown, new[] { "unknown", "hood", "123", "1-23" });

            // Assert
            Assert.IsNull(orderCommand);
            Assert.AreEqual(
                string.Format(OrderCommandParser.Error_BadLimitPriceParameter, "1-23"),
                orderCommandParser.LastError);
        }

        [TestMethod]
        public void ParsePrepareOrderCommand_ShouldReturnOrderCommand()
        {
            // Act
            var orderCommand = orderCommandParser.ParsePrepareOrderCommand(OrderType.Unknown, new[] { "unknown", "hood", "123", "1.23" });

            // Assert
            Assert.IsNotNull(orderCommand);
            Assert.AreEqual(OrderType.Unknown, orderCommand.Type);
            Assert.AreEqual("hood", orderCommand.Symbol);
            Assert.AreEqual(123, orderCommand.Size);
            Assert.AreEqual(1.23m, orderCommand.LimitPrice);
        }

        [TestMethod]
        public void ParseCancelOpenOrderCommand_ShouldReturnNull_WithMissingIndex()
        {
            // Act
            var cancelOpenOrderCommand = orderCommandParser.ParseCancelOpenOrderCommand(new[] { "cancel" });

            // Assert
            Assert.IsNull(cancelOpenOrderCommand);
            Assert.AreEqual(OrderCommandParser.Error_MissingIndexParameter, orderCommandParser.LastError);
        }

        [TestMethod]
        public void ParseCancelOpenOrderCommand_ShouldReturnNull_WithBadIndex()
        {
            // Act
            var cancelOpenOrderCommand = orderCommandParser.ParseCancelOpenOrderCommand(new[] { "cancel", "hood" });

            // Assert
            Assert.IsNull(cancelOpenOrderCommand);
            Assert.AreEqual(
                string.Format(OrderCommandParser.Error_BadIndexParameter, "hood"),
                orderCommandParser.LastError);
        }

        [TestMethod]
        public void ParseCancelOpenOrderCommand_ShouldReturnCancelOrderCommand()
        {
            // Act
            var cancelOpenOrderCommand = orderCommandParser.ParseCancelOpenOrderCommand(new[] { "cancel", "1" });

            // Assert
            Assert.IsNotNull(cancelOpenOrderCommand);
            Assert.AreEqual(1, cancelOpenOrderCommand.OpenOrderIndexToCancel);
            Assert.IsNull(orderCommandParser.LastError);
        }

        [TestMethod]
        public void Parse_ShouldReturnBuyOrder_WithBuyTypeParameter()
        {
            // Act
            var buyOrderCommand = orderCommandParser.Parse(new[] { "buy", "hood", "1" }) as PrepareOrderCommand;
            var bOrderCommand = orderCommandParser.Parse(new[] { "b", "hood", "1" }) as PrepareOrderCommand;

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
            var sellOrderCommand = orderCommandParser.Parse(new[] { "sell", "hood" }) as PrepareOrderCommand;
            var sOrderCommand = orderCommandParser.Parse(new[] { "s", "hood" }) as PrepareOrderCommand;

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

        [TestMethod]
        public void Parse_ShouldReturnCancelOrderCommand_WithCancelCommand()
        {
            // Act
            var cancelOpenOrderCommand = orderCommandParser.Parse(new[] { "cancel", "1" }) as CancelOpenOrderCommand;

            // Assert
            Assert.IsNotNull(cancelOpenOrderCommand);
            Assert.IsNull(orderCommandParser.LastError);
        }

    }
}