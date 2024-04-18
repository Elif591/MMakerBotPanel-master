namespace MMakerBotPanel.WebServices.Upbit.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class UpbitDataModel
    {
        public UpbitDataModel()
        {
            genericResult = new GenericResult();
            UpbitChartDataModels = new List<UpbitChartDataModel>();
            UpbitDataDetailModels = new List<UpbitDataDetailModel>();
        }
        public GenericResult genericResult;
        public List<UpbitChartDataModel> UpbitChartDataModels;
        public List<UpbitDataDetailModel> UpbitDataDetailModels;
    }

}