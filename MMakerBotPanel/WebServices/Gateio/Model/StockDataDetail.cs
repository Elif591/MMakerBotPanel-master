namespace MMakerBotPanel.WebServices.Gateio.Model
{
    public class StockDataDetail
    {
        public long startTime { get; set; }
        public long tradingVolume { get; set; }
        public double closePrice { get; set; }
        public double highestPrice { get; set; }
        public double lowestPrice { get; set; }
        public double openPrice { get; set; }
        public double tradingAmount { get; set; }
    }
}