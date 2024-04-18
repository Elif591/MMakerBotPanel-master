namespace MMakerBotPanel.WebServices.Coinsbit
{
    using MMakerBotPanel.Business.Exchange;
    using MMakerBotPanel.Models;
    using MMakerBotPanel.Models.ApiModels;
    using MMakerBotPanel.Models.CexBotModels;
    using MMakerBotPanel.Models.ChartsModel;
    using MMakerBotPanel.Models.ChartsModels;
    using MMakerBotPanel.Models.MakerMarketingModels;
    using MMakerBotPanel.WebServices.Coinsbit.Model;
    using MMakerBotPanel.WebServices.Interface;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    public class CoinsbitApiController : ICEXAPIController
    {
        private readonly CoinsbitWebServices services = new CoinsbitWebServices();
        private Dictionary<string, CoinBlob> res;
        public PairList GetPairList()
        {
            SymbolModel ret = services.Parity();
            PairList pairList = new PairList();
            foreach (string item in ret.CoinsbitSymbolDataModels.result)
            {
                string[] dataItem = item.Split('_');

                if (dataItem[1] == "USDT")
                {
                    Pair pair = new Pair
                    {
                        symbol = dataItem[0] + dataItem[1],
                        baseAsset = dataItem[0],
                        quoteAsset = dataItem[1]
                    };
                    pairList.Pairs.Add(pair);
                }

            }
            //_ = GetTrade();
            //  GetOrderBook();
            //GetBalance();
            //GetActiveOrder();
            // CreateSpotOrder();
            return pairList;
        }
        public List<CandleStickStockChartDataModel> GetChartData(string symbol, Interval interval, CandlestickQuantity? candlestickQuantity)
        {
            string symbol1 = symbol.Substring(0, symbol.Length - 4);
            string symbolCoinsbit = symbol1 + "_USDT";
            string time = "";
            string start = "";
            string unixTimestamp = Convert.ToString((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
            if (interval == Interval.hours4)
            {
                time = "14400";
            }
            else if (interval == Interval.minutes30)
            {
                time = "1800";
            }
            else if (interval == Interval.hours1)
            {
                time = "3600";
            }
            else if (interval == Interval.hours2)
            {
                time = "7200";
            }

            if (candlestickQuantity == CandlestickQuantity.Candle1000)
            {
                long startTime = Convert.ToInt64(unixTimestamp) - 31556926;
                start = Convert.ToString(startTime);
            }
            else if (candlestickQuantity == CandlestickQuantity.Candle200)
            {
                long startTime = Convert.ToInt64(unixTimestamp) - 360000;
                start = Convert.ToString(startTime);
            }
            else if (candlestickQuantity == CandlestickQuantity.Candle168)
            {
                long startTime = Convert.ToInt64(unixTimestamp) - 302400;
                start = Convert.ToString(startTime);
            }
            else if (candlestickQuantity == CandlestickQuantity.Candle360)
            {
                long startTime = Convert.ToInt64(unixTimestamp) - 1296000;
                start = Convert.ToString(startTime);
            }
            else if (candlestickQuantity == CandlestickQuantity.Candle1080)
            {
                long startTime = Convert.ToInt64(unixTimestamp) - 7776000;
                start = Convert.ToString(startTime);
            }

            StockDataModel ret = services.StockData(symbolCoinsbit, time, unixTimestamp, start);
            List<CandleStickStockChartDataModel> dataList = new List<CandleStickStockChartDataModel>();

            foreach (CoinsbitChartData item in ret.CoinsbitChartDataModels)
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
            string symbol1 = symbol.Substring(0, symbol.Length - 4);
            string symbolCoinsbit = symbol1 + "_USDT";
            string unixTimestamp = Convert.ToString((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
            long startTime = Convert.ToInt64(unixTimestamp) - 14400;
            string start = Convert.ToString(startTime);
            StockDataModel ret = services.StockData(symbolCoinsbit, "14400", unixTimestamp, start);
            List<CandleStickStockChartDataModel> dataList = new List<CandleStickStockChartDataModel>();
            foreach (CoinsbitChartData item in ret.CoinsbitChartDataModels)
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
            CexBotStatusModel bybitStatus = new CexBotStatusModel();
            if (ret.coinsbitStatusModels[0].date != 0)
            {
                DateTime dateTime = UnixTimeStampToDateTime(Convert.ToDouble(ret.coinsbitStatusModels[0].date));
                bybitStatus.CexBotStatusModels.status = true;
                bybitStatus.CexBotStatusModels.date = dateTime;
            }
            else
            {
                bybitStatus.CexBotStatusModels.status = false;
            }
            return bybitStatus;
        }
        public OrderBookResponseModel GetOrderBook(string symbol)
        {
            string symbol1 = symbol.Substring(0, symbol.Length - 4);
            string symbolCoinsbit = symbol1 + "_USDT";
            string side = "sell";
            OrderBookModel response = services.OrderBook(symbolCoinsbit, side);
            OrderBookResponseModel orderBook = new OrderBookResponseModel();
            foreach (Order item in response.result.orders)
            {
                double amount = Convert.ToDouble(item.amount.Replace('.', ','));
                double price = Convert.ToDouble(item.price.Replace('.', ','));
                double volume = amount * price;
                OrderBookBuy buy = new OrderBookBuy()
                {
                    Volume = volume.ToString(),
                    Price = Convert.ToDecimal(item.price)
                };
                orderBook.Buy.Add(buy);
            }
            side = "buy";
            OrderBookModel respons = services.OrderBook(symbolCoinsbit, side);
            foreach (Order item in respons.result.orders)
            {
                double amount = Convert.ToDouble(item.amount.Replace('.', ','));
                double price = Convert.ToDouble(item.price.Replace('.', ','));
                double volume = amount * price;
                OrderBookSell sell = new OrderBookSell()
                {
                    Volume = volume.ToString(),
                    Price = Convert.ToDecimal(item.price)
                };
                orderBook.Sell.Add(sell);
            }
            return orderBook;

        }
        public List<TradesResponseModel> GetTrade(string symbol)
        {
            string symbol1 = symbol.Substring(0, symbol.Length - 4);
            string symbolCoinsbit = symbol1 + "_USDT";
            string since = "5";
            TradeModel response = services.Trade(symbolCoinsbit, since);
            List<TradesResponseModel> trades = new List<TradesResponseModel>();
            foreach (TradesData item in response.trades)
            {
                double price = Convert.ToDouble(item.price.Replace('.', ','));
                double amount = Convert.ToDouble(item.amount.Replace('.', ','));
                double volume = price * amount;
                TradesResponseModel trade = new TradesResponseModel()
                {
                    Symbol = symbolCoinsbit,
                    Price = item.price,
                    Volume = volume.ToString(),
                    Timestamp = item.tid,
                    Type = item.type,
                };
                trades.Add(trade);
            }
            return trades;

        }
        public CreateOrderResponseModel CreateSpotOrder(CreateOrderRequestModel requestModel)
        {
            string symbol1 = requestModel.symbol.Substring(0, requestModel.symbol.Length - 4);
            string symbolCoinsbit = symbol1 + "_USDT";
            Tuple<string, string, string> APITuple = CoinsbitHelper.GetCoinsbitHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new CreateOrderResponseModel();
            }
            string timestamp = Convert.ToString((long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds);
            string request = "/api/v1/order/new";
            string market = symbolCoinsbit;
            string side = Enum.GetName(typeof(SIDE), requestModel.side);
            string amount = requestModel.amount;
            string price = requestModel.price;
            string nonce = timestamp;
            object body = new
            {
                request,
                market,
                side,
                amount,
                price,
                nonce,
            };
            string bodyJson = JsonConvert.SerializeObject(body);
            string payload = Convert.ToBase64String(Encoding.ASCII.GetBytes(bodyJson));
            string signature = GetSign(payload, secretKey);
            CreateOrderModel response = services.CreateOrder(signature, apiKey, payload, body);
            if (response.result.Count > 0)
            {
                CreateOrderResponseModel createOrderResponseModel = new CreateOrderResponseModel()
                {
                    Symbol = market,
                    OrderId = Convert.ToString(response.result.ToList().First().orderId),
                    Timestamp = Convert.ToInt64(response.result.ToList().First().timestamp)
                };
                return createOrderResponseModel;
            }
            else
            {
                return new CreateOrderResponseModel();
            }

        }
        public List<ActiveOrderResponseModel> GetActiveOrder(string symbol)
        {
            string symbol1 = symbol.Substring(0, symbol.Length - 4);
            string symbolCoinsbit = symbol1 + "_USDT";
            Tuple<string, string, string> APITuple = CoinsbitHelper.GetCoinsbitHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new List<ActiveOrderResponseModel>();
            }
            string timestamp = Convert.ToString((long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds);
            string request = "/api/v1/orders";
            string market = symbolCoinsbit;
            string nonce = timestamp;
            object body = new
            {
                request,
                market,
                nonce,
            };
            string bodyJson = JsonConvert.SerializeObject(body);
            string payload = Convert.ToBase64String(Encoding.ASCII.GetBytes(bodyJson));
            string signature = GetSign(payload, secretKey);
            ActiveOrderModel response = services.ActiveOrder(signature, apiKey, payload, body);
            List<ActiveOrderResponseModel> activeOrders = new List<ActiveOrderResponseModel>();
            foreach (ActiveOrderResult item in response.result.result)
            {
                ActiveOrderResponseModel activeOrder = new ActiveOrderResponseModel()
                {
                    Symbol = item.market,
                    Timestamp = Convert.ToInt64(item.timestamp),
                    OrderId = Convert.ToString(item.id),
                    Price = item.price,
                    Type = item.type,
                    Side = item.side,
                };
                activeOrders.Add(activeOrder);
            }
            return activeOrders;
        }
        public List<BalanceResponseModel> GetBalance()
        {
            Tuple<string, string, string> APITuple = CoinsbitHelper.GetCoinsbitHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new List<BalanceResponseModel>();
            }
            string timestamp = Convert.ToString((long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds);
            string request = "/api/v1/account/balances";
            string nonce = timestamp;
            object body = new
            {
                request,
                nonce,
            };
            string bodyJson = JsonConvert.SerializeObject(body);
            string payload = Convert.ToBase64String(Encoding.ASCII.GetBytes(bodyJson));
            string signature = GetSign(payload, secretKey);
            object response = services.Balance(signature, apiKey, payload, body);
            res = (Dictionary<string, CoinBlob>)response;
            List<BalanceResponseModel> balances = res.Select(p => new BalanceResponseModel { Symbol = p.Key, Balance = p.Value.freeze }).ToList();
            return balances;
        }
        public PairList GetPair(string parity)
        {
            GetPairModel response = services.GetPair(parity);
            PairList pairList = new PairList();

            foreach (var item in response.result)
            {

                string dataItem = item.name.Substring(0, item.name.Length - 4);
                if (dataItem.ToUpper() + "USDT" == parity)
                {
                    Pair pair = new Pair()
                    {
                        qtyFormat = item.minAmount,
                    };
                    pairList.Pairs.Add(pair);
                }
            }
            return pairList;
        }
        public List<CancelOrderResponseModel> CancelOrders(string symbol)
        {
            string symbol1 = symbol.Substring(0, symbol.Length - 4);
            string symbolCoinsbit = symbol1 + "_USDT";
            Tuple<string, string, string> APITuple = CoinsbitHelper.GetCoinsbitHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new List<CancelOrderResponseModel>();
            }
            List<ActiveOrderResponseModel> activeOrders = GetActiveOrder(symbol);
            string timestamp = Convert.ToString((long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds);
            string request = "/api/v1/order/cancel";
            string market = symbolCoinsbit;
            string nonce = timestamp;
            List<CancelOrderResponseModel> cancelOrders = new List<CancelOrderResponseModel>();
            foreach (var item in activeOrders)
            {
                object body = new
                {
                    request,
                    market,
                    nonce,
                    item.OrderId
                };
                string bodyJson = JsonConvert.SerializeObject(body);
                string payload = Convert.ToBase64String(Encoding.ASCII.GetBytes(bodyJson));
                string signature = GetSign(payload, secretKey);
                CancelOrderModel response = services.CancelOrder(signature, apiKey, payload, body);
                CancelOrderResponseModel cancelOrder = new CancelOrderResponseModel();
                if (response.success)
                {
                    cancelOrder.orderId = response.result.orderId.ToString();
                    cancelOrder.symbol = symbol;
                    cancelOrder.status = "Cancelled";

                }
                cancelOrders.Add(cancelOrder);
            }
            return cancelOrders;

        }
        public QueryOrderResponseModel QueryOrders(string symbol, string orderId)
        {
            throw new NotImplementedException();
        }
        public ComissionFeeResponseModel ComissionFee(string symbol)
        {
            ComissionFeeResponseModel comission = new ComissionFeeResponseModel()
            {
                symbol = symbol,
                takerCommission = "0.2",
                makerCommission = "0.2"
            };
            return comission;   
        }
        public string GetSign(string payload, string secretKey)
        {
            HMACSHA512 hmacsha512 = new HMACSHA512(Encoding.ASCII.GetBytes(secretKey));
            byte[] hash = hmacsha512.ComputeHash(Encoding.ASCII.GetBytes(payload));
            StringBuilder hex = new StringBuilder(hash.Length * 2);
            foreach (byte b in hash)
            {
                _ = hex.AppendFormat("{0:x2}", b);
            }
            return hex.ToString();
        }
        public DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }
    }
}