namespace MMakerBotPanel.WebServices.Bitget.Model
{
    public class QueryOrder
    {
        public string accountId { get; set; }
        public string symbol { get; set; }
        public string orderId { get; set; }
        public string clientOrderId { get; set; }
        public string price { get; set; }
        public string quantity { get; set; }
        public string orderType { get; set; }
        public string side { get; set; }
        public string status { get; set; }
        public string fillPrice { get; set; }
        public string fillQuantity { get; set; }
        public string fillTotalAmount { get; set; }
        public string enterPointSource { get; set; }
        public string feeDetail { get; set; }
        public string orderSource { get; set; }
        public string cTime { get; set; }
    }
}