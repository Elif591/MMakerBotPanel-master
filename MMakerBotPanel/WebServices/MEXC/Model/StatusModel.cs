namespace MMakerBotPanel.WebServices.MEXC.Model
{
    using MMakerBotPanel.Models;

    public class StatusModel
    {
        public StatusModel()
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult;
        public long serverTime { get; set; }
    }
}