namespace MMakerBotPanel.WebServices.MEXC.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class MexcDataModel
    {
        public MexcDataModel()
        {
            genericResult = new GenericResult();
            MexcDataDetailModels = new MexcDataDetailModel();
            MexcDataChartModels = new List<MexcDataChartModel>();
        }
        public GenericResult genericResult;
        public MexcDataDetailModel MexcDataDetailModels;
        public List<MexcDataChartModel> MexcDataChartModels;
    }
}