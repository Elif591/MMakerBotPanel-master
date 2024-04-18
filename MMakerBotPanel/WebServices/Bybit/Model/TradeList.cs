namespace MMakerBotPanel.WebServices.Bybit.Model
{
    public class TradeList
    {
        public string execId { get; set; }
        public string symbol { get; set; }
        public string price { get; set; }
        public string size { get; set; }
        public string side { get; set; }
        public string time { get; set; }
        public bool isBlockTrade { get; set; }
    }
}