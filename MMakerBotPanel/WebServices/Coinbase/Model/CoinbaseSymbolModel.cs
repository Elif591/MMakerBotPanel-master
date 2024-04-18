namespace MMakerBotPanel.WebServices.Coinbase.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class CoinbaseSymbolModel
    {
        public CoinbaseSymbolModel() { }

        public List<CoinbaseSymbol> CoinbaseSymbolList;
        public GenericResult genericResult { get; set; }
    }
}