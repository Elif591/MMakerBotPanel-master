namespace MMakerBotPanel.WebServices.Dextrade.Model
{
    using MMakerBotPanel.Models;

    public class CreateOrderModel
    {

        public CreateOrderModel()
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult { get; set; }

        public bool status { get; set; }
        public string message { get; set; }
        public DataExchange data { get; set; }
        public string token { get; set; }


    }
}