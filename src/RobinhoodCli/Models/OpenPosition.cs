using Deadlock.Robinhood.Model;

namespace RobinhoodCli.Models
{
    public class OpenPosition
    {
        public Position Position { get; set; }
        public Instrument Instrument { get; set; }

        public string GetInstrumentKey()
        {
            return Position?.Instrument?.Substring(38, 36);
        }
    }
}