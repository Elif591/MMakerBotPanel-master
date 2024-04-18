namespace MMakerBotPanel.WebServices.Huobi
{
    using MMakerBotPanel.Business;
    using MMakerBotPanel.Business.Exchange;
    using MMakerBotPanel.Models;
    using MMakerBotPanel.Models.ApiModels;
    using MMakerBotPanel.Models.CexBotModels;
    using MMakerBotPanel.Models.ChartsModel;
    using MMakerBotPanel.Models.ChartsModels;
    using MMakerBotPanel.Models.MakerMarketingModels;
    using MMakerBotPanel.WebServices.Huobi.Model;
    using MMakerBotPanel.WebServices.Interface;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    public class HuobiApiController : ICEXAPIController
    {
        private readonly HuobiWebServices services = new HuobiWebServices();

        public PairList GetPairList()
        {
            SymbolModel ret = services.Parity();
            PairList pairList = new PairList();
            foreach (Datum item in ret.data)
            {
                if (item.state == "online" && item.qcdn == "USDT")
                {
                    Pair pair = new Pair
                    {
                        symbol = item.bcdn + item.qcdn,
                        baseAsset = item.bcdn,
                        quoteAsset = item.qcdn
                    };
                    pairList.Pairs.Add(pair);
                }
            }
            _ = Account();
            //CreateSpotOrder();
            // GetOrderBook();
            //  GetTrade();
            // GetActiveOrder();
            /* = GetBalance();*/
            CancelOrders("ADAUSDT");
            return pairList;
        }
        public List<CandleStickStockChartDataModel> GetChartData(string symbol, Interval interval, CandlestickQuantity? candlestickQuantity)
        {
            int timeInterval = (int)interval;
            string timeIntervalStr;
            int candleQty = (int)candlestickQuantity;

            if ((int)interval == (int)Interval.minutes30)
            {
                timeIntervalStr = timeInterval.ToString() + "min";
            }
            else if ((int)interval == (int)Interval.hours1)
            {
                timeIntervalStr = "60min";
            }
            else if ((int)interval == (int)Interval.hours2)
            {
                timeIntervalStr = "60min";
                candleQty = 2000;
            }
            else
            {
                timeIntervalStr = timeInterval.ToString() + "hour";
            }

            if (candlestickQuantity == CandlestickQuantity.Candle1080)
            {
                candleQty = 1080;
            }

            StockDataModel ret = services.StockData(symbol, timeIntervalStr, candleQty.ToString());
            List<CandleStickStockChartDataModel> dataList = new List<CandleStickStockChartDataModel>();
            foreach (ChartData item in ret.HuobiChartModels)
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
            dataList.Reverse();
            return dataList;
        }
        public List<CandleStickStockChartDataModel> GetChartDataDate(string symbol, Interval interval, CandlestickQuantity? candlestickQuantity)
        {
            StockDataModel ret = services.StockData(symbol.ToLower(), "4hour", "4");
            List<CandleStickStockChartDataModel> dataList = new List<CandleStickStockChartDataModel>();
            foreach (ChartData item in ret.HuobiChartModels)
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
        public CexBotStatusModel Status()
        {
            StatusModel ret = services.Status();
            string serverTime = ret.data.ToString().Substring(0, ret.data.ToString().Length - 3);
            DateTime dateTime = UnixTimeStampToDateTime(Convert.ToDouble(serverTime));
            CexBotStatusModel gateioStatus = new CexBotStatusModel();
            if (ret.data != 0)
            {
                gateioStatus.CexBotStatusModels.status = true;
                gateioStatus.CexBotStatusModels.date = dateTime;
            }
            else
            {
                gateioStatus.CexBotStatusModels.status = false;
            }

            return gateioStatus;
        }
        public List<TradesResponseModel> GetTrade(string symbol)
        {
            TradeModel response = services.Trade(symbol);
            List<TradesResponseModel> trades = new List<TradesResponseModel>();
            foreach (TradeDatum item in response.data)
            {
                foreach (TradeDatum data in item.data)
                {
                    double volume = data.price * data.amount;
                    TradesResponseModel trade = new TradesResponseModel()
                    {
                        Symbol = symbol,
                        Price = data.price.ToString(),
                        Volume = volume.ToString(),
                        Timestamp = (long)data.ts,
                        Type = data.direction
                    };
                    trades.Add(trade);
                }

            }

            return trades;
        }
        public OrderBookResponseModel GetOrderBook(string symbol)
        {
            OrderBookModel response = services.OrderBook(symbol);
            OrderBookResponseModel orderBooks = new OrderBookResponseModel();
            foreach (List<double> item in response.tick.bids)
            {
                double[] dataItem = item.ToArray();
                double size = Convert.ToDouble(dataItem[1].ToString().Replace('.', ','));
                double price = Convert.ToDouble(dataItem[0].ToString().Replace('.', ','));
                double volume = size * price;
                OrderBookBuy buy = new OrderBookBuy()
                {
                    Volume = volume.ToString(),
                    Price = Convert.ToDecimal(dataItem[0]),
                };
                orderBooks.Buy.Add(buy);
            }
            foreach (List<double> item in response.tick.asks)
            {
                double[] dataItem = item.ToArray();
                double size = Convert.ToDouble(dataItem[1].ToString().Replace('.', ','));
                double price = Convert.ToDouble(dataItem[0].ToString().Replace('.', ','));
                double volume = size * price;
                OrderBookSell sell = new OrderBookSell()
                {
                    Volume = volume.ToString(),
                    Price = Convert.ToDecimal(dataItem[0]),
                };
                orderBooks.Sell.Add(sell);
            }
            return orderBooks;
        }
        public CreateOrderResponseModel CreateSpotOrder(CreateOrderRequestModel requestModel)
        {
            Tuple<string, string, string> APITuple = HuobiHelper.GetHuobiHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new CreateOrderResponseModel();
            }
            int accountId = HuobiCache.GetHuobiCash().accountId;
            string type = Enum.GetName(typeof(SIDE), requestModel.side).ToLower() + "-" + Enum.GetName(typeof(TYPE_TRADE), requestModel.typeTrade).ToLower();
            Dictionary<string, string> orderJsonBody = new Dictionary<string, string>()
                {
                {"account-id", accountId.ToString()},
                {"amount" , requestModel.amount},
                {"price", requestModel.price },
                {"source", "spot-api"},
                {"symbol", requestModel.symbol},
                {"type", type}
                };
            string timeOrder = DateTime.UtcNow.ToString("yyyy-MM-dd'T'HH:mm:ss");
            string timestamp = Uri.EscapeDataString(timeOrder);
            string signatureMethod = "HmacSHA256";
            string signatureVersion = "2";
            string methodOrder = "POST";
            string urlOrder = "api.huobi.pro";
            string endPointOrder = "/v1/order/orders/place";
            string standartBodyOder = $"AccessKeyId={apiKey}&SignatureMethod={signatureMethod}&SignatureVersion={signatureVersion}&Timestamp={timestamp}";
            string Signbody = $"{methodOrder}\n{urlOrder}\n{endPointOrder}\n{standartBodyOder}";
            string signatureOrder = GetSign(Signbody, secretKey);
            string signUrlEncodeOrder = Uri.EscapeDataString(signatureOrder);
            string orderBody = $"{standartBodyOder}&Signature={signUrlEncodeOrder}";
            long timeStamp = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds);
            CreateOrderModel response = services.CreateOrder(JsonConvert.SerializeObject(orderJsonBody), orderBody);
            CreateOrderResponseModel createOrderResponseModel = new CreateOrderResponseModel()
            {
                Symbol = requestModel.symbol,
                OrderId = Convert.ToString(response.data),
                Timestamp = timeStamp
            };
            return createOrderResponseModel;
        }
        public List<BalanceResponseModel> GetBalance()
        {
            Account();
            Tuple<string, string, string> APITuple = HuobiHelper.GetHuobiHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new List<BalanceResponseModel>();
            }
            int accountId = HuobiCache.GetHuobiCash().accountId;
            string endPoint = $"/v1/account/accounts/{accountId}/balance";
            string time = DateTime.UtcNow.ToString("yyyy-MM-dd'T'HH:mm:ss");
            string timestamp = Uri.EscapeDataString(time);
            string signatureMethod = "HmacSHA256";
            string signatureVersion = "2";
            string method = "GET";
            string url = "api.huobi.pro";
            string standartBody = $"AccessKeyId={apiKey}&SignatureMethod={signatureMethod}&SignatureVersion={signatureVersion}&Timestamp={timestamp}";
            string Signbody = $"{method}\n{url}\n{endPoint}\n{standartBody}";

            string signature = GetSign(Signbody, secretKey);
            string signUrlEncode = Uri.EscapeDataString(signature);
            string sign = $"{standartBody}&Signature={signUrlEncode}";
            BalanceModel response = services.Balance($"v1/account/accounts/{accountId}/balance", sign);
            List<BalanceResponseModel> balances = new List<BalanceResponseModel>();
            foreach (BalanceList item in response.data.list)
            {
                BalanceResponseModel balance = new BalanceResponseModel()
                {
                    Symbol = item.currency,
                    Balance = item.balance,
                };
                balances.Add(balance);
            }
            return balances;
        }
        public List<ActiveOrderResponseModel> GetActiveOrder(string symbol)
        {

            Tuple<string, string, string> APITuple = HuobiHelper.GetHuobiHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new List<ActiveOrderResponseModel>();
            }

            _ = HuobiCache.GetHuobiCash().accountId;
            string endPoint = $"/v1/order/openOrders";
            string time = DateTime.UtcNow.ToString("yyyy-MM-dd'T'HH:mm:ss");
            string timestamp = Uri.EscapeDataString(time);
            string signatureMethod = "HmacSHA256";
            string signatureVersion = "2";
            string method = "GET";
            string url = "api.huobi.pro";
            string standartBody = $"AccessKeyId={apiKey}&SignatureMethod={signatureMethod}&SignatureVersion={signatureVersion}&Timestamp={timestamp}";
            string Signbody = $"{method}\n{url}\n{endPoint}\n{standartBody}";

            string signature = GetSign(Signbody, secretKey);
            string signUrlEncode = Uri.EscapeDataString(signature);
            string sign = $"{standartBody}&Signature={signUrlEncode}";
            ActiveOrderModel response = services.ActiveOrder(sign);
            List<ActiveOrderResponseModel> activeOrders = new List<ActiveOrderResponseModel>();
            foreach (ActiveOrderDatum item in response.data)
            {
                string[] typeAndSide = item.type.Split('-');
                ActiveOrderResponseModel activeOrder = new ActiveOrderResponseModel()
                {
                    Symbol = item.symbol,
                    Timestamp = item.createdat,
                    OrderId = Convert.ToString(item.id),
                    Volume = item.filledcashamount,
                    Price = item.price,
                    Status = item.state,
                    Type = typeAndSide[0],
                    Side = typeAndSide[1]
                };
                activeOrders.Add(activeOrder);

            }
            return activeOrders;
        }
        public PairList GetPair(string parity)
        {
            throw new NotImplementedException();
        }
        public List<CancelOrderResponseModel> CancelOrders(string symbol)
        {
            Tuple<string, string, string> APITuple = HuobiHelper.GetHuobiHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new List<CancelOrderResponseModel>();
            }


            Dictionary<string, string> orderJsonBody = new Dictionary<string, string>()
                {
                {"symbol",symbol.ToLower()}

                };
            string timeOrder = DateTime.UtcNow.ToString("yyyy-MM-dd'T'HH:mm:ss");
            string timestamp = Uri.EscapeDataString(timeOrder);
            string signatureMethod = "HmacSHA256";
            string signatureVersion = "2";
            string methodOrder = "POST";
            string urlOrder = "api.huobi.pro";
            string endPointOrder = "/v1/order/orders/batchCancelOpenOrders";
            string standartBodyOder = $"AccessKeyId={apiKey}&SignatureMethod={signatureMethod}&SignatureVersion={signatureVersion}&Timestamp={timestamp}";
            string Signbody = $"{methodOrder}\n{urlOrder}\n{endPointOrder}\n{standartBodyOder}";
            string signatureOrder = GetSign(Signbody, secretKey);
            string signUrlEncodeOrder = Uri.EscapeDataString(signatureOrder);
            string orderBody = $"{standartBodyOder}&Signature={signUrlEncodeOrder}";
            long timeStamp = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds);
            CancelOrderModel response = services.CancelOrders(JsonConvert.SerializeObject(orderJsonBody), orderBody);
            List<CancelOrderResponseModel> cancelOrders = new List<CancelOrderResponseModel>();
            //foreach (var item in response.data)
            //{
            //    CancelOrderResponseModel cancelOrdersResponseModel = new CancelOrderResponseModel()
            //    {
            //        Symbol = item.,
            //        OrderId = Convert.ToString(response.data),
            //        Timestamp = timeStamp
            //    };

            //}

            return cancelOrders;
        }
        public QueryOrderResponseModel QueryOrders(string symbol, string orderId)
        {
            Tuple<string, string, string> APITuple = HuobiHelper.GetHuobiHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new QueryOrderResponseModel();
            }
            string endPoint = $"/v1/order/orders/{orderId}";
            string time = DateTime.UtcNow.ToString("yyyy-MM-dd'T'HH:mm:ss");
            string timestamp = Uri.EscapeDataString(time);
            string signatureMethod = "HmacSHA256";
            string signatureVersion = "2";
            string method = "GET";
            string url = "api.huobi.pro";
            string standartBody = $"AccessKeyId={apiKey}&SignatureMethod={signatureMethod}&SignatureVersion={signatureVersion}&Timestamp={timestamp}";
            string Signbody = $"{method}\n{url}\n{endPoint}\n{standartBody}";

            string signature = GetSign(Signbody, secretKey);
            string signUrlEncode = Uri.EscapeDataString(signature);
            string sign = $"{standartBody}&Signature={signUrlEncode}";
            QueryOrderModel response = services.QueryOrder(endPoint, sign);

            QueryOrderResponseModel queryOrder = new QueryOrderResponseModel();
            if (response.data != null)
            {
                queryOrder.symbol = response.data.symbol;
                queryOrder.orderId = Convert.ToString(response.data.id);
                queryOrder.status = response.data.state.ToUpper();
                queryOrder.type = response.data.type;
                queryOrder.side = response.data.type.Substring(0, 3);
                queryOrder.qty = response.data.amount;
                queryOrder.price = Convert.ToString(response.data.price);
            }
            return queryOrder;
        }
        public ComissionFeeResponseModel ComissionFee(string symbol)
        {

            ComissionFeeResponseModel comission = new ComissionFeeResponseModel()
            {
                symbol = symbol,
                makerCommission = "0.02",
                takerCommission = "0.02"
            };
            return comission;
        }
        public AccountModel Account()
        {
            Tuple<string, string, string> APITuple = HuobiHelper.GetHuobiHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new AccountModel();
            }
            string time = DateTime.UtcNow.ToString("yyyy-MM-dd'T'HH:mm:ss");
            string signatureMethod = "HmacSHA256";
            string signatureVersion = "2";
            string timestamp = Uri.EscapeDataString(time);
            string method = "GET";
            string url = "api.huobi.pro";
            string endPoint = "/v1/account/accounts";
            string standartBody = $"AccessKeyId={apiKey}&SignatureMethod={signatureMethod}&SignatureVersion={signatureVersion}&Timestamp={timestamp}";
            string body = $"{method}\n{url}\n{endPoint}\n{standartBody}";

            string signature = GetSign(body, secretKey);
            string signUrlEncode = Uri.EscapeDataString(signature);
            string accountBody = $"{standartBody}&Signature={signUrlEncode}";
            AccountModel res = services.Account(accountBody);
            int accountId = 0;
            if (res.data.Count > 0)
            {
                accountId = res.data.ToArray()[0].id;
            }
            HuobiCache.GetHuobiCash().accountId = accountId;
            return res;
        }
        public string GetSign(string body, string secretKey)
        {
            string hexSign = CryptographyHelper.GetHMAC256(body, secretKey, Models.RETURNTYPE.BASE64);
            return hexSign;
        }
        public DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }

    }
    public class HuobiCache
    {
        private static HuobiCache _huobiCache;

        private static readonly object locker = new object();
        private HuobiCache()
        {

        }

        public static HuobiCache GetHuobiCash()
        {
            lock (locker)
            {
                if (_huobiCache == null)
                {
                    _huobiCache = new HuobiCache();
                }
                return _huobiCache;
            }
        }

        public int accountId { get; set; }
    }
}