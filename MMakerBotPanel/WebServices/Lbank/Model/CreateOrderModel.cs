namespace MMakerBotPanel.WebServices.Lbank.Model
{
    using MMakerBotPanel.Models;

    public class CreateOrderModel
    {
        public CreateOrderModel()
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult { get; set; }

        public string order_id { get; set; }

    }
}