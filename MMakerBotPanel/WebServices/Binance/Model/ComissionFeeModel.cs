namespace MMakerBotPanel.WebServices.Binance.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class ComissionFeeModel
    {
        public string symbol { get; set; }
        public string makerCommission { get; set; }
        public string takerCommission { get; set; }
    }
}