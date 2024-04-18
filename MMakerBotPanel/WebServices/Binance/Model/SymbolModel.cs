namespace MMakerBotPanel.WebServices.Binance.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class SymbolModel
    {
        public SymbolModel()
        {
            genericResult = new GenericResult();
        }

        public string timezone { get; set; }
        public long serverTime { get; set; }

        public List<rateLimit> rateLimits;

        public List<exchangeFilter> exchangeFilters;

        public List<symbols> symbols;

        public GenericResult genericResult;

    }
}