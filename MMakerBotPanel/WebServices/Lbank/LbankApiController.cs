namespace MMakerBotPanel.WebServices.Lbank
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
    using MMakerBotPanel.WebServices.Lbank.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class LbankApiController : ICEXAPIController
    {
        private readonly LbankWebServices services = new LbankWebServices();

        public PairList GetPairList()
        {
            SymbolModel ret = services.Parity();
            PairList pairList = new PairList();
            foreach (string item in ret.data)
            {
                List<string> basequote = item.Split('_').ToList();
                if (basequote.Count > 0 && basequote.Count < 3)
                {
                    if (basequote[1].ToUpper() == "USDT")
                    {
                        Pair pair = new Pair
                        {
                            baseAsset = basequote[0].ToUpper(),
                            quoteAsset = basequote[1].ToUpper(),
                            symbol = basequote[0].ToUpper() + basequote[1].ToUpper()
                        };
                        pairList.Pairs.Add(pair);
                    }
                }
            }
            //  GetTrade();
            //GetOrderBook();
            //GetActiveOrder();
            // GetBalance();
            //CreateSpotOrder();
            return pairList;
        }
        public List<CandleStickStockChartDataModel> GetChartData(string symbol, Interval interval, CandlestickQuantity? candlestickQuantity)
        {
            string symbol1 = symbol.Substring(0, symbol.Length - 4);
            string symbolLbank = symbol1 + "_USDT";
            string time = "";
            int intervalTime = (int)interval;
            int unixTimestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            string candleQty = "";
            int candlestickQty = (int)candlestickQuantity;
            if (interval == Interval.minutes30)
            {
                time = "minute" + intervalTime.ToString();
            }
            else if (interval == Interval.hours2)
            {
                time = "hour1";
            }
            else
            {
                time = "hour" + intervalTime.ToString();
            }

            if (candlestickQuantity == CandlestickQuantity.Candle1000)
            {
                unixTimestamp -= 12956400;
                candleQty = candlestickQty.ToString();
            }
            else if (candlestickQuantity == CandlestickQuantity.Candle200)
            {
                unixTimestamp -= 360000;
                candleQty = candlestickQty.ToString();
            }
            else if (candlestickQuantity == CandlestickQuantity.Candle168)
            {
                unixTimestamp -= 302400;
                candleQty = candlestickQty.ToString();
            }
            else if (candlestickQuantity == CandlestickQuantity.Candle360)
            {
                unixTimestamp -= 1296000;
                candleQty = candlestickQty.ToString();
            }
            else if (candlestickQuantity == CandlestickQuantity.Candle1080 && intervalTime == 2)
            {
                unixTimestamp -= 7776000;
                candleQty = candlestickQty.ToString();
            }



            StockDataModel ret = services.StockData(symbolLbank.ToLower(), time, candleQty, unixTimestamp.ToString());
            List<CandleStickStockChartDataModel> dataList = new List<CandleStickStockChartDataModel>();

            foreach (ChartData item in ret.LbankChartDataModels)
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
            string symbolLbank = symbol1 + "_USDT";
            int unixTimestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            unixTimestamp -= 14400;
            StockDataModel ret = services.StockData(symbolLbank.ToLower(), "hour4", "4", unixTimestamp.ToString());
            List<CandleStickStockChartDataModel> dataList = new List<CandleStickStockChartDataModel>();

            foreach (ChartData item in ret.LbankChartDataModels)
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
            string serverTime = ret.ts.ToString().Substring(0, ret.ts.ToString().Length - 3);
            DateTime dateTime = UnixTimeStampToDateTime(Convert.ToDouble(serverTime));
            CexBotStatusModel gateioStatus = new CexBotStatusModel();
            if (ret.data.status != "0")
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
            string symbolLbank = symbol1 + "_USDT";
            OrderBookModel response = services.OrderBook(symbolLbank.ToLower());
            OrderBookResponseModel orderBooks = new OrderBookResponseModel();
            foreach (List<double> item in response.data.bids)
            {
                double[] dataItem = item.ToArray();
                double amount = dataItem[1];
                double price = dataItem[0];
                double volume = amount * price;
                OrderBookBuy buy = new OrderBookBuy()
                {
                    Volume = volume.ToString().Replace('.', ','),
                    Price = Convert.ToDecimal(dataItem[0]),
                };
                orderBooks.Buy.Add(buy);
            }
            foreach (List<double> item in response.data.asks)
            {
                double[] dataItem = item.ToArray();
                double amount = dataItem[1];
                double price = dataItem[0];
                double volume = amount * price;
                OrderBookSell sell = new OrderBookSell()
                {
                    Volume = volume.ToString().Replace('.', ','),
                    Price = Convert.ToDecimal(dataItem[0]),
                };
                orderBooks.Sell.Add(sell);
            }
            return orderBooks;
        }
        public List<TradesResponseModel> GetTrade(string symbol)
        {
            string symbol1 = symbol.Substring(0, symbol.Length - 4);
            string symbolLbank = symbol1 + "_USDT";
            TradeModel response = services.Trade(symbolLbank.ToLower());
            List<TradesResponseModel> trades = new List<TradesResponseModel>();
            foreach (TradesData item in response.data)
            {
                double volume = item.price * item.amount;

                TradesResponseModel trade = new TradesResponseModel()
                {
                    Symbol = symbolLbank.ToLower(),
                    Price = Convert.ToString(item.price),
                    Volume = Convert.ToString(volume),
                    Timestamp = Convert.ToInt64(item.date_ms),
                    Type = item.type
                };
                trades.Add(trade);
            }
            return trades;
        }
        public CreateOrderResponseModel CreateSpotOrder(CreateOrderRequestModel requestModel)
        {
            string symbol1 = requestModel.symbol.Substring(0, requestModel.symbol.Length - 4);
            string symbolLbank = symbol1 + "_USDT";
            Tuple<string, string, string> APITuple = LBankHelper.GetLBankHelper().GetUserAPIKeys();
            string api_key = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (api_key == "" || secretKey == "")
            {
                return new CreateOrderResponseModel();
            }

            string amount = requestModel.amount;
            string echostr = "JFNCD5D4C8SM44DLD4LSQCMDYWQEA945";
            string price = requestModel.price;
            string pair = symbolLbank.ToLower();
            string signature_method = "HmacSHA256";
            string timestamp = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds).ToString();
            string type = Enum.GetName(typeof(SIDE), requestModel.side);

            string parameterString = "amount=" + amount + "&api_key=" + api_key + "&echostr=" + echostr + "&price=" + price + "&signature_method=" + signature_method + "&symbol=" + pair + "&timestamp=" + timestamp + "&type=" + type;
            //string parameterString = "api_key=" + api_key + "&echostr=" + echostr + "&signature_method=" + signature_method + "&timestamp=" + timestamp;
            string preparedStr = CryptographyHelper.CreateMD5Hash(parameterString).ToUpper();
            string sign = CryptographyHelper.GetHMAC256(preparedStr, secretKey, RETURNTYPE.HEX);
            parameterString += "&sign=" + sign;
            CreateOrderModel response = services.CreateOrder(parameterString);
            CreateOrderResponseModel createOrderResponseModel = new CreateOrderResponseModel()
            {
                Symbol = pair,
                OrderId = Convert.ToString(response.order_id),
                Timestamp = Convert.ToInt64(timestamp)
            };
            return createOrderResponseModel;
        }
        public List<BalanceResponseModel> GetBalance()
        {
            Tuple<string, string, string> APITuple = LBankHelper.GetLBankHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new List<BalanceResponseModel>();
            }
            string echostr = "JFNCD5D4C8SM44DLD4LSQCMDYWQEA945";
            string signature_method = "HmacSHA256";
            string timestamp = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds).ToString();

            string parameterString = "api_key=" + apiKey + "&echostr=" + echostr + "&signature_method=" + signature_method + "&timestamp=" + timestamp;
            string preparedStr = CryptographyHelper.CreateMD5Hash(parameterString).ToUpper();
            string sign = CryptographyHelper.GetHMAC256(preparedStr, secretKey, RETURNTYPE.HEX);
            parameterString += "&sign=" + sign;
            BalanceModel response = services.Balance(parameterString);
            List<BalanceResponseModel> balances = new List<BalanceResponseModel>();
            foreach (SpotBalances item in response.balances)
            {
                BalanceResponseModel balance = new BalanceResponseModel()
                {
                    Symbol = item.coin,
                    Balance = item.assetAmt
                };
                balances.Add(balance);
            }
            return balances;
        }
        public List<ActiveOrderResponseModel> GetActiveOrder(string symbol)
        {
            string symbol1 = symbol.Substring(0, symbol.Length - 4);
            string symbolLbank = symbol1 + "_USDT";
            Tuple<string, string, string> APITuple = LBankHelper.GetLBankHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new List<ActiveOrderResponseModel>();
            }
            string echostr = "JFNCD5D4C8SM44DLD4LSQCMDYWQEA945";
            string signature_method = "HmacSHA256";
            string timestamp = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds).ToString();
            string pair = symbolLbank.ToLower();
            string page_length = "100";
            string current_page = "1";
            string parameterString = "api_key=" + apiKey + "&current_page=" + current_page + "&echostr=" + echostr + "&page_length=" + page_length + "&signature_method=" + signature_method + "&symbol=" + pair + "&timestamp=" + timestamp;
            string preparedStr = CryptographyHelper.CreateMD5Hash(parameterString).ToUpper();
            string sign = CryptographyHelper.GetHMAC256(preparedStr, secretKey, RETURNTYPE.HEX);
            parameterString += "&sign=" + sign;
            ActiveOrderModel response = services.ActiveOrder(parameterString);
            List<ActiveOrderResponseModel> activeOrders = new List<ActiveOrderResponseModel>();
            foreach (ActiveOrders item in response.activeOrders)
            {
                //Enum
                string status = "";
                if (item.status == "-1")
                {
                    status = "Cancelled";
                }
                else if (item.status == "0")
                {
                    status = "Unfilled";
                }
                else if (item.status == "1")
                {
                    status = "Partially filled";
                }
                else if (item.status == "2")
                {
                    status = "Completely filled";
                }
                else if (item.status == "3")
                {
                    status = "Partially filled has been cancelled";
                }
                else if (item.status == "4")
                {
                    status = "Cancellation is being processed";
                }
                ActiveOrderResponseModel activeOrder = new ActiveOrderResponseModel()
                {
                    Symbol = item.symbol,
                    Timestamp = item.time,
                    OrderId = item.orderId,
                    Price = item.price,
                    Side = item.type,
                    Status = status,
                    Type = "limit",
                    Volume = item.executedQty
                };
                activeOrders.Add(activeOrder);
            }
            return activeOrders;
        }
        public DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }

        public PairList GetPair(string parity)
        {
            string assestCode = parity.Substring(0, parity.Length - 4);
            GetPairModel getPairModel = services.GetPair(assestCode.ToLower());
            PairList pair = new PairList();
            pair.Pairs[0].qtyFormat = getPairModel.min.ToString();
            pair.Pairs[0].symbol = parity;
            return pair;
        }

        public List<CancelOrderResponseModel> CancelOrders(string symbol)
        {
            string symbol1 = symbol.Substring(0, symbol.Length - 4);
            string symbolLbank = symbol1 + "_USDT";
            Tuple<string, string, string> APITuple = LBankHelper.GetLBankHelper().GetUserAPIKeys();
            string api_key = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (api_key == "" || secretKey == "")
            {
                return new List<CancelOrderResponseModel>();
            }
            List<ActiveOrderResponseModel> getActiveOrder = GetActiveOrder(symbol);
            List<CancelOrderResponseModel> cancelOrders = new List<CancelOrderResponseModel>();
            if (getActiveOrder.Count() > 0)
            {
                foreach (ActiveOrderResponseModel order in getActiveOrder)
                {
                    string echostr = "JFNCD5D4C8SM44DLD4LSQCMDYWQEA945";
                    string pair = symbolLbank.ToLower();
                    string signature_method = "HmacSHA256";
                    string timestamp = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds).ToString();
                    string order_id = order.OrderId;
                    string parameterString = "api_key=" + api_key + "&echostr=" + echostr + "&order_id=" + order_id + "&signature_method=" + signature_method + "&symbol=" + pair + "&timestamp=" + timestamp;
                    string preparedStr = CryptographyHelper.CreateMD5Hash(parameterString).ToUpper();
                    string sign = CryptographyHelper.GetHMAC256(preparedStr, secretKey, RETURNTYPE.HEX);
                    parameterString += "&sign=" + sign;
                    CancelOrderModel response = services.CancelOrder(parameterString);
                    CancelOrderResponseModel cancelOrder = new CancelOrderResponseModel();
                    cancelOrder.symbol = symbol;
                    cancelOrder.orderId = order_id;
                    cancelOrders.Add(cancelOrder);
                }
            }
            return cancelOrders;

        }

        public QueryOrderResponseModel QueryOrders(string symbol, string orderId)
        {
            string symbol1 = symbol.Substring(0, symbol.Length - 4);
            string symbolLbank = symbol1 + "_USDT";
            Tuple<string, string, string> APITuple = LBankHelper.GetLBankHelper().GetUserAPIKeys();
            string api_key = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (api_key == "" || secretKey == "")
            {
                return new QueryOrderResponseModel();
            }
            string echostr = "JFNCD5D4C8SM44DLD4LSQCMDYWQEA945";
            string pair = symbolLbank.ToLower();
            string signature_method = "HmacSHA256";
            string timestamp = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds).ToString();
            string order_id = orderId;
            string parameterString = "api_key=" + api_key + "&echostr=" + echostr + "&order_id=" + order_id + "&signature_method=" + signature_method + "&symbol=" + pair + "&timestamp=" + timestamp;
            string preparedStr = CryptographyHelper.CreateMD5Hash(parameterString).ToUpper();
            string sign = CryptographyHelper.GetHMAC256(preparedStr, secretKey, RETURNTYPE.HEX);
            parameterString += "&sign=" + sign;
            QueryOrderModel response = services.QueryOrder(parameterString);
            QueryOrderResponseModel queryOrder = new QueryOrderResponseModel();
            queryOrder.price = response.price.ToString();
            queryOrder.qty = response.amount.ToString();
            queryOrder.symbol = response.symbol;
            queryOrder.type = response.type;    
            if(response.status == 1 || response.status == 2 || response.status == 3)
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
                makerCommission = "0.2",
                takerCommission = "0.2"
            };
            return comission;
        }
    }
}