namespace MMakerBotPanel.WebServices.Binance.Model
{
    public class rateLimit
    {
        public string rateLimitType { get; set; }
        public string interval { get; set; }
        public int intervalNum { get; set; }
        public int limit { get; set; }
    }
}