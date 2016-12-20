using System;
using System.Collections.Generic;
using RobinhoodCli.Models;

namespace RobinhoodCli.ExecutionResults
{
    public class PositionsExecutionResult : ExecutionResult
    {
        public List<OpenPosition> OpenPositions { get; set; }

        public override void UpdateExecutionContext(ExecutionContext executionContext)
        {
            executionContext.OpenPositions = OpenPositions;
        }

        public override void RenderResult()
        {
            if (LastError == null)
            {
                Console.WriteLine("Open positions:");
                foreach (var openPosition in OpenPositions)
                {
                    Console.WriteLine($"{openPosition.Symbol}: {openPosition.Quantity}");
                }
                return;
            }

            base.RenderResult();
        }
    }
}