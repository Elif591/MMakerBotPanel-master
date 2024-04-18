namespace MMakerBotPanel.WebServices.Coinbase
{
    using MMakerBotPanel.Business;
    using MMakerBotPanel.Business.Exchange;
    using MMakerBotPanel.Models.ApiModels;
    using MMakerBotPanel.Models.CexBotModels;
    using MMakerBotPanel.Models.ChartsModel;
    using MMakerBotPanel.Models.ChartsModels;
    using MMakerBotPanel.Models.MakerMarketingModels;
    using MMakerBotPanel.WebServices.Coinbase.Model;
    using MMakerBotPanel.WebServices.Interface;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    public class CoinbaseApiController : ICEXAPIController
    {
        private readonly CoinbaseWebServices services = new CoinbaseWebServices();

        public List<CandleStickStockChartDataModel> GetChartData(string symbol, Interval interval, CandlestickQuantity? candlestickQuantity)
        {
            string symbol1 = symbol.Substring(0, symbol.Length - 4);

            symbol = symbol1 + "-USDT";
            CoinbaseDataModel ret = services.getStockData(symbol, "3600");
            List<CandleStickStockChartDataModel> dataList = new List<CandleStickStockChartDataModel>();
            List<CandleStickStockChartDataModel> dataDexList = new List<CandleStickStockChartDataModel>();
            int sayac = 0;
            foreach (CoinbaseChartDataModel item in ret.CoinbaseChartDataModels)
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
                sayac++;
                if (sayac == 4)
                {
                    CandleStickStockChartDataModel dataDex = new CandleStickStockChartDataModel
                    {
                        Open = dataList[3].Open,
                        Date = dataList[3].Date,
                        Close = dataList[0].Close
                    };
                    double high = dataList[0].High;
                    for (int i = 0; i < 4; i++)
                    {
                        if (high < dataList[i].High)
                        { high = dataList[i].High; }
                    }
                    dataDex.High = high;
                    double low = dataList[0].Low;
                    for (int i = 0; i < 4; i++)
                    {
                        if (low > dataList[i].Low)
                        { low = dataList[i].Low; }
                    }
                    dataDex.Low = low;
                    for (int i = 0; i < 4; ++i)
                    {
                        dataDex.Volume += dataList[i].Volume;
                    }

                    dataDexList.Add(dataDex);
                    sayac = 0;
                    for (int i = 0; i < dataList.Count - 1; ++i)
                    {
                        _ = dataList.Remove(dataList[i]);
                    }
                    int count = dataList.Count;
                }

            }

            dataDexList.Reverse();
            return dataDexList;
        }

        public List<CandleStickStockChartDataModel> GetChartDataDate(string symbol, Interval interval, CandlestickQuantity? candlestickQuantity)
        {
            string symbol1 = symbol.Substring(0, symbol.Length - 4);

            symbol = symbol1 + "-USDT";
            CoinbaseDataModel ret = services.getStockDataDate(symbol, "3600");
            List<CandleStickStockChartDataModel> dataList = new List<CandleStickStockChartDataModel>();
            List<CandleStickStockChartDataModel> dataDexList = new List<CandleStickStockChartDataModel>();
            int sayac = 0;
            foreach (CoinbaseChartDataModel item in ret.CoinbaseChartDataModels)
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
                sayac++;
                if (sayac == 4)
                {
                    CandleStickStockChartDataModel dataDex = new CandleStickStockChartDataModel
                    {
                        Open = dataList[2].Open,
                        Date = dataList[0].Date,
                        Close = dataList[0].Close
                    };
                    double high = dataList[0].High;
                    for (int i = 0; i < 4; i++)
                    {
                        if (high < dataList[i].High)
                        { high = dataList[i].High; }
                    }
                    dataDex.High = high;
                    double low = dataList[0].Low;
                    for (int i = 0; i < 4; i++)
                    {
                        if (low > dataList[i].Low)
                        { low = dataList[i].Low; }
                    }
                    dataDex.Low = low;
                    for (int i = 0; i < 4; ++i)
                    {
                        dataDex.Volume += dataList[i].Volume;
                    }

                    dataDexList.Add(dataDex);
                    sayac = 0;
                    for (int i = 0; i < dataList.Count - 1; ++i)
                    {
                        _ = dataList.Remove(dataList[i]);
                    }
                    int count = dataList.Count;
                }

            }
            dataDexList.Reverse();
            return dataDexList;
        }

        public PairList GetPairList()
        {
            CoinbaseSymbolModel ret = services.getParity();
            PairList pairList = new PairList();

            foreach (CoinbaseSymbol item in ret.CoinbaseSymbolList)
            {
                if (item.status == "online" && item.quote_currency == "USDT" && !item.trading_disabled)
                {
                    Pair pair = new Pair();
                    string[] symboList = item.id.Split('-');
                    pair.symbol = symboList[0] + symboList[1];
                    pair.baseAsset = item.base_currency;
                    pair.quoteAsset = item.quote_currency;
                    pairList.Pairs.Add(pair);
                }
            }
            //_ = CreateSpotOrder();
            return pairList;
        }

        public CexBotStatusModel Status()
        {
            StatusModel ret = services.status();
            DateTime dateTime = UnixTimeStampToDateTime(Convert.ToDouble(ret.data.epoch));
            CexBotStatusModel coinbaseStatus = new CexBotStatusModel();
            if (ret.data != null)
            {
                coinbaseStatus.CexBotStatusModels.status = true;
                coinbaseStatus.CexBotStatusModels.date = dateTime;
            }
            else
            {
                coinbaseStatus.CexBotStatusModels.status = false;
            }

            return coinbaseStatus;
        }
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }

        //public CreateOrderResponseModel CreateSpotOrder(CreateOrderRequestModel requestModel)
        //{
        //    Tuple<string, string , string> APITuple = CoinbaseHelper.GetCoinbaseHelper().GetUserAPIKeys();
        //    string apiKey = APITuple.Item1;
        //    string secretKey = APITuple.Item2;
        //    if (apiKey == "" || secretKey == "")
        //    {
        //        return new CreateOrderResponseModel();
        //    }
        //    string timestamp = Convert.ToString(DateTimeOffset.Now.ToUnixTimeSeconds());
        //    string method = "POST";
        //    string endPoint = "/api/v3/brokerage/orders";
        //    string product_id = "BTC-USD";
        //    string client_order_id = "fe4feca8-c7dd-11ed-afa1-0242ac120002";
        //    //  string price = "0.1";
        //    //  string type = "limit";
        //    string side = "BUY";
        //    // string size = "30.000";
        //    var body = new
        //    {
        //        client_order_id,
        //        product_id,
        //        side,
        //    };
        //    string signText = $"{timestamp}{method}{endPoint}{JsonConvert.SerializeObject(body)}";
        //    _ = GetSign(signText, secretKey);
        //    _ = services.AuthorizeClient();

        //    //string res = services.CreateOrder(apiKey, timestamp, signature, body);
        //    return new CreateOrderResponseModel();
        //}


        public string GetSign(string signText, string decodeSecret)
        {
            return CryptographyHelper.GetHMAC256(signText, decodeSecret, Models.RETURNTYPE.BASE64);
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