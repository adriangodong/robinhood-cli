using System;
using System.Threading.Tasks;
using Deadlock.Robinhood;
using Deadlock.Robinhood.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RobinhoodCli.Commands;
using RobinhoodCli.Models;

namespace RobinhoodCli.Tests.Commands
{
    [TestClass]
    public class LoginCommandTests
    {

        private Mock<IRobinhoodClient> mockRobinhoodClient;

        [TestInitialize]
        public void Init()
        {
            mockRobinhoodClient = new Mock<IRobinhoodClient>();
        }

        [TestMethod]
        public async Task Execute_ShouldQueueDisplayError_WhenLoginCallFail()
        {
            // Arrange
            var loginCommand = new LoginCommand(null, null, false);
            mockRobinhoodClient
                .Setup(mock => mock.Login(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new Result<Authentication>()
                {
                    IsSuccessStatusCode = false
                });
            var executionContext = new ExecutionContext();

            // Act
            await loginCommand.Execute(mockRobinhoodClient.Object, executionContext);

            // Assert
            Assert.AreEqual(1, executionContext.CommandQueue.Count);
            Assert.IsNotNull(executionContext.CommandQueue.Peek() as DisplayErrorCommand);
            // TODO: assert error message
        }

        [TestMethod]
        public async Task Execute_ShouldQueueSetAuthenticationTokenCommand()
        {
            // Arrange
            var authenticationToken = Guid.NewGuid().ToString("N");
            var loginCommand = new LoginCommand(null, null, false);
            mockRobinhoodClient
                .Setup(mock => mock.Login(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new Result<Authentication>()
                {
                    IsSuccessStatusCode = true,
                    Data = new Authentication()
                    {
                        Token = authenticationToken
                    }
                });
            var executionContext = new ExecutionContext();

            // Act
            await loginCommand.Execute(mockRobinhoodClient.Object, executionContext);

            // Assert
            Assert.AreEqual(1, executionContext.CommandQueue.Count);
            Assert.IsNotNull(executionContext.CommandQueue.Peek() as SetAuthenticationTokenCommand);
            Assert.AreEqual(
                authenticationToken,
                (executionContext.CommandQueue.Peek() as SetAuthenticationTokenCommand).AuthenticationToken);
        }

        [TestMethod]
        public async Task Execute_ShouldQueueSaveAuthenticationTokenCommand_WhenSaveAuthenticationTokenIsTrue()
        {
            // Arrange
            var authenticationToken = Guid.NewGuid().ToString("N");
            var loginCommand = new LoginCommand(null, null, true);
            mockRobinhoodClient
                .Setup(mock => mock.Login(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new Result<Authentication>()
                {
                    IsSuccessStatusCode = true,
                    Data = new Authentication()
                    {
                        Token = authenticationToken
                    }
                });
            var executionContext = new ExecutionContext();

            // Act
            await loginCommand.Execute(mockRobinhoodClient.Object, executionContext);

            // Assert
            Assert.AreEqual(2, executionContext.CommandQueue.Count);
            Assert.IsNotNull(executionContext.CommandQueue.Peek() as SaveAuthenticationTokenCommand);
            Assert.AreEqual(
                authenticationToken,
                (executionContext.CommandQueue.Peek() as SaveAuthenticationTokenCommand).AuthenticationToken);
        }

    }
}