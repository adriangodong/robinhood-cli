using System;
using Deadlock.Robinhood.Model;
using RobinhoodCli.Commands;

namespace RobinhoodCli.CommandParsers
{
    internal class OrderCommandParser : ICommandParser
    {
        public const string Error_MissingSymbolParameter = "Missing symbol parameter";
        public const string Error_EmptySizeParameter = "Missing size parameter, required for buy order";
        public const string Error_BadSizeParameter = "Cannot parse size parameter '{0}'";
        public const string Error_BadLimitPriceParameter = "Cannot parse limit price parameter '{0}'";
        public const string Error_MissingIndexParameter = "Missing order index to cancel";
        public const string Error_BadIndexParameter = "Cannot parse order index parameter '{0}'";

        public ICommand Parse(string[] commandTokens)
        {
            switch (commandTokens[0])
            {
                case "buy":
                case "b":
                    return ParsePrepareOrderCommand(Side.Buy, commandTokens);

                case "sell":
                case "s":
                    return ParsePrepareOrderCommand(Side.Sell, commandTokens);

                case "cancel":
                    return ParseCancelOpenOrderCommand(commandTokens);
            }

            return null;
        }

        public string LastError { get; private set; }

        internal PrepareOrderCommand ParsePrepareOrderCommand(Side side, string[] commandTokens)
        {
            LastError = null;

            if (commandTokens.Length == 1)
            {
                LastError = Error_MissingSymbolParameter;
                return null;
            }

            var prepareOrderCommand = new PrepareOrderCommand
            {
                Side = side,
                Symbol = commandTokens[1]
            };

            if (commandTokens.Length == 2)
            {
                switch (side)
                {
                    case Side.Buy:
                        LastError = Error_EmptySizeParameter;
                        return null;

                    case Side.Sell:
                        LastError = null;
                        return prepareOrderCommand;

                    default:
                        throw new NotImplementedException();
                }
            }

            int size;
            if (!int.TryParse(commandTokens[2], out size))
            {
                LastError = string.Format(Error_BadSizeParameter, commandTokens[2]);
                return null;
            }
            prepareOrderCommand.Size = size;

            if (commandTokens.Length == 3)
            {
                LastError = null;
                return prepareOrderCommand;
            }

            decimal limitPrice;
            if (!decimal.TryParse(commandTokens[3], out limitPrice))
            {
                LastError = string.Format(Error_BadLimitPriceParameter, commandTokens[3]);
                return null;
            }
            prepareOrderCommand.LimitPrice = limitPrice;

            return prepareOrderCommand;
        }

        internal CancelOpenOrderCommand ParseCancelOpenOrderCommand(string[] commandTokens)
        {
            LastError = null;

            if (commandTokens.Length == 1)
            {
                LastError = Error_MissingIndexParameter;
                return null;
            }

            int openOrderIndexToCancel;
            if (!int.TryParse(commandTokens[1], out openOrderIndexToCancel))
            {
                LastError = string.Format(Error_BadIndexParameter, commandTokens[1]);
                return null;
            }

            return new CancelOpenOrderCommand(openOrderIndexToCancel);
        }
    }
}