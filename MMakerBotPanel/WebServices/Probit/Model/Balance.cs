namespace MMakerBotPanel.WebServices.Probit.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class Balance
    {
        public string currency_id { get; set; }
        public string total { get; set; }
        public string available { get; set; }
    }
}