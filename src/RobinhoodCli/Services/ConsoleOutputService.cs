using System;
using System.Collections.Generic;
using RobinhoodCli.Models;

namespace RobinhoodCli.Services
{
    internal class ConsoleOutputService : IOutputService
    {
        public void Info(string message)
        {
            Console.WriteLine(message);
        }

        public void Success(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"ERROR: {message}");
            Console.WriteLine();
            Console.ResetColor();
        }

        public void OpenOrders(List<OpenOrder> openOrders)
        {
            Console.WriteLine("Open orders:");
            foreach (var openOrder in openOrders)
            {
                Console.WriteLine($"[{openOrder.Index}] {openOrder.Order.Side} {openOrder.Instrument.Symbol} {openOrder.Order.Quantity} {openOrder.Order.Price}");
            }
        }

        public void OpenPositions(List<OpenPosition> openPositions)
        {
            Console.WriteLine("Open positions:");
            foreach (var openPosition in openPositions)
            {
                Console.WriteLine($"{openPosition.Instrument.Symbol}: {openPosition.Position.Quantity}");
            }
        }

        public void ExitCommand()
        {
            Console.WriteLine();
        }
    }
}