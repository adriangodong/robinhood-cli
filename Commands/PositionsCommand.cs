using System.Collections.Generic;
using System.Threading.Tasks;
using Deadlock.Robinhood;

namespace RobinhoodCli.Commands
{
    public class PositionsCommand : ICommand, ICommandParser
    {

        public async Task<ExecutionResult> Execute(ExecutionContext context)
        {
            using (RobinhoodClient client = new RobinhoodClient(context.AuthenticationToken))
            {
                var positionResult = await client.Positions(context.ActiveAccount);
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
        }

        public ICommand Parse(string[] commandTokens)
        {
            if (commandTokens[0] == "pos")
            {
                return new PositionsCommand();
            }

            return null;
        }

        public string LastError => null;

    }
}