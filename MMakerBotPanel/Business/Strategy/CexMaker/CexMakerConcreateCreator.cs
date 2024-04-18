using MMakerBotPanel.Business.Strategy.CexMaker.Model;
using MMakerBotPanel.Business.Strategy.Interfaces;
using MMakerBotPanel.Models;
using System.Collections.Generic;
using System;
using MMakerBotPanel.Models.MakerMarketingModels;
using System.Linq;
using MMakerBotPanel.Database.Context;
using MMakerBotPanel.Models.ChartsModel;
using MMakerBotPanel.Database.Model;
using MMakerBotPanel.WebServices.Interface;
using System.Threading;
using MMakerBotPanel.Models.ApiModels;

namespace MMakerBotPanel.Business.Strategy.CexSpread
{
    public class CexMakerConcreateCreator : FacConcreteCreatorAbstract
    {
        private static CexMakerConcreateCreator _CexSpreadConcreateCreator;

        private static readonly object _lock = new object();

        private CexMakerConcreateCreator()
        {

        }

        public static FacConcreteCreatorAbstract GetCexSpreadConcreateCreator()
        {
            lock (_lock)
            {
                if (_CexSpreadConcreateCreator == null)
                {
                    _CexSpreadConcreateCreator = new CexMakerConcreateCreator();
                }
                return _CexSpreadConcreateCreator;
            }
        }

