namespace MMakerBotPanel.WebServices.Dextrade.Model
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
        public bool status { get; set; }
        public List<Datum> data { get; set; }
    }
}