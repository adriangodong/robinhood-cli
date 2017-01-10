using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RobinhoodCli.Commands;
using RobinhoodCli.Models;
using RobinhoodCli.Services;

namespace RobinhoodCli.Tests.Commands
{
    [TestClass]
    public class LogoutCommandTests
    {

        private Mock<IOutputService> mockOutputService;

        [TestInitialize]
        public void Init()
        {
            mockOutputService = new Mock<IOutputService>();
        }

        [TestMethod]
        public async Task Execute_ShouldQueueSaveAuthenticationTokenCommandAndSetAuthenticationTokenCommand()
        {
            // Arrange
            var logoutCommand = new LogoutCommand();
            var executionContext = new ExecutionContext();

            // Act
            await logoutCommand.Execute(
                null,
                mockOutputService.Object,
                executionContext);

            // Assert
            Assert.AreEqual(2, executionContext.CommandQueue.Count);
            Assert.IsNotNull(executionContext.CommandQueue.Peek() as SaveAuthenticationTokenCommand);
            Assert.IsNull((executionContext.CommandQueue.Peek() as SaveAuthenticationTokenCommand).AuthenticationToken);
            executionContext.CommandQueue.Dequeue();
            Assert.IsNotNull(executionContext.CommandQueue.Peek() as SetAuthenticationTokenCommand);
            Assert.IsNull((executionContext.CommandQueue.Peek() as SetAuthenticationTokenCommand).AuthenticationToken);
        }

    }
}