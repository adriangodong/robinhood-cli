using System.Collections.Generic;
using System.Threading.Tasks;
using Deadlock.Robinhood;
using Deadlock.Robinhood.Model;
using RobinhoodCli.Models;
using RobinhoodCli.Services;

namespace RobinhoodCli.Commands
{
    internal class UpdateOpenPositionsCommand : ActiveAccountRequiredCommand
    {

        public override async Task ExecuteWithActiveAccount(
            IRobinhoodClient client,
            IOutputService output,
            ExecutionContext context,
            Account activeAccount)
        {
            var positionResult = await client.Positions(activeAccount.AccountNumber);
            if (!positionResult.IsSuccessStatusCode)
            {
                output.Error(positionResult.Content);
                return;
            }

            var openPositions = new List<OpenPosition>();
            foreach (var position in positionResult.Data.Results)
            {
                if (position.Quantity > 0)
                {
                    var openPosition = new OpenPosition();
                    openPosition.Position = position;

                    var instrumentResult = await client.Instrument(position.GetInstrumentKey());
                    if (instrumentResult.IsSuccessStatusCode)
                    {
                        openPosition.Instrument = instrumentResult.Data;
                    }
                    else
                    {
                        // TODO: replace with warning
                        output.Error(instrumentResult.Content);
                        return;
                    }

                    openPositions.Add(openPosition);
                }
            }

            // TODO: make ExecutionContext.OpenPositions setter internal like OpenOrders.
            context.OpenPositions = openPositions;
            output.OpenPositions(openPositions);
            output.ExitCommand();
        }

    }
}