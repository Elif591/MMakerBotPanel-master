namespace MMakerBotPanel.WebServices.Hotbit.Model
{
    using MMakerBotPanel.Models;

    public class StatusModel
    {
        public StatusModel()
        {
            genericResult = new GenericResult();

        }
        public GenericResult genericResult;
        public object error { get; set; }
        public int result { get; set; }
        public int id { get; set; }
    }
}