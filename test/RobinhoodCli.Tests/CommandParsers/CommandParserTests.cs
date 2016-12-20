using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RobinhoodCli.CommandParsers;

namespace RobinhoodCli.Tests.CommandParsers
{
    [TestClass]
    public class CommandParserTests
    {

        private Mock<ICommandParser> mockCommandParser;
        private CommandParser commandParser;

        [TestInitialize]
        public void Init()
        {
            mockCommandParser = new Mock<ICommandParser>();
            commandParser = new CommandParser(new List<ICommandParser>() { mockCommandParser.Object });
        }

        [TestMethod]
        public void Parse_ShouldReturnNull_WithEmptyCommand()
        {
            // Act
            var nullOrder = commandParser.Parse(null);
            var nullLastError = commandParser.LastError;
            var emptyOrder = commandParser.Parse(string.Empty);
            var emptyLastError = commandParser.LastError;
            var whiteSpaceOrder = commandParser.Parse(" ");
            var whiteSpaceLastError = commandParser.LastError;

            // Assert
            Assert.IsNull(nullOrder);
            Assert.AreEqual(CommandParser.Error_EmptyCommand, nullLastError);
            Assert.IsNull(emptyOrder);
            Assert.AreEqual(CommandParser.Error_EmptyCommand, emptyLastError);
            Assert.IsNull(whiteSpaceOrder);
            Assert.AreEqual(CommandParser.Error_EmptyCommand, whiteSpaceLastError);
        }

        [TestMethod]
        public void Parse_ShouldReturnNull_WithUnknownOrderType()
        {
            // Act
            var order = commandParser.Parse("exit");

            // Assert
            Assert.IsNull(order);
            Assert.AreEqual(
                string.Format(CommandParser.Error_UnknownFirstToken, "exit"),
                commandParser.LastError);
        }

        [TestMethod]
        public void Parse_ShouldCallCommandParser_WithTokenizedCommand()
        {
            // Arrange
            string[] actualCommandTokens = null;
            mockCommandParser
                .Setup(mock => mock.Parse(It.IsAny<string[]>()))
                .Callback<string[]>(commandTokens => actualCommandTokens = commandTokens);

            // Act
            commandParser.Parse("1 2 3");

            // Assert
            Assert.IsNotNull(actualCommandTokens);
            Assert.AreEqual(3, actualCommandTokens.Length);
            Assert.AreEqual("1", actualCommandTokens[0]);
            Assert.AreEqual("2", actualCommandTokens[1]);
            Assert.AreEqual("3", actualCommandTokens[2]);
        }

    }
}