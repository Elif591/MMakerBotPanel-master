namespace MMakerBotPanel.WebServices.Alterdice.Model
{
    using MMakerBotPanel.Models;

    public class CancelOrderModel
    {
        public CancelOrderModel()
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult;
        public bool status { get; set; }
        public string error { get; set; }
        public string token { get; set; }
    }
}