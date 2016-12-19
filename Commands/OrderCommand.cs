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

        public async Task<ExecutionResult> Execute(ExecutionContext context)
        {
            if (context.AuthenticationToken == null)
            {
                return new ExecutionResult()
                {
                    LastError = "Session is not authenticated. Use login command to authenticate."
                };
            }

            if (context.ActiveAccount == null)
            {
                return new ExecutionResult()
                {
                    LastError = "No active account configured. TODO."
                };
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Sending order: {Type} {Symbol} - {Size} shares - ${LimitPrice} limit");
            Console.ForegroundColor = ConsoleColor.Black;

            using (RobinhoodClient client = new RobinhoodClient(context.AuthenticationToken))
            {
                var newOrder = CreateNewOrder(context.ActiveAccount);
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

        internal NewOrder CreateNewOrder(string accountUrl)
        {
            return new NewOrder()
            {
                Account = accountUrl,
                // Instrument
                Symbol = Symbol,
                Side = Type == OrderType.Buy ? Side.Buy : Side.Sell,
                TimeInForce = "gfd",
                Trigger = "immediate",
                Type = LimitPrice.HasValue ? "limit" : "market",
                // Price
                // Quantity
            };
        }
    }
}