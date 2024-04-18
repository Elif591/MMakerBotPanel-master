namespace MMakerBotPanel.WebServices.Dextrade.Model
{
    public class DataDetail
    {
        public object low { get; set; }
        public object high { get; set; }
        public object volume { get; set; }
        public int time { get; set; }
        public object open { get; set; }
        public object close { get; set; }
        public int pair_id { get; set; }
        public string pair { get; set; }
    }
}