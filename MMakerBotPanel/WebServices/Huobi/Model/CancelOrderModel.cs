namespace MMakerBotPanel.WebServices.Huobi.Model
{
    using MMakerBotPanel.Models;

    public class CancelOrderModel
    {
        public CancelOrderModel()
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult { get; set; }

        public string status { get; set; }
        public CancelOrder data { get; set; }
    }
}