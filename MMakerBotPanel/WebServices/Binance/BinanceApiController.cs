namespace MMakerBotPanel.WebServices.Binance
{
    using MMakerBotPanel.Business;
    using MMakerBotPanel.Business.Exchange;
    using MMakerBotPanel.Models;
    using MMakerBotPanel.Models.ApiModels;
    using MMakerBotPanel.Models.CexBotModels;
    using MMakerBotPanel.Models.ChartsModel;
    using MMakerBotPanel.Models.ChartsModels;
    using MMakerBotPanel.Models.MakerMarketingModels;
    using MMakerBotPanel.WebServices.Binance.Model;
    using MMakerBotPanel.WebServices.Interface;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class BinanceApiController : ICEXAPIController
    {
        private readonly BinanceWebServices services = new BinanceWebServices();

        public PairList GetPairList()
        {
            SymbolModel ret = services.Parity();
            PairList pairList = new PairList();
            foreach (symbols item in ret.symbols)
            {
                if (item.status == "TRADING" && item.quoteAsset == "USDT")
                {
                    if (item.permissions.Where(x => x == "SPOT").Count() > 0)
                    {
                        Pair pair = new Pair
                        {
                            symbol = item.symbol,
                            baseAsset = item.baseAsset,
                            quoteAsset = item.quoteAsset,
                            qtyFormat = item.filters[1].minQty
                        };
                        pairList.Pairs.Add(pair);
                    }
                }
            }
            return pairList;
        }
        public PairList GetPair(string symbol)
        {
            SymbolModel ret = services.GetParityQtyFormat(symbol);
            PairList pairList = new PairList();
            if(ret.symbols != null)
            {
                foreach (symbols item in ret.symbols)
                {
                    if (item.status == "TRADING" && item.quoteAsset == "USDT")
                    {
                        if (item.permissions.Where(x => x == "SPOT").Count() > 0)
                        {
                            Pair pair = new Pair
                            {
                                symbol = item.symbol,
                                baseAsset = item.baseAsset,
                                quoteAsset = item.quoteAsset,
                                qtyFormat = item.filters[1].minQty
                            };
                            pairList.Pairs.Add(pair);
                        }
                    }
                }

            }
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

            foreach (StockDataDetail item in ret.stockDataDetailModels)
            {
                CandleStickStockChartDataModel data = new CandleStickStockChartDataModel
                {
                    Date = item.OpenTime,
                    Open = item.OpenPrice,
                    High = item.HighPrice,
                    Low = item.LowPrice,
                    Close = item.ClosePrice,
                    Volume = item.Volume
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

            foreach (StockDataDetail item in ret.stockDataDetailModels)
            {
                CandleStickStockChartDataModel data = new CandleStickStockChartDataModel
                {
                    Date = item.OpenTime,
                    Open = item.OpenPrice,
                    High = item.HighPrice,
                    Low = item.LowPrice,
                    Close = item.ClosePrice,
                    Volume = item.Volume
                };
                dataList.Add(data);
            }

            return dataList;
        }
        public CexBotStatusModel Status()
        {
            StatusModel ret = services.Status();
            TimeModel time = services.Time();
            string serverTime = time.serverTime.ToString().Substring(0, time.serverTime.ToString().Length - 3);
            DateTime dateTime = UnixTimeStampToDateTime(Convert.ToDouble(serverTime));
            CexBotStatusModel binanceStatus = new CexBotStatusModel();
            binanceStatus.CexBotStatusModels.status = ret.status == 0;
            binanceStatus.CexBotStatusModels.date = dateTime;
            return binanceStatus;
        }
        public OrderBookResponseModel GetOrderBook(string symbol)
        {
            OrderBookModel response = services.OrderBook(symbol);
            OrderBookResponseModel orderBooks = new OrderBookResponseModel();
            foreach (List<string> item in response.bids)
            {
                string[] dataItem = item.ToArray();
                OrderBookBuy buy = new OrderBookBuy()
                {
                    Volume = dataItem[0],
                    Price = Convert.ToDecimal(dataItem[1].Replace(".", ",")),
                };
                orderBooks.Buy.Add(buy);
            }
            foreach (List<string> item in response.asks)
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
        public List<TradesResponseModel> GetTrade(string symbol)
        {
            string type;
            TradeModel response = services.Trade(symbol);
            List<TradesResponseModel> trades = new List<TradesResponseModel>();

            foreach (TradesData item in response.trades)
            {
                type = item.isBuyerMaker ? "BUY" : "SELL";
                TradesResponseModel trade = new TradesResponseModel()
                {
                    Symbol = symbol,
                    Price = item.price,
                    Volume = item.qty,
                    Timestamp = item.time,
                    Type = type
                };
                trades.Add(trade);
            }

            return trades;
        }
        public CreateOrderResponseModel CreateSpotOrder(CreateOrderRequestModel requestModel)
        {
            Tuple<string, string , string> APITuple = BinanceHelper.GetBinanceHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new CreateOrderResponseModel();
            }
            string recvWindow = "20000";
            string timeInForce = "GTC";
            TimeModel ret = services.Time();
            long servertime = ret.serverTime;
            string type = requestModel.typeTrade.ToString().ToUpper();
            string sideBinance = requestModel.side.ToString().ToUpper();
            double amount = Convert.ToDouble(requestModel.amount.Replace(".", ","));
            decimal amountDec = new decimal(amount);
            string quantity = amountDec.ToString("0.000");
            string timestamp = Convert.ToString(servertime);
            string dataQueryString = "";
            if (requestModel.typeTrade == TYPE_TRADE.MARKET)
            {
                dataQueryString = "symbol=" + requestModel.symbol + "&side=" + sideBinance + "&type=" + type + "&quantity=" + quantity.Replace(",", ".") + "&recvWindow=" + recvWindow + "&timestamp=" + timestamp;
            }
            else
            {
                dataQueryString = "symbol=" + requestModel.symbol + "&side=" + sideBinance + "&type=" + type + "&timeInForce=" + timeInForce + "&quantity=" + quantity.Replace(",", ".") + "&price=" + requestModel.price.Replace(",", ".")+ "&recvWindow=" + recvWindow + "&timestamp=" + timestamp;
            }

            string signature = GetSign(dataQueryString, secretKey);
            CreateOrderModel response = services.CreateOrder(apiKey, dataQueryString, signature);
          
            CreateOrderResponseModel createOrderResponseModel = new CreateOrderResponseModel()
            {
                Symbol = requestModel.symbol,
                OrderId = Convert.ToString(response.orderId),
                Timestamp = servertime
            };
            return createOrderResponseModel;
        }
        public List<CancelOrderResponseModel> CancelOrders(string symbol)
        {
            Tuple<string, string, string> APITuple = BinanceHelper.GetBinanceHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new List<CancelOrderResponseModel>();
            }
            string recvWindow = "20000";
            TimeModel ret = services.Time();
            long servertime = ret.serverTime;
            string timestamp = Convert.ToString(servertime);
            string dataQueryString = "";
            dataQueryString = "symbol=" + symbol + "&recvWindow=" + recvWindow + "&timestamp=" + timestamp;
            string signature = GetSign(dataQueryString, secretKey);
            CancelOrderModel response = services.CancelOrder(apiKey, dataQueryString, signature);
            List<CancelOrderResponseModel> cancelOrderResponseModel = new List<CancelOrderResponseModel>();
            if (response.cancelOrderListModel != null)
            {
                foreach (var item in response.cancelOrderListModel)
                {

                    CancelOrderResponseModel createOrderResponse = new CancelOrderResponseModel()
                    {
                        symbol = item.symbol,
                        orderId = Convert.ToString(item.orderId),
                        status = item.status
                    };
                    cancelOrderResponseModel.Add(createOrderResponse);

                }
            }
            return cancelOrderResponseModel;
        }
        public QueryOrderResponseModel QueryOrders(string symbol, string orderId)
        {
            Tuple<string, string, string> APITuple = BinanceHelper.GetBinanceHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new QueryOrderResponseModel();
            }
            string recvWindow = "20000";
            TimeModel ret = services.Time();
            long servertime = ret.serverTime;
            string timestamp = Convert.ToString(servertime);
            string dataQueryString = "symbol=" + symbol + "&orderId=" + long.Parse(orderId) + "&recvWindow=" + recvWindow + "&timestamp=" + timestamp;
            string signature = GetSign(dataQueryString, secretKey);
            QueryOrderModel response = services.QueryOrder(apiKey, dataQueryString, signature);

            QueryOrderResponseModel queryResponse = new QueryOrderResponseModel()
            {
                symbol = response.symbol,
                orderId = Convert.ToString(response.orderId),
                status = response.status,
                type = response.type,
                side = response.side,
                qty = response.origQty,
                price = Convert.ToString(response.price),
                origQuoteOrderQty = response.origQuoteOrderQty
            };

            return queryResponse;
        }
        public ComissionFeeResponseModel ComissionFee(string symbol)
        {
            Tuple<string, string, string> APITuple = BinanceHelper.GetBinanceHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new ComissionFeeResponseModel();
            }
            string recvWindow = "20000";
            TimeModel ret = services.Time();
            long servertime = ret.serverTime;
            string timestamp = Convert.ToString(servertime);
            string dataQueryString = "symbol=" + symbol + "&recvWindow=" + recvWindow + "&timestamp=" + timestamp;
            string signature = GetSign(dataQueryString, secretKey);
            ComissionModel response = services.ComissionFee(apiKey, dataQueryString, signature);

            ComissionFeeResponseModel comission = new ComissionFeeResponseModel()
            {
                symbol = response.comissionFee[0].symbol,
                makerCommission = response.comissionFee[0].makerCommission,
                takerCommission = response.comissionFee[0].takerCommission
            };
         
            return comission;
        }
        public List<ActiveOrderResponseModel> GetActiveOrder(string symbol)
        {
            Tuple<string, string, string> APITuple = BinanceHelper.GetBinanceHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new List<ActiveOrderResponseModel>();
            }
            TimeModel ret = services.Time();
            long servertime = ret.serverTime;
            string timestamp = Convert.ToString(servertime);
            long recvWindow = 60000;
            string dataQueryString = "symbol=" + symbol + "&timestamp=" + timestamp + "&recvWindow=" + recvWindow;
            string signature = GetSign(dataQueryString, secretKey);
            ActiveOrderModel response = services.ActiveOrder(apiKey, dataQueryString, signature);
            List<ActiveOrderResponseModel> activeOrders = new List<ActiveOrderResponseModel>();
            foreach (ActiveOrder item in response.activeOrders)
            {
                ActiveOrderResponseModel activeOrder = new ActiveOrderResponseModel()
                {
                    Symbol = symbol,
                    Timestamp = item.time,
                    OrderId = Convert.ToString(item.orderId),
                    Volume = item.origQty,
                    Price = item.price,
                    Status = item.status,
                    Type = item.type,
                    Side = item.side
                };
                activeOrders.Add(activeOrder);
            }
            return activeOrders;
        }
        public List<BalanceResponseModel> GetBalance()
        {
            Tuple<string, string , string> APITuple = BinanceHelper.GetBinanceHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new List<BalanceResponseModel>();
            }
            TimeModel ret = services.Time();
            long servertime = ret.serverTime;
            string timestamp = Convert.ToString(servertime);
            long recvWindow = 60000;
            string dataQueryString = "timestamp=" + timestamp + "&recvWindow=" + recvWindow;
            string signature = GetSign(dataQueryString, secretKey);
            BalanceModel response = services.Balance(apiKey, dataQueryString, signature);
            List<BalanceResponseModel> balances = new List<BalanceResponseModel>();
            foreach (BalanceData item in response.getWallets)
            {
                BalanceResponseModel balance = new BalanceResponseModel()
                {
                    Symbol = item.asset,
                    Balance = item.free
                };
                balances.Add(balance);
            }
            return balances;
        }
        public string GetSign(string parameterStr, string secretKey)
        {
            return CryptographyHelper.GetHMAC256(parameterStr, secretKey, RETURNTYPE.HEX);
        }
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }

    }
}