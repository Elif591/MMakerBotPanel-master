namespace MMakerBotPanel.WebServices.Kucoin.Model
{
    public class GetWalletItem
    {
        public string id { get; set; }
        public string currency { get; set; }
        public string amount { get; set; }
        public string fee { get; set; }
        public string balance { get; set; }
        public string accountType { get; set; }
        public string bizType { get; set; }
        public string direction { get; set; }
        public long createdAt { get; set; }
        public string context { get; set; }
    }
}