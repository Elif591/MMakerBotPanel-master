namespace MMakerBotPanel.WebServices.Kraken.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class CancelOrderModel
    {
        public CancelOrderModel()
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult;
        public List<object> error { get; set; }
        public CancelOrders result { get; set; }
    }
}