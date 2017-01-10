using System;
using System.Threading.Tasks;
using Deadlock.Robinhood.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RobinhoodCli.Commands;
using RobinhoodCli.Models;

namespace RobinhoodCli.Tests.Commands
{
    [TestClass]
    public class SetAuthenticationTokenCommandTests
    {

        [TestMethod]
        public async Task Execute_ShouldSetExectionContextAuthenticationToken()
        {
            // Arrange
            var authenticationToken = Guid.NewGuid().ToString("N");
            var setAuthenticationTokenCommand = new SetAuthenticationTokenCommand(authenticationToken);
            var executionContext = new ExecutionContext();

            // Act
            await setAuthenticationTokenCommand.Execute(null, null, executionContext);

            // Assert
            Assert.AreEqual(authenticationToken, executionContext.AuthenticationToken);
        }

        [TestMethod]
        public async Task Execute_ShouldUnsetExectionContextActiveAccount_WhenAuthenticationTokenIsNull()
        {
            // Arrange
            var setAuthenticationTokenCommand = new SetAuthenticationTokenCommand(null);
            var executionContext = new ExecutionContext()
            {
                ActiveAccount = new Account()
            };

            // Act
            await setAuthenticationTokenCommand.Execute(null, null, executionContext);

            // Assert
            Assert.IsNull(executionContext.ActiveAccount);
        }

        [TestMethod]
        public async Task Execute_ShouldQueueUpdateCommands_WhenAuthenticationTokenIsNotNull()
        {
            // Arrange
            var authenticationToken = Guid.NewGuid().ToString("N");
            var setAuthenticationTokenCommand = new SetAuthenticationTokenCommand(authenticationToken);
            var executionContext = new ExecutionContext();

            // Act
            await setAuthenticationTokenCommand.Execute(null, null, executionContext);

            // Assert
            Assert.AreEqual(3, executionContext.CommandQueue.Count);
            Assert.IsNotNull(executionContext.CommandQueue.Dequeue() as AccountCommand);
            Assert.IsNotNull(executionContext.CommandQueue.Dequeue() as UpdateOpenPositionsCommand);
            Assert.IsNotNull(executionContext.CommandQueue.Dequeue() as UpdateOpenOrdersCommand);
        }

    }
}