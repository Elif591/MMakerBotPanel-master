namespace MMakerBotPanel.Models.MakerMarketingModels
{
    public class ActiveOrderResponseModel
    {
        public long Timestamp { get; set; }
        public string Symbol { get; set; }
        public string OrderId { get; set; }
        public string Volume { get; set; }
        public string Price { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string Side { get; set; }
    }
}