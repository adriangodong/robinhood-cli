using Deadlock.Robinhood.Model;

namespace RobinhoodCli.Models
{
    internal class OpenOrder
    {
        public Order Order { get; set; }
        public Instrument Instrument { get; set; }

        public string GetInstrumentKey()
        {
            return Order?.Instrument?.Substring(38, 36);
        }
    }
}