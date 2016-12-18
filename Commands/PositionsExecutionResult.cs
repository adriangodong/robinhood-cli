using System.Collections.Generic;

namespace RobinhoodCli.Commands
{
    public class PositionsExecutionResult : ExecutionResult
    {
        public List<OpenPosition> OpenPositions { get; set; }
    }

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