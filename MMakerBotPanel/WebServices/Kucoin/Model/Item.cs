namespace MMakerBotPanel.WebServices.Kucoin.Model
{
    public class Item
    {
        public string id { get; set; }
        public string symbol { get; set; }
        public string opType { get; set; }
        public string type { get; set; }
        public string side { get; set; }
        public string price { get; set; }
        public string size { get; set; }
        public string funds { get; set; }
        public string dealFunds { get; set; }
        public string dealSize { get; set; }
        public string fee { get; set; }
        public string feeCurrency { get; set; }
        public string stp { get; set; }
        public string stop { get; set; }
        public bool stopTriggered { get; set; }
        public string stopPrice { get; set; }
        public string timeInForce { get; set; }
        public bool postOnly { get; set; }
        public bool hidden { get; set; }
        public bool iceberg { get; set; }
        public string visibleSize { get; set; }
        public int cancelAfter { get; set; }
        public string channel { get; set; }
        public string clientOid { get; set; }
        public string remark { get; set; }
        public string tags { get; set; }
        public bool isActive { get; set; }
        public bool cancelExist { get; set; }
        public long createdAt { get; set; }
        public string tradeType { get; set; }
    }
}