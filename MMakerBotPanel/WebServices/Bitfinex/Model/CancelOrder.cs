namespace MMakerBotPanel.WebServices.Bitfinex.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class CancelOrder
    {
        public string MTS { get; set; }
        public string TYPE { get; set; }
        public string MESSAGE_ID { get; set; }
        public string CODE { get; set; }
        public string STATUS { get; set; }
        public string TEXT { get; set; }
        public List<object> CancelOrders { get; set; }
    }
}