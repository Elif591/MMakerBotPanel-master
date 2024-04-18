namespace MMakerBotPanel.WebServices.Dextrade.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class SymbolModel
    {
        public SymbolModel()
        {
            genericResult = new GenericResult();
            data = new List<Symboldata>();
        }
        public bool status { get; set; }
        public List<Symboldata> data { get; set; }
        public GenericResult genericResult { get; set; }

    }
}