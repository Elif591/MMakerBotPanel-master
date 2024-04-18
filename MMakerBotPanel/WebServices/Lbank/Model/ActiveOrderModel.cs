namespace MMakerBotPanel.WebServices.Lbank.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class ActiveOrderModel
    {
        public ActiveOrderModel()
        {
            genericResult = new GenericResult();
            activeOrders = new List<ActiveOrders>();
        }
        public GenericResult genericResult { get; set; }
        public List<ActiveOrders> activeOrders { get; set; }
    }
}