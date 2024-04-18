namespace MMakerBotPanel.WebServices.Huobi.Model
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

        public string status { get; set; }
        public List<ActiveOrderDatum> data { get; set; }
    }
}