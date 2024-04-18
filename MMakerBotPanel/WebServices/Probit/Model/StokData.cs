namespace MMakerBotPanel.WebServices.Probit.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class StokData
    {
        public string market_id { get; set; }
        public string open { get; set; }
        public string close { get; set; }
        public string low { get; set; }
        public string high { get; set; }
        public string base_volume { get; set; }
        public string quote_volume { get; set; }
        public DateTime start_time { get; set; }
        public DateTime end_time { get; set; }
    }
}