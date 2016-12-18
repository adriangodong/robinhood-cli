using System;

namespace RobinhoodCli.Commands
{
    public class OrderCommand : ICommand
    {
        public OrderType Type { get; set; }
        public string Symbol { get; set; }
        public int? Size { get; set; }
        public decimal? LimitPrice { get; set; }

        public void Execute()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Sending order: {Type} {Symbol} - {Size} shares - ${LimitPrice} limit");
            Console.ForegroundColor = ConsoleColor.Black;
        }
    }
}