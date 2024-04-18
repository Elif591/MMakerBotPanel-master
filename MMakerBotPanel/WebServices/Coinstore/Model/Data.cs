namespace MMakerBotPanel.WebServices.Coinstore.Model
{
    using System.Collections.Generic;

    public class Data
    {
        public string channel { get; set; }
        public List<Item> item { get; set; }
        public string symbol { get; set; }
        public int instrumentId { get; set; }
    }
}