namespace MMakerBotPanel.WebServices.Binance.Model
{
    public class StockDataDetail
    {
        public long OpenTime { get; set; }
        public double OpenPrice { get; set; }
        public double HighPrice { get; set; }
        public double LowPrice { get; set; }
        public double ClosePrice { get; set; }
        public long Volume { get; set; }
        public long CloseTime { get; set; }
        public double QuoteAssetVolume { get; set; }
        public int Trades { get; set; }
        public double TakerBuyBaseAssetVolume { get; set; }
        public double TakerBuyQuoteAssetVolume { get; set; }
    }
}