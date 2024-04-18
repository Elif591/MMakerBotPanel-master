namespace MMakerBotPanel.WebServices.Coinsbit.Model
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
        public bool success { get; set; }
        public string message { get; set; }
        public List<ResultExchange> result { get; set; }

    }
}