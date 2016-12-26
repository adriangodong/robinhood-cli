using System;
using System.Collections.Generic;
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
    public class UpdateOpenOrdersCommandTests
    {

        private Mock<IRobinhoodClient> mockRobinhoodClient;

        [TestInitialize]
        public void Init()
        {
            mockRobinhoodClient = new Mock<IRobinhoodClient>();
        }

        [TestMethod]
        public void DerivedFromAciveAccountRequiredCommand()
        {
            // Arrange
            var updateOpenOrdersCommand = new UpdateOpenOrdersCommand();

            // Assert
            Assert.IsNotNull(updateOpenOrdersCommand as ActiveAccountRequiredCommand);
        }

        [TestMethod]
        public async Task ExecuteWithActiveAccount_ShouldQueueDisplayError_WhenOrdersCallFail()
        {
            // Arrange
            var resultContent = Guid.NewGuid().ToString("N");
            var updateOpenOrdersCommand = new UpdateOpenOrdersCommand();
            mockRobinhoodClient
                .Setup(mock => mock.Orders())
                .ReturnsAsync(new Result<Page<Order>>()
                {
                    Content = resultContent,
                    IsSuccessStatusCode = false
                });
            var executionContext = new ExecutionContext();

            // Act
            await updateOpenOrdersCommand.ExecuteWithActiveAccount(mockRobinhoodClient.Object, executionContext);

            // Assert
            Assert.AreEqual(1, executionContext.CommandQueue.Count);
            Assert.IsNotNull(executionContext.CommandQueue.Peek() as DisplayErrorCommand);
            Assert.AreEqual(
                resultContent,
                (executionContext.CommandQueue.Peek() as DisplayErrorCommand).Error);
        }

        [TestMethod]
        public async Task ExecuteWithActiveAccount_ShouldReplaceExistingOpenOrders()
        {
            // Arrange
            var updateOpenOrdersCommand = new UpdateOpenOrdersCommand();
            mockRobinhoodClient
                .Setup(mock => mock.Orders())
                .ReturnsAsync(new Result<Page<Order>>()
                {
                    IsSuccessStatusCode = true,
                    Data = new Page<Order>()
                    {
                        Results = new List<Order>()
                        {
                            new Order()
                            {
                                State = "confirmed"
                            }
                        }
                    }
                });
            mockRobinhoodClient
                .Setup(mock => mock.Instrument(It.IsAny<string>()))
                .ReturnsAsync(new Result<Instrument>()
                {
                    IsSuccessStatusCode = true,
                    Data = new Instrument()
                });
            var executionContext = new ExecutionContext()
            {
                OpenOrders = new List<OpenOrder>()
                {
                    new OpenOrder()
                }
            };

            // Act
            await updateOpenOrdersCommand.ExecuteWithActiveAccount(mockRobinhoodClient.Object, executionContext);

            // Assert
            Assert.AreEqual(1, executionContext.OpenOrders.Count);
        }

    }
}