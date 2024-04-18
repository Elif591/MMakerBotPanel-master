namespace MMakerBotPanel.WebServices.Binance.Model
{
    public class BalanceData
    {
        public string asset { get; set; }
        public string free { get; set; }
        public string locked { get; set; }
        public string freeze { get; set; }
        public string withdrawing { get; set; }
        public string ipoable { get; set; }
        public string btcValuation { get; set; }
    }
}