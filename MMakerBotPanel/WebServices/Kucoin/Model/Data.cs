namespace MMakerBotPanel.WebServices.Kucoin.Model
{
    public class Data
    {
        public string symbol { get; set; }
        public string name { get; set; }
        public string baseCurrency { get; set; }
        public string quoteCurrency { get; set; }
        public string feeCurrency { get; set; }
        public string market { get; set; }
        public string baseMinSize { get; set; }
        public string quoteMinSize { get; set; }
        public string baseMaxSize { get; set; }
        public string quoteMaxSize { get; set; }
        public string baseIncrement { get; set; }
        public string quoteIncrement { get; set; }
        public string priceIncrement { get; set; }
        public string priceLimitRate { get; set; }
        public string minFunds { get; set; }
        public bool isMarginEnabled { get; set; }
        public bool enableTrading { get; set; }
    }
}