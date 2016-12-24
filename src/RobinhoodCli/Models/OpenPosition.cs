using Deadlock.Robinhood.Model;

namespace RobinhoodCli.Models
{
    public class OpenPosition
    {
        public Position Position { get; set; }
        public Instrument Instrument { get; set; }
    }
}