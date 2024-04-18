namespace MMakerBotPanel.WebServices.Bitget.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class ActiveOrdersModel
    {
        public ActiveOrdersModel()
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult;
        public string code { get; set; }
        public string message { get; set; }
        public List<ActiveOrders> data { get; set; }
    }
}