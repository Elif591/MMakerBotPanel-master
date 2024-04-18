namespace MMakerBotPanel.WebServices.Kraken
{
    using MMakerBotPanel.Business.Exchange;
    using MMakerBotPanel.Models.ApiModels;
    using MMakerBotPanel.Models.CexBotModels;
    using MMakerBotPanel.Models.ChartsModel;
    using MMakerBotPanel.Models.ChartsModels;
    using MMakerBotPanel.Models.MakerMarketingModels;
    using MMakerBotPanel.WebServices.Interface;
    using MMakerBotPanel.WebServices.Kraken.Model;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    public class KrakenApiController : ICEXAPIController
    {
        private readonly KrakenWebServices services = new KrakenWebServices();

        public List<CandleStickStockChartDataModel> GetChartData(string symbol, Interval interval, CandlestickQuantity? candlestickQuantity)
        {
            KrakenCandleData ret = services.getStockData(symbol, "240");
            List<CandleStickStockChartDataModel> dataList = new List<CandleStickStockChartDataModel>();

            foreach (List<object> item in ret.result.CandleData)
            {
                CandleStickStockChartDataModel data = new CandleStickStockChartDataModel();
                CultureInfo CI = new CultureInfo("en-US", false);
                data.Date = Convert.ToInt64(item.ToArray()[0]) * 1000;
                data.Open = Convert.ToDouble(item.ToArray()[1], CI);
                data.High = Convert.ToDouble(item.ToArray()[2], CI);
                data.Low = Convert.ToDouble(item.ToArray()[3], CI);
                data.Close = Convert.ToDouble(item.ToArray()[4], CI);
                data.Volume = (long)Convert.ToDouble(item.ToArray()[6], CI);
                dataList.Add(data);
            }

            return dataList;
        }
        public List<CandleStickStockChartDataModel> GetChartDataDate(string symbol, Interval interval, CandlestickQuantity? candlestickQuantity)
        {
            KrakenCandleData ret = services.getStockDataDate(symbol, "240");
            List<CandleStickStockChartDataModel> dataList = new List<CandleStickStockChartDataModel>();

            if (ret.result != null)
            {
                foreach (List<object> item in ret.result.CandleData)
                {
                    CandleStickStockChartDataModel data = new CandleStickStockChartDataModel();
                    CultureInfo CI = new CultureInfo("en-US", false);
                    data.Date = Convert.ToInt64(item.ToArray()[0]) * 1000;
                    data.Open = Convert.ToDouble(item.ToArray()[1], CI);
                    data.High = Convert.ToDouble(item.ToArray()[2], CI);
                    data.Low = Convert.ToDouble(item.ToArray()[3], CI);
                    data.Close = Convert.ToDouble(item.ToArray()[4], CI);
                    data.Volume = (long)Convert.ToDouble(item.ToArray()[6], CI);
                    dataList.Add(data);
                }
            }
            return dataList;
        }
        public PairList GetPairList()
        {
            KrakenSymbolModel ret = services.getParity();
            PairList pairList = new PairList();

            foreach (KrakenSymbol item in ret.krakenSymbols)
            {
                if (item.status == "online" && item.quote == "USDT")
                {
                    Pair pair = new Pair
                    {
                        symbol = item.altname,
                        baseAsset = item.@base,
                        quoteAsset = item.quote
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
            DateTime dateTime = UnixTimeStampToDateTime(Convert.ToDouble(ret.result.unixtime));
            CexBotStatusModel gateioStatus = new CexBotStatusModel();
            if (ret.result.unixtime != 0)
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
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }
        public CreateOrderResponseModel CreateSpotOrder(CreateOrderRequestModel requestModel)
        {
            Tuple<string, string, string> APITuple = KrakenHelper.GetKrakenHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new CreateOrderResponseModel();
            }
            string nonce = Convert.ToInt64(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds).ToString();
            var data = new Dictionary<string, string>
            {
                { "nonce", nonce},
                { "ordertype", requestModel.side.ToString() },
                { "pair", requestModel.symbol },
                { "price", requestModel.price },
                { "type", requestModel.typeTrade.ToString() },
                { "volume", requestModel.amount }
            };

            string signature = GetKrakenSignature("/0/private/AddOrder", data, secretKey);
            KrakenExchange order = services.CreateOrder(signature, apiKey, nonce, data);
            CreateOrderResponseModel createOrderResponseModel = new CreateOrderResponseModel()
            {
                Symbol = requestModel.symbol,
                OrderId = order.result.txid[0],
                Timestamp = Convert.ToInt64(nonce)
            };
            return createOrderResponseModel;

        }
        public List<BalanceResponseModel> GetBalance()
        {
            Tuple<string, string, string> APITuple = KrakenHelper.GetKrakenHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new List<BalanceResponseModel>();
            }
            string nonce = Convert.ToInt64(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds).ToString();
            var data = new Dictionary<string, string>
            {
                { "nonce", nonce},
            };
            string signature = GetKrakenSignature("/0/private/TradeBalance", data, secretKey);
            BalanceModel response = services.GetBalance(signature, apiKey, nonce, data);
            List<BalanceResponseModel> balances = new List<BalanceResponseModel>();
            JObject jsonObject = JObject.Parse(response.result);
            var coinBalances = jsonObject["result"].ToObject<Dictionary<string, string>>();
            foreach (var coinBalance in coinBalances)
            {
                BalanceResponseModel balance = new BalanceResponseModel()
                {
                    Symbol = coinBalance.Key,
                    Balance = coinBalance.Value,
                };
                balances.Add(balance);
            }
            return balances;
        }
        public List<ActiveOrderResponseModel> GetActiveOrder(string symbol)
        {
            Tuple<string, string, string> APITuple = KrakenHelper.GetKrakenHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new List<ActiveOrderResponseModel>();
            }
            string nonce = Convert.ToInt64(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds).ToString();
            var data = new Dictionary<string, string>
            {
                { "nonce", nonce},
            };
            string signature = GetKrakenSignature("/0/private/OpenOrders", data, secretKey);
            ActiveOrdersModel response = services.GetActiveOrders(signature, apiKey, nonce, data);
            List<ActiveOrderResponseModel> openOrders = new List<ActiveOrderResponseModel>();
            JObject jsonObject = JObject.Parse(response.result);
            var coinBalances = jsonObject["result"].ToObject<Dictionary<string, string>>();
            foreach (var coinBalance in coinBalances)
            {
                ActiveOrderResponseModel balance = new ActiveOrderResponseModel()
                {
                    OrderId = coinBalance.Key,

                };
                openOrders.Add(balance);
            }
            return openOrders;
        }

        public OrderBookResponseModel GetOrderBook(string symbol)
        {
            OrderBookModel orderBook = services.GetOrderBook(symbol);
            OrderBookResponseModel orderBookResponseModel = new OrderBookResponseModel();

            foreach (List<object> itemList in orderBook.result.Bids.Values.SelectMany(x => x.BidsList))
            {
                string volume = itemList[1].ToString();  // assuming volume is at index 1
                decimal price = Convert.ToDecimal(itemList[0]);  // assuming price is at index 0

                OrderBookBuy buyItem = new OrderBookBuy
                {
                    Volume = volume,
                    Price = price
                };

                orderBookResponseModel.Buy.Add(buyItem);
            }
            foreach (List<object> itemList in orderBook.result.Asks.Values.SelectMany(x => x.AsksList))
            {
                string volume = itemList[1].ToString();  // assuming volume is at index 1
                decimal price = Convert.ToDecimal(itemList[0]);  // assuming price is at index 0

                OrderBookSell sellItem = new OrderBookSell
                {
                    Volume = volume,
                    Price = price
                };

                orderBookResponseModel.Sell.Add(sellItem);
            }
            return orderBookResponseModel;
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
            Tuple<string, string, string> APITuple = KrakenHelper.GetKrakenHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new List<CancelOrderResponseModel>();
            }
            string nonce = Convert.ToInt64(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds).ToString();
            var data = new Dictionary<string, string>
            {
                { "nonce", nonce},
            };
            string signature = GetKrakenSignature("/0/private/CancelAll", data, secretKey);
            CancelOrderModel response = services.CancelOrders(apiKey,signature , data);
            List<CancelOrderResponseModel> cancelOrders = new List<CancelOrderResponseModel>();
            for (int i = 0; i < response.result.count; i++)
            {
                CancelOrderResponseModel cancelOrderResponseModel = new CancelOrderResponseModel();
                cancelOrderResponseModel.symbol = symbol;
                cancelOrders.Add(cancelOrderResponseModel);      
            }
            return cancelOrders;

        }

        public QueryOrderResponseModel QueryOrders(string symbol, string orderId)
        {
            throw new NotImplementedException();
        }

        public ComissionFeeResponseModel ComissionFee(string symbol)
        {
            throw new NotImplementedException();
        }
        public static string GetKrakenSignature(string urlpath, Dictionary<string, string> data, string secret)
        {
            var postdata = new System.Web.UI.Page().Server.UrlEncode(string.Join("&", data));
            var encoded = Encoding.UTF8.GetBytes(data["nonce"] + postdata);
            var urlPathBytes = Encoding.UTF8.GetBytes(urlpath);
            var sha256 = new SHA256Managed().ComputeHash(encoded);
            var message = new byte[urlPathBytes.Length + sha256.Length];
            Buffer.BlockCopy(urlPathBytes, 0, message, 0, urlPathBytes.Length);
            Buffer.BlockCopy(sha256, 0, message, urlPathBytes.Length, sha256.Length);

            var secretBytes = Convert.FromBase64String(secret);
            using (var hmac = new HMACSHA512(secretBytes))
            {
                var mac = hmac.ComputeHash(message);
                var sigdigest = Convert.ToBase64String(mac);
                return sigdigest;
            }
        }
    }
}