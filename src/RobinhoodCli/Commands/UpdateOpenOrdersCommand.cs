using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Deadlock.Robinhood;
using RobinhoodCli.Models;

namespace RobinhoodCli.Commands
{
    internal class UpdateOpenOrdersCommand : ActiveAccountRequiredCommand
    {

        public override async Task ExecuteWithActiveAccount(
            IRobinhoodClient client,
            ExecutionContext context)
        {
            var ordersResult = await client.Orders();
            if (!ordersResult.IsSuccessStatusCode)
            {
                context.ReplaceCommandQueueWithDisplayError(ordersResult.Content);
                return;
            }

            context.ClearOpenOrders();
            foreach (var order in ordersResult.Data.Results)
            {
                if (order.State == "confirmed")
                {
                    var instrumentResult = await client.Instrument(order.GetInstrumentKey());
                    if (!instrumentResult.IsSuccessStatusCode)
                    {
                        context.ReplaceCommandQueueWithDisplayError(instrumentResult.Content);
                        return;
                    }

                    context.AddOpenOrder(order, instrumentResult.Data);
                }
            }

            RenderOpenOrders(context.OpenOrders);
        }

        internal void RenderOpenOrders(List<OpenOrder> openOrders)
        {
            Console.WriteLine("Open orders:");
            foreach (var openOrder in openOrders)
            {
                Console.WriteLine($"[{openOrder.Index}] {openOrder.Order.Side} {openOrder.Instrument.Symbol} {openOrder.Order.Quantity} {openOrder.Order.Price}");
            }
            Console.WriteLine();
        }

    }
}