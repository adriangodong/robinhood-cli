using System;
using System.Threading.Tasks;

namespace RobinhoodCli.Commands
{
    public class OrderCommand : ICommand
    {
        public OrderType Type { get; set; }
        public string Symbol { get; set; }
        public int? Size { get; set; }
        public decimal? LimitPrice { get; set; }

        public Task<ExecutionResult> Execute(string authenticationToken)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Sending order: {Type} {Symbol} - {Size} shares - ${LimitPrice} limit");
            Console.ForegroundColor = ConsoleColor.Black;

            return Task.FromResult(ExecutionResult.NoResult);
        }
    }
}