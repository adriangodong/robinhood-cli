using System.Collections.Generic;
using RobinhoodCli.Models;

namespace RobinhoodCli.Services
{
    internal interface IOutputService
    {
        void Info(string message);
        void Success(string message);
        void Error(string message);
        void OpenOrders(List<OpenOrder> openOrders);
        void OpenPositions(List<OpenPosition> openPositions);
        void ExitCommand();
    }
}