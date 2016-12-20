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
                return new ExecutionResult("Session is not authenticated. Use login command to authenticate.");
            }

            if (context.ActiveAccount == null)
            {
                return new ExecutionResult("No active account selected or found.");
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Sending order: {Type} {Symbol} - {Size} shares - ${LimitPrice} limit");
            Console.ForegroundColor = ConsoleColor.Black;

            using (RobinhoodClient client = new RobinhoodClient(context.AuthenticationToken))
            {
                if (!Size.HasValue)
                {
                    if (Type == OrderType.Buy)
                    {
                        return new ExecutionResult("Buy order without size.");
                    }
                    if (Type == OrderType.Sell)
                    {
                        // Get open position size from context
                        // Else get open position from API
                    }
                }

                if (!LimitPrice.HasValue)
                {
                    // Market order, get quote for price
                    var symbolResult = await client.Quote(Symbol);
                    if (!symbolResult.IsSuccessStatusCode)
                    {
                        return new ExecutionResult("Failed to get quote for market order.");
                    }

                    LimitPrice = symbolResult.Data.AskPrice;
                }

                var accountUrl = $"https://api.robinhood.com/accounts/{context.ActiveAccount.AccountNumber}/";
                var newOrder = CreateNewOrder(accountUrl);
                var newOrderResult = await client.Orders(newOrder);

                if (!newOrderResult.IsSuccessStatusCode)
                {
                    return new ExecutionResult(newOrderResult.Content);
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
                Price = LimitPrice.Value,
                Quantity = Size.Value
            };
        }
    }
}