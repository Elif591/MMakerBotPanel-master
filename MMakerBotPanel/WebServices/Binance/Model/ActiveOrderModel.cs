namespace MMakerBotPanel.WebServices.Binance.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class ActiveOrderModel
    {
        public ActiveOrderModel()
        {
            genericResult = new GenericResult();
            activeOrders = new List<ActiveOrder>();
        }
        public GenericResult genericResult;
        public List<ActiveOrder> activeOrders;

    }
}