namespace MMakerBotPanel.WebServices.Bitfinex.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class ActiveOrderModel
    {
        public ActiveOrderModel()
        {
            genericResult = new GenericResult();

        }
        public GenericResult genericResult { get; set; }
        public List<List<object>> ActiveOrders { get; set; }
    }
}