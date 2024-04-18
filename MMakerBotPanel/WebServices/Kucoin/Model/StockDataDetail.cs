namespace MMakerBotPanel.WebServices.Kucoin.Model
{
    public class StockDataDetail
    {
        public long startTime { get; set; }
        public double openingPrice { get; set; }
        public double closingPrice { get; set; }
        public double highestPrice { get; set; }
        public double lowestPrice { get; set; }
        public long transactionVolume { get; set; }
        public double transactionAmount { get; set; }
    }
}