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

            var openOrders = new List<OpenOrder>();
            foreach (var order in ordersResult.Data.Results)
            {
                if (order.State == "confirmed")
                {
                    var openOrder = new OpenOrder();
                    openOrder.Order = order;

                    var instrumentResult = await client.Instrument(openOrder.GetInstrumentKey());
                    if (instrumentResult.IsSuccessStatusCode)
                    {
                        openOrder.Instrument = instrumentResult.Data;
                    }
                    else
                    {
                        context.ReplaceCommandQueueWithDisplayError(instrumentResult.Content);
                        return;
                    }

                    openOrders.Add(openOrder);
                }
            }
            context.OpenOrders = openOrders;
            RenderOpenOrders(openOrders);
        }

        internal void RenderOpenOrders(List<OpenOrder> openOrders)
        {
            Console.WriteLine();
            Console.WriteLine("Open orders:");
            foreach (var openOrder in openOrders)
            {
                Console.WriteLine($"{openOrder.Order.Side} {openOrder.Instrument.Symbol} {openOrder.Order.Quantity} {openOrder.Order.Price}");
            }
        }

    }
}