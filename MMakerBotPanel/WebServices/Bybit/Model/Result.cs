namespace MMakerBotPanel.WebServices.Bybit.Model
{
    public class Result
    {
        public string name { get; set; }
        public string alias { get; set; }
        public string baseCurrency { get; set; }
        public string quoteCurrency { get; set; }
        public string basePrecision { get; set; }
        public string quotePrecision { get; set; }
        public string minTradeQuantity { get; set; }
        public string minTradeAmount { get; set; }
        public string minPricePrecision { get; set; }
        public string maxTradeQuantity { get; set; }
        public string maxTradeAmount { get; set; }
        public int category { get; set; }
        public bool innovation { get; set; }
        public bool showStatus { get; set; }
    }
}