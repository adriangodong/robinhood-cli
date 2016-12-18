using System;
using RobinhoodCli.Commands;

namespace RobinhoodCli
{
    public class CommandParser
    {

        public const string Error_EmptyCommand = "Empty command";
        public const string Error_UnknownFirstToken = "Unknown first token '{0}'";
        public const string Error_EmptySymbol = "Missing symbol parameter";
        public const string Error_EmptySizeParameter = "Missing size parameter, required for buy order";
        public const string Error_BadSizeParameter = "Cannot parse size parameter '{0}'";
        public const string Error_BadLimitPriceParameter = "Cannot parse limit price parameter '{0}'";

        public OrderCommand Parse(string command)
        {
            if (string.IsNullOrWhiteSpace(command))
            {
                LastError = Error_EmptyCommand;
                return null;
            }

            var tokens = command.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            switch (tokens[0])
            {
                case "buy":
                case "b":
                    return ParseCommandTokens(OrderType.Buy, tokens);

                case "sell":
                case "s":
                    return ParseCommandTokens(OrderType.Sell, tokens);

                default:
                    LastError = string.Format(Error_UnknownFirstToken, tokens[0]);
                    return null;
            }
        }

        internal OrderCommand ParseCommandTokens(OrderType type, string[] commandTokens)
        {
            if (commandTokens.Length == 1)
            {
                LastError = Error_EmptySymbol;
                return null;
            }

            var order = new OrderCommand
            {
                Type = type,
                Symbol = commandTokens[1]
            };

            if (commandTokens.Length == 2)
            {
                switch (type)
                {
                    case OrderType.Buy:
                        LastError = Error_EmptySizeParameter;
                        return null;

                    case OrderType.Sell:
                        LastError = null;
                        return order;

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
            order.Size = size;

            if (commandTokens.Length == 3)
            {
                LastError = null;
                return order;
            }

            decimal limitPrice;
            if (!decimal.TryParse(commandTokens[3], out limitPrice))
            {
                LastError = string.Format(Error_BadLimitPriceParameter, commandTokens[3]);
                return null;
            }
            order.LimitPrice = limitPrice;

            return order;
        }

        public string LastError { get; private set; }

    }
}