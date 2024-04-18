namespace MMakerBotPanel.WebServices.Upbit
{
    using MMakerBotPanel.Models;
    using MMakerBotPanel.Models.ApiModels;
    using MMakerBotPanel.Models.CexBotModels;
    using MMakerBotPanel.Models.ChartsModel;
    using MMakerBotPanel.Models.ChartsModels;
    using MMakerBotPanel.Models.MakerMarketingModels;
    using MMakerBotPanel.WebServices.Interface;
    using MMakerBotPanel.WebServices.Upbit.Model;
    using System;
    using System.Collections.Generic;

    public class UpbitApiController : ICEXAPIController
    {
        private readonly UpbitWebServices services = new UpbitWebServices();

        public List<CandleStickStockChartDataModel> GetChartData(string symbol, Interval interval, CandlestickQuantity? candlestickQuantity)
        {
            string symbol1 = symbol.Substring(0, symbol.Length - 4);
            string symbolUpbit = "USDT-" + symbol1;

            UpbitDataModel ret = services.getStockData(symbolUpbit, "minutes/240");
            List<CandleStickStockChartDataModel> dataList = new List<CandleStickStockChartDataModel>();

            foreach (UpbitChartDataModel item in ret.UpbitChartDataModels)
            {
                CandleStickStockChartDataModel data = new CandleStickStockChartDataModel
                {
                    Date = item.time,
                    Open = item.open,
                    High = item.high,
                    Low = item.low,
                    Close = item.close,
                    Volume = item.volume
                };
                dataList.Add(data);
            }
            dataList.Reverse();
            return dataList;
        }

        public List<CandleStickStockChartDataModel> GetChartDataDate(string symbol, Interval interval, CandlestickQuantity? candlestickQuantity)
        {
            string symbol1 = symbol.Substring(0, symbol.Length - 4);
            string symbolUpbit = "USDT-" + symbol1;

            UpbitDataModel ret = services.getStockDataDate(symbolUpbit, "minutes/240");
            List<CandleStickStockChartDataModel> dataList = new List<CandleStickStockChartDataModel>();

            foreach (UpbitChartDataModel item in ret.UpbitChartDataModels)
            {
                CandleStickStockChartDataModel data = new CandleStickStockChartDataModel
                {
                    Date = item.time,
                    Open = item.open,
                    High = item.high,
                    Low = item.low,
                    Close = item.close,
                    Volume = item.volume
                };
                dataList.Add(data);
            }
            dataList.Reverse();
            return dataList;
        }

        public PairList GetPairList()
        {
            UpbitSymbolModel ret = services.getParity();
            PairList pairList = new PairList();
            foreach (UpbitSymbolDetailModel item in ret.UpbitSymbolDetailModels)
            {
                string[] splitList = item.market.Split('-');
                string baseSymbol = splitList[1];
                string quoteSymbol = splitList[0];


                if (splitList[0] == "USDT")
                {
                    Pair pair = new Pair
                    {
                        symbol = baseSymbol + quoteSymbol,
                        baseAsset = splitList[1],
                        quoteAsset = splitList[0]
                    };
                    pairList.Pairs.Add(pair);
                }

            }
            return pairList;
        }

        public CexBotStatusModel Status()
        {
            StatusModel ret = services.status();
            CexBotStatusModel upbitStatus = new CexBotStatusModel();
            if (ret.statusUpbitModels[0].timestamp != 0)
            {
                string serverTime = ret.statusUpbitModels[0].timestamp.ToString().Substring(0, ret.statusUpbitModels[0].timestamp.ToString().Length - 3);
                DateTime dateTime = UnixTimeStampToDateTime(Convert.ToDouble(serverTime));
                upbitStatus.CexBotStatusModels.status = true;
                upbitStatus.CexBotStatusModels.date = dateTime;
            }
            else
            {
                upbitStatus.CexBotStatusModels.status = false;
            }

            return upbitStatus;
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }

        public List<BalanceResponseModel> GetBalance()
        {
            throw new NotImplementedException();
        }

        public List<ActiveOrderResponseModel> GetActiveOrder()
        {
            throw new NotImplementedException();
        }

        public OrderBookResponseModel GetOrderBook()
        {
            throw new NotImplementedException();
        }

        public List<TradesResponseModel> GetTrade()
        {
            throw new NotImplementedException();
        }

        public List<ActiveOrderResponseModel> GetActiveOrder(string symbol)
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
        CreateOrderResponseModel ICEXAPIController.CreateSpotOrder(CreateOrderRequestModel request)
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
