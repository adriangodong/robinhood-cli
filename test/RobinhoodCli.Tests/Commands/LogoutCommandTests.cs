using System;
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
    public class LogoutCommandTests
    {

        [TestMethod]
        public async Task Execute_ShouldQueueSaveAuthenticationTokenCommandAndSetAuthenticationTokenCommand()
        {
            // Arrange
            var logoutCommand = new LogoutCommand();
            var executionContext = new ExecutionContext();

            // Act
            await logoutCommand.Execute(null, executionContext);

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