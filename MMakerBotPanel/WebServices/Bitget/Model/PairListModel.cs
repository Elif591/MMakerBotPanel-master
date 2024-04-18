namespace MMakerBotPanel.WebServices.Bitget.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class PairListModel
    {
        public PairListModel()
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult { get; set; }
        public string code { get; set; }
        public string msg { get; set; }
        public List<Symbols> data { get; set; }
    }
}