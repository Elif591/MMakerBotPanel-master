namespace MMakerBotPanel.WebServices.Bitfinex
{
    using MMakerBotPanel.Business.Exchange;
    using MMakerBotPanel.Models;
    using MMakerBotPanel.Models.ApiModels;
    using MMakerBotPanel.Models.CexBotModels;
    using MMakerBotPanel.Models.ChartsModel;
    using MMakerBotPanel.Models.ChartsModels;
    using MMakerBotPanel.Models.MakerMarketingModels;
    using MMakerBotPanel.WebServices.Bitfinex.Model;
    using MMakerBotPanel.WebServices.Interface;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    public class BitfinexApiController : ICEXAPIController
    {
        private readonly BitfinexWebServices services = new BitfinexWebServices();

        public PairList GetPairList()
        {
            SymbolModel ret = services.Parity("ALL");
            PairList pairList = new PairList();
            foreach (List<object> item in ret.BitfinexSymbolDataModels.Data)
            {
                string[] objects = Array.ConvertAll<object, string>(item.ToArray(), ConvertObjectToString);
                string symbol = objects[0].Substring(objects[0].Length - 3, 3);

                if (symbol == "USD")
                {

                    Pair pair = new Pair
                    {
                        symbol = objects[0].Trim('t'),
                        baseAsset = objects[0].Trim('t').Substring(0, objects[0].Length - 3),
                        quoteAsset = objects[0].Substring(objects[0].Length - 3, 3)
                    };
                    ;
                    pairList.Pairs.Add(pair);

                }
            }
            //GetTrade();
            //GetOrderBook();
            // GetActiveOrder();
            // GetBalance();
            //_ = CreateSpotOrder();
            return pairList;
        }
        public List<CandleStickStockChartDataModel> GetChartData(string symbol, Interval interval, CandlestickQuantity? candlestickQuantity)
        {
            StockDataModel ret;
            string symbol1 = symbol.Substring(0, 1);
            if (symbol1 != "f")
            {
                string symboBit = "t" + symbol;
                ret = services.StockData(symboBit, "4h", "1000");
            }
            else
            {

                ret = services.StockData(symbol, "4h", "1000");
            }

            List<CandleStickStockChartDataModel> dataList = new List<CandleStickStockChartDataModel>();

            foreach (StockDataDetail item in ret.stockDataDetailModels)
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
            StockDataModel ret;
            string symbol1 = symbol.Substring(0, 1);
            if (symbol1 != "f")
            {
                string symboBit = "t" + symbol;
                ret = services.StockData(symboBit, "4h", "1");
            }
            else
            {

                ret = services.StockData(symbol, "4h", "1");
            }

            List<CandleStickStockChartDataModel> dataList = new List<CandleStickStockChartDataModel>();

            foreach (StockDataDetail item in ret.stockDataDetailModels)
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
            return dataList;
        }
        public CexBotStatusModel Status()
        {
            StatusModel ret = services.Status();
            TimeModel time = services.Time();
            string serverTime = time.time[0][1].ToString().Substring(0, time.time[0][1].ToString().Length - 3);
            DateTime dateTime = UnixTimeStampToDateTime(Convert.ToDouble(serverTime));
            CexBotStatusModel bitfinexStatus = new CexBotStatusModel();
            if (ret.status[0] != 0)
            {
                bitfinexStatus.CexBotStatusModels.status = true;
                bitfinexStatus.CexBotStatusModels.date = dateTime;
            }
            else
            {
                bitfinexStatus.CexBotStatusModels.status = false;
            }

            return bitfinexStatus;

        }
        public TradeModel GetTrade()
        {
            string symbol = "tBTCUSD";
            TradeModel res = services.Trade(symbol);
            return res;
        }
        public CreateOrderResponseModel CreateSpotOrder(CreateOrderRequestModel requestModel)
        {
            Tuple<string, string, string> APITuple = BitfinexHelper.GetBitfinexHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new CreateOrderResponseModel();
            }
            long timestamp = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;
            string type = requestModel.typeTrade.ToString();
            string symbol = "t" + requestModel.symbol;
            string amount = requestModel.amount;
            string price = requestModel.price;
            object body = new
            {
                type,
                symbol,
                amount,
                price
            };
            string path = "v2/auth/w/order/submit";
            string signStr = $"/api/{path}{timestamp}{JsonConvert.SerializeObject(body)}";
            string signature = GetSign(secretKey, signStr);
            CreateOrderModel response = services.CreateOrder(timestamp.ToString(), apiKey, signature, body);
            CreateOrderResponseModel createOrderResponseModel = new CreateOrderResponseModel()
            {
                Symbol = symbol,
                OrderId = Convert.ToString(response.DATA.ToList().First().ID),
                Timestamp = timestamp
            };
            return createOrderResponseModel;
        }
        public List<BalanceResponseModel> GetBalance()
        {
            Tuple<string, string, string> APITuple = BitfinexHelper.GetBitfinexHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new List<BalanceResponseModel>();
            }
            long timestamp = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;
            string path = "v2/auth/r/wallets";
            string signStr = "/api/" + path + $"{timestamp}";
            string signature = GetSign(secretKey, signStr);
            BalanceModel response = services.Balance(timestamp.ToString(), apiKey, signature);
            List<BalanceResponseModel> balances = new List<BalanceResponseModel>();
            foreach (List<object> item in response.Wallets)
            {
                object[] dataItem = item.ToArray();
                BalanceResponseModel balance = new BalanceResponseModel()
                {
                    Symbol = Convert.ToString(dataItem[1]),
                    Balance = Convert.ToString(dataItem[2]),
                };
                balances.Add(balance);
            }
            return balances;
        }
        public List<ActiveOrderResponseModel> GetActiveOrder(string symbol)
        {
            Tuple<string, string, string> APITuple = BitfinexHelper.GetBitfinexHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new List<ActiveOrderResponseModel>();
            }
            long timestamp = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds);
            string path = "v2/auth/r/orders";
            string signStr = "/api/" + path + $"{timestamp}";
            string signature = GetSign(secretKey, signStr);
            ActiveOrderModel response = services.ActiveOrder(timestamp.ToString(), apiKey, signature.ToLower(), symbol);
            List<ActiveOrderResponseModel> activeOrders = new List<ActiveOrderResponseModel>();
            foreach (List<object> item in response.ActiveOrders)
            {
                ActiveOrderResponseModel activeOrder = new ActiveOrderResponseModel()
                {
                    Timestamp = timestamp,
                    Symbol = symbol,
                    OrderId = item[0].ToString(),
                    Status = item[13].ToString(),
                    Type = item[8].ToString(),
                };
                activeOrders.Add(activeOrder);
            }
            return activeOrders;
        }
        public OrderBookResponseModel GetOrderBook(string symbol)
        {
            OrderBookModel res = services.OrderBook(symbol);
            OrderBookResponseModel orderBook = new OrderBookResponseModel();
            foreach (List<double> item in res.Orders)
            {
                if (item[2] > 0)
                {
                    OrderBookBuy buy = new OrderBookBuy();
                    buy.Price = (decimal)item[0];
                    buy.Volume = (item[1] * item[2]).ToString();
                    orderBook.Buy.Add(buy);
                }
                else
                {
                    OrderBookSell sell = new OrderBookSell();
                    sell.Price = (decimal)item[0];
                    sell.Volume = (item[1] * item[2]).ToString();
                    orderBook.Sell.Add(sell);
                }

            }
            return orderBook;
        }
        public List<TradesResponseModel> GetTrade(string symbol)
        {
            throw new NotImplementedException();
        }
        public PairList GetPair(string parity)
        {
            SymbolModel ret = services.Parity(parity);
            PairList pairList = new PairList();
            foreach (List<object> item in ret.BitfinexSymbolDataModels.Data)
            {
                string[] objects = Array.ConvertAll<object, string>(item.ToArray(), ConvertObjectToString);
                string symbol = objects[0].Substring(objects[0].Length - 3, 3);

                if (symbol == "USD")
                {
                    Pair pair = new Pair
                    {
                        symbol = objects[0].Trim('t'),
                        baseAsset = objects[0].Trim('t').Substring(0, objects[0].Length - 3),
                        quoteAsset = objects[0].Substring(objects[0].Length - 3, 3),
                        qtyFormat = "0.001"
                    };
                    pairList.Pairs.Add(pair);

                }
            }
            return pairList;
        }
        public List<CancelOrderResponseModel> CancelOrders(string symbol)
        {
            Tuple<string, string, string> APITuple = BitfinexHelper.GetBitfinexHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new List<CancelOrderResponseModel>();
            }
            long timestamp = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds);
            string path = "v2/auth/w/order/cancel/multi";

            string signStr = "/api/" + path + $"{timestamp}";
            string signature = GetSign(secretKey, signStr);
            CancelOrderModel response = services.CancelOrder(timestamp.ToString(), apiKey, signature.ToLower());
            List<CancelOrderResponseModel> cancelOrders = new List<CancelOrderResponseModel>();
            foreach (CancelOrder item in response.CancelOrderList)
            {
                CancelOrderResponseModel cancelOrder = new CancelOrderResponseModel()
                {
                    symbol = symbol,
                    orderId = item.CancelOrders[0].ToString(),
                    status = item.CancelOrders[13].ToString()
                };
                cancelOrders.Add(cancelOrder);

            }
            return cancelOrders;
        }
        public QueryOrderResponseModel QueryOrders(string symbol, string orderId)
        {
            Tuple<string, string, string> APITuple = BitfinexHelper.GetBitfinexHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new QueryOrderResponseModel();
            }
            long timestamp = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds);
            string path = "v2/auth/r/orders";
            object body = new
            {
                id = orderId

            };
            string signStr = "/api/" + path + $"{timestamp}{JsonConvert.SerializeObject(body)}";
            string signature = GetSign(secretKey, signStr);
            QueryOrderModel response = services.QueryOrder(timestamp.ToString(), apiKey, signature.ToLower(), body);
            QueryOrderResponseModel queryOrder = new QueryOrderResponseModel();
            foreach (List<object> item in response.QueryOrders)
            {
                queryOrder.symbol = symbol;
                queryOrder.orderId = orderId;
                queryOrder.qty = item[6].ToString();
                queryOrder.price = item[16].ToString();
                queryOrder.status = item[13].ToString();
                if (item[13].ToString() == "PARTIALLY FILLED")
                {
                    queryOrder.status = "FILLED";
                }
              
            }
            return queryOrder;
        }
        public ComissionFeeResponseModel ComissionFee(string symbol)
        {
            ComissionFeeResponseModel comissionFee = new ComissionFeeResponseModel()
            {
                symbol = symbol,
                makerCommission = "0.1",
                takerCommission = "0.1"
            };
            return comissionFee;    
        }
        public string GetSign(string signStr, string secretKey)
        {

            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] keyBytes = encoding.GetBytes(secretKey);
            byte[] messageBytes = encoding.GetBytes(signStr);

            using (HMACSHA384 hmacsha384 = new HMACSHA384(keyBytes))
            {
                byte[] hashMessage = hmacsha384.ComputeHash(messageBytes);
                return ByteArrayToString(hashMessage);
            }
        }
        private static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
            {
                _ = hex.AppendFormat("{0:x2}", b);
            }

            return hex.ToString();
        }
        private string ConvertObjectToString(object obj)
        {
            return obj?.ToString() ?? string.Empty;
        }
        public DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }
    }
}