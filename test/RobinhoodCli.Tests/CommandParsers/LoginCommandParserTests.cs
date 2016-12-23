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

        [TestMethod]
        public void Parse_ShouldReturnNull_WithLoginTokenAndNoToken()
        {
            // Act
            var authCommand = loginCommandParser.Parse(new[] { "login-token" });

            // Assert
            Assert.IsNull(authCommand);
            Assert.AreEqual(LoginCommandParser.Error_MissingToken, loginCommandParser.LastError);
        }

        [TestMethod]
        public void Parse_ShouldReturnNull_WithLoginTokenAndInvalidToken()
        {
            // Arrange
            var token = "1234567890";

            // Act
            var authCommand = loginCommandParser.Parse(new[] { "login-token", token });

            // Assert
            Assert.IsNull(authCommand);
            Assert.AreEqual(
                string.Format(LoginCommandParser.Error_InvalidTokenFormat, 10),
                loginCommandParser.LastError);
        }

        [TestMethod]
        public void Parse_ShouldReturnAuthCommand_WithLoginToken()
        {
            // Arrange
            var token = "1234567890123456789012345678901234567890";

            // Act
            var authCommand = loginCommandParser.Parse(new[] { "login-token", token }) as SetAuthenticationTokenCommand;

            // Assert
            Assert.IsNotNull(authCommand);
            Assert.AreEqual(token, authCommand.AuthenticationToken);
            Assert.IsNull(loginCommandParser.LastError);
        }

    }
}