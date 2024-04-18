namespace MMakerBotPanel.WebServices.Binance.Model
{
    public class Fill
    {
        public string price { get; set; }
        public string qty { get; set; }
        public string commission { get; set; }
        public string commissionAsset { get; set; }
        public int tradeId { get; set; }
    }
}