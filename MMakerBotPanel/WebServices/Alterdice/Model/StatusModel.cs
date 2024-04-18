namespace MMakerBotPanel.WebServices.Alterdice.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class StatusModel
    {
        public StatusModel()
        {
            genericResult = new GenericResult();
        }
        public bool status { get; set; }
        public List<Datum> data { get; set; }
        public GenericResult genericResult { get; set; }
    }
}