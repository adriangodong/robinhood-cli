using System.Collections.Generic;
using Deadlock.Robinhood.Model;
using RobinhoodCli.Commands;

namespace RobinhoodCli.Models
{
    internal class ExecutionContext
    {
        public string AuthenticationToken { get; set; }
        public Account ActiveAccount { get; set; }
        public List<OpenPosition> OpenPositions { get; set; }
        public List<OpenOrder> OpenOrders { get; set; }
        public Queue<ICommand> CommandQueue { get; private set; }

        public ExecutionContext()
        {
            CommandQueue = new Queue<ICommand>();
        }

        public void ReplaceCommandQueueWithDisplayError(string error)
        {
            CommandQueue.Clear();
            CommandQueue.Enqueue(new DisplayErrorCommand(error));
        }
    }
}