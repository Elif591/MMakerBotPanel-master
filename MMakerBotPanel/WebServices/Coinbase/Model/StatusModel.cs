namespace MMakerBotPanel.WebServices.Coinbase.Model
{
    using MMakerBotPanel.Models;

    public class StatusModel
    {
        public StatusModel()
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult;
        public Data data { get; set; }
    }
}