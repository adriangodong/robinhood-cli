namespace RobinhoodCli.Models
{
    public class OpenPosition
    {
        public string AccountUrl { get; set; }
        public string InstrumentUrl { get; set; }
        public string Symbol { get; set; }
        public decimal Quantity { get; set; }

        public string GetInstrumentKey()
        {
            return InstrumentUrl.Substring(38, 36);
        }
    }
}