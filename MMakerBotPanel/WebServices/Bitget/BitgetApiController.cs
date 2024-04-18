namespace MMakerBotPanel.WebServices.Bitget
{
    using MMakerBotPanel.Business;
    using MMakerBotPanel.Business.Exchange;
    using MMakerBotPanel.Models;
    using MMakerBotPanel.Models.ApiModels;
    using MMakerBotPanel.Models.CexBotModels;
    using MMakerBotPanel.Models.ChartsModel;
    using MMakerBotPanel.Models.ChartsModels;
    using MMakerBotPanel.Models.MakerMarketingModels;
    using MMakerBotPanel.WebServices.Bitget.Model;
    using MMakerBotPanel.WebServices.Interface;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;

    public class BitgetApiController : ICEXAPIController
    {
        private readonly BitgetWebServices services = new BitgetWebServices();

        public PairList GetPairList()
        {
            PairListModel ret = services.Parity();
            PairList pairList = new PairList();
            foreach (Symbols item in ret.data)
            {
                if (item.quoteCoin == "USDT")
                {
                    Pair pair = new Pair
                    {
                        symbol = item.symbolName,
                        baseAsset = item.baseCoin,
                        quoteAsset = item.quoteCoin
                    };
                    ;
                    pairList.Pairs.Add(pair);

                }
            }
           // GetBalance();
            return pairList;
        }

        public List<CandleStickStockChartDataModel> GetChartData(string symbol, Interval interval, CandlestickQuantity? candlestickQuantity)
        {
            int timeInterval = (int)interval;
            string timeIntervalStr;
            if ((int)interval == (int)Interval.minutes30)
            {
                timeIntervalStr = timeInterval.ToString() + "min";
            }
            else
            {
                timeIntervalStr = timeInterval.ToString() + "h";
            }
            int candleQty = (int)candlestickQuantity;
            if (candlestickQuantity == CandlestickQuantity.Candle1080)
            {
                candleQty = 1000;
            }
            string parity = symbol + "_SPBL";
            StockDataModel ret = services.StockData(parity, timeIntervalStr, candleQty.ToString());
            List<CandleStickStockChartDataModel> dataList = new List<CandleStickStockChartDataModel>();
            CultureInfo CI = new CultureInfo("en-US");
            foreach (StockData item in ret.data)
            {
                CandleStickStockChartDataModel data = new CandleStickStockChartDataModel();

                data.Date = float.Parse(item.ts);
                data.Open = Convert.ToDouble(item.open.Replace(".", ","));
                data.High = Convert.ToDouble(item.high.Replace(".", ","));
                data.Low = Convert.ToDouble(item.low.Replace(".", ","));
                data.Close = Convert.ToDouble(item.close.Replace(".", ","));
                data.Volume = long.Parse(item.usdtVol.Split('.')[0]);

                dataList.Add(data);
            }

            return dataList;
        }

        public List<CandleStickStockChartDataModel> GetChartDataDate(string symbol, Interval interval, CandlestickQuantity? candlestickQuantity)
        {
            int timeInterval = (int)interval;
            string timeIntervalStr;
            if ((int)interval == (int)Interval.minutes30)
            {
                timeIntervalStr = timeInterval.ToString() + "min";
            }
            else
            {
                timeIntervalStr = timeInterval.ToString() + "h";
            }
            int candleQty = (int)candlestickQuantity;
            if (candlestickQuantity == CandlestickQuantity.Candle1080)
            {
                candleQty = 1000;
            }
            string parity = symbol + "_SPBL";
            StockDataModel ret = services.StockData(parity, timeIntervalStr, candleQty.ToString());
            List<CandleStickStockChartDataModel> dataList = new List<CandleStickStockChartDataModel>();
            CultureInfo CI = new CultureInfo("en-US");
            foreach (StockData item in ret.data)
            {
                CandleStickStockChartDataModel data = new CandleStickStockChartDataModel();

                data.Date = float.Parse(item.ts);
                data.Open = Convert.ToDouble(item.open.Replace(".", ","));
                data.High = Convert.ToDouble(item.high.Replace(".", ","));
                data.Low = Convert.ToDouble(item.low.Replace(".", ","));
                data.Close = Convert.ToDouble(item.close.Replace(".", ","));
                data.Volume = long.Parse(item.usdtVol.Split('.')[0]);

                dataList.Add(data);
            }

            return dataList;
        }
        public CreateOrderResponseModel CreateSpotOrder(CreateOrderRequestModel request)
        {
            Tuple<string, string, string> APITuple = BitgetHelper.GetBitgetHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new CreateOrderResponseModel();
            }
            string symbol = request.symbol + "_SPBL";
            string side = request.side.ToString();
            string orderType = request.typeTrade.ToString();
            string price = request.price.Replace(",", ".");
            string quantity = request.amount.Replace(",", ".");
            var body = new
            {
                symbol = symbol.ToUpper(),
                side = side.ToLower(),
                orderType = orderType.ToLower().Replace("ı", "i"),
                force = "normal",
                price = price,
                quantity = quantity
            };
            string method = "POST";
            string endpoint = "/api/spot/v1/trade/orders";
            string timestamp = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds).ToString();
            string dataQueryString = $"{timestamp}{method}{endpoint}{JsonConvert.SerializeObject(body)}";
            string signature = GetSign(dataQueryString, secretKey);
            CreateOrderModel response = services.CreateOrder(apiKey, body, signature, APITuple.Item3, timestamp);
            CreateOrderResponseModel createOrderResponseModel = new CreateOrderResponseModel()
            {
                Symbol = request.symbol,
                OrderId = Convert.ToString(response.data.orderId),
                Timestamp = Convert.ToInt64(timestamp)
            };
            return createOrderResponseModel;
        }

        public List<CancelOrderResponseModel> CancelOrders(string symbol)
        {
            Tuple<string, string, string> APITuple = BitgetHelper.GetBitgetHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new List<CancelOrderResponseModel>();
            }
            string parity = symbol + "_SPBL";
            var body = new
            {
                symbol = parity.ToUpper(),
            };
            string method = "POST";
            string endpoint = "/api/spot/v1/trade/cancel-symbol-order";
            string timestamp = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds).ToString();
            string dataQueryString = $"{timestamp}{method}{endpoint}{JsonConvert.SerializeObject(body)}";
            string signature = GetSign(dataQueryString, secretKey);
            CancelOrderModel response = services.CancelOrder(apiKey, body, signature, APITuple.Item3, timestamp);
            List<CancelOrderResponseModel> cancelOrderResponseModel = new List<CancelOrderResponseModel>();
            foreach (CancelOrder item in response.data)
            {

                CancelOrderResponseModel createOrderResponse = new CancelOrderResponseModel()
                {
                    symbol = symbol,
                    orderId = Convert.ToString(item.orderId),
                    status = "Cancel"
                };
                cancelOrderResponseModel.Add(createOrderResponse);
            }
            return cancelOrderResponseModel;
        }

        public ComissionFeeResponseModel ComissionFee(string symbol)
        {
            ComissionFeeResponseModel comissionFee = new ComissionFeeResponseModel()
            {
                makerCommission = "0.001",
                takerCommission = "0.001"
            };
            return comissionFee;    
        }

        public List<ActiveOrderResponseModel> GetActiveOrder(string symbol)
        {
            Tuple<string, string, string> APITuple = BitgetHelper.GetBitgetHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new List<ActiveOrderResponseModel>();
            }
            var body = new
            {
                symbol = symbol.ToUpper(),
            };
            string method = "POST";
            string endpoint = "/api/spot/v1/trade/open-orders";
            string timestamp = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds).ToString();
            string dataQueryString = $"{timestamp}{method}{endpoint}";
            string signature = GetSign(dataQueryString, secretKey);
            ActiveOrdersModel response = services.GetActiveOrders(apiKey, body, signature, APITuple.Item3, timestamp);
            List<ActiveOrderResponseModel> activeOrders = new List<ActiveOrderResponseModel>();
            foreach (ActiveOrders item in response.data)
            {
                ActiveOrderResponseModel activeOrder = new ActiveOrderResponseModel()
                {
                    Timestamp = Convert.ToInt64(timestamp),
                    Symbol = symbol,
                    OrderId = item.orderId,
                    Status = item.status,
                    Type = item.orderType,
                };
                activeOrders.Add(activeOrder);
            }
            return activeOrders;

        }

        public List<BalanceResponseModel> GetBalance()
        {
            Tuple<string, string, string> APITuple = BitgetHelper.GetBitgetHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new List<BalanceResponseModel>();
            }
            string body = "";
            string method = "POST";
            string endpoint = "/api/spot/v1/account/bills";
            string timestamp = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds).ToString();
            string dataQueryString = $"{timestamp}{method}{endpoint}";
            string signature = GetSign(dataQueryString, secretKey);
            BalanceModel response = services.GetBalance(apiKey, body, signature, APITuple.Item3, timestamp);
            List<BalanceResponseModel> balances = new List<BalanceResponseModel>();
            foreach (Balance item in response.data)
            {
                BalanceResponseModel balance = new BalanceResponseModel()
                {
                    Symbol = item.coinName,
                    Balance = item.balance
                };
                balances.Add(balance);
            }
            return balances;
        }

        public OrderBookResponseModel GetOrderBook(string symbol)
        {
            string query = $"?symbol={symbol}&type=step0&limit=100";
            OrderBookModel response = services.GetOrderBook(query);
            OrderBookResponseModel orderBooks = new OrderBookResponseModel();
            foreach (List<string> item in response.data.bids)
            {
                string[] dataItem = item.ToArray();
                OrderBookBuy buy = new OrderBookBuy()
                {
                    Volume = dataItem[0],
                    Price = Convert.ToDecimal(dataItem[1].Replace(".", ",")),
                };
                orderBooks.Buy.Add(buy);
            }
            foreach (List<string> item in response.data.asks)
            {
                string[] dataItem = item.ToArray();
                OrderBookSell sell = new OrderBookSell()
                {
                    Volume = dataItem[0],
                    Price = Convert.ToDecimal(dataItem[1]),
                };
                orderBooks.Sell.Add(sell);
            }
            return orderBooks;
        }

        public PairList GetPair(string parity)
        {
            string query = $"?symbol={parity}";
            PairListModel response = services.GetPair(query);
            PairList pairList = new PairList();
            foreach (Symbols item in response.data)
            {
                if (item.quoteCoin == "USDT")
                {
                    Pair pair = new Pair
                    {
                        symbol = item.symbolName,
                        qtyFormat = item.minTradeAmount

                    };
                    ;
                    pairList.Pairs.Add(pair);

                }
            }
            return pairList;
        }

        public List<TradesResponseModel> GetTrade(string symbol)
        {
            throw new NotImplementedException();
        }

        public QueryOrderResponseModel QueryOrders(string symbol, string orderId)
        {
            Tuple<string, string, string> APITuple = BitgetHelper.GetBitgetHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new QueryOrderResponseModel();
            }
            string parity = symbol + "_SPBL";
            var body = new
            {
                symbol = parity.ToUpper(),
                orderId = orderId
            };
            string method = "POST";
            string endpoint = "/api/spot/v1/trade/orderInfo";
            string timestamp = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds).ToString();
            string dataQueryString = $"{timestamp}{method}{endpoint}{JsonConvert.SerializeObject(body)}";
            string signature = GetSign(dataQueryString, secretKey);
            QueryOrderModel response = services.QueryOrder(apiKey, body, signature, APITuple.Item3, timestamp);
            QueryOrderResponseModel queryResponse = new QueryOrderResponseModel();
            queryResponse.symbol = response.data[0].symbol;
            queryResponse.orderId = Convert.ToString(response.data[0].orderId);
            if (response.data[0].status == "full_fill")
            {
                queryResponse.status = "FILLED";
            }
            queryResponse.type = response.data[0].orderType;
            queryResponse.side = response.data[0].side;
            queryResponse.qty = response.data[0].quantity;
            queryResponse.price = Convert.ToString(response.data[0].price);
            queryResponse.origQuoteOrderQty = response.data[0].fillQuantity;
            return queryResponse;
        }

        public CexBotStatusModel Status()
        {
            throw new NotImplementedException();
        }
        public string GetSign(string dataQueryString, string secretKey)
        {
            return CryptographyHelper.GetHMAC256(dataQueryString, secretKey, RETURNTYPE.BASE64);
        }
    }
}