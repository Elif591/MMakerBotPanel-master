namespace MMakerBotPanel.WebServices.Probit.Model
{
    using MMakerBotPanel.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class BalanceModel
    {

        public BalanceModel() 
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult { get; set; }
        public List<Balance> data { get; set; }
    }
}