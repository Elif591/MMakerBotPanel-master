namespace MMakerBotPanel.WebServices.Alterdice.Model
{
    using MMakerBotPanel.Models;

    public class CreateOrderModel
    {
        public CreateOrderModel()
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult;
        public bool status { get; set; }
        public string message { get; set; }
        public DataCreateOrder data { get; set; }
        public string token { get; set; }

    }
}