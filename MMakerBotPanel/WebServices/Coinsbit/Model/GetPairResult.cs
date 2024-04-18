namespace MMakerBotPanel.WebServices.Coinsbit.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class GetPairResult
    {
        public string name { get; set; }
        public int moneyPrec { get; set; }
        public string stock { get; set; }
        public string money { get; set; }
        public int stockPrec { get; set; }
        public int feePrec { get; set; }
        public string minAmount { get; set; }
    }
}