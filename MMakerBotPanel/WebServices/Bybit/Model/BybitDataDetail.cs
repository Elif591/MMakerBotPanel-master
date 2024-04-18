namespace MMakerBotPanel.WebServices.Bybit.Model
{
    using System.Collections.Generic;

    public class BybitDataDetail
    {
        public int ret_code { get; set; }
        public string ret_msg { get; set; }
        public List<List<object>> result { get; set; }
        public object ext_info { get; set; }
        public object ext_code { get; set; }
    }
}