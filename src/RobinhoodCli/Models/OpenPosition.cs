using Deadlock.Robinhood.Model;

namespace RobinhoodCli.Models
{
    internal class OpenPosition
    {
        public Position Position { get; set; }
        public Instrument Instrument { get; set; }
    }
}