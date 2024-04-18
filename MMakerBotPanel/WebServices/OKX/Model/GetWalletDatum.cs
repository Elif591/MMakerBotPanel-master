namespace MMakerBotPanel.WebServices.OKX.Model
{
    using System.Collections.Generic;

    public class GetWalletDatum
    {
        public string adjEq { get; set; }
        public List<Detail> details { get; set; }
        public string imr { get; set; }
        public string isoEq { get; set; }
        public string mgnRatio { get; set; }
        public string mmr { get; set; }
        public string notionalUsd { get; set; }
        public string ordFroz { get; set; }
        public string totalEq { get; set; }
        public string uTime { get; set; }
    }
}