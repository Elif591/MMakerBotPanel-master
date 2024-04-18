namespace MMakerBotPanel.WebServices.Probit.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class OrderBook
    {
        public string side { get; set; }
        public string price { get; set; }
        public string quantity { get; set; }
    }
}