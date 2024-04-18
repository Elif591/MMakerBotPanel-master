namespace MMakerBotPanel.WebServices.OKX
{
    using MMakerBotPanel.Business;
    using MMakerBotPanel.Business.Exchange;
    using MMakerBotPanel.Models;
    using MMakerBotPanel.Models.ApiModels;
    using MMakerBotPanel.Models.CexBotModels;
    using MMakerBotPanel.Models.ChartsModel;
    using MMakerBotPanel.Models.ChartsModels;
    using MMakerBotPanel.Models.MakerMarketingModels;
    using MMakerBotPanel.WebServices.Interface;
    using MMakerBotPanel.WebServices.OKX.Model;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    public class OKXApiController : ICEXAPIController
    {
        private readonly OKXWebServices services = new OKXWebServices();

        public PairList GetPairList()
        {
            SymbolModel ret = services.Parity();
            PairList pairList = new PairList();
            foreach (Datum item in ret.data)
            {
                string[] splitList = item.instId.Split('-');
                string baseSymbol = splitList[0];
                string quoteSymbol = splitList[1];

                if (quoteSymbol == "USDT")
                {
                    Pair pair = new Pair
                    {
                        symbol = splitList[0] + splitList[1],
                        baseAsset = baseSymbol,
                        quoteAsset = quoteSymbol
                    };
                    pairList.Pairs.Add(pair);
                }
            }
            // GetOrderBook();
            // GetTrade();
            // GetBalance();
            // GetActiveOrder();
            // CreateSpotOrder();
            return pairList;
        }
        public List<CandleStickStockChartDataModel> GetChartData(string symbol, Interval interval, CandlestickQuantity? candlestickQuantity)
        {
            string symbol1 = symbol.Substring(0, symbol.Length - 4);
            string symbolOKX = symbol1 + "-USDT";
            int timeInterval = (int)interval;
            string time = "";
            int candlestickQty = (int)candlestickQuantity;
            string candleQTY = "";
            if (interval == Interval.minutes30)
            {
                time = timeInterval.ToString() + "m";
            }
            else
            {
                time = timeInterval.ToString() + "H";
            }

            if (candlestickQuantity == CandlestickQuantity.Candle1080)
            {
                candleQTY = "1000";
            }
            else
            {
                candleQTY = candlestickQty.ToString();
            }

            StockDataModel ret = services.StockData(symbolOKX, time, candleQTY);
            List<CandleStickStockChartDataModel> dataList = new List<CandleStickStockChartDataModel>();

            foreach (StockDataDetail item in ret.OKXChartDataDetailModels)
            {
                CandleStickStockChartDataModel data = new CandleStickStockChartDataModel
                {
                    Date = item.Time,
                    Open = item.openPrice,
                    High = item.highPrice,
                    Low = item.lowPrice,
                    Close = item.closePrice,
                    Volume = item.Volume
                };
                dataList.Add(data);
            }
            dataList.Reverse();

            return dataList;
        }
        public List<CandleStickStockChartDataModel> GetChartDataDate(string symbol, Interval interval, CandlestickQuantity? candlestickQuantity)
        {
            string symbol1 = symbol.Substring(0, symbol.Length - 4);
            string symbolOKX = symbol1 + "-USDT";
            StockDataModel ret = services.StockData(symbolOKX, "4H", "1");
            List<CandleStickStockChartDataModel> dataList = new List<CandleStickStockChartDataModel>();

            foreach (StockDataDetail item in ret.OKXChartDataDetailModels)
            {
                CandleStickStockChartDataModel data = new CandleStickStockChartDataModel
                {
                    Date = item.Time,
                    Open = item.openPrice,
                    High = item.highPrice,
                    Low = item.lowPrice,
                    Close = item.closePrice,
                    Volume = item.Volume
                };
                dataList.Add(data);
            }
            dataList.Reverse();
            return dataList;
        }
        public CexBotStatusModel Status()
        {
            StatusModel ret = services.Status();
            CexBotStatusModel gateioStatus = new CexBotStatusModel();
            if (ret.data[0].ts != "0")
            {
                string serverTime = ret.data[0].ts.ToString().Substring(0, ret.data[0].ts.ToString().Length - 3);
                DateTime dateTime = UnixTimeStampToDateTime(Convert.ToDouble(serverTime));
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
            string symbol1 = symbol.Substring(0, symbol.Length - 4);
            string symbolOKX = symbol1 + "-USDT";
            TradeModel response = services.Trade(symbolOKX);
            List<TradesResponseModel> trades = new List<TradesResponseModel>();
            foreach (TradeDatum item in response.data)
            {
                double price = Convert.ToDouble(item.px, CultureInfo.InvariantCulture);
                double size = Convert.ToDouble(item.sz, CultureInfo.InvariantCulture);
                double volume = price * size;
                TradesResponseModel trade = new TradesResponseModel()
                {
                    Symbol = symbolOKX,
                    Price = item.px,
                    Volume = volume.ToString(),
                    Timestamp = Convert.ToInt64(item.ts),
                    Type = item.side
                };
                trades.Add(trade);
            }
            return trades;
        }
        public OrderBookResponseModel GetOrderBook(string symbol)
        {
            string symbol1 = symbol.Substring(0, symbol.Length - 4);
            string symbolOKX = symbol1 + "-USDT";
            OrderBookModel response = services.OrderBook(symbolOKX);
            OrderBookResponseModel orderBook = new OrderBookResponseModel();
            foreach (OrderBookDatum item in response.data)
            {
                foreach (List<string> bidItem in item.bids)
                {
                    string[] dataItem = bidItem.ToArray();
                    double size = Convert.ToDouble(dataItem[1], CultureInfo.InvariantCulture);
                    double price = Convert.ToDouble(dataItem[0], CultureInfo.InvariantCulture);
                    double volume = size * price;
                    OrderBookBuy buy = new OrderBookBuy()
                    {
                        Price = Convert.ToDecimal(dataItem[0]),
                        Volume = volume.ToString(),
                    };
                    orderBook.Buy.Add(buy);
                }
                foreach (List<string> sellItem in item.asks)
                {
                    string[] dataItem = sellItem.ToArray();
                    double size = Convert.ToDouble(dataItem[1], CultureInfo.InvariantCulture);
                    double price = Convert.ToDouble(dataItem[0], CultureInfo.InvariantCulture);
                    double volume = size * price;
                    OrderBookSell sell = new OrderBookSell()
                    {
                        Price = Convert.ToDecimal(dataItem[0]),
                        Volume = volume.ToString()
                    };
                    orderBook.Sell.Add(sell);
                }
            }
            return orderBook;
        }
        public CreateOrderResponseModel CreateSpotOrder(CreateOrderRequestModel requestModel)
        {
            string symbol1 = requestModel.symbol.Substring(0, requestModel.symbol.Length - 4);
            string symbolOKX = symbol1 + "-USDT";
            Tuple<string, string, string> APITuple = OKXHelper.GetOKXHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            string passphrase = APITuple.Item3;
            if (apiKey == "" || secretKey == "")
            {
                return new CreateOrderResponseModel();
            }
            string isoTimestamp = DateTime.UtcNow.ToString("yyyy-MM-dd'T'HH:mm:ss.fffK", CultureInfo.InvariantCulture);
            long timestamp = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds);
            string method = "POST";
            string endPoint = "/api/v5/trade/order";
            string passPhrase = passphrase;
            string pair = symbolOKX;
            var body = new
            {
                instId = pair,
                tdMode = "cash",
                side = Enum.GetName(typeof(SIDE), requestModel.side),
                ordType = Enum.GetName(typeof(TYPE_TRADE), requestModel.typeTrade),
                px = requestModel.price,
                sz = requestModel.amount
            };
            string prreHashString = $"{isoTimestamp}{method}{endPoint}{JsonConvert.SerializeObject(body)}";
            string signature = GetSign(prreHashString, secretKey);
            CreateOrderModel response = services.CreateOrder(apiKey, isoTimestamp, signature, body, passPhrase);
            CreateOrderResponseModel createOrderResponseModel = new CreateOrderResponseModel()
            {
                Symbol = pair,
                OrderId = Convert.ToString(response.data[0].ordId),
                Timestamp = Convert.ToInt64(timestamp)
            };
            return createOrderResponseModel;

        }
        public List<BalanceResponseModel> GetBalance()
        {
            Tuple<string, string, string> APITuple = OKXHelper.GetOKXHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            string passphrase = APITuple.Item3;
            if (apiKey == "" || secretKey == "")
            {
                return new List<BalanceResponseModel>();
            }
            string isoTimestamp = DateTime.UtcNow.ToString("yyyy-MM-dd'T'HH:mm:ss.fffK", CultureInfo.InvariantCulture);
            string method = "GET";
            string endPoint = "/api/v5/account/balance";
            string passPhrase = passphrase;
            string prreHashString = $"{isoTimestamp}{method}{endPoint}";
            string signature = GetSign(prreHashString, secretKey);
            BalanceModel response = services.Balance(apiKey, isoTimestamp, signature, passPhrase);
            List<BalanceResponseModel> balances = new List<BalanceResponseModel>();
            foreach (GetWalletDatum item in response.data)
            {
                foreach (Detail dataItem in item.details)
                {
                    BalanceResponseModel balance = new BalanceResponseModel()
                    {
                        Symbol = dataItem.ccy,
                        Balance = dataItem.cashBal
                    };
                    balances.Add(balance);
                }
            }
            return balances;
        }
        public List<ActiveOrderResponseModel> GetActiveOrder(string symbol)
        {
            Tuple<string, string, string> APITuple = OKXHelper.GetOKXHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            string passphrase = APITuple.Item3;
            if (apiKey == "" || secretKey == "")
            {
                return new List<ActiveOrderResponseModel>();
            }
            string isoTimestamp = DateTime.UtcNow.ToString("yyyy-MM-dd'T'HH:mm:ss.fffK", CultureInfo.InvariantCulture);
            string method = "GET";
            string endPoint = "/api/v5/trade/orders-pending?instType=SPOT";
            string passPhrase = passphrase;
            string prreHashString = $"{isoTimestamp}{method}{endPoint}";
            string signature = GetSign(prreHashString, secretKey);
            ActiveOrderModel ressponse = services.ActiveOrder(apiKey, isoTimestamp, signature, passPhrase);
            List<ActiveOrderResponseModel> activeOrders = new List<ActiveOrderResponseModel>();
            foreach (ActiveOrdersDatum item in ressponse.data)
            {
                ActiveOrderResponseModel activeOrder = new ActiveOrderResponseModel()
                {
                    Symbol = item.instId,
                    Timestamp = Convert.ToInt64(item.cTime),
                    OrderId = item.ordId,
                    Price = item.px,
                    Type = item.ordType,
                    Side = item.side,
                    Volume = item.ccy
                };
                activeOrders.Add(activeOrder);
            }
            return activeOrders;
        }
        public string GetSign(string prreHashString, string secretKey)
        {
            return CryptographyHelper.GetHMAC256(prreHashString, secretKey, Models.RETURNTYPE.BASE64);

        }
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }
        public PairList GetPair(string parity)
        {
            SymbolModel ret = services.Parity();
            PairList pairList = new PairList();
            foreach (Datum item in ret.data)
            {
                string[] splitList = item.instId.Split('-');
                string baseSymbol = splitList[0];
                string quoteSymbol = splitList[1];

                if (quoteSymbol == "USDT")
                {
                    Pair pair = new Pair
                    {
                        symbol = splitList[0] + splitList[1],
                        baseAsset = baseSymbol,
                        quoteAsset = quoteSymbol,
                        qtyFormat = "0.01"

                    };
                    pairList.Pairs.Add(pair);
                }
            }
            return pairList;
        }
        public List<CancelOrderResponseModel> CancelOrders(string symbol)
        {
            string symbol1 = symbol.Substring(0, symbol.Length - 4);
            string symbolOKX = symbol1 + "-USDT";
            Tuple<string, string, string> APITuple = OKXHelper.GetOKXHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            string passphrase = APITuple.Item3;
            if (apiKey == "" || secretKey == "")
            {
                return new List<CancelOrderResponseModel>();
            }
            string isoTimestamp = DateTime.UtcNow.ToString("yyyy-MM-dd'T'HH:mm:ss.fffK", CultureInfo.InvariantCulture);
            long timestamp = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds);
            string method = "POST";
            string endPoint = "api/v5/sprd/mass-cancel";
            string passPhrase = passphrase;
            string pair = symbolOKX;
            var body = new
            {
                sprdId = $"{symbolOKX}_{symbolOKX}-SPOT"
            };
            string prreHashString = $"{isoTimestamp}{method}{endPoint}{JsonConvert.SerializeObject(body)}";
            string signature = GetSign(prreHashString, secretKey);
            CancelOrderModel response = services.CancelOrders(apiKey, isoTimestamp, signature, passPhrase, body);
            List<CancelOrderResponseModel> cancelOrderList = new List<CancelOrderResponseModel>();
            foreach (var item in response.data)
            {
                CancelOrderResponseModel cancelOrderResponseModel = new CancelOrderResponseModel()
                {
                    symbol = symbol,
                    orderId = "",
                    status = item.result.ToString()
                };
                cancelOrderList.Add(cancelOrderResponseModel);
            }

            return cancelOrderList;
        }
        public QueryOrderResponseModel QueryOrders(string symbol, string orderId)
        {
            Tuple<string, string, string> APITuple = OKXHelper.GetOKXHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            string passphrase = APITuple.Item3;
            if (apiKey == "" || secretKey == "")
            {
                return new QueryOrderResponseModel();
            }
            string isoTimestamp = DateTime.UtcNow.ToString("yyyy-MM-dd'T'HH:mm:ss.fffK", CultureInfo.InvariantCulture);
            string method = "GET";
            string endPoint = $"api/v5/trade/order?ordId={orderId}&instId={symbol}";
            string passPhrase = passphrase;
            string prreHashString = $"{isoTimestamp}{method}{endPoint}";
            string signature = GetSign(prreHashString, secretKey);
            QueryOrderModel ressponse = services.QueryOrder(apiKey, isoTimestamp, signature, passPhrase, orderId, symbol);
            QueryOrderResponseModel queryOrders = new QueryOrderResponseModel();
            foreach (QueryOrders item in ressponse.data)
            {
                QueryOrderResponseModel queryOrder = new QueryOrderResponseModel()
                {
                    orderId = item.ordId,
                    price = item.px,
                    side = item.side,
                    qty = item.sz,
                    status = item.state.ToUpper(),
                    symbol = symbol,
                    type = item.ordType,
                };
                queryOrders = queryOrder;
            }
            return queryOrders;
        }
        public ComissionFeeResponseModel ComissionFee(string symbol)
        {
            Tuple<string, string, string> APITuple = OKXHelper.GetOKXHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            string passphrase = APITuple.Item3;
            if (apiKey == "" || secretKey == "")
            {
                return new ComissionFeeResponseModel();
            }
            string isoTimestamp = DateTime.UtcNow.ToString("yyyy-MM-dd'T'HH:mm:ss.fffK", CultureInfo.InvariantCulture);
            string method = "GET";
            string endPoint = $"api/v5/account/trade-fee?instType=SPOT&instId={symbol}";
            string passPhrase = passphrase;
            string prreHashString = $"{isoTimestamp}{method}{endPoint}";
            string signature = GetSign(prreHashString, secretKey);
            ComissionFeeModel response = services.ComissionFee(apiKey, isoTimestamp, signature, passPhrase, symbol);

            ComissionFeeResponseModel comission = new ComissionFeeResponseModel()
            {
                symbol = symbol,
                makerCommission = response.data[0].maker,
                takerCommission = response.data[0].taker
            };
            return comission;
        }
    }
}
