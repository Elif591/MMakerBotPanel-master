namespace MMakerBotPanel.WebServices.Kucoin
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
    using MMakerBotPanel.WebServices.Kucoin.Model;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    public class KucoinApiController : ICEXAPIController
    {
        private readonly KucoinWebServices services = new KucoinWebServices();

        public PairList GetPairList()
        {
            SymbolModel ret = services.Parity();
            PairList pairList = new PairList();
            foreach (Data item in ret.data)
            {
                if (item.enableTrading && item.quoteCurrency == "USDT")
                {
                    Pair pair = new Pair();
                    string[] symboList = item.symbol.Split('-');
                    pair.symbol = symboList[0] + symboList[1];
                    pair.baseAsset = item.baseCurrency;
                    pair.quoteAsset = item.quoteCurrency;
                    pairList.Pairs.Add(pair);
                }

            }
           // GetTrade();
            //  GetOrderBook();
            //  GetBalance();
            //  GetActiveOrder();
            // CreateSpotOrder();
            return pairList;
        }
        public List<CandleStickStockChartDataModel> GetChartData(string symbol, Interval interval, CandlestickQuantity? candlestickQuantity)
        {
            string symbol1 = symbol.Substring(0, symbol.Length - 4);
            string symbolKucoin = symbol1 + "-USDT";
            int unixTimestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            unixTimestamp -= 31104000;
            StockDataModel ret = services.StockData(symbolKucoin, "4hour", unixTimestamp.ToString());
            List<CandleStickStockChartDataModel> dataList = new List<CandleStickStockChartDataModel>();

            foreach (StockDataDetail item in ret.KucoinDataDetailModels)
            {
                CandleStickStockChartDataModel data = new CandleStickStockChartDataModel
                {
                    Date = item.startTime * 1000,
                    Open = item.openingPrice,
                    High = item.highestPrice,
                    Low = item.lowestPrice,
                    Close = item.closingPrice,
                    Volume = item.transactionVolume * 10000
                };
                dataList.Add(data);
            }
            dataList.Reverse();
            return dataList;
        }
        public List<CandleStickStockChartDataModel> GetChartDataDate(string symbol, Interval interval, CandlestickQuantity? candlestickQuantity)
        {
            string symbol1 = symbol.Substring(0, symbol.Length - 4);
            string symbolKucoin = symbol1 + "-USDT";
            int unixTimestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            unixTimestamp -= 14400;
            StockDataModel ret = services.StockData(symbolKucoin, "4hour", unixTimestamp.ToString());
            List<CandleStickStockChartDataModel> dataList = new List<CandleStickStockChartDataModel>();

            foreach (StockDataDetail item in ret.KucoinDataDetailModels)
            {
                CandleStickStockChartDataModel data = new CandleStickStockChartDataModel
                {
                    Date = item.startTime * 1000,
                    Open = item.openingPrice,
                    High = item.highestPrice,
                    Low = item.lowestPrice,
                    Close = item.closingPrice,
                    Volume = item.transactionVolume * 10000
                };
                dataList.Add(data);
            }
            dataList.Reverse();
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
        public OrderBookResponseModel GetOrderBook(string symbol)
        {
            string symbol1 = symbol.Substring(0, symbol.Length - 4);
            string symbolKucoin = symbol1 + "-USDT";
            OrderBookModel response = services.OrderBook(symbolKucoin);
            OrderBookResponseModel orderBooks = new OrderBookResponseModel();
            foreach (List<string> item in response.data.bids)
            {
                string[] dataItem = item.ToArray();
                double size = Convert.ToDouble(dataItem[1].Replace('.', ','));
                double price = Convert.ToDouble(dataItem[0].Replace('.', ','));
                double volume = size * price;
                OrderBookBuy buy = new OrderBookBuy()
                {
                    Volume = volume.ToString(),
                    Price = Convert.ToDecimal(dataItem[0]),
                };
                orderBooks.Buy.Add(buy);
            }
            foreach (List<string> item in response.data.asks)
            {
                string[] dataItem = item.ToArray();
                double size = Convert.ToDouble(dataItem[1].Replace('.', ','));
                double price = Convert.ToDouble(dataItem[0].Replace('.', ','));
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
        public List<TradesResponseModel> GetTrade(string symbol)
        {
            string symbol1 = symbol.Substring(0, symbol.Length - 4);
            string symbolKucoin = symbol1 + "-USDT";
            TradeModel response = services.Trade(symbolKucoin);
            List<TradesResponseModel> trades = new List<TradesResponseModel>();
            foreach (TradesData item in response.data)
            {
                double price = Convert.ToDouble(item.price.Replace('.', ','));
                double size = Convert.ToDouble(item.size.Replace('.', ','));
                double volume = price * size;
                TradesResponseModel trade = new TradesResponseModel()
                {
                    Symbol = symbolKucoin,
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
            string symbol1 = requestModel.symbol.Substring(0, requestModel.symbol.Length - 4);
            string symbolKucoin = symbol1 + "-USDT";
            Tuple<string, string , string> APITuple = KucoinHelper.GetKucoinHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new CreateOrderResponseModel();
            }
            string timestamp = Convert.ToString(DateTimeOffset.Now.ToUnixTimeMilliseconds());
            string method = "POST";
            string endPoint = "/api/v1/orders";
            string clientOid = "177791555";
            string pair = symbolKucoin;
            string side = Enum.GetName(typeof(SIDE), requestModel.side);
            string type = Enum.GetName(typeof(TYPE_TRADE), requestModel.typeTrade);
            string price = requestModel.price;
            string size = requestModel.amount;
            object body = new
            {
                clientOid,
                pair,
                side,
                type,
                price,
                size,

            };
            string data_json = JsonConvert.SerializeObject(body);
            string data = $"{timestamp}{method}{endPoint}{data_json}";
            string signature = GetSignOrPassPhrase(data, secretKey);
            string pass = "Bilgisanet";
            string passPhrase = GetSignOrPassPhrase(pass, secretKey);
            CreateOrderModel response = services.CreateOrder(apiKey, signature, timestamp, passPhrase, data_json);
            CreateOrderResponseModel createOrderResponseModel = new CreateOrderResponseModel()
            {
                Symbol = pair,
                OrderId = Convert.ToString(response.orderId),
                Timestamp = Convert.ToInt64(timestamp)
            };
            return createOrderResponseModel;
        }
        public List<ActiveOrderResponseModel> GetActiveOrder(string symbol)
        {
            Tuple<string, string , string> APITuple = KucoinHelper.GetKucoinHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new List<ActiveOrderResponseModel>();
            }
            string timestamp = Convert.ToString(DateTimeOffset.Now.ToUnixTimeMilliseconds());
            string method = "GET";
            string endPoint = "/api/v1/orders?status=active";
            string data = $"{timestamp}{method}{endPoint}";
            string signature = GetSignOrPassPhrase(data, secretKey);
            string pass = "Bilgisanet";
            string passPhrase = GetSignOrPassPhrase(pass, secretKey);
            ActiveOrderModel response = services.ActiveOrder(apiKey, signature, timestamp, passPhrase);
            List<ActiveOrderResponseModel> activeOrders = new List<ActiveOrderResponseModel>();
            if (response.items != null)
            {
                foreach (Item item in response.items)
                {
                    string status = item.isActive ? "active" : "deal";
                    ActiveOrderResponseModel activeOrder = new ActiveOrderResponseModel()
                    {
                        Symbol = item.symbol,
                        Timestamp = item.createdAt,
                        OrderId = item.id,
                        Price = item.price,
                        Status = status,
                        Type = item.type,
                        Side = item.side,
                    };
                    activeOrders.Add(activeOrder);
                }
                return activeOrders;
            }
            else
            {
                return new List<ActiveOrderResponseModel>();
            }

        }
        public List<BalanceResponseModel> GetBalance()
        {
            Tuple<string, string , string> APITuple = KucoinHelper.GetKucoinHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new List<BalanceResponseModel>();
            }
            string timestamp = Convert.ToString(DateTimeOffset.Now.ToUnixTimeMilliseconds());
            string method = "GET";
            string endPoint = "/api/v1/accounts/ledgers";
            string data = $"{timestamp}{method}{endPoint}";
            string signature = GetSignOrPassPhrase(data, secretKey);
            string pass = "Bilgisanet";
            string passPhrase = GetSignOrPassPhrase(pass, secretKey);
            BalanceModel response = services.Balance(apiKey, signature, timestamp, passPhrase);
            List<BalanceResponseModel> balances = new List<BalanceResponseModel>();
            if (response.items != null)
            {
                foreach (GetWalletItem item in response.items)
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
            else
            {
                return balances;
            }
        }
        public PairList GetPair(string parity)
        {
            SymbolModel ret = services.Parity();
            PairList pairList = new PairList();
            foreach (Data item in ret.data)
            {
                if (item.enableTrading && item.quoteCurrency == "USDT")
                {
                    if(item.symbol == parity)
                    {
                        Pair pair = new Pair();
                        string[] symboList = item.symbol.Split('-');
                        pair.symbol = symboList[0] + symboList[1];
                        pair.baseAsset = item.baseCurrency;
                        pair.quoteAsset = item.quoteCurrency;
                        pair.qtyFormat = item.quoteMinSize;
                        pairList.Pairs.Add(pair);
                    }
                }
            }
            return pairList;
        }

        public List<CancelOrderResponseModel> CancelOrders(string symbol)
        {
            Tuple<string, string, string> APITuple = KucoinHelper.GetKucoinHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new List<CancelOrderResponseModel>();
            }
            string timestamp = Convert.ToString(DateTimeOffset.Now.ToUnixTimeMilliseconds());
            string method = "DELETE";
            string endPoint = $"/api/v1/stop-order/cancel?symbol={symbol}";
            string data = $"{timestamp}{method}{endPoint}";
            string signature = GetSignOrPassPhrase(data, secretKey);
            string pass = "Bilgisanet";
            string passPhrase = GetSignOrPassPhrase(pass, secretKey);
            CancelOrderModel response = services.CancelOrder(apiKey, signature, timestamp, passPhrase , symbol);
            List <CancelOrderResponseModel> cancelOrders = new List<CancelOrderResponseModel>();
            foreach (string item in response.cancelledOrderIds)
            {
                CancelOrderResponseModel cancelOrder = new CancelOrderResponseModel()
                {
                    orderId = item
                };
                cancelOrders.Add(cancelOrder);  
            }
            return cancelOrders;
        }

        public QueryOrderResponseModel QueryOrders(string symbol, string orderId)
        {
            Tuple<string, string, string> APITuple = KucoinHelper.GetKucoinHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new QueryOrderResponseModel();
            }
            string timestamp = Convert.ToString(DateTimeOffset.Now.ToUnixTimeMilliseconds());
            string method = "GET";
            string endPoint = $"/api/v1/orders/{orderId}";
            string data = $"{timestamp}{method}{endPoint}";
            string signature = GetSignOrPassPhrase(data, secretKey);
            string pass = APITuple.Item3;
            string passPhrase = GetSignOrPassPhrase(pass, secretKey);
            QueryOrderModel response = services.QueryOrder(apiKey, signature, timestamp, passPhrase, orderId);
            QueryOrderResponseModel queryOrder = new QueryOrderResponseModel();
            queryOrder.price = response.price;
            queryOrder.symbol = response.symbol; 
            queryOrder.orderId = response.id;   
            queryOrder.type = response.type;
            queryOrder.side = response.side;
            if (response.isActive)
            {
                queryOrder.status = "NEW";
            }
            else
            {
                queryOrder.status = "FILLED";

            }
            return queryOrder;  
        }

        public ComissionFeeResponseModel ComissionFee(string symbol)
        {
            ComissionFeeResponseModel comission = new ComissionFeeResponseModel()
            {
                symbol = symbol,
                makerCommission = "0.1",
                takerCommission = "0.1"
            };
            return comission;
        }
        public string GetSignOrPassPhrase(string data, string secret)
        {
            return CryptographyHelper.GetHMAC256(data, secret, Models.RETURNTYPE.BASE64);
        }
        public DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }
    }
}