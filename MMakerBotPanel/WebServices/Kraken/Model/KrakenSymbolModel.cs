namespace MMakerBotPanel.WebServices.Kraken.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class KrakenSymbolModel
    {
        public List<KrakenSymbol> krakenSymbols = new List<KrakenSymbol>();
        public GenericResult genericResult { get; set; }
    }
}

