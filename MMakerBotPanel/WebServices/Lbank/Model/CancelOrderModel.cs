namespace MMakerBotPanel.WebServices.Lbank.Model
{
    using MMakerBotPanel.Models;

    public class CancelOrderModel
    {
        public CancelOrderModel()
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult { get; set; }

        public string order_id { get; set; }
        public object success { get; set; }
        public object error { get; set; }
    }
}