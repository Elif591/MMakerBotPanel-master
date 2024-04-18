namespace MMakerBotPanel.WebServices.Probit.Model
{
    using MMakerBotPanel.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class ActiveOrderModel
    {
        public ActiveOrderModel() 
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult { get; set; }
        public List<ActiveOrder> data { get; set; }
    }
}