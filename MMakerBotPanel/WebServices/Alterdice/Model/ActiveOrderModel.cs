namespace MMakerBotPanel.WebServices.Alterdice.Model
{
    using MMakerBotPanel.Models;

    public class ActiveOrderModel
    {
        public ActiveOrderModel()
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult;
        public bool status { get; set; }
        public ActiveOrderData data { get; set; }
        public string token { get; set; }
    }
}