namespace MMakerBotPanel.WebServices.Bybit.Model
{
    public class ActiveOrderList
    {
        public string symbol { get; set; }
        public string orderType { get; set; }
        public string orderLinkId { get; set; }
        public string orderId { get; set; }
        public string cancelType { get; set; }
        public string avgPrice { get; set; }
        public string stopOrderType { get; set; }
        public string lastPriceOnCreated { get; set; }
        public string orderStatus { get; set; }
        public string takeProfit { get; set; }
        public string cumExecValue { get; set; }
        public int triggerDirection { get; set; }
        public string isLeverage { get; set; }
        public string rejectReason { get; set; }
        public string price { get; set; }
        public string orderIv { get; set; }
        public string createdTime { get; set; }
        public string tpTriggerBy { get; set; }
        public int positionIdx { get; set; }
        public string timeInForce { get; set; }
        public string leavesValue { get; set; }
        public string updatedTime { get; set; }
        public string side { get; set; }
        public string triggerPrice { get; set; }
        public string cumExecFee { get; set; }
        public string leavesQty { get; set; }
        public string slTriggerBy { get; set; }
        public bool closeOnTrigger { get; set; }
        public string cumExecQty { get; set; }
        public bool reduceOnly { get; set; }
        public string qty { get; set; }
        public string stopLoss { get; set; }
        public string triggerBy { get; set; }
    }
}