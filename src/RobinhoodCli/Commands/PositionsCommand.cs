using System.Collections.Generic;
using System.Threading.Tasks;
using Deadlock.Robinhood;
using RobinhoodCli.ExecutionResults;
using RobinhoodCli.Models;

namespace RobinhoodCli.Commands
{
    public class PositionsCommand : ICommand
    {

        public async Task<ExecutionResult> Execute(
            IRobinhoodClient client,
            ExecutionContext context)
        {
            var positionResult = await client.Positions(context.ActiveAccount.AccountNumber);
            if (positionResult.IsSuccessStatusCode)
            {
                var openPositions = new List<OpenPosition>();
                foreach (var position in positionResult.Data.Results)
                {
                    if (position.Quantity > 0)
                    {
                        var openPosition = new OpenPosition();
                        openPosition.AccountUrl = position.Account;
                        openPosition.InstrumentUrl = position.Instrument;

                        var instrumentResult = await client.Instrument(openPosition.GetInstrumentKey());
                        if (instrumentResult.IsSuccessStatusCode)
                        {
                            openPosition.Symbol = instrumentResult.Data.Symbol;
                            openPosition.Quantity = position.Quantity;
                            openPositions.Add(openPosition);
                        }
                        else
                        {
                            return new ExecutionResult()
                            {
                                LastError = instrumentResult.Content
                            };
                        }
                    }
                }

                return new PositionsExecutionResult()
                {
                    OpenPositions = openPositions
                };
            }
            else
            {
                return new ExecutionResult()
                {
                    LastError = positionResult.Content
                };
            }
        }

        public string LastError => null;

    }
}