namespace MMakerBotPanel.WebServices.Kucoin.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class ActiveOrderModel
    {
        public ActiveOrderModel()
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult { get; set; }
        public int currentPage { get; set; }
        public int pageSize { get; set; }
        public int totalNum { get; set; }
        public int totalPage { get; set; }
        public List<Item> items { get; set; }
    }
}