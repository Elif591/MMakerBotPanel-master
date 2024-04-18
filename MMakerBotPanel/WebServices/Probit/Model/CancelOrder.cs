namespace MMakerBotPanel.WebServices.Probit.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class CancelOrder
    {
        public string id { get; set; }
        public string user_id { get; set; }
        public string market_id { get; set; }
        public string type { get; set; }
        public string side { get; set; }
        public string quantity { get; set; }
        public string limit_price { get; set; }
        public string cost { get; set; }
        public string time_in_force { get; set; }
        public string filled_cost { get; set; }
        public string filled_quantity { get; set; }
        public string open_quantity { get; set; }
        public string cancelled_quantity { get; set; }
        public string status { get; set; }
        public DateTime time { get; set; }
    }
}