namespace MMakerBotPanel.WebServices.Upbit.Model
{
    using System;

    public class UpbitDataDetailModel
    {
        public string market { get; set; }
        public DateTime candle_date_time_utc { get; set; }
        public DateTime candle_date_time_kst { get; set; }
        public double opening_price { get; set; }
        public double high_price { get; set; }
        public double low_price { get; set; }
        public double trade_price { get; set; }
        public long timestamp { get; set; }
        public double candle_acc_trade_price { get; set; }
        public double candle_acc_trade_volume { get; set; }
        public int unit { get; set; }
    }
}