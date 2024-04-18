namespace MMakerBotPanel.WebServices.Gateio.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class SymbolModel
    {
        public SymbolModel()
        {
            genericResult = new GenericResult();
        }

        public List<Datum> data { get; set; }
        public GenericResult genericResult { get; set; }
    }
}