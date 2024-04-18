namespace MMakerBotPanel.WebServices.Upbit.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class UpbitSymbolModel
    {
        public UpbitSymbolModel()
        {
            genericResult = new GenericResult();
            UpbitSymbolDetailModels = new List<UpbitSymbolDetailModel>();
        }

        public GenericResult genericResult;
        public List<UpbitSymbolDetailModel> UpbitSymbolDetailModels;
    }

}