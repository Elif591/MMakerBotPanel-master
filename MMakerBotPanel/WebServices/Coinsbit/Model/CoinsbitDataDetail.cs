namespace MMakerBotPanel.WebServices.Coinsbit.Model
{
    public class CoinsbitDataDetail
    {
        public int code { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
        public Result result { get; set; }
    }
}