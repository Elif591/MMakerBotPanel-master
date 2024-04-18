namespace MMakerBotPanel.WebServices.Huobi.Model
{
    using MMakerBotPanel.Models;

    public class StatusModel
    {
        public StatusModel()
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult;
        public long data { get; set; }
        public string status { get; set; }
    }

}