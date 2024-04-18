namespace MMakerBotPanel.WebServices.Huobi.Model
{
    using MMakerBotPanel.Models;

    public class CreateOrderModel
    {
        public CreateOrderModel()
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult { get; set; }
        public string status { get; set; }
        public string data { get; set; }
    }
}