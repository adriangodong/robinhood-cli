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
    public class ExecuteOrderCommandTests
    {

        private Mock<IRobinhoodClient> mockRobinhoodClient;

        [TestInitialize]
        public void Init()
        {
            mockRobinhoodClient = new Mock<IRobinhoodClient>();
        }

        [TestMethod]
        public void DerivedFromAuthenticationRequiredCommand()
        {
            // Arrange
            var executeOrderCommand = new ExecuteOrderCommand(null);

            // Assert
            Assert.IsNotNull(executeOrderCommand as AuthenticationRequiredCommand);
        }

        [TestMethod]
        public async Task ExecuteWithAuthentication_ShouldQueueDisplayError_WhenOrdersCallFail()
        {
            // Arrange
            var executeOrderCommand = new ExecuteOrderCommand(new NewOrder());
            mockRobinhoodClient
                .Setup(mock => mock.Orders(It.IsAny<NewOrder>()))
                .ReturnsAsync(new Result<Order>()
                {
                    IsSuccessStatusCode = false
                });
            var executionContext = new ExecutionContext();

            // Act
            await executeOrderCommand.ExecuteWithAuthentication(mockRobinhoodClient.Object, executionContext);

            // Assert
            Assert.AreEqual(1, executionContext.CommandQueue.Count);
            Assert.IsNotNull(executionContext.CommandQueue.Peek() as DisplayErrorCommand);
            // TODO: assert error message
        }

        [TestMethod]
        public async Task ExecuteWithAuthentication_ShouldQueueUpdateOpenOrdersCommand()
        {
            // Arrange
            var activeAccount = new Account()
            {
                Deactivated = false
            };
            var executeOrderCommand = new ExecuteOrderCommand(new NewOrder());
            mockRobinhoodClient
                .Setup(mock => mock.Orders(It.IsAny<NewOrder>()))
                .ReturnsAsync(new Result<Order>()
                {
                    IsSuccessStatusCode = true
                });
            var executionContext = new ExecutionContext();

            // Act
            await executeOrderCommand.ExecuteWithAuthentication(mockRobinhoodClient.Object, executionContext);

            // Assert
            Assert.AreEqual(1, executionContext.CommandQueue.Count);
            Assert.IsNotNull(executionContext.CommandQueue.Peek() as UpdateOpenOrdersCommand);
        }

    }
}