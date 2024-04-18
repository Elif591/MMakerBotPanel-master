namespace MMakerBotPanel.WebServices.MEXC.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class CancelOrdersModel
    {
        public CancelOrdersModel()
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult { get; set; }
        public List<CancelOrders> cancelOrders { get; set; }
    }
}