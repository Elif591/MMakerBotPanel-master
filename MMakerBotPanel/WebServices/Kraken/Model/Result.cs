namespace MMakerBotPanel.WebServices.Kraken.Model
{
    using System.Collections.Generic;

    public class Result
    {
        public Result()
        {
            CandleData = new List<List<object>>();
        }
        public List<List<object>> CandleData { get; set; }
        public int last { get; set; }
    }
}