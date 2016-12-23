using System;
using System.Linq;
using System.Threading.Tasks;
using Deadlock.Robinhood;
using RobinhoodCli.Models;

namespace RobinhoodCli.Commands
{
    internal class AccountCommand : AuthenticationRequiredCommand
    {

        public override async Task ExecuteWithAuthentication(
            IRobinhoodClient client,
            ExecutionContext context)
        {
            var result = await client.Accounts();
            if (!result.IsSuccessStatusCode)
            {
                context.ReplaceCommandQueueWithDisplayError($"Account listing failed: {result.Content}");
                return;
            }

            var activeAccount = result.Data.Results.FirstOrDefault(account => !account.Deactivated);
            if (activeAccount == null)
            {
                context.ReplaceCommandQueueWithDisplayError($"No active account found");
                return;
            }

            context.ActiveAccount = activeAccount;
            Console.WriteLine($"Active account is {activeAccount.AccountNumber}");
            Console.WriteLine();
        }

    }
}