namespace MMakerBotPanel.Models.ApiModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class QueryOrderResponseModel
    {
        public QueryOrderResponseModel()
        {


        }

        public string symbol { get; set; }
        public string orderId { get; set; }
        public string status { get; set; }
        public string type { get; set; }
        public string side { get; set; }
        public string qty { get; set; }
        public string price { get; set; }
        public string origQuoteOrderQty { get; set; }
    }
}