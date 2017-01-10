using System.Collections.Generic;
using System.Threading.Tasks;
using Deadlock.Robinhood;
using Deadlock.Robinhood.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RobinhoodCli.Commands;
using RobinhoodCli.Models;
using RobinhoodCli.Services;

namespace RobinhoodCli.Tests.Commands
{
    [TestClass]
    public class AccountCommandTests
    {

        private Mock<IRobinhoodClient> mockRobinhoodClient;
        private Mock<IOutputService> mockOutputService;

        [TestInitialize]
        public void Init()
        {
            mockRobinhoodClient = new Mock<IRobinhoodClient>();
            mockOutputService = new Mock<IOutputService>();
        }

        [TestMethod]
        public void DerivedFromAuthenticationRequiredCommand()
        {
            // Arrange
            var accountCommand = new AccountCommand();

            // Assert
            Assert.IsNotNull(accountCommand as AuthenticationRequiredCommand);
        }

        [TestMethod]
        public async Task ExecuteWithAuthentication_ShouldQueueDisplayError_WhenAccountsCallFail()
        {
            // Arrange
            var accountCommand = new AccountCommand();
            mockRobinhoodClient
                .Setup(mock => mock.Accounts())
                .ReturnsAsync(new Result<Page<Account>>() { IsSuccessStatusCode = false });
            var executionContext = new ExecutionContext();

            // Act
            await accountCommand.ExecuteWithAuthentication(
                mockRobinhoodClient.Object,
                mockOutputService.Object,
                executionContext);

            // Assert
            Assert.AreEqual(0, executionContext.CommandQueue.Count);
            mockOutputService.Verify(
                mock => mock.Error(It.IsAny<string>()), // TODO: assert actual error message
                Times.Once);
        }

        [TestMethod]
        public async Task ExecuteWithAuthentication_ShouldQueueDisplayError_WhenNoActiveAccount()
        {
            // Arrange
            var inactiveAccount = new Account()
            {
                Deactivated = true
            };
            var accountCommand = new AccountCommand();
            mockRobinhoodClient
                .Setup(mock => mock.Accounts())
                .ReturnsAsync(new Result<Page<Account>>()
                {
                    IsSuccessStatusCode = true,
                    Data = new Page<Account>()
                    {
                        Results = new List<Account>()
                        {
                            inactiveAccount
                        }
                    }
            });
            var executionContext = new ExecutionContext();

            // Act
            await accountCommand.ExecuteWithAuthentication(
                mockRobinhoodClient.Object,
                mockOutputService.Object,
                executionContext);

            // Assert
            Assert.AreEqual(0, executionContext.CommandQueue.Count);
            mockOutputService.Verify(
                mock => mock.Error(It.IsAny<string>()), // TODO: assert actual error message
                Times.Once);
        }

        [TestMethod]
        public async Task ExecuteWithAuthentication_ShouldSetActiveAccount()
        {
            // Arrange
            var activeAccount = new Account()
            {
                Deactivated = false
            };
            var accountCommand = new AccountCommand();
            mockRobinhoodClient
                .Setup(mock => mock.Accounts())
                .ReturnsAsync(new Result<Page<Account>>()
                {
                    IsSuccessStatusCode = true,
                    Data = new Page<Account>()
                    {
                        Results = new List<Account>()
                        {
                            activeAccount
                        }
                    }
                });
            var executionContext = new ExecutionContext();

            // Act
            await accountCommand.ExecuteWithAuthentication(
                mockRobinhoodClient.Object,
                mockOutputService.Object,
                executionContext);

            // Assert
            Assert.AreEqual(0, executionContext.CommandQueue.Count);
            Assert.AreSame(activeAccount, executionContext.ActiveAccount);
        }

    }
}