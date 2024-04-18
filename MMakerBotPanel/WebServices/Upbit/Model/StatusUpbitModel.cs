namespace MMakerBotPanel.WebServices.Upbit.Model
{
    public class StatusUpbitModel
    {
        public string market { get; set; }
        public string trade_date_utc { get; set; }
        public string trade_time_utc { get; set; }
        public long timestamp { get; set; }
        public double trade_price { get; set; }
        public double trade_volume { get; set; }
        public double prev_closing_price { get; set; }
        public double change_price { get; set; }
        public string ask_bid { get; set; }
        public long sequential_id { get; set; }
    }
}