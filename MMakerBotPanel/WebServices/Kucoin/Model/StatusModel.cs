namespace MMakerBotPanel.WebServices.Kucoin.Model
{
    using MMakerBotPanel.Models;

    public class StatusModel
    {
        public StatusModel()
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult;
        public string code { get; set; }
        public long data { get; set; }
    }
}