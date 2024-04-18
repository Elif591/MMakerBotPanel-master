namespace MMakerBotPanel.WebServices.Gateio.Model
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
        public string currency_pair { get; set; }
        public int total { get; set; }
        public List<Order> orders { get; set; }
    }
}