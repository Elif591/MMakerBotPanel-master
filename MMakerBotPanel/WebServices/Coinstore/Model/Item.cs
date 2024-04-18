namespace MMakerBotPanel.WebServices.Coinstore.Model
{
    public class Item
    {
        public int endTime { get; set; }
        public string interval { get; set; }
        public int startTime { get; set; }
        public string amount { get; set; }
        public int firstTradeId { get; set; }
        public int lastTradeId { get; set; }
        public string volume { get; set; }
        public string close { get; set; }
        public string open { get; set; }
        public string high { get; set; }
        public string low { get; set; }
    }
}