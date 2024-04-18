namespace MMakerBotPanel.WebServices.Alterdice.Model
{
    public class StockDataDetail
    {
        public long low { get; set; }
        public long high { get; set; }
        public int volume { get; set; }
        public int time { get; set; }
        public long open { get; set; }
        public long close { get; set; }
        public int pair_id { get; set; }
        public string pair { get; set; }
    }
}