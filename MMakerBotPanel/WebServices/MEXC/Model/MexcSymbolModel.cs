namespace MMakerBotPanel.WebServices.MEXC.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class MexcSymbolModel
    {
        public MexcSymbolModel()
        {
            genericResult = new GenericResult();
        }
        public int code { get; set; }
        public List<Datum> data { get; set; }

        public GenericResult genericResult { get; set; }
    }


}