namespace MMakerBotPanel.WebServices.Lbank.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class SymbolModel
    {
        public SymbolModel()
        {
            genericResult = new GenericResult();
        }
        public string result { get; set; }
        public List<string> data { get; set; }
        public int error_code { get; set; }
        public long ts { get; set; }
        public GenericResult genericResult;

    }
}