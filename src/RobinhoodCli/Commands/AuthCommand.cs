using System.Threading.Tasks;
using Deadlock.Robinhood;

namespace RobinhoodCli.Commands
{
    public class AuthCommand : ICommand
    {

        public string AuthenticationToken { get; set; }

        public Task<ExecutionResult> Execute(
            IRobinhoodClient client,
            ExecutionContext context)
        {
            if (AuthenticationToken != null)
            {
                return Task.FromResult<ExecutionResult>(new AuthenticationExecutionResult()
                {
                    AuthenticationToken = AuthenticationToken
                });
            }

            return Task.FromResult(ExecutionResult.NoResult);
        }

    }
}