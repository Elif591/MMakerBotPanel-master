namespace MMakerBotPanel.WebServices.OKX.Model
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
        public string code { get; set; }
        public string msg { get; set; }
        public List<ActiveOrdersDatum> data { get; set; }
    }
}