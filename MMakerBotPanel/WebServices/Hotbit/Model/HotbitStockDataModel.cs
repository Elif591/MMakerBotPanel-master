namespace MMakerBotPanel.WebServices.Hotbit.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class HotbitStockDataModel
    {
        public HotbitStockDataModel()
        {
            result = new List<List<object>>();
            genericResult = new GenericResult();

        }
        public object error { get; set; }
        public List<List<object>> result { get; set; }
        public long id { get; set; }
        public GenericResult genericResult { get; set; }
    }
}