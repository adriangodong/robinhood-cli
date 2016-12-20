using System.Collections.Generic;
using Deadlock.Robinhood.Model;
using RobinhoodCli.Commands;

namespace RobinhoodCli.Models
{
    public class ExecutionContext
    {
        public string AuthenticationToken { get; set; }
        public Account ActiveAccount { get; set; }
        public List<OpenPosition> OpenPositions { get; set; }

        public Queue<ICommand> CommandQueue { get; private set; }

        public ExecutionContext()
        {
            CommandQueue = new Queue<ICommand>();
        }
    }
}