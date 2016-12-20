using System.Linq;
using System.Threading.Tasks;
using Deadlock.Robinhood;

namespace RobinhoodCli.Commands
{
    public class AccountCommand : ICommand
    {
        public async Task<ExecutionResult> Execute(
            IRobinhoodClient client,
            ExecutionContext context)
        {
            if (context.AuthenticationToken == null)
            {
                return ExecutionResult.NoResult;
            }

            var result = await client.Accounts();
            if (!result.IsSuccessStatusCode)
            {
                return new ExecutionResult()
                {
                    LastError = $"Account listing failed: {result.Content}"
                };
            }

            return new AccountsExecutionResult()
            {
                Account = result.Data.Results.FirstOrDefault(account => !account.Deactivated)
            };
        }
    }
}