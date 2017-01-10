using Deadlock.Robinhood.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RobinhoodCli.Commands;
using RobinhoodCli.Models;
using RobinhoodCli.Services;

namespace RobinhoodCli.Tests.Commands
{
    [TestClass]
    public class ActiveAccountRequiredCommandTests
    {

        private Mock<IOutputService> mockOutputService;

        // We need to use a Mock here because ActiveAccountRequiredCommand is abstract.
        private Mock<ActiveAccountRequiredCommand> mockActiveAccountRequiredCommand;

        [TestInitialize]
        public void Init()
        {
            mockOutputService = new Mock<IOutputService>();
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
            mockActiveAccountRequiredCommand.Object.ExecuteWithAuthentication(
                null,
                mockOutputService.Object,
                executionContext);

            // Assert
            Assert.AreEqual(0, executionContext.CommandQueue.Count);
            mockOutputService.Verify(
                mock => mock.Error(ActiveAccountRequiredCommand.Error_NoActiveAccount),
                Times.Once);
        }

        [TestMethod]
        public void ExecuteWithAuthentication_ShouldCallExecuteWithActiveAccount_WhenActiveAccountSet()
        {
            // Arrange
            var account = new Account();
            var executionContext = new ExecutionContext()
            {
                ActiveAccount = account
            };

            // Act
            mockActiveAccountRequiredCommand.Object.ExecuteWithAuthentication(null, null, executionContext);

            // Assert
            Assert.AreEqual(0, executionContext.CommandQueue.Count);
            mockActiveAccountRequiredCommand
                .Verify(mock => mock.ExecuteWithActiveAccount(
                    null, null, executionContext, account), Times.Once);
        }

    }
}