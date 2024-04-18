namespace MMakerBotPanel.WebServices.Bybit.Model
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Web;

    public class Pairs
    {
        public string symbol { get; set; }
        public string baseCoin { get; set; }
        public string quoteCoin { get; set; }
        public string innovation { get; set; }
        public string status { get; set; }
        public string marginTrading { get; set; }
        public LotSizeFilter lotSizeFilter { get; set; }
    }
}