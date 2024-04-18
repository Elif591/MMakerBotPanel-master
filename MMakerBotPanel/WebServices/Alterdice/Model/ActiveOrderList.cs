﻿namespace MMakerBotPanel.WebServices.Alterdice.Model
{
    public class ActiveOrderList
    {
        public int id { get; set; }
        public int type { get; set; }
        public int status { get; set; }
        public int type_trade { get; set; }
        public string pair { get; set; }
        public double volume { get; set; }
        public int volume_done { get; set; }
        public double rate { get; set; }
        public double price { get; set; }
        public int time_create { get; set; }
        public object time_done { get; set; }
        public object price_done { get; set; }
    }
}