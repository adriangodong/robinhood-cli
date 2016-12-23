using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Deadlock.Robinhood;
using RobinhoodCli.Models;

namespace RobinhoodCli.Commands
{
    internal class UpdateOpenPositionsCommand : ActiveAccountRequiredCommand
    {

        public override async Task ExecuteWithActiveAccount(
            IRobinhoodClient client,
            ExecutionContext context)
        {
            var positionResult = await client.Positions(context.ActiveAccount.AccountNumber);
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

                    var instrumentResult = await client.Instrument(openPosition.GetInstrumentKey());
                    if (instrumentResult.IsSuccessStatusCode)
                    {
                        openPosition.Instrument = instrumentResult.Data;
                    }
                    else
                    {
                        context.ReplaceCommandQueueWithDisplayError(instrumentResult.Content);
                        return;
                    }

                    openPositions.Add(openPosition);
                }
            }

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