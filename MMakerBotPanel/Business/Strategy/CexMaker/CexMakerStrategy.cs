namespace MMakerBotPanel.Business.Strategy.CexSpread
{
    using MMakerBotPanel.Business.Services.Models;
    using MMakerBotPanel.Business.Strategy.CexMaker.Model;
    using MMakerBotPanel.Business.Strategy.Interfaces;
    using MMakerBotPanel.Database.Context;
    using MMakerBotPanel.Database.Model;
    using MMakerBotPanel.Models;
    using MMakerBotPanel.Models.ApiModels;
    using MMakerBotPanel.Models.ChartsModel;
    using MMakerBotPanel.Models.MakerMarketingModels;
    using MMakerBotPanel.WebServices.Interface;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CexMakerStrategy : FacStrategyObserver
    {
        public string exchange;
        public string symbol;
        public double comissionBuyy;
        public double depositt;
        public double comissionSelll;
        public string qtyFormat;
        public int userID;
        public double takeProfit;
        public double stopLass;
        private static readonly object _lock = new object();
        public CexMakerStrategy(object parameters)
        {
            CexMakerParameterModel makerParameter = parameters as CexMakerParameterModel;
            workerID = makerParameter.workerID;
            type = WORKERTYPE.AdvancedMMakerBot;
            depositt = makerParameter.deposit;
            exchange = makerParameter.exchange;
            symbol = makerParameter.symbol;
            userID = makerParameter.userID;
            takeProfit = makerParameter.takeProfit;
            stopLass = makerParameter.stopLass;
            comissionBuyy = makerParameter.comissionBuy;
            comissionSelll = makerParameter.comissionSell;
            qtyFormat = makerParameter.qtyFormat;
        }

        public override void Tick()
        {
            lock (_lock)
            {
                try
                {
                    CacheHelper.Add<Int64>((WorkerID.ToString() + "_MakerTick"), Convert.ToInt64((DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds), DateTimeOffset.Now.AddMinutes(10));
                    CEXEXCHANGE enumExchange = (CEXEXCHANGE)Enum.Parse(typeof(CEXEXCHANGE), exchange);
                    ICEXAPIController cEXAPIController = CexExchangeSelect.CexSelect(enumExchange);
                    List<CandleStickStockChartDataModel> marketData = cEXAPIController.GetChartData(symbol, Models.ChartsModels.Interval.hours4, Models.ChartsModels.CandlestickQuantity.Candle1);
                    double marketPrice = 0.0;
                    foreach (var item in marketData)
                    {
                        marketPrice = item.Close;
                    }
                    if (marketPrice >= takeProfit)
                    {
                        using (ModelContext db = new ModelContext())
                        {
                            MakerParameter makerParameter = db.MakerParameters.OrderByDescending(m => m.MakerParameterID).FirstOrDefault(m => m.WorkerID == workerID);
                            makerParameter.makerStatus = BOTSTATUS.takeProfit;
                            db.SaveChanges();
                        }
                        TradingBotCreator.GetTradingBotCreator().StopStrategy(workerID);
                    }
                    if (marketPrice <= stopLass)
                    {
                        using (ModelContext db = new ModelContext())
                        {
                            MakerParameter makerParameter = db.MakerParameters.OrderByDescending(m => m.MakerParameterID).FirstOrDefault(m => m.WorkerID == workerID);
                            makerParameter.makerStatus = BOTSTATUS.stoploss;
                            db.SaveChanges();
                        }
                        TradingBotCreator.GetTradingBotCreator().StopStrategy(workerID);
                    }

                    List<CancelOrderResponseModel> cancelOrders = cEXAPIController.CancelOrders(symbol);
                    if (cancelOrders.Count() > 0)
                    {
                        foreach (var item in cancelOrders)
                        {
                            if (item.orderId != null || item.orderId != "0")
                            {
                                continue;
                            }
                            else
                            {
                                //hata
                            }
                        }

                        (double, double, double) bidAsk = CalculateBidAndAsk(exchange, symbol);
                        double spread = CalculateSpread(bidAsk.Item1, bidAsk.Item2);
                        USERRISKTYPE riskLevel;
                        using (ModelContext db = new ModelContext())
                        {
                            riskLevel = db.Users.FirstOrDefault(m => m.UserID == userID).riskType;
                        }
                        decimal deposit = (decimal)depositt;
                        (double, double, double, double) buyOrSell = CalculateBuySellOrderPrice(exchange, symbol, Convert.ToDouble(deposit), riskLevel, comissionBuyy, comissionSelll, spread);
                        List<CandleStickStockChartDataModel> marketmakerData = CexExchangeSelect.CexSelect(enumExchange).GetChartDataDate(symbol, Models.ChartsModels.Interval.hours4, Models.ChartsModels.CandlestickQuantity.Candle1);
                        if (buyOrSell.Item1 != 0 && buyOrSell.Item2 != 0)
                        {
                            List<BalanceResponseModel> balances = CexExchangeSelect.CexSelect(enumExchange).GetBalance();
                            string symbol2 = symbol.Replace("USDT", string.Empty);
                            double amountUsdt = 0;
                            double sumUsdt = 0;

                            double comissionSell = (buyOrSell.Item2 / buyOrSell.Item1) * comissionSelll;
                            double amount = (buyOrSell.Item2 / buyOrSell.Item1) + comissionSell;
                            double comissionBuy = (buyOrSell.Item4 / buyOrSell.Item3) * comissionBuyy;
                            double buyUsdtAmount = (buyOrSell.Item4 / buyOrSell.Item3) + comissionBuy;
                            double amountFarki = 0;
                            if (balances.Count > 0)
                            {
                                foreach (BalanceResponseModel balance in balances)
                                {
                                    if (balance.Symbol.Contains(symbol2))
                                    {
                                        if (Convert.ToDouble(balance.Balance.Replace(".", ",")) <= amount)
                                        {
                                            amountFarki = amount - Convert.ToDouble(balance.Balance.Replace(".", ","));
                                            if (amountFarki < 1)
                                            {
                                                amount = 1;
                                            }
                                            else
                                            {
                                                amount = amountFarki;
                                            }
                                            amountUsdt = amountFarki * Convert.ToDouble(marketData[0].Close);
                                            sumUsdt = amountUsdt + (buyUsdtAmount * Convert.ToDouble(marketData[0].Close));
                                            break;
                                        }
                                        else
                                        {
                                           
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                TradingBotCreator.GetTradingBotCreator().StopStrategy(workerID);
                            }

                            //if (sumUsdt == 0)
                            //{
                            //    amountUsdt = amount * Convert.ToDouble(marketData[0].Close);
                            //    sumUsdt = amountUsdt + (buyUsdtAmount * Convert.ToDouble(marketData[0].Close));
                            //}
                            CreateOrderResponseModel getCreateOrder = new CreateOrderResponseModel();
                            if (sumUsdt != 0)
                            {
                                foreach (BalanceResponseModel balance in balances)
                                {
                                    if (balance.Symbol.Contains("USDT"))
                                    {

                                        if (buyOrSell.Item2 != 0)
                                        {
                                            CreateOrderRequestModel order = new CreateOrderRequestModel();
                                            order.typeTrade = TYPE_TRADE.MARKET;
                                            order.side = SIDE.Buy;
                                            order.amount = amount.ToString(qtyFormat);
                                            order.price = marketData[0].Close.ToString();
                                            order.symbol = symbol;
                                            getCreateOrder = cEXAPIController.CreateSpotOrder(order);
                                            if (getCreateOrder.OrderId != null && getCreateOrder.OrderId != "0")
                                            {
                                                using (ModelContext db = new ModelContext())
                                                {
                                                    Dashboard dash = db.Dashboards.FirstOrDefault(m => m.UserID == userID);
                                                    if (dash == null)
                                                    {
                                                        Dashboard dashboard = new Dashboard();
                                                        dashboard.TotalOrders = +1;
                                                        dashboard.UserID = userID;
                                                        db.Dashboards.Add(dashboard);
                                                    }
                                                    else
                                                    {
                                                        dash.TotalOrders = +1;
                                                    }

                                                    db.SaveChanges();
                                                    break;
                                                }

                                            }
                                            else
                                            {
                                                TradingBotCreator.GetTradingBotCreator().StopStrategy(workerID);

                                            }
                                        }
                                    }
                                }
                            }

                            double comissionBuyOrSell = 0;
                            double BuyAmount = 0;

                            comissionBuyOrSell = (buyOrSell.Item4 / buyOrSell.Item3) * comissionBuy;
                            BuyAmount = (buyOrSell.Item4 / buyOrSell.Item3) + comissionBuyOrSell;
                            CreateOrderRequestModel buy = new CreateOrderRequestModel();
                            buy.amount = BuyAmount.ToString(qtyFormat);
                            buy.typeTrade = TYPE_TRADE.LIMIT;
                            buy.price = Math.Round(buyOrSell.Item3, 4).ToString();
                            buy.side = SIDE.Buy;
                            buy.symbol = symbol;
                            CreateOrderResponseModel response = cEXAPIController.CreateSpotOrder(buy);
                            if (response.OrderId != null && response.OrderId != "0")
                            {
                                using (ModelContext db = new ModelContext())
                                {
                                    Dashboard dash = db.Dashboards.FirstOrDefault(m => m.UserID == userID);
                                    if (dash == null)
                                    {
                                        Dashboard dashboard = new Dashboard();
                                        dashboard.TotalOrders = +1;
                                        dashboard.UserID = userID;
                                        db.Dashboards.Add(dashboard);
                                    }
                                    else
                                    {
                                        dash.TotalOrders = +1;
                                    }

                                    db.SaveChanges();
                                }
                            }
                            else
                            {
                                //hata
                            }

                            double comissionBuySell = 0;
                            double SellAmount = 0;

                            comissionBuySell = (buyOrSell.Item2 / buyOrSell.Item1) * comissionSell;
                            SellAmount = (buyOrSell.Item2 / buyOrSell.Item1) - comissionBuySell;
                            CreateOrderRequestModel sell = new CreateOrderRequestModel();
                            sell.amount = SellAmount.ToString(qtyFormat);
                            sell.typeTrade = TYPE_TRADE.LIMIT;
                            sell.price = Math.Round(buyOrSell.Item1, 4).ToString();
                            sell.side = SIDE.Sell;
                            sell.symbol = symbol;
                            CreateOrderResponseModel respons = cEXAPIController.CreateSpotOrder(sell);
                            if (respons.OrderId != null && respons.OrderId != "0")
                            {
                                using (ModelContext db = new ModelContext())
                                {
                                    Dashboard dash = db.Dashboards.FirstOrDefault(m => m.UserID == userID);
                                    if (dash == null)
                                    {
                                        Dashboard dashboard = new Dashboard();
                                        dashboard.TotalOrders = +1;
                                        dashboard.UserID = userID;
                                        db.Dashboards.Add(dashboard);
                                    }
                                    else
                                    {
                                        dash.TotalOrders = +1;
                                    }

                                    db.SaveChanges();
                                }
                            }
                            else
                            {
                                //hata
                            }
                        }
                    }
                    CacheHelper.Remove((WorkerID.ToString() + "_MakerTick"));
                }
                catch (Exception ex)
                {
                    Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                }
            }
        }

        public override void OnNext(ObserverMessage value)
        {
            try
            {
                Int64 cache = CacheHelper.Get<Int64>(WorkerID.ToString());
                Int64 cacheTick = CacheHelper.Get<Int64>((WorkerID.ToString() + "_MakerTick"));
                if (cache == 0 && cacheTick == 0)
                {
                    Logger.GetLogger().LogTick();
                    Tick();
                    CacheHelper.Add<Int64>(WorkerID.ToString(), Convert.ToInt64((DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds), DateTimeOffset.Now.AddSeconds(3));
                }

            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
            }
        }


        private (double, double, double, double) CalculateBuySellOrderPrice(string exchange, string symbol, double deposit, USERRISKTYPE risk, double comissionBuy, double comissionSell, double spread)
        {
            try
            {
                CEXEXCHANGE enumExchange = (CEXEXCHANGE)Enum.Parse(typeof(CEXEXCHANGE), exchange);
                List<BalanceResponseModel> balances = CexExchangeSelect.CexSelect(enumExchange).GetBalance();
                double sellOrderPrice = 0;
                double buyOrderPrice = 0;
                double sellOrderAmount = 0;
                double buyOrderAmount = 0;


                List<CandleStickStockChartDataModel> marketData = CexExchangeSelect.CexSelect(enumExchange).GetChartDataDate(symbol, Models.ChartsModels.Interval.hours4, Models.ChartsModels.CandlestickQuantity.Candle1);

                switch (risk)
                {
                    case USERRISKTYPE.LowRiskProfile:
                        sellOrderPrice = (marketData[0].Close + 0.001) + spread;
                        buyOrderPrice = (marketData[0].Close - 0.001) - spread;
                        break;
                    case USERRISKTYPE.MediumRiskProfile:
                        sellOrderPrice = (marketData[0].Close + 0.0001) + spread;
                        buyOrderPrice = (marketData[0].Close - 0.0001) - spread;
                        break;
                    case USERRISKTYPE.HighRiskProfile:
                        sellOrderPrice = (marketData[0].Close + 0.00001) + spread;
                        buyOrderPrice = (marketData[0].Close - 0.00001) - spread;
                        break;
                    default:
                        sellOrderPrice = 0;
                        buyOrderPrice = 0;
                        break;

                }

                sellOrderAmount = deposit / 3;
                buyOrderAmount = deposit / 3;

                return (sellOrderPrice, sellOrderAmount, buyOrderPrice, buyOrderAmount);


            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return (0, 0, 0, 0);

            }
        }

        private (double, double, double) CalculateBidAndAsk(string exchange, string parity)
        {
            double bid = 0;
            double ask = 0;
            double volume = 0;
            try
            {
                CEXEXCHANGE enumExchange = (CEXEXCHANGE)Enum.Parse(typeof(CEXEXCHANGE), exchange);
                OrderBookResponseModel orderBook = CexExchangeSelect.CexSelect(enumExchange).GetOrderBook(parity);
                if (orderBook.Buy.Count > 0)
                {
                    bid = Convert.ToDouble(orderBook.Buy.Max(item => item.Volume).Replace(".", ","));
                }

                if (orderBook.Sell.Count > 0)
                {
                    ask = Convert.ToDouble(orderBook.Sell.Min(item => item.Volume).Replace(".", ","));
                }


                return (bid, ask, volume);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return (bid, ask, volume);
            }
        }

        private double SpreadRate(double bid, double ask)
        {
            double spreadOrani = (ask - bid) / bid * 10000;
            return spreadOrani;

        }

        private double CalculateSpread(double bid, double ask)
        {
            double spread = 0;
            double spreadRate = SpreadRate(bid, ask);
            try
            {
                spread = (ask - bid) * spreadRate;
                return spread;
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return spread;
            }

        }

    }
}