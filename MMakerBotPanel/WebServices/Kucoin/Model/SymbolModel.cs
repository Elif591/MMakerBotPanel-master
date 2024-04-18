namespace MMakerBotPanel.WebServices.Kucoin.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class SymbolModel
    {
        public string code { get; set; }
        public List<Data> data { get; set; }
        public GenericResult genericResult { get; set; }
    }

}