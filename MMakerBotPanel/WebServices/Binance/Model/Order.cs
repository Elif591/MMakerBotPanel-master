namespace MMakerBotPanel.WebServices.Binance.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class Order
    {
        public string symbol { get; set; }
        public int orderId { get; set; }
        public string clientOrderId { get; set; }
    }
}