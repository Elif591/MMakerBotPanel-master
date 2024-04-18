namespace MMakerBotPanel.WebServices.Coinsbit.Model
{
    using MMakerBotPanel.Models;

    public class ActiveOrderModel
    {
        public ActiveOrderModel()
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
        public ActiveOrderResult result { get; set; }
    }
}