namespace MMakerBotPanel.WebServices.OKX.Model
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
        public string code { get; set; }
        public List<Data> data { get; set; }
        public string msg { get; set; }

    }
}