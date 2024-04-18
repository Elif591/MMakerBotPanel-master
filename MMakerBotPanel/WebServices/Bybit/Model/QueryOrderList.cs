namespace MMakerBotPanel.WebServices.Bybit.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class QueryOrderList
    {
        public string orderId { get; set; }
        public string orderLinkId { get; set; }
        public string blockTradeId { get; set; }
        public string symbol { get; set; }
        public string price { get; set; }
        public string qty { get; set; }
        public string side { get; set; }
        public string isLeverage { get; set; }
        public int positionIdx { get; set; }
        public string orderStatus { get; set; }
        public string cancelType { get; set; }
        public string rejectReason { get; set; }
        public string avgPrice { get; set; }
        public string leavesQty { get; set; }
        public string leavesValue { get; set; }
        public string cumExecQty { get; set; }
        public string cumExecValue { get; set; }
        public string cumExecFee { get; set; }
        public string timeInForce { get; set; }
        public string orderType { get; set; }
        public string stopOrderType { get; set; }
        public string orderIv { get; set; }
        public string triggerPrice { get; set; }
        public string takeProfit { get; set; }
        public string stopLoss { get; set; }
        public string tpTriggerBy { get; set; }
        public string slTriggerBy { get; set; }
        public int triggerDirection { get; set; }
        public string triggerBy { get; set; }
        public string lastPriceOnCreated { get; set; }
        public bool reduceOnly { get; set; }
        public bool closeOnTrigger { get; set; }
        public string smpType { get; set; }
        public int smpGroup { get; set; }
        public string smpOrderId { get; set; }
        public string tpslMode { get; set; }
        public string tpLimitPrice { get; set; }
        public string slLimitPrice { get; set; }
        public string placeType { get; set; }
        public string createdTime { get; set; }
        public string updatedTime { get; set; }
    }
}