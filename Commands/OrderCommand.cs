using System;
using System.Threading.Tasks;
using Deadlock.Robinhood;
using Deadlock.Robinhood.Model;

namespace RobinhoodCli.Commands
{
    public class OrderCommand : ICommand
    {
        public OrderType Type { get; set; }
        public string Symbol { get; set; }
        public int? Size { get; set; }
        public decimal? LimitPrice { get; set; }

        public async Task<ExecutionResult> Execute(string authenticationToken)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Sending order: {Type} {Symbol} - {Size} shares - ${LimitPrice} limit");
            Console.ForegroundColor = ConsoleColor.Black;

            using (RobinhoodClient client = new RobinhoodClient(authenticationToken))
            {
                var newOrder = CreateNewOrder();
                var newOrderResult = await client.Orders(newOrder);

                if (!newOrderResult.IsSuccessStatusCode)
                {
                    return new ExecutionResult()
                    {
                        LastError = newOrderResult.Content
                    };
                }
            }

            return ExecutionResult.NoResult;
        }

        internal NewOrder CreateNewOrder()
        {
            return new NewOrder()
            {
            };
        }
    }
}