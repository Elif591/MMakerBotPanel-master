namespace MMakerBotPanel.WebServices.Bitfinex.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class TimeModel
    {
        public TimeModel()
        {
            genericResult = new GenericResult();

        }
        public GenericResult genericResult;
        public List<List<double>> time { get; set; }
    }

}