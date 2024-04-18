namespace MMakerBotPanel.WebServices.Hotbit.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class HotbitSymbolModel
    {
        public HotbitSymbolModel()
        {
            genericResult = new GenericResult();
        }
        public object error { get; set; }
        public List<Result> result { get; set; }
        public long id { get; set; }
        public GenericResult genericResult { get; set; }
    }
}