namespace MMakerBotPanel.WebServices.Bitget.Model
{
    public class Symbols
    {
        public string symbol { get; set; }
        public string symbolName { get; set; }
        public string baseCoin { get; set; }
        public string quoteCoin { get; set; }
        public string minTradeAmount { get; set; }
        public string maxTradeAmount { get; set; }
        public string takerFeeRate { get; set; }
        public string makerFeeRate { get; set; }
        public string priceScale { get; set; }
        public string quantityScale { get; set; }
        public string minTradeUSDT { get; set; }
        public string status { get; set; }
        public string buyLimitPriceRatio { get; set; }
        public string sellLimitPriceRatio { get; set; }
    }
}