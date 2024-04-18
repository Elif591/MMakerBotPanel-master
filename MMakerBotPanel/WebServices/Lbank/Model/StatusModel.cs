namespace MMakerBotPanel.WebServices.Lbank.Model
{
    using MMakerBotPanel.Models;

    public class StatusModel
    {
        public StatusModel()
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult;
        public string result { get; set; }
        public Data data { get; set; }
        public int error_code { get; set; }
        public long ts { get; set; }
    }
}