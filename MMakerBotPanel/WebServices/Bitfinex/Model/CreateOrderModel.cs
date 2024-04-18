namespace MMakerBotPanel.WebServices.Bitfinex.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class CreateOrderModel
    {
        public CreateOrderModel()
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult { get; set; }

        public int MTS { get; set; }
        public string TYPE { get; set; }
        public int MESSAGE_ID { get; set; }
        public int CODE { get; set; }
        public string STATUS { get; set; }
        public string TEXT { get; set; }
        public List<Order> DATA { get; set; }

    }
}