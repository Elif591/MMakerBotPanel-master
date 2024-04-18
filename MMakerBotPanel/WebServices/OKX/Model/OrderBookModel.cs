namespace MMakerBotPanel.WebServices.OKX.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class OrderBookModel
    {
        public OrderBookModel()
        {
            genericResult = new GenericResult();
            data = new List<OrderBookDatum>();
        }
        public GenericResult genericResult { get; set; }
        public string code { get; set; }
        public string msg { get; set; }
        public List<OrderBookDatum> data { get; set; }
    }
}