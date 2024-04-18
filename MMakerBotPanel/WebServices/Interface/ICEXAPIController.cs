namespace MMakerBotPanel.WebServices.Interface
{
    using MMakerBotPanel.Models.ApiModels;
    using MMakerBotPanel.Models.CexBotModels;
    using MMakerBotPanel.Models.ChartsModel;
    using MMakerBotPanel.Models.ChartsModels;
    using MMakerBotPanel.Models.MakerMarketingModels;
    using System.Collections.Generic;

    public interface ICEXAPIController
    {
        PairList GetPairList();
        List<CandleStickStockChartDataModel> GetChartData(string symbol, Interval interval , CandlestickQuantity? candlestickQuantity);
        List<CandleStickStockChartDataModel> GetChartDataDate(string symbol, Interval interval , CandlestickQuantity? candlestickQuantity);
        CexBotStatusModel Status();
        CreateOrderResponseModel CreateSpotOrder(CreateOrderRequestModel request);
        List<BalanceResponseModel> GetBalance();
        List<ActiveOrderResponseModel> GetActiveOrder(string symbol);
        OrderBookResponseModel GetOrderBook(string symbol);
        List<TradesResponseModel> GetTrade(string symbol);
        PairList GetPair(string parity);
        List<CancelOrderResponseModel> CancelOrders(string symbol);
        QueryOrderResponseModel QueryOrders(string symbol, string orderId);
        ComissionFeeResponseModel ComissionFee(string symbol);
    }
}
