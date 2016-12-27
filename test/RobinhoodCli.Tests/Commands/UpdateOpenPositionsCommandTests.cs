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
    public class UpdateOpenPositionsCommandTests
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
            var updateOpenPositionsCommand = new UpdateOpenPositionsCommand();

            // Assert
            Assert.IsNotNull(updateOpenPositionsCommand as ActiveAccountRequiredCommand);
        }

        [TestMethod]
        public async Task ExecuteWithActiveAccount_ShouldQueueDisplayError_WhenPositionsCallFail()
        {
            // Arrange
            var resultContent = Guid.NewGuid().ToString("N");
            var updateOpenPositionsCommand = new UpdateOpenPositionsCommand();
            mockRobinhoodClient
                .Setup(mock => mock.Positions(It.IsAny<string>()))
                .ReturnsAsync(new Result<Page<Position>>()
                {
                    Content = resultContent,
                    IsSuccessStatusCode = false
                });
            var executionContext = new ExecutionContext()
            {
                ActiveAccount = new Account()
            };

            // Act
            await updateOpenPositionsCommand.ExecuteWithActiveAccount(mockRobinhoodClient.Object, executionContext);

            // Assert
            Assert.AreEqual(1, executionContext.CommandQueue.Count);
            Assert.IsNotNull(executionContext.CommandQueue.Peek() as DisplayErrorCommand);
            Assert.AreEqual(
                resultContent,
                (executionContext.CommandQueue.Peek() as DisplayErrorCommand).Error);
        }

        [TestMethod]
        public async Task ExecuteWithActiveAccount_ShouldReplaceExistingOpenPositions()
        {
            // Arrange
            var updateOpenPositionsCommand = new UpdateOpenPositionsCommand();
            mockRobinhoodClient
                .Setup(mock => mock.Positions(It.IsAny<string>()))
                .ReturnsAsync(new Result<Page<Position>>()
                {
                    IsSuccessStatusCode = true,
                    Data = new Page<Position>()
                    {
                        Results = new List<Position>()
                        {
                            new Position()
                            {
                                Quantity = 100
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
                ActiveAccount = new Account(),
                OpenPositions = new List<OpenPosition>()
                {
                    new OpenPosition()
                }
            };

            // Act
            await updateOpenPositionsCommand.ExecuteWithActiveAccount(mockRobinhoodClient.Object, executionContext);

            // Assert
            Assert.AreEqual(1, executionContext.OpenPositions.Count);
        }

    }
}