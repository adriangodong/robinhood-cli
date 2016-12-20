using System.Collections.Generic;
using Deadlock.Robinhood.Model;

namespace RobinhoodCli
{
    public class ExecutionContext
    {
        public string AuthenticationToken { get; set; }
        public Account ActiveAccount { get; set; }
        public Queue<Commands.ICommand> CommandQueue { get; private set; }

        public ExecutionContext()
        {
            CommandQueue = new Queue<Commands.ICommand>();
        }
    }
}