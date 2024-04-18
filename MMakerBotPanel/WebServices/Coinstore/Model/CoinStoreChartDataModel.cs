namespace MMakerBotPanel.WebServices.Coinstore.Model
{
    public class CoinStoreChartDataModel
    {
        public double low { get; set; }
        public double high { get; set; }
        public long volume { get; set; }
        public long time { get; set; }
        public double open { get; set; }
        public double close { get; set; }
        public long pair_id { get; set; }
        public string pair { get; set; }
    }
}