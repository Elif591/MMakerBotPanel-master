namespace MMakerBotPanel.WebServices.MEXC.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class Balance
    {
        public string asset { get; set; }
        public string free { get; set; }
        public string locked { get; set; }
    }
}