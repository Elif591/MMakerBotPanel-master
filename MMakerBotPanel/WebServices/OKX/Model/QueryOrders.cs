namespace MMakerBotPanel.WebServices.OKX.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class QueryOrders
    {
        public string instType { get; set; }
        public string instId { get; set; }
        public string ccy { get; set; }
        public string ordId { get; set; }
        public string clOrdId { get; set; }
        public string tag { get; set; }
        public string px { get; set; }
        public string sz { get; set; }
        public string pnl { get; set; }
        public string ordType { get; set; }
        public string side { get; set; }
        public string posSide { get; set; }
        public string tdMode { get; set; }
        public string accFillSz { get; set; }
        public string fillPx { get; set; }
        public string tradeId { get; set; }
        public string fillSz { get; set; }
        public string fillTime { get; set; }
        public string state { get; set; }
        public string avgPx { get; set; }
        public string lever { get; set; }
        public string attachAlgoClOrdId { get; set; }
        public string tpTriggerPx { get; set; }
        public string tpTriggerPxType { get; set; }
        public string tpOrdPx { get; set; }
        public string slTriggerPx { get; set; }
        public string slTriggerPxType { get; set; }
        public string slOrdPx { get; set; }
        public string stpId { get; set; }
        public string stpMode { get; set; }
        public string feeCcy { get; set; }
        public string fee { get; set; }
        public string rebateCcy { get; set; }
        public string rebate { get; set; }
        public string tgtCcy { get; set; }
        public string category { get; set; }
        public string reduceOnly { get; set; }
        public string cancelSource { get; set; }
        public string cancelSourceReason { get; set; }
        public string quickMgnType { get; set; }
        public string algoClOrdId { get; set; }
        public string algoId { get; set; }
        public string uTime { get; set; }
        public string cTime { get; set; }
    }
}