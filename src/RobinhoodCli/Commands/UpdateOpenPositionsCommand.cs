using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Deadlock.Robinhood;
using Deadlock.Robinhood.Model;
using RobinhoodCli.Models;

namespace RobinhoodCli.Commands
{
    internal class UpdateOpenPositionsCommand : ActiveAccountRequiredCommand
    {

        public override async Task ExecuteWithActiveAccount(
            IRobinhoodClient client,
            ExecutionContext context,
            Account activeAccount)
        {
            var positionResult = await client.Positions(activeAccount.AccountNumber);
            if (!positionResult.IsSuccessStatusCode)
            {
                context.ReplaceCommandQueueWithDisplayError(positionResult.Content);
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
                        context.ReplaceCommandQueueWithDisplayError(instrumentResult.Content);
                        return;
                    }

                    openPositions.Add(openPosition);
                }
            }

            // TODO: make ExecutionContext.OpenPositions setter internal like OpenOrders.
            context.OpenPositions = openPositions;
            RenderOpenPositions(openPositions);
        }

        internal void RenderOpenPositions(List<OpenPosition> openPositions)
        {
            Console.WriteLine("Open positions:");
            foreach (var openPosition in openPositions)
            {
                Console.WriteLine($"{openPosition.Instrument.Symbol}: {openPosition.Position.Quantity}");
            }
            Console.WriteLine();
        }

    }
}