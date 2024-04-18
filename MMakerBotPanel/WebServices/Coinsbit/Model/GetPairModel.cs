namespace MMakerBotPanel.WebServices.Coinsbit.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class GetPairModel
    {
        public GetPairModel()
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult { get; set; }
        public int code { get; set; }
        public Message message { get; set; }
        public List<GetPairResult> result { get; set; }
    }
}