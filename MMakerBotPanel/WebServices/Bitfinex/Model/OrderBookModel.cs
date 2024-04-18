namespace MMakerBotPanel.WebServices.Bitfinex.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class OrderBookModel
    {
        public OrderBookModel()
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult { get; set; }
        public List<List<double>> Orders { get; set; }
    }
}