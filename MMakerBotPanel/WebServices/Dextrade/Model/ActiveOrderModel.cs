namespace MMakerBotPanel.WebServices.Dextrade.Model
{
    using MMakerBotPanel.Models;

    public class ActiveOrderModel
    {
        public ActiveOrderModel()
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult { get; set; }
        public bool status { get; set; }
        public Data data { get; set; }
        public string token { get; set; }
    }
}