using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RobinhoodCli.Commands;
using RobinhoodCli.Models;
using RobinhoodCli.Services;

namespace RobinhoodCli.Tests.Commands
{
    [TestClass]
    public class AuthenticationRequiredCommandTests
    {

        private Mock<IOutputService> mockOutputService;
        // We need to use a Mock here because AuthenticationRequiredCommand is abstract.
        private Mock<AuthenticationRequiredCommand> mockAuthenticationRequiredCommand;

        [TestInitialize]
        public void Init()
        {
            mockOutputService = new Mock<IOutputService>();
            mockAuthenticationRequiredCommand = new Mock<AuthenticationRequiredCommand>()
            {
                CallBase = true
            };
        }

        [TestMethod]
        public void Execute_ShouldQueueDisplayError_WhenNoAuthenticationToken()
        {
            // Arrange
            var executionContext = new ExecutionContext();

            // Act
            mockAuthenticationRequiredCommand.Object.Execute(
                null, mockOutputService.Object, executionContext);

            // Assert
            Assert.AreEqual(0, executionContext.CommandQueue.Count);
            mockOutputService.Verify(
                mock => mock.Error(AuthenticationRequiredCommand.Error_Unauthenticated),
                Times.Once);
        }

        [TestMethod]
        public void Execute_ShouldCallExecuteWithAuthentication_WhenAuthenticated()
        {
            // Arrange
            var executionContext = new ExecutionContext()
            {
                AuthenticationToken = Guid.NewGuid().ToString("N")
            };

            // Act
            mockAuthenticationRequiredCommand.Object.ExecuteWithAuthentication(null, null, executionContext);

            // Assert
            Assert.AreEqual(0, executionContext.CommandQueue.Count);
            mockAuthenticationRequiredCommand
                .Verify(mock => mock.ExecuteWithAuthentication(null, null, executionContext), Times.Once);
        }

    }
}