namespace MMakerBotPanel.WebServices.OKX.Model
{
    public class StockDataDetail
    {
        public long Time { get; set; }
        public long Volume { get; set; }
        public double closePrice { get; set; }
        public double highPrice { get; set; }
        public double lowPrice { get; set; }
        public double openPrice { get; set; }
    }
}