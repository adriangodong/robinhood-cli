using Deadlock.Robinhood.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RobinhoodCli.Commands;
using RobinhoodCli.Models;

namespace RobinhoodCli.Tests.Commands
{
    [TestClass]
    public class ActiveAccountRequiredCommandTests
    {

        Mock<ActiveAccountRequiredCommand> mockActiveAccountRequiredCommand;

        [TestInitialize]
        public void Init()
        {
            mockActiveAccountRequiredCommand = new Mock<ActiveAccountRequiredCommand>()
            {
                CallBase = true
            };
        }

        [TestMethod]
        public void DerivedFromAuthenticationRequiredCommand()
        {
            // Assert
            Assert.IsNotNull(mockActiveAccountRequiredCommand.Object as AuthenticationRequiredCommand);
        }

        [TestMethod]
        public void ExecuteWithAuthentication_ShouldQueueDisplayError_WhenNoActiveAccount()
        {
            // Arrange
            var executionContext = new ExecutionContext();

            // Act
            mockActiveAccountRequiredCommand.Object.ExecuteWithAuthentication(null, executionContext);

            // Assert
            Assert.AreEqual(1, executionContext.CommandQueue.Count);
            Assert.IsNotNull(executionContext.CommandQueue.Peek() as DisplayErrorCommand);
            Assert.AreEqual(
                ActiveAccountRequiredCommand.Error_NoActiveAccount,
                (executionContext.CommandQueue.Peek() as DisplayErrorCommand).Error);
        }

        [TestMethod]
        public void ExecuteWithAuthentication_ShouldCallExecuteWithActiveAccount_WhenActiveAccountSet()
        {
            // Arrange
            var executionContext = new ExecutionContext()
            {
                ActiveAccount = new Account()
            };

            // Act
            mockActiveAccountRequiredCommand.Object.ExecuteWithAuthentication(null, executionContext);

            // Assert
            Assert.AreEqual(0, executionContext.CommandQueue.Count);
            mockActiveAccountRequiredCommand
                .Verify(mock => mock.ExecuteWithActiveAccount(null, executionContext), Times.Once);
        }

    }
}