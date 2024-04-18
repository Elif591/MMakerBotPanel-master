namespace MMakerBotPanel.WebServices.Probit
{
    using MMakerBotPanel.Models.ApiModels;
    using MMakerBotPanel.Models.CexBotModels;
    using MMakerBotPanel.Models.ChartsModel;
    using MMakerBotPanel.Models.ChartsModels;
    using MMakerBotPanel.Models.MakerMarketingModels;
    using MMakerBotPanel.WebServices.Probit.Model;
    using MMakerBotPanel.WebServices.Interface;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using MMakerBotPanel.Business.Exchange;

    public class ProbitApiController : ICEXAPIController
    {
        private readonly ProbitWebServices services = new ProbitWebServices();
        public List<CancelOrderResponseModel> CancelOrders(string symbol)
        {
            Tuple<string, string, string> APITuple = ProbitHelper.GetProbitHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new List<CancelOrderResponseModel>();
            }
            var authHeader = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(apiKey + ":" + secretKey));
            TokenModel token = services.GetToken(authHeader);
            string auth = $"Bearer {token.access_token}";
            List<CancelOrderResponseModel> cancelOrders = new List<CancelOrderResponseModel>();
            List<ActiveOrderResponseModel> activeOrder = GetActiveOrder(symbol);
            foreach (ActiveOrderResponseModel order in activeOrder)
            {
                object body = new
                {
                    market_id = symbol,
                    order_id = order.OrderId

                };
                CancelOrderModel cancelOrder = services.CancelOrder(auth, body);
                CancelOrderResponseModel cancelOrderResponse = new CancelOrderResponseModel();
                cancelOrderResponse.status = cancelOrder.data.status;
                cancelOrderResponse.symbol = symbol;
                cancelOrderResponse.orderId = cancelOrder.data.id;

                cancelOrders.Add(cancelOrderResponse);
            }
            return cancelOrders;
        }

        public ComissionFeeResponseModel ComissionFee(string symbol)
        {
            ComissionFeeResponseModel comissionFee = new ComissionFeeResponseModel()
            {
                makerCommission = "0.2",
                takerCommission = "0.2"
            };
            return comissionFee;
        }

        public CreateOrderResponseModel CreateSpotOrder(CreateOrderRequestModel request)
        {
            Tuple<string, string, string> APITuple = ProbitHelper.GetProbitHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new CreateOrderResponseModel();
            }
            var authHeader = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(apiKey + ":" + secretKey));
            TokenModel token = services.GetToken(authHeader);
            string auth = $"Bearer {token.access_token}";

            object body = new
            {
                market_id = request.symbol,
                type = request.typeTrade,
                side = request.side,
                time_in_force = "gtc",
                limit_price = request.price,
                quantity = request.amount
            };
            CreateOrderModel response = services.CreateOrder(auth, body);
            CreateOrderResponseModel createOrder = new CreateOrderResponseModel();
            createOrder.OrderId = response.id;
            createOrder.Symbol = response.market_id;
            DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            long timestamp = (long)(response.time.ToUniversalTime() - unixEpoch).TotalMilliseconds;
            createOrder.Timestamp = timestamp;

            return createOrder;
        }

        public List<ActiveOrderResponseModel> GetActiveOrder(string symbol)
        {
            Tuple<string, string, string> APITuple = ProbitHelper.GetProbitHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new List<ActiveOrderResponseModel>();
            }
            var authHeader = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(apiKey + ":" + secretKey));
            TokenModel token = services.GetToken(authHeader);
            string auth = $"Bearer {token.access_token}";
            ActiveOrderModel response = services.GetActiveOrder(auth, symbol);

            List<ActiveOrderResponseModel> activeOrders = new List<ActiveOrderResponseModel>();
            foreach (ActiveOrder item in response.data)
            {
                ActiveOrderResponseModel activeOrder = new ActiveOrderResponseModel()
                {
                    Symbol = item.market_id,
                    OrderId = item.id,
                    Status = item.status,
                    Side = item.side,
                    Price = item.limit_price
                };
                activeOrders.Add(activeOrder);

            }
            return activeOrders;
        }

        public List<BalanceResponseModel> GetBalance()
        {
            Tuple<string, string, string> APITuple = ProbitHelper.GetProbitHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new List<BalanceResponseModel>();
            }
            var authHeader = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(apiKey + ":" + secretKey));
            TokenModel token = services.GetToken(authHeader);
            string auth = $"Bearer {token.access_token}";
            BalanceModel response = services.GetBalance(auth);
            List<BalanceResponseModel> balances = new List<BalanceResponseModel>();
            foreach (Balance item in response.data)
            {
                BalanceResponseModel balance = new BalanceResponseModel()
                {
                    Symbol = item.currency_id,
                    Balance = item.total
                };
                balances.Add(balance);

            }
            return balances;
        }

        public List<CandleStickStockChartDataModel> GetChartData(string symbol, Interval interval, CandlestickQuantity? candlestickQuantity)
        {
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
                time = timeInterval.ToString() + "h";
            }

            if (candlestickQuantity == CandlestickQuantity.Candle1080)
            {
                candleQTY = "1000";
            }
            else
            {
                candleQTY = candlestickQty.ToString();
            }

            DateTime now = DateTime.Now;
            string endTime = now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            DateTime end = now.AddDays(-167);
            string startTime = end.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            string query = $"?market_ids={symbol}&start_time={Uri.EscapeDataString(startTime)}&end_time={Uri.EscapeDataString(endTime)}&interval={time}&sort=asc&limit={candleQTY}";
            StokDataModel data = services.StockData(query);
            List<CandleStickStockChartDataModel> stockDatas = new List<CandleStickStockChartDataModel>();
            foreach (var item in data.data)
            {
                DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                TimeSpan elapsedTime = item.start_time - epoch;
                long timestamp = (long)elapsedTime.TotalMilliseconds;
                CandleStickStockChartDataModel stockData = new CandleStickStockChartDataModel();
                stockData.Date = timestamp;
                stockData.Open = Convert.ToDouble(item.open.Replace(".", ","));
                stockData.High = Convert.ToDouble(item.high.Replace(".", ","));
                stockData.Low = Convert.ToDouble(item.low.Replace(".", ","));
                stockData.Close = Convert.ToDouble(item.close.Replace(".", ","));
                stockData.Volume = Convert.ToInt64(Convert.ToDouble(item.quote_volume.Replace(".", ",")));
                stockDatas.Add(stockData);
            }
            return stockDatas;
        }

        public List<CandleStickStockChartDataModel> GetChartDataDate(string symbol, Interval interval, CandlestickQuantity? candlestickQuantity)
        {
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
                time = timeInterval.ToString() + "h";
            }

            if (candlestickQuantity == CandlestickQuantity.Candle1080)
            {
                candleQTY = "1000";
            }
            else
            {
                candleQTY = candlestickQty.ToString();
            }

            DateTime now = DateTime.Now;
            string endTime = now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            DateTime end = now.AddDays(-1);
            string startTime = end.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            string query = $"?market_ids={symbol}&start_time={Uri.EscapeDataString(startTime)}&end_time={Uri.EscapeDataString(endTime)}&interval=1m&sort=asc&limit={candleQTY}";
            StokDataModel data = services.StockData(query);
            List<CandleStickStockChartDataModel> stockDatas = new List<CandleStickStockChartDataModel>();
            foreach (var item in data.data)
            {
                DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                TimeSpan elapsedTime = item.start_time - epoch;
                long timestamp = (long)elapsedTime.TotalMilliseconds;
                CandleStickStockChartDataModel stockData = new CandleStickStockChartDataModel();
                stockData.Date = timestamp;
                stockData.Open = Convert.ToDouble(item.open.Replace(".", ","));
                stockData.High = Convert.ToDouble(item.high.Replace(".", ","));
                stockData.Low = Convert.ToDouble(item.low.Replace(".", ","));
                stockData.Close = Convert.ToDouble(item.close.Replace(".", ","));
                stockData.Volume = Convert.ToInt64(Convert.ToDouble(item.quote_volume.Replace(".", ",")));
                stockDatas.Add(stockData);
            }
            return stockDatas;
        }

        public OrderBookResponseModel GetOrderBook(string symbol)
        {
            string query = $"?market_id={symbol}";
            OrderBookModel response = services.GetOrderBook(query);
            OrderBookResponseModel orderBook = new OrderBookResponseModel();
            foreach (OrderBook item in response.data)
            {
                if (item.side == "sell")
                {
                    OrderBookSell sell = new OrderBookSell()
                    {
                        Price = Convert.ToInt64(item.price.Replace(".", ",")),
                        Volume = item.quantity
                    };
                    orderBook.Sell.Add(sell);
                }
                else
                {
                    OrderBookBuy buy = new OrderBookBuy()
                    {
                        Price = Convert.ToInt64(item.price.Replace(".", ",")),
                        Volume = item.quantity
                    };
                    orderBook.Buy.Add(buy);
                }
            }
            return orderBook;
        }

        public PairList GetPair(string parity)
        {
            SymbolModel ret = services.Parity();
            PairList pairList = new PairList();
            foreach (Symbol item in ret.data)
            {
                if (parity == item.id)
                {
                    Pair pair = new Pair
                    {
                        symbol = item.id,
                        qtyFormat = item.min_quantity
                    };
                    pairList.Pairs.Add(pair);
                }
            }
            return pairList;
        }

        public PairList GetPairList()
        {
            SymbolModel ret = services.Parity();
            PairList pairList = new PairList();
            foreach (Symbol item in ret.data)
            {
                if (item.quote_currency_id == "USDT")
                {
                    Pair pair = new Pair
                    {
                        symbol = item.id,
                        baseAsset = item.base_currency_id,
                        quoteAsset = item.quote_currency_id
                    };
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
            Tuple<string, string, string> APITuple = ProbitHelper.GetProbitHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new QueryOrderResponseModel();
            }
            var authHeader = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(apiKey + ":" + secretKey));
            TokenModel token = services.GetToken(authHeader);
            string auth = $"Bearer {token.access_token}";
            string query = $"?market_id={symbol}&order_id={orderId}";
            QueryOrderModel queryOrder = services.QueryOrder(auth, query);
            QueryOrderResponseModel queryOrderResponse = new QueryOrderResponseModel();
            foreach (var item in queryOrder.data)
            {
                queryOrderResponse.orderId = item.client_order_id;
                queryOrderResponse.symbol = item.market_id;
                queryOrderResponse.status = item.status;
                queryOrderResponse.price = item.limit_price;
                queryOrderResponse.side = item.side;
                queryOrderResponse.type = item.type;

            }
            return queryOrderResponse;
        }

        public CexBotStatusModel Status()
        {
            throw new NotImplementedException();
        }
    }
}