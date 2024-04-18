namespace MMakerBotPanel.WebServices.MEXC
{
    using MMakerBotPanel.Business;
    using MMakerBotPanel.Business.Exchange;
    using MMakerBotPanel.Models.ApiModels;
    using MMakerBotPanel.Models.CexBotModels;
    using MMakerBotPanel.Models.ChartsModel;
    using MMakerBotPanel.Models.ChartsModels;
    using MMakerBotPanel.Models.MakerMarketingModels;
    using MMakerBotPanel.WebServices.Interface;
    using MMakerBotPanel.WebServices.MEXC.Model;
    using System;
    using System.Collections.Generic;

    public class MEXCApiController : ICEXAPIController
    {
        private readonly MEXCWebServices services = new MEXCWebServices();

        public List<CandleStickStockChartDataModel> GetChartData(string symbol, Interval interval, CandlestickQuantity? candlestickQuantity)
        {
            MexcDataModel ret = services.getStockData(symbol, "4h");
            List<CandleStickStockChartDataModel> dataList = new List<CandleStickStockChartDataModel>();

            foreach (MexcDataChartModel item in ret.MexcDataChartModels)
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
            MexcDataModel ret = services.getStockDataDate(symbol, "4h");
            List<CandleStickStockChartDataModel> dataList = new List<CandleStickStockChartDataModel>();

            foreach (MexcDataChartModel item in ret.MexcDataChartModels)
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

        public PairList GetPairList()
        {
            MexcSymbolModel ret = services.getParity();
            PairList pairList = new PairList();
            foreach (Datum item in ret.data)
            {
                string[] splitList = item.symbol.Split('_');
                _ = splitList[0];
                _ = splitList[1];

                if (item.state == "ENABLED" && splitList[1] == "USDT")
                {
                    Pair pair = new Pair
                    {
                        symbol = splitList[0] + splitList[1],
                        baseAsset = splitList[0],
                        quoteAsset = splitList[1]
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
            CexBotStatusModel gateioStatus = new CexBotStatusModel();
            if (ret.serverTime != 0)
            {
                string serverTime = ret.serverTime.ToString().Substring(0, ret.serverTime.ToString().Length - 3);
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

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }

        public CreateOrderResponseModel CreateSpotOrder(CreateOrderRequestModel requestModel)
        {
            Tuple<string, string, string> APITuple = MEXCHelper.GetMEXCHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new CreateOrderResponseModel();
            }
            string symbol = requestModel.symbol;
            string side = requestModel.side.ToString().ToUpper();
            string type = requestModel.typeTrade.ToString().ToUpper();
            string quantity = requestModel.amount;
            string price = requestModel.price;
            string timestamp = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds).ToString();
            string queryString = $"symbol={symbol}&side={side}&type={type}&quantity={quantity}&price={price}&timestamp={timestamp}";
            string signature = GetSign(queryString, secretKey);
            MEXCExchange response = services.CreateOrder(apiKey, signature, queryString);
            CreateOrderResponseModel createOrderResponseModel = new CreateOrderResponseModel()
            {
                Symbol = symbol,
                OrderId = Convert.ToString(response.data),
                Timestamp = Convert.ToInt64(timestamp)
            };
            return createOrderResponseModel;
        }
        public string GetSign(string queryString, string secretKey)
        {
            return CryptographyHelper.GetHMAC256(queryString, secretKey, Models.RETURNTYPE.HEX);
        }

        public List<BalanceResponseModel> GetBalance()
        {
            Tuple<string, string, string> APITuple = MEXCHelper.GetMEXCHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new List<BalanceResponseModel>();
            }

            string timestamp = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds).ToString();
            string queryString = $"timestamp={timestamp}";
            string signature = GetSign(queryString, secretKey);
            BalanceModel response = services.GetBalance(apiKey, signature, queryString);
            List<BalanceResponseModel> balances = new List<BalanceResponseModel>();

            foreach (var item in response.balances)
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

        public List<ActiveOrderResponseModel> GetActiveOrder(string symbol)
        {
            Tuple<string, string, string> APITuple = MEXCHelper.GetMEXCHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new List<ActiveOrderResponseModel>();
            }

            string timestamp = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds).ToString();
            string queryString = $"symbol={symbol}&timestamp={timestamp}";
            string signature = GetSign(queryString, secretKey);
            ActiveOrdersModel response = services.ActiveOrders(apiKey, signature, queryString);
            List<ActiveOrderResponseModel> activeOrders = new List<ActiveOrderResponseModel>();
            foreach (var item in response.activeOrders)
            {
                ActiveOrderResponseModel activeorder = new ActiveOrderResponseModel()
                {
                    Symbol = item.symbol,
                    OrderId = item.clientOrderId,
                    Price = item.price,
                    Side = item.side,
                    Status = item.status,
                };
                activeOrders.Add(activeorder);
            }

            return activeOrders;
        }

        public OrderBookResponseModel GetOrderBook(string symbol)
        {

            string queryString = $"symbol={symbol}";

            OrderBookModel response = services.OrderBook(queryString);
            OrderBookResponseModel orderBookResponseModel = new OrderBookResponseModel();

            foreach (List<string> itemList in response.bids)
            {
                string volume = itemList[1].ToString();
                decimal price = Convert.ToDecimal(itemList[0]);

                OrderBookBuy buyItem = new OrderBookBuy
                {
                    Volume = volume,
                    Price = price
                };

                orderBookResponseModel.Buy.Add(buyItem);
            }
            foreach (List<string> itemList in response.asks)
            {
                string volume = itemList[1].ToString();
                decimal price = Convert.ToDecimal(itemList[0]);

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
            Tuple<string, string, string> APITuple = MEXCHelper.GetMEXCHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new List<CancelOrderResponseModel>();
            }

            string timestamp = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds).ToString();
            string queryString = $"symbol={symbol}&timestamp={timestamp}";
            string signature = GetSign(queryString, secretKey);
            CancelOrdersModel response = services.CancelOrders(apiKey, signature, queryString);
            List<CancelOrderResponseModel> cancelOrders = new List<CancelOrderResponseModel>();

            foreach (var item in response.cancelOrders)
            {
                CancelOrderResponseModel cancelOrder = new CancelOrderResponseModel()
                {
                    symbol = item.symbol,
                    orderId = item.clientOrderId,
                    status = item.status,

                };
                cancelOrders.Add(cancelOrder);
            }

            return cancelOrders;
        }

        public QueryOrderResponseModel QueryOrders(string symbol, string orderId)
        {
            Tuple<string, string, string> APITuple = MEXCHelper.GetMEXCHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new QueryOrderResponseModel();
            }
            string timestamp = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds).ToString();
            string queryString = $"symbol={symbol}&orderId={orderId}&timestamp={timestamp}";
            string signature = GetSign(queryString, secretKey);
            QueryOrderModel response = services.QueryOrders(apiKey, signature, queryString);
            QueryOrderResponseModel queryOrder = new QueryOrderResponseModel()
            {
                symbol = response.symbol,
                orderId = response.clientOrderId,
                price = response.price,
                side = response.side,
                status = response.status,
            };
            return queryOrder;
        }

        public ComissionFeeResponseModel ComissionFee(string symbol)
        {
            ComissionFeeResponseModel comissionFee = new ComissionFeeResponseModel();
            comissionFee.symbol = symbol;
            comissionFee.makerCommission = "0.01";
            comissionFee.takerCommission = "0.01";
            return comissionFee;    
        }
    }
}