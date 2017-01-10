using System.Linq;
using System.Threading.Tasks;
using Deadlock.Robinhood;
using RobinhoodCli.Models;
using RobinhoodCli.Services;

namespace RobinhoodCli.Commands
{
    internal class AccountCommand : AuthenticationRequiredCommand
    {

        public override async Task ExecuteWithAuthentication(
            IRobinhoodClient client,
            IOutputService output,
            ExecutionContext context)
        {
            var result = await client.Accounts();
            if (!result.IsSuccessStatusCode)
            {
                context.CommandQueue.Clear();
                output.Error($"Account listing failed: {result.Content}");
                return;
            }

            var activeAccount = result.Data.Results.FirstOrDefault(account => !account.Deactivated);
            if (activeAccount == null)
            {
                context.CommandQueue.Clear();
                output.Error($"No active account found");
                return;
            }

            context.ActiveAccount = activeAccount;
            output.Info($"Active account is {activeAccount.AccountNumber}");
            output.ExitCommand();
        }

    }
}