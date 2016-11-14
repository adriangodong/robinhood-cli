namespace RobinhoodCli
{
    public class Order
    {
        public OrderType Type { get; set; }
        public string Symbol { get; set; }
        public int? Size { get; set; }
        public decimal? LimitPrice { get; set; }
    }
}