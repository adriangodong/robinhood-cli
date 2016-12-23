using Deadlock.Robinhood.Model;

namespace RobinhoodCli.Models
{
    internal class OpenOrder
    {
        public int Index { get; set; }
        public Order Order { get; set; }
        public Instrument Instrument { get; set; }
    }
}