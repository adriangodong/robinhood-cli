using System;
using System.Threading.Tasks;
using Deadlock.Robinhood;

namespace RobinhoodCli.Commands
{
    public class PositionsCommand : ICommand, ICommandParser
    {

        public async Task<ExecutionResult> Execute(string authenticationToken)
        {
            using (RobinhoodClient client = new RobinhoodClient(authenticationToken))
            {
                var positionResult = await client.Positions();
                if (positionResult.IsSuccessStatusCode)
                {
                    Console.WriteLine("Open positions:");
                    foreach (var position in positionResult.Data.Results)
                    {
                        if (position.Quantity > 0)
                        {
                            var instrumentResult = await client.Instrument(position.Instrument.Substring(38, 36));
                            if (instrumentResult.IsSuccessStatusCode)
                            {
                                Console.WriteLine($"{instrumentResult.Data.Symbol}: {position.Quantity}");
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

                    return ExecutionResult.NoResult;
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