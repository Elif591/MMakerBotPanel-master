namespace MMakerBotPanel.WebServices.Coinstore.Model
{
    public class StatusData
    {
        public string channel { get; set; }
        public int time { get; set; }
        public int tradeId { get; set; }
        public string volume { get; set; }
        public string price { get; set; }
        public string takerSide { get; set; }
        public int seq { get; set; }
        public long ts { get; set; }
        public string symbol { get; set; }
        public int instrumentId { get; set; }
    }
}