namespace MMakerBotPanel.WebServices.Gateio.Model
{
    using MMakerBotPanel.Models;

    public class StatusModel
    {
        public StatusModel()
        {
            genericResult = new GenericResult();

        }
        public GenericResult genericResult;
        public long server_time { get; set; }
    }
}