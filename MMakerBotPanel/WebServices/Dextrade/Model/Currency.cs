namespace MMakerBotPanel.WebServices.Dextrade.Model
{
    public class Currency
    {
        public string iso3 { get; set; }
        public string name { get; set; }
        //public Networks networks { get; set; }
        public int refill { get; set; }
        public int withdraw { get; set; }
    }
}