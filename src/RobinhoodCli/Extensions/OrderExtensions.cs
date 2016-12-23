using Deadlock.Robinhood.Model;

namespace RobinhoodCli.Models
{
    internal static class OrderExtensions
    {
        public static string GetInstrumentKey(this Order order)
        {
            return order?.Instrument?.Substring(38, 36);
        }
    }
}