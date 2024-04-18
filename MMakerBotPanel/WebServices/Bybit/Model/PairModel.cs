namespace MMakerBotPanel.WebServices.Bybit.Model
{
    using MMakerBotPanel.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class PairModel
    {
        public PairModel()
        {
            genericResult = new GenericResult();

        }
        public GenericResult genericResult { get; set; }
        public int retCode { get; set; }
        public string retMsg { get; set; }
        public PairResult result { get; set; }
        public long time { get; set; }
    }
}