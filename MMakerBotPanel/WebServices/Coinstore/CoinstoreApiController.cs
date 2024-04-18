namespace MMakerBotPanel.WebServices.Coinstore
{
    using MMakerBotPanel.Business;
    using MMakerBotPanel.Models.ApiModels;
    using MMakerBotPanel.Models.CexBotModels;
    using MMakerBotPanel.Models.ChartsModel;
    using MMakerBotPanel.Models.ChartsModels;
    using MMakerBotPanel.Models.MakerMarketingModels;
    using MMakerBotPanel.WebServices.Coinstore.Model;
    using MMakerBotPanel.WebServices.Interface;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    public class CoinstoreApiController : ICEXAPIController
    {
        private readonly CoinstoreWebServices services = new CoinstoreWebServices();

        public List<CandleStickStockChartDataModel> GetChartData(string symbol, Interval interval, CandlestickQuantity? candlestickQuantity)
        {

            CoinStoreDataModel ret = services.getStockData(symbol, "4hour");
            List<CandleStickStockChartDataModel> dataList = new List<CandleStickStockChartDataModel>();

            foreach (CoinStoreChartDataModel item in ret.CoinStoreChartDataModels)
            {
                CandleStickStockChartDataModel data = new CandleStickStockChartDataModel
                {
                    Date = item.time * 1000,
                    Open = item.open,
                    High = item.high,
                    Low = item.low,
                    Close = item.close,
                    Volume = item.volume
                };
                dataList.Add(data);
            }

            return dataList;
        }

        public List<CandleStickStockChartDataModel> GetChartDataDate(string symbol, Interval interval, CandlestickQuantity? candlestickQuantity)
        {

            CoinStoreDataModel ret = services.getStockDataDate(symbol, "4hour");
            List<CandleStickStockChartDataModel> dataList = new List<CandleStickStockChartDataModel>();
            CoinStoreChartDataModel data = ret.CoinStoreChartDataModels.Count > 0
                ? ret.CoinStoreChartDataModels[ret.CoinStoreChartDataModels.Count - 1]
                : ret.CoinStoreChartDataModels[0];
            CandleStickStockChartDataModel dataItem = new CandleStickStockChartDataModel
            {
                Date = data.time * 1000,
                Open = data.open,
                High = data.high,
                Low = data.low,
                Close = data.close,
                Volume = data.volume
            };
            dataList.Add(dataItem);


            return dataList;
        }

        public PairList GetPairList()
        {
            CoinstoreSymbolModel ret = services.getParity();
            PairList pairList = new PairList();
            foreach (Datum item in ret.CoinstoreSymbolDataModels.data)
            {
                string symbol = item.symbol.Substring(item.symbol.Length - 4, 4);

                if (symbol == "USDT")
                {
                    _ = item.symbol.Substring(0, item.symbol.Length - 4);
                    Pair pair = new Pair
                    {
                        Id = item.instrumentId,
                        symbol = item.symbol,
                        baseAsset = item.symbol.Substring(0, item.symbol.Length - 4),
                        quoteAsset = "USDT"
                    };
                    pairList.Pairs.Add(pair);
                }

            }

            //_ = CreateSpotOrder();
            return pairList;
        }

        public CexBotStatusModel Status()
        {
            StatusModel ret = services.status();
            DateTime dateTime = UnixTimeStampToDateTime(Convert.ToDouble(ret.data[0].time));
            CexBotStatusModel bybitStatus = new CexBotStatusModel();
            if (ret.data[0].time != 0)
            {
                bybitStatus.CexBotStatusModels.status = true;
                bybitStatus.CexBotStatusModels.date = dateTime;
            }
            else
            {
                bybitStatus.CexBotStatusModels.status = false;
            }

            return bybitStatus;
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {

            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }

        //public CreateOrderResponseModel CreateSpotOrder(CreateOrderRequestModel requestModel)
        //{
        //    Tuple<string, string, string> APITuple = CoinstoreHelper.GetCoinstoreHelper().GetUserAPIKeys();
        //    string apiKey = APITuple.Item1;
        //    string secretKey = APITuple.Item2;
        //    if (apiKey == "" || secretKey == "")
        //    {
        //        return new CreateOrderResponseModel();
        //    }
        //    long timestamp = Convert.ToInt64(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds);
        //    string symbol = "TRXUSDT";
        //    var body = new
        //    {
        //        symbol,
        //        side = "BUY",
        //        ordType = "LIMIT",
        //        ordPrice = 6.2542e-2,
        //        ordQty = "12435461",
        //        timestamp
        //    };
        //    string signature = GetSign(timestamp, secretKey, body);
        //    _ = services.newOrder(signature, apiKey, Convert.ToString(timestamp), body);
        //    CreateOrderResponseModel createOrderResponseModel = new CreateOrderResponseModel()
        //    {
        //        Symbol = symbol,
        //        //OrderId = Convert.ToString(response.result.ToList().First().orderId),
        //        //Timestamp = Convert.ToInt64(response.result.ToList().First().timestamp)
        //    };
        //    return createOrderResponseModel;
        //}

        public string GetSign(long timestamp, string secretKey, object body)
        {
            string time = Convert.ToString(timestamp / 30000);
            string hashKey = CryptographyHelper.GetHMAC256(time, secretKey, Models.RETURNTYPE.HEX);
            string patameters = JsonConvert.SerializeObject(body);
            string sign = CryptographyHelper.GetHMAC256(patameters, hashKey, Models.RETURNTYPE.HEX);
            return sign;
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

        public CreateOrderResponseModel CreateSpotOrder(CreateOrderRequestModel request)
        {
            throw new NotImplementedException();
        }
    }
}