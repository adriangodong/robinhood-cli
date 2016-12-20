using System.Collections.Generic;

namespace RobinhoodCli
{
    public class ExecutionContext
    {
        public string AuthenticationToken { get; set; }
        public string ActiveAccount { get; set; }
        public Queue<Commands.ICommand> CommandQueue { get; private set; }

        public ExecutionContext()
        {
            CommandQueue = new Queue<Commands.ICommand>();
        }
    }
}