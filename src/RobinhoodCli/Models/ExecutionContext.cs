using System.Collections.Generic;
using Deadlock.Robinhood.Model;
using RobinhoodCli.Commands;

namespace RobinhoodCli.Models
{
    internal class ExecutionContext
    {

        public string AuthenticationToken { get; set; }
        public Account ActiveAccount { get; set; }
        public List<OpenPosition> OpenPositions { get; set; }
        public List<OpenOrder> OpenOrders { get; internal set; }
        public Queue<ICommand> CommandQueue { get; private set; }

        public ExecutionContext()
        {
            CommandQueue = new Queue<ICommand>();
            OpenOrders = new List<OpenOrder>();
        }

        public void ReplaceCommandQueueWithDisplayError(string error)
        {
            CommandQueue.Clear();
            CommandQueue.Enqueue(new DisplayErrorCommand(error));
        }

        public void AddOpenOrder(Order order, Instrument instrument)
        {
            OpenOrders.Add(new OpenOrder()
            {
                Index = GetNextOpenOrderIndex(),
                Order = order,
                Instrument = instrument
            });
        }

        public void ClearOpenOrders()
        {
            OpenOrders.Clear();
        }

        internal int GetNextOpenOrderIndex()
        {
            int maxIndex = 0;

            foreach (var openOrder in OpenOrders)
            {
                if (maxIndex < openOrder.Index)
                {
                    maxIndex = openOrder.Index;
                }
            }

            return ++maxIndex;
        }

    }
}