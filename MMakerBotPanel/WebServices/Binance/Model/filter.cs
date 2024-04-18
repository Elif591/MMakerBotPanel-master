namespace MMakerBotPanel.WebServices.Binance.Model
{
    public class filter
    {
        public string filterType { get; set; }
        public string minPrice { get; set; }
        public string maxPrice { get; set; }
        public string tickSize { get; set; }
        public string multiplierUp { get; set; }
        public string multiplierDown { get; set; }
        public int avgPriceMins { get; set; }
        public string minQty { get; set; }
        public string maxQty { get; set; }
        public string stepSize { get; set; }
        public string minNotional { get; set; }
        public bool applyToMarket { get; set; }
        public int limit { get; set; }
        public int minTrailingAboveDelta { get; set; }
        public int maxTrailingAboveDelta { get; set; }
        public int minTrailingBelowDelta { get; set; }
        public int maxTrailingBelowDelta { get; set; }
        public int maxNumOrders { get; set; }
        public int maxNumAlgoOrders { get; set; }
    }
}