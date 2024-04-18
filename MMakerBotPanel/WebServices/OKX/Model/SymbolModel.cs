namespace MMakerBotPanel.WebServices.OKX.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class SymbolModel
    {
        public SymbolModel()
        {
            genericResult = new GenericResult();
        }
        public string code { get; set; }
        public string msg { get; set; }
        public List<Datum> data { get; set; }
        public GenericResult genericResult { get; set; }
    }
}