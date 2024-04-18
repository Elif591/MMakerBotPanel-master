namespace MMakerBotPanel.WebServices.Dextrade
{
    using MMakerBotPanel.Business;
    using MMakerBotPanel.Business.Exchange;
    using MMakerBotPanel.Models;
    using MMakerBotPanel.Models.ApiModels;
    using MMakerBotPanel.Models.CexBotModels;
    using MMakerBotPanel.Models.ChartsModel;
    using MMakerBotPanel.Models.ChartsModels;
    using MMakerBotPanel.Models.MakerMarketingModels;
    using MMakerBotPanel.WebServices.Dextrade.Model;
    using MMakerBotPanel.WebServices.Interface;
    using System;
    using System.Collections.Generic;

    public class DextradeApiController : ICEXAPIController
    {
        private readonly DexTradeWebServices services = new DexTradeWebServices();

        public PairList GetPairList()
        {
            SymbolModel ret = services.Parity();
            PairList pairList = new PairList();
            foreach (Symboldata item in ret.data)
            {
                if (item.quote == "USDT")
                {
                    Pair pair = new Pair
                    {
                        Id = item.id,
                        symbol = item.pair,
                        baseAsset = item.@base,
                        quoteAsset = item.quote
                    };
                    pairList.Pairs.Add(pair);
                }

            }
            // GetTrade();
            //GetOrderBook();
            //GetBalance();
            // GetActiveOrder();
            // CreateSpotOrder();

            return pairList;
        }
        public List<CandleStickStockChartDataModel> GetChartData(string symbol, Interval interval, CandlestickQuantity? candlestickQuantity)
        {
            string time = "60";
            int candleStickQty = 0;
            if (candlestickQuantity == CandlestickQuantity.Candle1000)
            {
                candleStickQty = 4000;
            }
            else if (candlestickQuantity == CandlestickQuantity.Candle1)
            {
                candleStickQty = 2;
            }
            else if (candlestickQuantity == CandlestickQuantity.Candle200)
            {
                candleStickQty = 200;
            }
            else if (candlestickQuantity == CandlestickQuantity.Candle168)
            {
                candleStickQty = 168;
            }
            else if (candlestickQuantity == CandlestickQuantity.Candle360)
            {
                candleStickQty = 360;
            }
            else if (candlestickQuantity == CandlestickQuantity.Candle1080)
            {
                candleStickQty = 2160;
            }

            StockDataModel ret = services.StockData(symbol, time, candleStickQty.ToString());
            List<CandleStickStockChartDataModel> dataList = new List<CandleStickStockChartDataModel>();
            List<CandleStickStockChartDataModel> dataDexList = new List<CandleStickStockChartDataModel>();
            int sayac = 0;
            foreach (ChartData item in ret.Dex_TradeChartDataModels)
            {
                CandleStickStockChartDataModel data = new CandleStickStockChartDataModel
                {
                    Date = item.time * 1000,
                    Open = item.open / 100000000,
                    High = item.high / 100000000,
                    Low = item.low / 100000000,
                    Close = item.close / 100000000,
                    Volume = item.volume / 100000000
                };
                dataList.Add(data);
                sayac++;
                if (sayac == 4)
                {
                    CandleStickStockChartDataModel dataDex = new CandleStickStockChartDataModel
                    {
                        Open = dataList[3].Open,
                        Date = dataList[3].Date,
                        Close = dataList[0].Close
                    };
                    double high = dataList[0].High;
                    for (int i = 0; i < 4; i++)
                    {
                        if (high < dataList[i].High)
                        { high = dataList[i].High; }
                    }
                    dataDex.High = high;
                    double low = dataList[0].Low;
                    for (int i = 0; i < 4; i++)
                    {
                        if (low > dataList[i].Low)
                        { low = dataList[i].Low; }
                    }
                    dataDex.Low = low;
                    for (int i = 0; i < 4; ++i)
                    {
                        dataDex.Volume += dataList[i].Volume;
                    }

                    dataDexList.Add(dataDex);
                    sayac = 0;
                    for (int i = 0; i < dataList.Count - 1; ++i)
                    {
                        _ = dataList.Remove(dataList[i]);
                    }
                    int count = dataList.Count;
                }

            }
            dataDexList.Reverse();
            return dataDexList;
        }
        public List<CandleStickStockChartDataModel> GetChartDataDate(string symbol, Interval interval, CandlestickQuantity? candlestickQuantity)
        {
            StockDataModel ret = services.StockData(symbol, "60", "2");
            List<CandleStickStockChartDataModel> dataList = new List<CandleStickStockChartDataModel>();
            List<CandleStickStockChartDataModel> dataDexList = new List<CandleStickStockChartDataModel>();
            int sayac = 0;
            foreach (ChartData item in ret.Dex_TradeChartDataModels)
            {
                CandleStickStockChartDataModel data = new CandleStickStockChartDataModel
                {
                    Date = item.time * 1000,
                    Open = item.open / 100000000,
                    High = item.high / 100000000,
                    Low = item.low / 100000000,
                    Close = item.close / 100000000,
                    Volume = item.volume / 100000000
                };
                dataList.Add(data);
                sayac++;
                if (sayac == 4)
                {
                    CandleStickStockChartDataModel dataDex = new CandleStickStockChartDataModel
                    {
                        Open = dataList[2].Open,
                        Date = dataList[0].Date,
                        Close = dataList[0].Close
                    };
                    double high = dataList[0].High;
                    for (int i = 0; i < 4; i++)
                    {
                        if (high < dataList[i].High)
                        {
                            high = dataList[i].High;
                        }
                    }
                    dataDex.High = high;
                    double low = dataList[0].Low;
                    for (int i = 0; i < 4; i++)
                    {
                        if (low > dataList[i].Low)
                        {
                            low = dataList[i].Low;
                        }
                    }
                    dataDex.Low = low;
                    for (int i = 0; i < 4; ++i)
                    {
                        dataDex.Volume += dataList[i].Volume;
                    }

                    dataDexList.Add(dataDex);
                    sayac = 0;


                    for (int i = 0; i < dataList.Count - 1; ++i)
                    {
                        _ = dataList.Remove(dataList[i]);
                    }
                    int count = dataList.Count;
                }
            }
            return dataDexList;
        }
        public CexBotStatusModel Status()
        {
            StatusModel ret = services.Status();
            CexBotStatusModel alterStatus = new CexBotStatusModel();
            if (ret.status)
            {
                DateTime dateTime = UnixTimeStampToDateTime(Convert.ToDouble(ret.data[0].timestamp));
                alterStatus.CexBotStatusModels.status = true;
                alterStatus.CexBotStatusModels.date = dateTime;
            }
            else
            {
                alterStatus.CexBotStatusModels.status = false;
            }

            return alterStatus;
        }
        public OrderBookResponseModel GetOrderBook(string symbol)
        {
            OrderBookModel response = services.OrderBook(symbol);
            OrderBookResponseModel orderBooks = new OrderBookResponseModel();
            foreach (Buy item in response.data.buy)
            {
                OrderBookBuy buy = new OrderBookBuy()
                {
                    Volume = Convert.ToString(item.volume),
                    Price = Convert.ToDecimal(item.rate)
                };
                orderBooks.Buy.Add(buy);
            }
            return orderBooks;
        }
        public List<TradesResponseModel> GetTrade(string symbol)
        {
            TradeModel response = services.Trade(symbol);
            List<TradesResponseModel> trades = new List<TradesResponseModel>();
            foreach (TradesData item in response.data)
            {
                TradesResponseModel trade = new TradesResponseModel()
                {
                    Symbol = symbol,
                    Price = Convert.ToString(item.price),
                    Volume = Convert.ToString(item.volume),
                    Timestamp = item.timestamp,
                    Type = item.type,
                };
                trades.Add(trade);
            }
            return trades;
        }
        public CreateOrderResponseModel CreateSpotOrder(CreateOrderRequestModel requestModel)
        {
            Tuple<string, string, string> APITuple = DextradeHelper.GetDextradeHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new CreateOrderResponseModel();
            }
            string type_trade = Convert.ToString(Convert.ToInt32(requestModel.typeTrade));
            string type = Convert.ToString(Convert.ToInt32(requestModel.side));
            string rate = requestModel.price;
            string volume = requestModel.amount;
            string pair = requestModel.symbol;
            string request_id = Convert.ToString(Convert.ToInt64(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds));
            string parameters = pair + rate + request_id + type + type_trade + volume + secretKey;
            object body = new
            {
                type_trade,
                type,
                rate,
                volume,
                pair,
                request_id,
            };
            string sign = GetSign(parameters);
            long timestamp = Convert.ToInt64(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds);
            CreateOrderModel response = services.CreateOrder(sign, apiKey, body);
            if (response.data != null)
            {
                CreateOrderResponseModel createOrderResponseModel = new CreateOrderResponseModel()
                {
                    Symbol = pair,
                    OrderId = Convert.ToString(response.data.id),
                    Timestamp = timestamp
                };
                return createOrderResponseModel;
            }
            else
            {
                return new CreateOrderResponseModel();
            }


        }
        public List<ActiveOrderResponseModel> GetActiveOrder(string symbol)
        {
            Tuple<string, string, string> APITuple = DextradeHelper.GetDextradeHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new List<ActiveOrderResponseModel>();
            }
            string request_id = Convert.ToString(Convert.ToInt64(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds));
            string parameters = request_id + secretKey;
            object body = new
            {
                request_id
            };
            string sign = GetSign(parameters);
            ActiveOrderModel response = services.ActiveOrder(sign, apiKey, body);
            List<ActiveOrderResponseModel> activeOrders = new List<ActiveOrderResponseModel>();
            foreach (ActiveOrderList item in response.data.list)
            {
                string side;
                string status = "";
                string type = "";
                //Enum 
                side = item.type == 0 ? "Buy" : "Sell";
                if (item.status == 0)
                {
                    status = "process";
                }
                else if (item.status == 1)
                {
                    status = "added to book";
                }
                else if (item.status == 2)
                {
                    status = "done";
                }
                else if (item.status == 3)
                {
                    status = "canceled";
                }
                if (item.type_trade == 0)
                {
                    type = "Limit";
                }
                else if (item.type_trade == 1)
                {
                    type = "Market";
                }
                ActiveOrderResponseModel activeOrder = new ActiveOrderResponseModel()
                {
                    Symbol = item.pair,
                    OrderId = Convert.ToString(item.id),
                    Volume = Convert.ToString(item.volume),
                    Timestamp = item.time_create,
                    Price = Convert.ToString(item.price),
                    Type = type,
                    Side = side,
                    Status = status,
                };
                activeOrders.Add(activeOrder);
            }
            return activeOrders;
        }
        public List<BalanceResponseModel> GetBalance()
        {
            Tuple<string, string, string> APITuple = DextradeHelper.GetDextradeHelper().GetUserAPIKeys();
            string apiKey = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (apiKey == "" || secretKey == "")
            {
                return new List<BalanceResponseModel>();
            }
            string request_id = Convert.ToString(Convert.ToInt64(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds));
            string parameters = request_id + secretKey;
            object body = new
            {
                request_id
            };
            string sign = GetSign(parameters);
            BalanceModel response = services.Balance(sign, apiKey, body);
            List<BalanceResponseModel> balances = new List<BalanceResponseModel>();
            if(response.data.list == null)
            {
                return balances;
            }
            foreach (GetWalletList item in response.data.list)
            {
                BalanceResponseModel balance = new BalanceResponseModel()
                {
                    Balance = Convert.ToString(item.balance),
                    Symbol = item.currency.iso3
                };
                balances.Add(balance);
            }
            return balances;
        }

        public PairList GetPair(string parity)
        {
            PairList pairList = new PairList();
            SymbolModel ret = services.Parity();

            foreach (Symboldata item in ret.data)
            {
                if (item.quote == "USDT")
                {
                    if (item.pair == parity)
                    {
                        Pair pair = new Pair
                        {
                            Id = item.id,
                            symbol = item.pair,
                            baseAsset = item.@base,
                            quoteAsset = item.quote,
                            qtyFormat = "0.01"
                        };
                        pairList.Pairs.Add(pair);
                    }
                }
            }
            return pairList;
        }

        public List<CancelOrderResponseModel> CancelOrders(string symbol)
        {
            List<ActiveOrderResponseModel> getActiveOrders = GetActiveOrder(symbol);
            List<CancelOrderResponseModel> cancelOrder = new List<CancelOrderResponseModel>();
            if (getActiveOrders.Count > 0)
            {
                foreach (var item in getActiveOrders)
                {
                    string orderId = item.OrderId;
                    Tuple<string, string, string> APITuple = DextradeHelper.GetDextradeHelper().GetUserAPIKeys();
                    string login_token = APITuple.Item1;
                    string secretKey = APITuple.Item2;
                    if (login_token == "" || secretKey == "")
                    {
                        return new List<CancelOrderResponseModel>();
                    }
                    long request_id = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds);
                    string signatureText = request_id.ToString() + secretKey;
                    string signature = GetSign(signatureText);
                    object body = new
                    {
                        request_id,
                        orderId
                    };
                    CancelOrderModel Cancelorders = services.CancelOrder(login_token, signature, body);

                    if (Cancelorders.status)
                    {
                        CancelOrderResponseModel cancel = new CancelOrderResponseModel()
                        {
                            status = "true",
                            symbol = symbol,

                        };
                        cancelOrder.Add(cancel);
                    }
                    else if (!Cancelorders.status && Cancelorders.error == "Order not found")
                    {
                        CancelOrderResponseModel cancel = new CancelOrderResponseModel()
                        {
                            status = "true",
                            symbol = symbol,

                        };
                        cancelOrder.Add(cancel);
                    }
                }
            }
            return cancelOrder;
        }

        public QueryOrderResponseModel QueryOrders(string symbol, string orderId)
        {
            Tuple<string, string, string> APITuple = DextradeHelper.GetDextradeHelper().GetUserAPIKeys();
            string login_token = APITuple.Item1;
            string secretKey = APITuple.Item2;
            if (login_token == "" || secretKey == "")
            {
                return new QueryOrderResponseModel();
            }
            long request_id = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds);
            string signatureText = request_id.ToString() + secretKey;
            string signature = GetSign(signatureText);
            object body = new
            {
                request_id,
                orderId
            };
            QueryOrderModel response = services.QueryOrder(login_token, signature, body);
            QueryOrderResponseModel queryOrder = new QueryOrderResponseModel()
            {
                symbol = symbol,
                orderId = orderId,
                status = response.status.ToString(),
                type = response.data.type.ToString(),
                side = response.data.type_trade.ToString(),
                qty = response.data.volume.ToString(),
                price = response.data.price.ToString(),
            };
            return queryOrder;
        }

        public ComissionFeeResponseModel ComissionFee(string symbol)
        {
            ComissionFeeResponseModel comissionFeeResponseModel = new ComissionFeeResponseModel()
            {
                symbol = symbol,
                makerCommission = "0.1",
                takerCommission = "0.1"
            };
            return comissionFeeResponseModel;
        }
        public string GetSign(string parameters)
        {
            return CryptographyHelper.CreateSHA256(parameters, RETURNTYPE.HEX);
        }
        public DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }

    }
}