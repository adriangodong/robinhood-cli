using System.Linq;
using System.Threading.Tasks;
using Deadlock.Robinhood;
using RobinhoodCli.Models;
using RobinhoodCli.Services;

namespace RobinhoodCli.Commands
{
    internal class AccountCommand : AuthenticationRequiredCommand
    {

        public const string Error_AccountsFailed = "Account listing failed.";
        public const string Error_NoActiveAccountFound = "No active account found.";

        public override async Task ExecuteWithAuthentication(
            IRobinhoodClient client,
            IOutputService output,
            ExecutionContext context)
        {
            var result = await client.Accounts();
            if (!result.IsSuccessStatusCode)
            {
                context.CommandQueue.Clear();
                output.ErrorWithContent(Error_AccountsFailed, result.Content);
                return;
            }

            var activeAccount = result.Data.Results.FirstOrDefault(account => !account.Deactivated);
            if (activeAccount == null)
            {
                context.CommandQueue.Clear();
                output.Error(Error_NoActiveAccountFound);
                return;
            }

            context.ActiveAccount = activeAccount;
            output.Success($"Active account is {activeAccount.AccountNumber}");
            output.Info($"Available cash: ${activeAccount.Cash}");
            output.Info($"Withdrawable cash: ${activeAccount.CashAvailableForWithdrawal}");
            output.Info($"Uncleared deposits: ${activeAccount.UnclearedDeposits}");
            output.Info($"Unsettled funds: ${activeAccount.UnsettledFunds}");
            output.Info($"Buying power: ${activeAccount.Cash + activeAccount.UnsettledFunds}");
            output.ExitCommand();
        }

    }
}