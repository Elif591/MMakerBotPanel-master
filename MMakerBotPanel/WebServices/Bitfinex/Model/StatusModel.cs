namespace MMakerBotPanel.WebServices.Bitfinex.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class StatusModel
    {
        public StatusModel()
        {
            genericResult = new GenericResult();
        }

        public GenericResult genericResult;

        public List<int> status { get; set; }
    }
}