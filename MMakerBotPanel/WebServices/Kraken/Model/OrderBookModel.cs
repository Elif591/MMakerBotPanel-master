namespace MMakerBotPanel.WebServices.Kraken.Model
{
    using MMakerBotPanel.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class OrderBookModel
    {
        public OrderBookModel()
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult;
        public List<object> error { get; set; }
        public OrderBookCoin result { get; set; }
    }
}