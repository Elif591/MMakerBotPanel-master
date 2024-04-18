namespace MMakerBotPanel.Models.ApiModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class CancelOrderResponseModel
    {
        public string symbol { get; set; }
        public string orderId { get; set; }
        public string status { get; set; }
    }
}