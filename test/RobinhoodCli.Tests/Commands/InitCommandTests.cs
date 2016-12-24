using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RobinhoodCli.Commands;
using RobinhoodCli.Models;

namespace RobinhoodCli.Tests.Commands
{
    [TestClass]
    public class InitCommandTests
    {

        [TestMethod]
        public async Task Execute_ShouldNotQueueSetAuthenticationTokenCommand_WhenAuthenticationTokenNotFound()
        {
            // Arrange
            var initCommand = new InitCommand(null);
            var executionContext = new ExecutionContext();

            // Act
            await initCommand.Execute(null, executionContext);

            // Assert
            Assert.AreEqual(0, executionContext.CommandQueue.Count);
        }


        [TestMethod]
        public async Task Execute_ShouldQueueSetAuthenticationTokenCommand_WhenAuthenticationTokenFound()
        {
            // Arrange
            var authenticationKey = Guid.NewGuid().ToString("N");
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration
                .SetupGet(mock => mock[InitCommand.AuthenticationTokenConfigurationKey])
                .Returns(authenticationKey);
            var initCommand = new InitCommand(mockConfiguration.Object);
            var executionContext = new ExecutionContext();

            // Act
            await initCommand.Execute(null, executionContext);

            // Assert
            Assert.AreEqual(1, executionContext.CommandQueue.Count);
            Assert.IsNotNull(executionContext.CommandQueue.Peek() as SetAuthenticationTokenCommand);
            Assert.AreEqual(
                authenticationKey,
                (executionContext.CommandQueue.Peek() as SetAuthenticationTokenCommand).AuthenticationToken);
        }

    }
}