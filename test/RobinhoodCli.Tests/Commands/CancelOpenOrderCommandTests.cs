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
    public class CancelOpenOrderCommandTests
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
            var cancelOpenOrderCommand = new CancelOpenOrderCommand(0);

            // Assert
            Assert.IsNotNull(cancelOpenOrderCommand as AuthenticationRequiredCommand);
        }

        [TestMethod]
        public async Task ExecuteWithAuthentication_ShouldQueueDisplayError_WhenOrderIndexNotFound()
        {
            // Arrange
            var cancelOpenOrderCommand = new CancelOpenOrderCommand(0);
            var executionContext = new ExecutionContext();

            // Act
            await cancelOpenOrderCommand.ExecuteWithAuthentication(
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
        public async Task ExecuteWithAuthentication_ShouldQueueDisplayError_WhenCancelOrderCallFailed()
        {
            // Arrange
            var cancelOpenOrderCommand = new CancelOpenOrderCommand(1);
            mockRobinhoodClient
                .Setup(mock => mock.CancelOrder(It.IsAny<string>()))
                .ReturnsAsync(new Result<object>()
                {
                    IsSuccessStatusCode = false
                });
            var executionContext = new ExecutionContext()
            {
                OpenOrders = new List<OpenOrder>()
                {
                    new OpenOrder()
                    {
                        Index = 1,
                        Order = new Order()
                        {
                        }
                    }
                }
            };

            // Act
            await cancelOpenOrderCommand.ExecuteWithAuthentication(
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
        public async Task ExecuteWithAuthentication_ShouldQueueUpdateOpenOrdersCommand()
        {
            // Arrange
            var cancelOpenOrderCommand = new CancelOpenOrderCommand(1);
            mockRobinhoodClient
                .Setup(mock => mock.CancelOrder(It.IsAny<string>()))
                .ReturnsAsync(new Result<object>()
                {
                    IsSuccessStatusCode = true
                });
            var executionContext = new ExecutionContext()
            {
                OpenOrders = new List<OpenOrder>()
                {
                    new OpenOrder()
                    {
                        Index = 1,
                        Order = new Order()
                        {
                        }
                    }
                }
            };

            // Act
            await cancelOpenOrderCommand.ExecuteWithAuthentication(
                mockRobinhoodClient.Object,
                mockOutputService.Object,
                executionContext);

            // Assert
            Assert.AreEqual(1, executionContext.CommandQueue.Count);
            Assert.IsNotNull(executionContext.CommandQueue.Peek() as UpdateOpenOrdersCommand);
            // TODO: assert info message
        }

    }
}