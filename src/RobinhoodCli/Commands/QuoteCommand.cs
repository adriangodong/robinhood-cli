using System.Threading.Tasks;
using Deadlock.Robinhood;
using RobinhoodCli.Models;
using RobinhoodCli.Services;

namespace RobinhoodCli.Commands
{
    internal class QuoteCommand : AuthenticationRequiredCommand
    {

        internal string Symbol { get; private set; }

        public QuoteCommand(string symbol)
        {
            Symbol = symbol;
        }

        public override async Task ExecuteWithAuthentication(
            IRobinhoodClient client,
            IOutputService output,
            ExecutionContext context)
        {
            var result = await client.Quote(Symbol);
            if (!result.IsSuccessStatusCode)
            {
                output.Error($"Quote failed: {result.Content}");
                return;
            }

            output.Info($"Bid: ${result.Data.BidPrice}x{result.Data.BidSize} | Ask: ${result.Data.AskPrice}x{result.Data.AskSize}");
            output.ExitCommand();
        }

    }
}