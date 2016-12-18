using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RobinhoodCli.Commands;

namespace RobinhoodCli
{
    [TestClass]
    public class CommandParserTests
    {

        private CommandParser commandParser;

        [TestInitialize]
        public void Init()
        {
            commandParser = new CommandParser(new List<ICommandParser>());
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
            // TODO
        }

    }
}