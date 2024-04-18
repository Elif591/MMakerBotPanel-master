namespace MMakerBotPanel.WebServices.MEXC.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class ActiveOrdersModel
    {
        public ActiveOrdersModel()
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult { get; set; }
        public List<ActiveOrders> activeOrders { get; set; }
    }
}