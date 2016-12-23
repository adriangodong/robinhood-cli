using System;
using System.Linq;
using System.Threading.Tasks;
using Deadlock.Robinhood.Model;
using Deadlock.Robinhood;
using RobinhoodCli.ExecutionResults;
using RobinhoodCli.Models;

namespace RobinhoodCli.Commands
{
    public class PrepareOrderCommand : ICommand
    {
        public OrderType Type { get; set; }
        public string Symbol { get; set; }
        public decimal? Size { get; set; }
        public decimal? LimitPrice { get; set; }

        public async Task<ExecutionResult> Execute(
            IRobinhoodClient client,
            ExecutionContext context)
        {
            if (context.AuthenticationToken == null)
            {
                return new ExecutionResult("Session is not authenticated. Use login command to authenticate.");
            }

            if (context.ActiveAccount == null)
            {
                return new ExecutionResult("No active account selected or found.");
            }

            string instrumentUrl = null;
            if (!Size.HasValue)
            {
                if (Type == OrderType.Buy)
                {
                    return new ExecutionResult("Buy order without size.");
                }
                if (Type == OrderType.Sell)
                {
                    // Get open position size from context
                    var openPosition = context.OpenPositions
                        .FirstOrDefault(op => string.Equals(op.Symbol, Symbol, StringComparison.OrdinalIgnoreCase));

                    // Else get open position from API
                    if (openPosition == null)
                    {
                        return new ExecutionResult("Can't find open position to sell. TODO");
                    }

                    Size = openPosition.Quantity;
                    instrumentUrl = openPosition.InstrumentUrl;
                }
            }

            var quoteResult = await client.Quote(Symbol);
            if (!quoteResult.IsSuccessStatusCode)
            {
                return new ExecutionResult("Failed to get quote for market order.");
            }

            if (!LimitPrice.HasValue)
            {
                // Market order, set price from quote
                LimitPrice = quoteResult.Data.AskPrice;
            }

            var accountUrl = $"https://api.robinhood.com/accounts/{context.ActiveAccount.AccountNumber}/";
            var newOrder = CreateNewOrder(accountUrl, quoteResult.Data.Instrument);
            return new PrepareOrderExecutionResult(newOrder);
        }

        internal NewOrder CreateNewOrder(string accountUrl, string instrumentUrl)
        {
            return new NewOrder()
            {
                Account = accountUrl,
                Instrument = instrumentUrl,
                Symbol = Symbol.ToUpper(),
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