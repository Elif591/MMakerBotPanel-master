namespace MMakerBotPanel.WebServices.Bybit.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class PairResult
    {
        public string category { get; set; }
        public List<Pairs> list { get; set; }

    }
}