namespace MMakerBotPanel.WebServices.Gateio.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class CancelOrderModel
    {
        public CancelOrderModel() 
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult { get; set; }

        public List<CancelOrders> cancelOrders { get; set; }

    }
}