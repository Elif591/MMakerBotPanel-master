namespace MMakerBotPanel.WebServices.Bybit.Model
{
    using System.Collections.Generic;

    public class GetSymbolData
    {
        public int ret_code { get; set; }
        public string ret_msg { get; set; }
        public object ext_code { get; set; }
        public object ext_info { get; set; }
        public List<Result> result { get; set; }
    }
}