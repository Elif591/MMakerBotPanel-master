namespace MMakerBotPanel.WebServices.Huobi.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class SymbolModel
    {
        public SymbolModel()
        {
            genericResult = new GenericResult();
        }
        public string status { get; set; }
        public List<Datum> data { get; set; }
        public string ts { get; set; }
        public int full { get; set; }
        public GenericResult genericResult { get; set; }

    }
}