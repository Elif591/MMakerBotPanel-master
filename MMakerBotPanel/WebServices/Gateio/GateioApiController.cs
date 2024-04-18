namespace MMakerBotPanel.WebServices.Gateio
{
    using MMakerBotPanel.Business;
    using MMakerBotPanel.Business.Exchange;
    using MMakerBotPanel.Models;
    using MMakerBotPanel.Models.ApiModels;
    using MMakerBotPanel.Models.CexBotModels;
    using MMakerBotPanel.Models.ChartsModel;
    using MMakerBotPanel.Models.ChartsModels;
    using MMakerBotPanel.Models.MakerMarketingModels;
    using MMakerBotPanel.WebServices.Gateio.Model;
    using MMakerBotPanel.WebServices.Interface;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO.Ports;

    public class GateioApiController : ICEXAPIController
    {
        private readonly GateioWebServices services = new GateioWebServices();

        public PairList GetPairList()
        {
            SymbolModel ret = services.Parity();
            PairList pairList = new PairList();
            foreach (Datum item in ret.data)
            {
                if (item.trade_status == "tradable" && item.quote == "USDT")
                {
                    Pair pair = new Pair
                    {
                        symbol = item.@base + item.quote,
                        baseAsset = item.@base,
                        quoteAsset = item.quote
                    };
                    pairList.Pairs.Add(pair);
                }

            }
            // GetTrade();
            //  GetOrderBook();
            // GetActiveOrder();
            // GetBalance();
            //CreateSpotOrder();
            GetPair("BTCUSDT");
            return pairList;
        }
        public List<CandleStickStockChartDataModel> GetChartData(string symbol, Interval interval, CandlestickQuantity? candlestickQuantity)
        {
            string symbol1 = symbol.Substring(0, symbol.Length - 4);
            string symbolGateio = symbol1 + "_USDT";

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
            StockDataModel ret = services.StockData(symbolGateio, timeIntervalStr, candleQty.ToString());
            List<CandleStickStockChartDataModel> dataList = new List<CandleStickStockChartDataModel>();

            foreach (StockDataDetail item in ret.GateioDataDetailModels)
            {
                CandleStickStockChartDataModel data = new CandleStickStockChartDataModel
                {
                    Date = item.startTime * 1000,
                    Open = item.openPrice,
                    High = item.highestPrice,
                    Low = item.lowestPrice,
                    Close = item.closePrice,
                    Volume = item.tradingVolume
                };
                dataList.Add(data);
            }

            return dataList;
        }
        public List<CandleStickStockChartDataModel> GetChartDataDate(string symbol, Interval interval, CandlestickQuantity? candlestickQuantity)
        {
            string symbol1 = symbol.Substring(0, symbol.Length - 4);
            string symbolGateio = symbol1 + "_USDT";

            StockDataModel ret = services.StockData(symbolGateio, "4h", "1");
            List<CandleStickStockChartDataModel> dataList = new List<CandleStickStockChartDataModel>();

            foreach (StockDataDetail item in ret.GateioDataDetailModels)
            {
                CandleStickStockChartDataModel data = new CandleStickStockChartDataModel
                {
                    Date = item.startTime * 1000,
                    Open = item.openPrice,
                    High = item.highestPrice,
                    Low = item.lowestPrice,
                    Close = item.closePrice,
                    Volume = item.tradingVolume
                };
                dataList.Add(data);
            }

            return dataList;
        }
        public CexBotStatusModel Status()
        {
            StatusModel ret = services.Status();
            string serverTime = ret.server_time.ToString().Substring(0, ret.server_time.ToString().Length - 3);
            DateTime dateTime = UnixTimeStampToDateTime(Convert.ToDouble(serverTime));
            CexBotStatusModel gateioStatus = new CexBotStatusModel();
            gateioStatus.CexBotStatusModels.status = ret.server_time != 0;
            gateioStatus.CexBotStatusModels.date = dateTime;
            return gateioStatus;
        }
        public OrderBookResponseModel GetOrderBook(string symbol)
        {
            string symbol1 = symbol.Substring(0, symbol.Length - 4);
            string symbolGateio = symbol1 + "_USDT";
            OrderBookModel response = services.OrderBook(symbolGateio);
            OrderBookResponseModel orderBook = new OrderBookResponseModel();
            foreach (List<string> item in response.bids)
            {
                string[] dataItem = item.ToArray();
                double amount = Convert.ToDouble(dataItem[1].Replace('.', ','));
                double price = Convert.ToDouble(dataItem[0].Replace('.', ','));
                double volume = amount * price;
                OrderBookBuy buy = new OrderBookBuy()
                {
                    Volume = volume.ToString(),
                    Price = Convert.ToDecimal(dataItem[0]),
                };
                orderBook.Buy.Add(buy);
            }
            foreach (List<string> item in response.asks)
            {
                string[] dataItem = item.ToArray();
                double amount = Convert.ToDouble(dataItem[1].Replace('.', ','));
                double price = Convert.ToDouble(dataItem[0].Replace('.', ','));
                double volume = amount * price;
                OrderBookSell sell = new OrderBookSell()
                {
                    Volume = volume.ToString(),
                    Price = Convert.ToDecimal(dataItem[0]),
                };
                orderBook.Sell.Add(sell);
            }
            return orderBook;
        }
        public List<TradesResponseModel> GetTrade(string symbol)
        {
            string symbol1 = symbol.Substring(0, symbol.Length - 4);
            string symbolGateio = symbol1 + "_USDT";
            TradeModel response = services.Trade(symbolGateio);
            List<TradesResponseModel> trades = new List<TradesResponseModel>();
            foreach (TradesData item in response.trades)
            {
                double price = Convert.ToDouble(item.price.Replace('.', ','));
                double amount = Convert.ToDouble(item.amount.Replace('.', ','));
                double volume = price * amount;

                TradesResponseModel trade = new TradesResponseModel()
                {
                    Symbol = symbolGateio,
                    Price = item.price,
                    Volume = volume.ToString(),
                    Timestamp = Convert.ToInt64(item.create_time),
                    Type = item.side
                };
                trades.Add(trade);
            }

            return trades;

        }
        public CreateOrderResponseModel CreateSpotOrder(CreateOrderRequestModel requestModel)
        {
            string symbol1 = requestModel.symbol.Substring(0, requestModel.symbol.Length - 4);
            string symbolGateio = symbol1 + "_USDT";
            Tuple<string, string, string> APITuple = GateHelper.GetGateHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new CreateOrderResponseModel();
            }
            string currency_pair = symbolGateio;
            string type = Enum.GetName(typeof(TYPE_TRADE), requestModel.typeTrade);
            string account = "spot";
            string side = Enum.GetName(typeof(SIDE), requestModel.side);
            string amount = requestModel.amount;
            string price = requestModel.price;
            string time_in_force = "gtc";
            string iceberg = "0";
            string timestamp = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds).ToString();
            long timeStamp = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds);
            var body = new
            {
                currency_pair,
                type,
                account,
                side,
                iceberg,
                amount,
                price,
                time_in_force,
            };
            string bodyString = JsonConvert.SerializeObject(body);
            string bodyHash = CryptographyHelper.CreateSHA512(bodyString);
            string signBody = "POST\n/api/v4/spot/orders\n\n" + bodyHash + "\n" + timestamp;
            string Sign = GetSign(signBody, secretKey);
            CreateOrderModel response = services.CreateOrder(apiKey, timestamp, Sign, body);
            CreateOrderResponseModel createOrderResponseModel = new CreateOrderResponseModel()
            {
                Symbol = currency_pair,
                OrderId = Convert.ToString(response.id),
                Timestamp = timeStamp
            };
            return createOrderResponseModel;
        }
        public List<BalanceResponseModel> GetBalance()
        {
            Tuple<string, string, string> APITuple = GateHelper.GetGateHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new List<BalanceResponseModel>();
            }
            string timestamp = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds).ToString();
            string body = "";
            string bodyHash = CryptographyHelper.CreateSHA512(body);
            string signBody = "GET\n/api/v4/wallet/total_balance\n\n" + bodyHash + "\n" + timestamp;
            string Sign = GetSign(signBody, secretKey);
            BalanceModel response = services.Balance(apiKey, timestamp, Sign);
            List<BalanceResponseModel> balances = new List<BalanceResponseModel>();

            BalanceResponseModel balance = new BalanceResponseModel()
            {
                Symbol = response.details.spot.currency,
                Balance = response.details.spot.amount
            };
            balances.Add(balance);


            return balances;
        }
        public List<ActiveOrderResponseModel> GetActiveOrder(string symbol)
        {
            Tuple<string, string, string> APITuple = GateHelper.GetGateHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new List<ActiveOrderResponseModel>();
            }
            string timestamp = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds).ToString();
            string body = "";
            string bodyHash = CryptographyHelper.CreateSHA512(body);
            string signBody = "GET\n/api/v4/spot/open_orders\n\n" + bodyHash + "\n" + timestamp;
            string Sign = GetSign(signBody, secretKey);
            ActiveOrderModel response = services.ActiveOrder(apiKey, timestamp, Sign);
            List<ActiveOrderResponseModel> activeOrders = new List<ActiveOrderResponseModel>();
            if (response.orders != null)
            {
                foreach (Order item in response.orders)
                {
                    ActiveOrderResponseModel activeOrder = new ActiveOrderResponseModel()
                    {
                        Symbol = item.currency_pair,
                        Timestamp = Convert.ToInt64(item.create_time),
                        OrderId = item.id,
                        Price = item.price,
                        Status = item.status,
                        Type = item.type,
                        Side = item.side,
                        Volume = item.filled_total
                    };
                    activeOrders.Add(activeOrder);
                }
                return activeOrders;
            }
            else
            {
                return activeOrders;
            }

        }
        public PairList GetPair(string parity)
        {
            string symbol1 = parity.Substring(0, parity.Length - 4);
            string symbolGateio = symbol1 + "_USDT";

            PairModel response = services.ComissionFee(symbolGateio);

            PairList pairList = new PairList();

            if (response.trade_status == "tradable" && response.quote == "USDT")
            {
                Pair pair = new Pair();

                pair.symbol = response.@base + response.quote;
                pair.baseAsset = response.@base;
                pair.quoteAsset = response.quote;
                if(response.min_base_amount != null)
                {
                    pair.qtyFormat = response.min_base_amount;
                }
                else
                {
                    pair.qtyFormat = "0.001";
                }
               
                
                pairList.Pairs.Add(pair);

            }
            return pairList;
        }
        public List<CancelOrderResponseModel> CancelOrders(string symbol)
        {
            string symbol1 = symbol.Substring(0, symbol.Length - 4);
            string symbolGateio = symbol1 + "_USDT";
            Tuple<string, string, string> APITuple = GateHelper.GetGateHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new List<CancelOrderResponseModel>();
            }
            string queryParam = $"currency_pair={symbolGateio}";
            string body = "";
            string timestamp = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds).ToString();
            //string bodyString = JsonConvert.SerializeObject(body);
            string bodyHash = CryptographyHelper.CreateSHA512(body);
            string method = "DELETE";
            string url = "/api/v4/spot/orders";
            string signBody = $"{method}\n{url}\n{queryParam}\n{bodyHash}\n{timestamp}";
            string Sign = GetSign(signBody, secretKey);
            CancelOrderModel response = services.CancelOrders(apiKey, timestamp, Sign, queryParam);
            List<CancelOrderResponseModel> cancelOrderResponseModel = new List<CancelOrderResponseModel>();
            foreach (var item in response.cancelOrders)
            {
                CancelOrderResponseModel createOrderResponse = new CancelOrderResponseModel()
                {
                    symbol = item.currency_pair,
                    orderId = Convert.ToString(item.id),
                    status = item.status
                };
                cancelOrderResponseModel.Add(createOrderResponse);
            }

            return cancelOrderResponseModel;
        }
        public QueryOrderResponseModel QueryOrders(string symbol, string orderId)
        {
            string symbol1 = symbol.Substring(0, symbol.Length - 4);
            string symbolGateio = symbol1 + "_USDT";
            Tuple<string, string, string> APITuple = GateHelper.GetGateHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new QueryOrderResponseModel();
            }
            string queryParam = $"currency_pair={symbolGateio}";
            string body = "";
            string timestamp = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds).ToString();
            //string bodyString = JsonConvert.SerializeObject(body);
            string bodyHash = CryptographyHelper.CreateSHA512(body);
            string method = "GET";
            string url = $"/api/v4/spot/orders/{orderId}";
            string signBody = $"{method}\n{url}\n{queryParam}\n{bodyHash}\n{timestamp}";
            string Sign = GetSign(signBody, secretKey);
            QueryOrderModel response = services.QueryOrder(apiKey, timestamp, Sign, queryParam, orderId);
            QueryOrderResponseModel queryResponse = new QueryOrderResponseModel();

            queryResponse.symbol = response.currency_pair;
            queryResponse.orderId = Convert.ToString(response.id);
            if (response.status == "closed")
            {
                queryResponse.status = "FILLED";
            }

            queryResponse.type = response.type;
            queryResponse.side = response.side;
            queryResponse.qty = response.amount;
            queryResponse.price = Convert.ToString(response.price);
            return queryResponse;
        }
        public ComissionFeeResponseModel ComissionFee(string symbol)
        {
            string symbol1 = symbol.Substring(0, symbol.Length - 4);
            string symbolGateio = symbol1 + "_USDT";

            PairModel response = services.ComissionFee(symbolGateio);

            ComissionFeeResponseModel comission = new ComissionFeeResponseModel()
            {
                symbol = response.id,
                makerCommission = response.fee,
                takerCommission = response.fee
            };
            return comission;
        }
        public string GetSign(string signBody, string secretKey)
        {
            return CryptographyHelper.GetHMAC512(signBody, secretKey, Models.RETURNTYPE.HEX);
        }
        public DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }
    }
}