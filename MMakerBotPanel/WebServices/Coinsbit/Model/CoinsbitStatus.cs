namespace MMakerBotPanel.WebServices.Coinsbit.Model
{
    public class CoinsbitStatus
    {
        public long tid { get; set; }
        public int date { get; set; }
        public string price { get; set; }
        public string type { get; set; }
        public string amount { get; set; }
        public string total { get; set; }
    }
}