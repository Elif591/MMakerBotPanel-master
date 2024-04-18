namespace MMakerBotPanel.WebServices.Binance.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class CreateOrderModel
    {
        public CreateOrderModel()
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult;
        public string symbol { get; set; }
        public long orderId { get; set; }
        public int orderListId { get; set; }
        public string clientOrderId { get; set; }
        public long transactTime { get; set; }
        public string price { get; set; }
        public string origQty { get; set; }
        public string executedQty { get; set; }
        public string cummulativeQuoteQty { get; set; }
        public string status { get; set; }
        public string timeInForce { get; set; }
        public string type { get; set; }
        public string side { get; set; }
        public int strategyId { get; set; }
        public int strategyType { get; set; }
        public long workingTime { get; set; }
        public string selfTradePreventionMode { get; set; }
        public List<Fill> fills { get; set; }

    }
}