        public override bool BotStartingMethods(object parameters)
        {
            try
            {
                CexMakerParameterModel cexParameter = (CexMakerParameterModel)parameters;
                CEXEXCHANGE enumExchange = (CEXEXCHANGE)Enum.Parse(typeof(CEXEXCHANGE), cexParameter.exchange);
                (double, double, double) bidAsk = CalculateBidAndAsk(cexParameter.exchange, cexParameter.symbol);
                double spread = CalculateSpread(bidAsk.Item1, bidAsk.Item2);
                ICEXAPIController cEXAPIController = CexExchangeSelect.CexSelect(enumExchange);
                USERRISKTYPE riskLevel;
                using (ModelContext db = new ModelContext())
                {
                    riskLevel = db.Users.FirstOrDefault(m => m.UserID == cexParameter.userID).riskType;
                }
                decimal deposit = (decimal)cexParameter.deposit;
                (double, double, double, double) buyOrSell = CalculateBuySellOrderPrice(cexParameter.exchange, cexParameter.symbol, Convert.ToDouble(deposit), riskLevel, cexParameter.comissionBuy, cexParameter.comissionSell, spread);
                List<CandleStickStockChartDataModel> marketData = CexExchangeSelect.CexSelect(enumExchange).GetChartDataDate(cexParameter.symbol, Models.ChartsModels.Interval.hours4, Models.ChartsModels.CandlestickQuantity.Candle1);
                if (buyOrSell.Item1 != 0 && buyOrSell.Item2 != 0)
                {
                    List<BalanceResponseModel> balances = CexExchangeSelect.CexSelect(enumExchange).GetBalance();
                    string symbol2 = cexParameter.symbol.Replace("USDT", string.Empty);
                    double amountUsdt = 0;
                    double sumUsdt = 0;

                    double comissionSell = (buyOrSell.Item2 / buyOrSell.Item1) * cexParameter.comissionSell;
                    double amount = (buyOrSell.Item2 / buyOrSell.Item1) +(comissionSell * 10);
                    double comissionBuy = (buyOrSell.Item4 / buyOrSell.Item3) * cexParameter.comissionBuy;
                    double buyUsdtAmount = (buyOrSell.Item4 / buyOrSell.Item3) + comissionBuy;

                    if (balances.Count > 0)
                    {
                        foreach (BalanceResponseModel balance in balances)
                        {
                            if (balance.Symbol.Contains(symbol2))
                            {
                                if (Convert.ToDouble(balance.Balance.Replace(".", ",")) <= amount)
                                {
                                    double amountFarki = amount - Convert.ToDouble(balance.Balance.Replace(".", ","));
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
                                    sumUsdt = buyUsdtAmount;
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        //hata
                    }

                    if (sumUsdt == 0)
                    {
                        amountUsdt = amount * Convert.ToDouble(marketData[0].Close);
                        sumUsdt = amountUsdt + (buyUsdtAmount * Convert.ToDouble(marketData[0].Close));
                    }

                    CreateOrderResponseModel getCreateOrder = new CreateOrderResponseModel();
                    if (sumUsdt != 0)
                    {
                        foreach (BalanceResponseModel balance in balances)
                        {
                            if (balance.Symbol.Contains("USDT"))
                            {
                                if (Convert.ToDouble(balance.Balance.Replace(".", ",")) >= sumUsdt)
                                {
                                    if (buyOrSell.Item2 != 0)
                                    {

                                        CreateOrderRequestModel order = new CreateOrderRequestModel();
                                        order.typeTrade = TYPE_TRADE.MARKET;
                                        order.side = SIDE.Buy;
                                        order.amount = amount.ToString(cexParameter.qtyFormat);
                                        order.price = marketData[0].Close.ToString();
                                        order.symbol = cexParameter.symbol;
                                        getCreateOrder = cEXAPIController.CreateSpotOrder(order);
                                        using (ModelContext db = new ModelContext())
                                        {
                                            Dashboard dash = db.Dashboards.FirstOrDefault(m => m.UserID == cexParameter.userID);
                                            if (dash == null)
                                            {
                                                Dashboard dashboard = new Dashboard();
                                                dashboard.TotalOrders = +1;
                                                dashboard.UserID = cexParameter.userID;
                                                db.Dashboards.Add(dashboard);
                                            }
                                            else
                                            {
                                                dash.TotalOrders = +1;
                                            }

                                            db.SaveChanges();
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    double comissionBuySell = 0;
                    double SellAmount = 0;

                    comissionBuySell = (buyOrSell.Item2 / buyOrSell.Item1) * cexParameter.comissionSell;
                    SellAmount = (buyOrSell.Item2 / buyOrSell.Item1) - comissionBuySell;

                    CreateOrderRequestModel sell = new CreateOrderRequestModel();
                    sell.amount = SellAmount.ToString(cexParameter.qtyFormat);
                    sell.typeTrade = TYPE_TRADE.LIMIT;
                    sell.price = Math.Round(buyOrSell.Item1 , 4).ToString();
                    sell.side = SIDE.Sell;
                    sell.symbol = cexParameter.symbol;
                    CreateOrderResponseModel respons = cEXAPIController.CreateSpotOrder(sell);
                    if (respons.OrderId != null && respons.OrderId != "0")
                    {
                        using (ModelContext db = new ModelContext())
                        {
                            Dashboard dash = db.Dashboards.FirstOrDefault(m => m.UserID == cexParameter.userID);
                            if (dash == null)
                            {
                                Dashboard dashboard = new Dashboard();
                                dashboard.TotalOrders = +1;
                                dashboard.UserID = cexParameter.userID;
                                db.Dashboards.Add(dashboard);
                            }
                            else
                            {
                                dash.TotalOrders = +1;
                            }

                            db.SaveChanges();
                        }
                    }
                    Thread.Sleep(500);
                    double comissionBuyOrSell= 0;
                    double BuyAmount = 0;

                    comissionBuyOrSell = (buyOrSell.Item4 / buyOrSell.Item3) * cexParameter.comissionBuy;
                    BuyAmount = (buyOrSell.Item4 / buyOrSell.Item3) + comissionBuyOrSell;
                    CreateOrderRequestModel buy = new CreateOrderRequestModel();
                    buy.amount = BuyAmount.ToString(cexParameter.qtyFormat);
                    buy.typeTrade = TYPE_TRADE.LIMIT;
                    buy.price = Math.Round(buyOrSell.Item3 , 4).ToString();
                    buy.side = SIDE.Buy;
                    buy.symbol = cexParameter.symbol;
                    CreateOrderResponseModel response = cEXAPIController.CreateSpotOrder(buy);
                    if (response.OrderId != null && response.OrderId != "0")
                    {
                        using (ModelContext db = new ModelContext())
                        {
                            Dashboard dash = db.Dashboards.FirstOrDefault(m => m.UserID == cexParameter.userID);
                            if (dash == null)
                            {
                                Dashboard dashboard = new Dashboard();
                                dashboard.TotalOrders = +1;
                                dashboard.UserID = cexParameter.userID;
                                db.Dashboards.Add(dashboard);
                            }
                            else
                            {
                                dash.TotalOrders = +1;
                            }

                            db.SaveChanges();
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return false;
            }
        }

        public override bool BotStoppingMethods(FacIStrategy strategy)
        {
            try
            {
                CexMakerStrategy _stopStrategy = (CexMakerStrategy)strategy;
                CEXEXCHANGE enumExchange = (CEXEXCHANGE)Enum.Parse(typeof(CEXEXCHANGE), _stopStrategy.exchange);
                ICEXAPIController cEXAPIController = CexExchangeSelect.CexSelect(enumExchange);
                List<CancelOrderResponseModel> cancelOrders = cEXAPIController.CancelOrders(_stopStrategy.symbol);
                Thread.Sleep(500);
                List<BalanceResponseModel> balances = cEXAPIController.GetBalance();
                string symbol1 = _stopStrategy.symbol.Replace("USDT", "");
                CreateOrderResponseModel getCreateOrder = new CreateOrderResponseModel();
                foreach (BalanceResponseModel balance in balances)
                {
                    if (balance.Symbol.Contains(symbol1))
                    {
                        CreateOrderRequestModel order = new CreateOrderRequestModel();
                        double balanceDob = Convert.ToDouble(balance.Balance.Replace(".", ","));
                        double comissionSell = balanceDob * _stopStrategy.comissionSelll;
                        order.typeTrade = TYPE_TRADE.MARKET;
                        order.side = SIDE.Sell;
                        order.amount = (balanceDob - comissionSell).ToString(_stopStrategy.qtyFormat);
                        order.symbol = _stopStrategy.symbol;
                        getCreateOrder = cEXAPIController.CreateSpotOrder(order);

                        if (getCreateOrder.OrderId != "0")
                        {
                            using (ModelContext db = new ModelContext())
                            {
                                Dashboard dash = db.Dashboards.FirstOrDefault(m => m.UserID == _stopStrategy.userID);
                                if (dash == null)
                                {
                                    Dashboard dashboard = new Dashboard();
                                    dashboard.TotalOrders = +1;
                                    dashboard.UserID = _stopStrategy.userID;
                                    db.Dashboards.Add(dashboard);
                                }
                                else
                                {
                                    dash.TotalOrders = +1;
                                }

                                db.SaveChanges();
                            }
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                List<CancelOrderResponseModel> cancelOrder = cEXAPIController.CancelOrders(_stopStrategy.symbol);

                return true;
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return false;
            }
        }



        private (double, double, double, double) CalculateBuySellOrderPrice(string exchange, string symbol, double deposit, USERRISKTYPE risk, double comissionBuy, double comissionSell, double spread)
        {
            try
            {
                CEXEXCHANGE enumExchange = (CEXEXCHANGE)Enum.Parse(typeof(CEXEXCHANGE), exchange);
                List<BalanceResponseModel> balances = CexExchangeSelect.CexSelect(enumExchange).GetBalance();

                double usdtBalance = 0.0;
                double sellOrderPrice = 0;
                double buyOrderPrice = 0;
                double sellOrderAmount = 0;
                double buyOrderAmount = 0;
                foreach (BalanceResponseModel balance in balances)
                {
                    if (balance.Symbol.Contains("USDT"))
                    {
                        usdtBalance = Convert.ToDouble(balance.Balance.Replace(".", ","));
                    }
                }
                if (deposit < usdtBalance)
                {

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
                else
                {
                    return (sellOrderPrice, sellOrderAmount, buyOrderPrice, buyOrderAmount);
                }

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