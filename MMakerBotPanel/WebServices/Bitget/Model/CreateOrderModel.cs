namespace MMakerBotPanel.WebServices.Bitget.Model
{
    using MMakerBotPanel.Models;

    public class CreateOrderModel
    {
        public CreateOrderModel()
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult;

        public string code { get; set; }
        public string msg { get; set; }
        public CreateOrder data { get; set; }
    }
}