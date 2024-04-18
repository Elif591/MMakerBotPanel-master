namespace MMakerBotPanel.Models.CexBotModels
{
    using System.Collections.Generic;

    public class PairList
    {
        public PairList()
        {
            Pairs = new List<Pair>();
        }

        public EXCHANGETYPE ExchangeType { get; set; }
        public CEXEXCHANGE CexExchange { get; set; }
        public DEXEXCHANGE DexExchange { get; set; }

        public List<Pair> Pairs;
    }

    public class Pair
    {
        public int Id { get; set; }
        public string symbol { get; set; }
        public string baseAsset { get; set; }
        public string quoteAsset { get; set; }
        public string qtyFormat { get; set; }
    }

}