using Microsoft.VisualStudio.TestTools.UnitTesting;
using RobinhoodCli.CommandParsers;
using RobinhoodCli.Commands;

namespace RobinhoodCli.Tests.CommandParsers
{
    [TestClass]
    public class LoginCommandParserTests
    {

        private LoginCommandParser loginCommandParser;

        [TestInitialize]
        public void Init()
        {
            loginCommandParser = new LoginCommandParser();
        }

        [TestMethod]
        public void Parse_ShouldReturnNull_WithUnknownPrefix()
        {
            // Act
            var loginCommand = loginCommandParser.Parse(new[] { "unknown" });

            // Assert
            Assert.IsNull(loginCommand);
        }

        [TestMethod]
        public void Parse_ShouldReturnNull_WithLoginAndNoUsername()
        {
            // Act
            var loginCommand = loginCommandParser.Parse(new[] { "login" });

            // Assert
            Assert.IsNull(loginCommand);
            Assert.AreEqual(LoginCommandParser.Error_MissingParameter, loginCommandParser.LastError);
        }

        [TestMethod]
        public void Parse_ShouldReturnNull_WithLoginAndNoPassword()
        {
            // Act
            var loginCommand = loginCommandParser.Parse(new[] { "login", "username" });

            // Assert
            Assert.IsNull(loginCommand);
            Assert.AreEqual(LoginCommandParser.Error_MissingParameter, loginCommandParser.LastError);
        }

        [TestMethod]
        public void Parse_ShouldReturnLoginCommand_WithLogin()
        {
            // Act
            var loginCommand = loginCommandParser.Parse(new[] { "login", "username", "password" }) as LoginCommand;

            // Assert
            Assert.IsNotNull(loginCommand);
            Assert.AreEqual("username", loginCommand.Username);
            Assert.AreEqual("password", loginCommand.Password);
            Assert.IsNull(loginCommandParser.LastError);
        }

    }
}