using System;
using System.Linq;
using System.Threading.Tasks;
using Deadlock.Robinhood.Model;
using Deadlock.Robinhood;
using RobinhoodCli.Models;

namespace RobinhoodCli.Commands
{
    internal class PrepareOrderCommand : ActiveAccountRequiredCommand
    {

        public OrderType Type { get; set; }
        public string Symbol { get; set; }
        public decimal? Size { get; set; }
        public decimal? LimitPrice { get; set; }

        public override async Task ExecuteWithActiveAccount(
            IRobinhoodClient client,
            ExecutionContext context)
        {

            var accountUrl = $"https://api.robinhood.com/accounts/{context.ActiveAccount.AccountNumber}/";
            var newOrder = new NewOrder()
            {
                Account = accountUrl,
                Symbol = Symbol.ToUpper(),
                Side = Type == OrderType.Buy ? Side.Buy : Side.Sell,
                TimeInForce = "gfd",
                Trigger = "immediate",
                Type = LimitPrice.HasValue ? "limit" : "market",
            };

            if (!Size.HasValue)
            {
                if (Type == OrderType.Buy)
                {
                    context.ReplaceCommandQueueWithDisplayError("Buy order without size.");
                    return;
                }
                if (Type == OrderType.Sell)
                {
                    // Get open position size from context
                    var openPosition = context.OpenPositions
                        .FirstOrDefault(op => string.Equals(op.Instrument.Symbol, Symbol, StringComparison.OrdinalIgnoreCase));

                    // Else get open position from API
                    if (openPosition == null)
                    {
                        context.ReplaceCommandQueueWithDisplayError("Can't find open position to sell. TODO");
                        return;
                    }

                    Size = openPosition.Position.Quantity;
                }
            }

            var quoteResult = await client.Quote(Symbol);
            if (!quoteResult.IsSuccessStatusCode)
            {
                context.ReplaceCommandQueueWithDisplayError("Failed to get quote for market order.");
                return;
            }

            if (!LimitPrice.HasValue)
            {
                // Market order, set price from quote
                LimitPrice = quoteResult.Data.AskPrice;
            }

            newOrder.Instrument = quoteResult.Data.Instrument;
            newOrder.Quantity = Size.Value;
            newOrder.Price = LimitPrice.Value;

            context.CommandQueue.Enqueue(new ExecuteOrderCommand(newOrder));
        }

    }
}