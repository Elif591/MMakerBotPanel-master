namespace MMakerBotPanel.WebServices.Probit.Model
{
    using MMakerBotPanel.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class CancelOrderModel
    {
        public CancelOrderModel() 
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult { get; set; }
        public CancelOrder data { get; set; }
    }
}