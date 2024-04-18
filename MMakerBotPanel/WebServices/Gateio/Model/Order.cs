﻿namespace MMakerBotPanel.WebServices.Gateio.Model
{
    public class Order
    {
        public string id { get; set; }
        public string text { get; set; }
        public string create_time { get; set; }
        public string update_time { get; set; }
        public string currency_pair { get; set; }
        public string status { get; set; }
        public string type { get; set; }
        public string account { get; set; }
        public string side { get; set; }
        public string amount { get; set; }
        public string price { get; set; }
        public string time_in_force { get; set; }
        public string left { get; set; }
        public string filled_total { get; set; }
        public string fee { get; set; }
        public string fee_currency { get; set; }
        public string point_fee { get; set; }
        public string gt_fee { get; set; }
        public bool gt_discount { get; set; }
        public string rebated_fee { get; set; }
        public string rebated_fee_currency { get; set; }
    }
}