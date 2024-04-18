namespace MMakerBotPanel.WebServices.Lbank.Model
{
    public class ActiveOrders
    {
        public string symbol { get; set; }
        public string orderId { get; set; }
        public string clientOrderId { get; set; }
        public string price { get; set; }
        public string origQty { get; set; }
        public string executedQty { get; set; }
        public string cummulativeQuoteQty { get; set; }
        public string status { get; set; }
        public string type { get; set; }
        public long time { get; set; }
        public long updateTime { get; set; }
        public string origQuoteOrderQty { get; set; }
    }
}