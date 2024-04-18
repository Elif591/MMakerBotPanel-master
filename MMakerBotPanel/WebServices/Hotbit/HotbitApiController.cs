namespace MMakerBotPanel.WebServices.Hotbit
{
    using MMakerBotPanel.Models;
    using MMakerBotPanel.Models.ApiModels;
    using MMakerBotPanel.Models.CexBotModels;
    using MMakerBotPanel.Models.ChartsModel;
    using MMakerBotPanel.Models.ChartsModels;
    using MMakerBotPanel.Models.MakerMarketingModels;
    using MMakerBotPanel.WebServices.Hotbit.Model;
    using MMakerBotPanel.WebServices.Interface;
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    public class HotbitApiController : ICEXAPIController
    {
        private readonly HotbitWebServices services = new HotbitWebServices();

        public List<CandleStickStockChartDataModel> GetChartData(string symbol, Interval interval, CandlestickQuantity? candlestickQuantity)
        {
            symbol = symbol.Substring(0, symbol.Length - 4) + "/USDT";
            _ = new DateTime(1970, 1, 1, 0, 0, 0);
            long endTime = (long)(DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
            long startTime = endTime - 7171200;
            HotbitStockDataModel ret = services.getStockData(symbol, "14400", startTime, endTime);
            List<CandleStickStockChartDataModel> dataList = new List<CandleStickStockChartDataModel>();

            CultureInfo CI = new CultureInfo("en-US", false);
            foreach (List<object> item in ret.result)
            {
                CandleStickStockChartDataModel data = new CandleStickStockChartDataModel
                {
                    Date = Convert.ToInt64(item.ToArray()[0]) * 1000,
                    Open = Convert.ToDouble(item.ToArray()[1], CI),
                    Close = Convert.ToDouble(item.ToArray()[2], CI),
                    High = Convert.ToDouble(item.ToArray()[3], CI),
                    Low = Convert.ToDouble(item.ToArray()[4], CI),
                    Volume = (long)Convert.ToDouble(item.ToArray()[6], CI)
                };
                dataList.Add(data);
            }

            return dataList;
        }

        public List<CandleStickStockChartDataModel> GetChartDataDate(string symbol, Interval interval, CandlestickQuantity? candlestickQuantity)
        {
            symbol = symbol.Substring(0, symbol.Length - 4) + "/USDT";


            int endTime = (int)(DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds;

            long startTime = endTime - 14400;

            HotbitStockDataModel ret = services.getStockDataDate(symbol, "14400", startTime, endTime);
            List<CandleStickStockChartDataModel> dataList = new List<CandleStickStockChartDataModel>();

            CultureInfo CI = new CultureInfo("en-US", false);

            if (ret.result.Count > 0)
            {
                List<object> item = ret.result.ToArray()[ret.result.Count - 1];

                CandleStickStockChartDataModel data = new CandleStickStockChartDataModel
                {
                    Date = Convert.ToInt64(item.ToArray()[0]) * 1000,
                    Open = Convert.ToDouble(item.ToArray()[1], CI),
                    Close = Convert.ToDouble(item.ToArray()[2], CI),
                    High = Convert.ToDouble(item.ToArray()[3], CI),
                    Low = Convert.ToDouble(item.ToArray()[4], CI),
                    Volume = (long)Convert.ToDouble(item.ToArray()[6], CI)
                };
                dataList.Add(data);


            }

            return dataList;
        }

        public PairList GetPairList()
        {
            HotbitSymbolModel ret = services.getParity();
            PairList pairList = new PairList();
            foreach (Result item in ret.result)
            {
                if (item.money == "USDT")
                {
                    Pair pair = new Pair
                    {
                        symbol = item.name,
                        baseAsset = item.stock,
                        quoteAsset = item.money
                    };
                    pairList.Pairs.Add(pair);
                }

            }


            return pairList;
        }

        public CexBotStatusModel Status()
        {
            StatusModel ret = services.status();
            DateTime dateTime = UnixTimeStampToDateTime(Convert.ToDouble(ret.result));
            CexBotStatusModel gateioStatus = new CexBotStatusModel();
            gateioStatus.CexBotStatusModels.status = ret.result != 0;
            gateioStatus.CexBotStatusModels.date = dateTime;
            return gateioStatus;
        }


        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }

        public CreateOrderResponseModel CreateSpotOrder(CreateOrderRequestModel requestModel)
        {
            throw new NotImplementedException();
        }

        public OrderBookResponseModel GetOrderBook(string symbol)
        {
            throw new NotImplementedException();
        }

        public List<TradesResponseModel> GetTrade(string symbol)
        {
            throw new NotImplementedException();
        }

        public List<BalanceResponseModel> GetBalance()
        {
            throw new NotImplementedException();
        }

        public List<ActiveOrderResponseModel> GetActiveOrder(string symbol)
        {
            throw new NotImplementedException();
        }

        public PairList GetPair(string parity)
        {
            throw new NotImplementedException();
        }

        public List<CancelOrderResponseModel> CancelOrders(string symbol)
        {
            throw new NotImplementedException();
        }

        public QueryOrderResponseModel QueryOrders(string symbol, string orderId)
        {
            throw new NotImplementedException();
        }

        public ComissionFeeResponseModel ComissionFee(string symbol)
        {
            throw new NotImplementedException();
        }
    }
}