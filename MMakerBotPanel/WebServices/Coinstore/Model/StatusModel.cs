namespace MMakerBotPanel.WebServices.Coinstore.Model
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
        public List<StatusData> data { get; set; }
        public int code { get; set; }
    }


}