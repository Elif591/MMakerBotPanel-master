namespace MMakerBotPanel.WebServices.Probit.Model
{
    using MMakerBotPanel.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class SymbolModel
    {
        public SymbolModel()
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult;
        public List<Symbol> data { get; set; }
    }
}