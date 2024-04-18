namespace MMakerBotPanel.WebServices.Bitget.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class CancelOrder
    {
        public string orderId { get; set; }
        public string clientOrderId { get; set; }
    }
}