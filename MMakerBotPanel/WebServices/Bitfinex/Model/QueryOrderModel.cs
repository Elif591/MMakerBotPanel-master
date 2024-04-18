namespace MMakerBotPanel.WebServices.Bitfinex.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class QueryOrderModel
    {

        public QueryOrderModel()
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult { get; set; }
        public List<List<object>> QueryOrders { get; set; }
    }
}