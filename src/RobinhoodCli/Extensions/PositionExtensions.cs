using Deadlock.Robinhood.Model;

namespace RobinhoodCli.Models
{
    internal static class PositionExtensions
    {
        public static string GetInstrumentKey(this Position position)
        {
            return position?.Instrument?.Substring(38, 36);
        }
    }
}