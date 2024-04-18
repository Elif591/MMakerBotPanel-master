namespace MMakerBotPanel.WebServices.Bitget.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class CancelOrderModel
    {
        public CancelOrderModel()
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult;
        public string code { get; set; }
        public string message { get; set; }
        public List<CancelOrder> data { get; set; }

    }
}