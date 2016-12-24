using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RobinhoodCli.Commands;
using RobinhoodCli.Models;

namespace RobinhoodCli.Tests.Commands
{
    [TestClass]
    public class AuthenticationRequiredCommandTests
    {

        Mock<AuthenticationRequiredCommand> mockAuthenticationRequiredCommand;

        [TestInitialize]
        public void Init()
        {
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
            mockAuthenticationRequiredCommand.Object.Execute(null, executionContext);

            // Assert
            Assert.AreEqual(1, executionContext.CommandQueue.Count);
            Assert.IsNotNull(executionContext.CommandQueue.Peek() as DisplayErrorCommand);
            Assert.AreEqual(
                AuthenticationRequiredCommand.Error_Unauthenticated,
                (executionContext.CommandQueue.Peek() as DisplayErrorCommand).Error);
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
            mockAuthenticationRequiredCommand.Object.ExecuteWithAuthentication(null, executionContext);

            // Assert
            Assert.AreEqual(0, executionContext.CommandQueue.Count);
            mockAuthenticationRequiredCommand
                .Verify(mock => mock.ExecuteWithAuthentication(null, executionContext), Times.Once);
        }

    }
}