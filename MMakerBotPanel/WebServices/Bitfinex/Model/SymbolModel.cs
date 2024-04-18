namespace MMakerBotPanel.WebServices.Bitfinex.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class SymbolModel
    {
        public SymbolModel()
        {
            genericResult = new GenericResult();
            BitfinexSymbolDataModels = new BitfinexSymbolDataModel();
        }
        public GenericResult genericResult;
        public BitfinexSymbolDataModel BitfinexSymbolDataModels;
    }

    public class BitfinexSymbolDataModel
    {
        public List<List<object>> Data { get; set; }
    }


}