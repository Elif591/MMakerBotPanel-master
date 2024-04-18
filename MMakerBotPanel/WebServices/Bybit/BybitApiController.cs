namespace MMakerBotPanel.WebServices.Bybit
{
    using MMakerBotPanel.Business;
    using MMakerBotPanel.Business.Exchange;
    using MMakerBotPanel.Models;
    using MMakerBotPanel.Models.ApiModels;
    using MMakerBotPanel.Models.CexBotModels;
    using MMakerBotPanel.Models.ChartsModel;
    using MMakerBotPanel.Models.ChartsModels;
    using MMakerBotPanel.Models.MakerMarketingModels;
    using MMakerBotPanel.WebServices.Bybit.Model;
    using MMakerBotPanel.WebServices.Interface;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    public class BybitApiController : ICEXAPIController
    {
        private readonly BybitWebServices services = new BybitWebServices();

        public PairList GetPairList()
        {
            SymbolModel ret = services.Parity();
            PairList pairList = new PairList();

            foreach (Result item in ret.GetSymbolDatasModels.result)
            {
                if (item.quoteCurrency == "USDT")
                {
                    Pair pair = new Pair
                    {
                        symbol = item.@alias,
                        baseAsset = item.baseCurrency,
                        quoteAsset = item.quoteCurrency
                    };
                    pairList.Pairs.Add(pair);
                }
            }
            //GetTrade();
            // GetOrderBook();
            //GetBalance();
            //  GetActiveOrder();
            //CreateSpotOrder();

            return pairList;
        }
        public List<CandleStickStockChartDataModel> GetChartData(string symbol, Interval interval, CandlestickQuantity? candlestickQuantity)
        {
            int timeInterval = (int)interval;
            string timeIntervalStr;
            if ((int)interval == (int)Interval.minutes30)
            {
                timeIntervalStr = timeInterval.ToString() + "m";
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
            StockDataModel ret = services.StockData(symbol, timeIntervalStr, candleQty.ToString());
            List<CandleStickStockChartDataModel> dataList = new List<CandleStickStockChartDataModel>();

            foreach (BybitDataChart item in ret.BybitDataChartModels)
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
        public List<CandleStickStockChartDataModel> GetChartDataDate(string symbol, Interval interval, CandlestickQuantity? candlestickQuantity)
        {
            int timeInterval = (int)interval;
            string timeIntervalStr;
            if ((int)interval == (int)Interval.minutes30)
            {
                timeIntervalStr = timeInterval.ToString() + "m";
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
            StockDataModel ret = services.StockData(symbol, timeIntervalStr, candleQty.ToString());
            List<CandleStickStockChartDataModel> dataList = new List<CandleStickStockChartDataModel>();

            foreach (BybitDataChart item in ret.BybitDataChartModels)
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
            string serverTime = ret.time.ToString().Substring(0, ret.time.ToString().Length - 3);
            DateTime dateTime = UnixTimeStampToDateTime(Convert.ToDouble(serverTime));
            CexBotStatusModel bybitStatus = new CexBotStatusModel();
            if (ret.retMsg == "OK")
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
        public OrderBookResponseModel GetOrderBook(string symbol)
        {
            OrderBookModel response = services.OrderBook(symbol);
            OrderBookResponseModel orderBooks = new OrderBookResponseModel();
            foreach (List<string> item in response.result.b)
            {
                string[] dataBuy = item.ToArray();
                double size = Convert.ToDouble(dataBuy[1].Replace('.', ','));
                double price = Convert.ToDouble(dataBuy[0].Replace('.', ','));
                double volume = size * price;
                OrderBookBuy buy = new OrderBookBuy()
                {
                    Volume = volume.ToString(),
                    Price = Convert.ToDecimal(dataBuy[0])
                };
                orderBooks.Buy.Add(buy);
            }
            foreach (List<string> item in response.result.a)
            {
                string[] dataBuy = item.ToArray();
                double size = Convert.ToDouble(dataBuy[1].Replace('.', ','));
                double price = Convert.ToDouble(dataBuy[0].Replace('.', ','));
                double volume = size * price;
                OrderBookSell sell = new OrderBookSell()
                {
                    Volume = volume.ToString(),
                    Price = Convert.ToDecimal(dataBuy[0])
                };
                orderBooks.Sell.Add(sell);
            }
            return orderBooks;
        }
        public List<TradesResponseModel> GetTrade(string symbol)
        {
            TradeModel response = services.Trade(symbol);
            List<TradesResponseModel> trades = new List<TradesResponseModel>();
            foreach (TradeList item in response.result.list)
            {
                double price = Convert.ToDouble(item.price.Replace('.', ','));
                double size = Convert.ToDouble(item.size.Replace('.', ','));
                double volume = size * price;
                TradesResponseModel trade = new TradesResponseModel()
                {
                    Symbol = symbol,
                    Price = item.price,
                    Volume = volume.ToString(),
                    Timestamp = Convert.ToInt64(item.time),
                    Type = item.side
                };
                trades.Add(trade);
            }
            return trades;
        }
        public CreateOrderResponseModel CreateSpotOrder(CreateOrderRequestModel requestModel)
        {
            Tuple<string, string, string> APITuple = BybitHelper.GetBybitHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new CreateOrderResponseModel();
            }
            StatusModel ret = services.Status();
            long serverTime = ret.time;
            int recvWindow = 50000;
            string category = "spot";
            string side = Enum.GetName(typeof(SIDE), requestModel.side);
            string orderType = Enum.GetName(typeof(TYPE_TRADE), requestModel.typeTrade);
            string qty = requestModel.amount;
            string price = requestModel.price;
            object body = new
            {
                category,
                requestModel.symbol,
                side,
                orderType,
                price,
                qty,

            };
            string signature = GetSign(Convert.ToString(serverTime), apiKey, recvWindow.ToString(), JsonConvert.SerializeObject(body), secretKey);
            CreateOrderModel response = services.CreateOrder(signature.ToLower(), apiKey, Convert.ToString(serverTime), recvWindow.ToString(), body);
            CreateOrderResponseModel createOrderResponseModel = new CreateOrderResponseModel()
            {
                Symbol = requestModel.symbol,
                OrderId = Convert.ToString(response.result.orderId),
                Timestamp = response.time
            };
            return createOrderResponseModel;
        }
        public List<ActiveOrderResponseModel> GetActiveOrder(string symbol)
        {
            Tuple<string, string, string> APITuple = BybitHelper.GetBybitHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new List<ActiveOrderResponseModel>();
            }
            StatusModel ret = services.Status();
            long serverTime = ret.time;
            int recvWindow = 50000;
            string category = "spot";
            string queryString = $"category={category}";
            string signature = GetSign(Convert.ToString(serverTime), apiKey, recvWindow.ToString(), queryString, secretKey);
            ActiveOrderModel response = services.ActiveOrder(signature.ToLower(), apiKey, Convert.ToString(serverTime), recvWindow.ToString(), queryString);
            List<ActiveOrderResponseModel> activeOrders = new List<ActiveOrderResponseModel>();
            foreach (ActiveOrderList item in response.result.list)
            {
                ActiveOrderResponseModel activeOrder = new ActiveOrderResponseModel()
                {
                    Timestamp = Convert.ToInt64(item.createdTime),
                    Symbol = item.symbol,
                    OrderId = item.orderId,
                    Volume = item.qty,
                    Price = item.price,
                    Status = item.orderStatus,
                    Type = item.orderType,
                    Side = item.side,

                };
                activeOrders.Add(activeOrder);
            }
            return activeOrders;
        }
        public List<BalanceResponseModel> GetBalance()
        {
            Tuple<string, string, string> APITuple = BybitHelper.GetBybitHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new List<BalanceResponseModel>();
            }
            StatusModel ret = services.Status();
            long serverTime = ret.time;
            int recvWindow = 50000;
            string accountType = "SPOT";
            string queryString = $"accountType={accountType}";
            string signature = GetSign(Convert.ToString(serverTime), apiKey, recvWindow.ToString(), queryString, secretKey);
            BalanceModel response = services.Balance(signature.ToLower(), apiKey, Convert.ToString(serverTime), recvWindow.ToString(), queryString);
            List<BalanceResponseModel> balances = new List<BalanceResponseModel>();
            foreach (GetWalletList item in response.result.list)
            {
                List<Coin> coinList = item.coin;
                foreach (Coin dataItem in coinList)
                {
                    BalanceResponseModel balance = new BalanceResponseModel()
                    {
                        Symbol = dataItem.coin,
                        Balance = dataItem.walletBalance
                    };
                    balances.Add(balance);
                }

            }
            return balances;
        }
        public PairList GetPair(string parity)
        {
            PairList pairList = new PairList();
            PairModel response = services.Pair(parity);
            foreach (var item in response.result.list)
            {
                Pair pair = new Pair()
                {
                    symbol = item.symbol,
                    qtyFormat = item.lotSizeFilter.minOrderAmt
                };
                pairList.Pairs.Add(pair);
            }
            return pairList;
        }
        public List<CancelOrderResponseModel> CancelOrders(string symbol)
        {
            Tuple<string, string, string> APITuple = BybitHelper.GetBybitHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new List<CancelOrderResponseModel>();
            }
            StatusModel ret = services.Status();
            long serverTime = ret.time;
            int recvWindow = 50000;
            string category = "spot";
            object body = new
            {
                category,
                symbol
            };
            string signature = GetSign(Convert.ToString(serverTime), apiKey, recvWindow.ToString(), JsonConvert.SerializeObject(body), secretKey);
            CancelOrderModel response = services.CancelOrder(signature.ToLower(), apiKey, Convert.ToString(serverTime), recvWindow.ToString(), body);
            CancelOrderResponseModel cancelOrder = new CancelOrderResponseModel();
            List<CancelOrderResponseModel> cancelOrders = new List<CancelOrderResponseModel>();
            foreach (var item in response.result.list)
            {
                cancelOrder.orderId = item.orderId;
                cancelOrders.Add(cancelOrder);
            }
            return cancelOrders;
        }
        public QueryOrderResponseModel QueryOrders(string symbol, string orderId)
        {
            Tuple<string, string, string> APITuple = BybitHelper.GetBybitHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new QueryOrderResponseModel();
            }
            StatusModel ret = services.Status();
            long serverTime = ret.time;
            int recvWindow = 50000;
            string queryString = $"symbol={symbol}&orderId={orderId}";
            string signature = GetSign(Convert.ToString(serverTime), apiKey, recvWindow.ToString(), queryString, secretKey);
            QueryOrderModel response = services.QueryOrder(signature.ToLower(), apiKey, Convert.ToString(serverTime), recvWindow.ToString(), queryString);
            QueryOrderResponseModel queryOrder = new QueryOrderResponseModel();
            foreach (var item in response.result.list)
            {
                queryOrder.price = item.price;
                queryOrder.type = item.orderType;
                queryOrder.side = item.side;    
                queryOrder.qty = item.qty;
                queryOrder.status = item.orderStatus.ToUpper();

            }
            return queryOrder;
        }
        public ComissionFeeResponseModel ComissionFee(string symbol)
        {
            ComissionFeeResponseModel comission = new ComissionFeeResponseModel()
            {
                symbol = symbol,
                makerCommission = "0.1",
                takerCommission = "0.1",
            };
            return comission;
        }
        public string GetSign(string timestamp, string apiKey, string recvWindow, string queryString, string secretKey)
        {
            string parameters = timestamp + apiKey + recvWindow + queryString;
            return CryptographyHelper.GetHMAC256(parameters, secretKey, RETURNTYPE.HEX);
        }
        public DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }

    }
}