namespace MMakerBotPanel.Business.Strategy.CexGrid
{
    using MMakerBotPanel.Business.Strategy.CexGrid.Model;
    using MMakerBotPanel.Business.Strategy.Interfaces;
    using MMakerBotPanel.Models.MakerMarketingModels;
    using MMakerBotPanel.Models;
    using System.Collections.Generic;
    using MMakerBotPanel.Models.ChartsModel;
    using MMakerBotPanel.WebServices.Interface;
    using System.Linq;
    using MMakerBotPanel.Models.CexBotModels;
    using System;
    using MMakerBotPanel.Models.ApiModels;
    using System.Threading;
    using MMakerBotPanel.Database.Context;
    using MMakerBotPanel.Database.Model;

    public class CexGridConcreateCreator : FacConcreteCreatorAbstract
    {
        private static CexGridConcreateCreator _CexGridConcreateCreator;

        private static readonly object _lock = new object();

        private CexGridConcreateCreator()
        {

        }

        public static FacConcreteCreatorAbstract GetCexGridConcreateCreator()
        {
            lock (_lock)
            {
                if (_CexGridConcreateCreator == null)
                {
                    _CexGridConcreateCreator = new CexGridConcreateCreator();
                }
                return _CexGridConcreateCreator;
            }
        }

        public override bool BotStartingMethods(object parameters)
        {
            try
            {
                CexGridParameterModel cexParameter = (CexGridParameterModel)parameters;
                CEXEXCHANGE enumExchange = (CEXEXCHANGE)Enum.Parse(typeof(CEXEXCHANGE), cexParameter.exchange);
                ICEXAPIController cEXAPIController = CexExchangeSelect.CexSelect(enumExchange);
                List<BalanceResponseModel> balances = new List<BalanceResponseModel>();
                balances = cEXAPIController.GetBalance();
                PairList pair = cEXAPIController.GetPair(cexParameter.symbol);
                string qtyFormat = "";
                if (pair != null)
                {
                    qtyFormat = pair.Pairs[0].qtyFormat;
                    string[] qtyFormatter = qtyFormat.Split('1');
                    qtyFormat = qtyFormatter[0] + "1";
                    qtyFormat = qtyFormat.Replace("1", "0");
                }
                cexParameter.qtyFormat = qtyFormat; 

                List<decimal> gridPrices = CalculateGridLevels(cexParameter.maxPrice, cexParameter.minPrice, cexParameter.gridCount);
                decimal marketPrice = GetMarketPrice(enumExchange, cexParameter.symbol);
                List<CreateOrderRequestModel> buySellPoints = CalculateBuySellPoints(gridPrices, marketPrice);
                ComissionFeeResponseModel comission = cEXAPIController.ComissionFee(cexParameter.symbol);
                cexParameter.comissionBuy = Convert.ToDouble(comission.makerCommission.Replace("." , ","));
                cexParameter.comissionSell = Convert.ToDouble(comission.takerCommission.Replace("." , ","));
                double gridPerAmount = CalculatePointPerAmount(cexParameter.deposit, cexParameter.gridCount);
                double amount = 0;
                double buyUsdtAmount = 0;
                foreach (CreateOrderRequestModel point in buySellPoints)
                {
                    if (point.side == SIDE.Sell)
                    {
                        double comissionFee = (gridPerAmount / Convert.ToDouble(point.price.Replace(".", ","))) * cexParameter.comissionSell;
                        double price = Convert.ToDouble(point.price.Replace(".", ","));
                        amount += (gridPerAmount / price) + comissionFee;

                    }

                }
                double comission2 = gridPerAmount * cexParameter.comissionBuy;
                buyUsdtAmount = buySellPoints.Where(m => m.side == SIDE.Buy).Count() * (gridPerAmount + comission2);
                string symbol = cexParameter.symbol.Replace("USDT", string.Empty);
                double amountUsdt = 0;
                double sumUsdt = 0;
                if(balances.Count > 0)
                {
                    foreach (BalanceResponseModel balance in balances)
                    {
                        if (balance.Symbol.Contains(symbol))
                        {
                            if (Convert.ToDouble(balance.Balance.Replace(".", ",")) <= amount)
                            {
                                double amountFarki = amount - Convert.ToDouble(balance.Balance.Replace(".", ","));
                                if (amountFarki < 1)
                                {
                                    amountFarki = 0;
                                }
                                else
                                {
                                    amount = amountFarki;
                                }

                                amountUsdt = amountFarki * Convert.ToDouble(marketPrice);
                                sumUsdt = amountUsdt + buyUsdtAmount;
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
                    amountUsdt = amount * Convert.ToDouble(marketPrice);
                    sumUsdt = amountUsdt + buyUsdtAmount;
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
                                if (amount != 0)
                                {
                                    CreateOrderRequestModel order = new CreateOrderRequestModel();
                                    order.typeTrade = TYPE_TRADE.MARKET;
                                    order.side = SIDE.Buy;
                                    order.amount = amount.ToString(qtyFormat);
                                    order.price = marketPrice.ToString();
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

                    if (getCreateOrder.OrderId != "0")
                    {
                        foreach (BalanceResponseModel balance in balances)
                        {
                            if (balance.Symbol.Contains(symbol))
                            {
                                if (Convert.ToDouble(balance.Balance.Replace(".", ",")) >= amount)
                                {
                                    break;
                                }
                                else
                                {
                                    //hata
                                }
                            }

                            if (balance.Symbol.Contains("USDT"))
                            {
                                if (Convert.ToDouble(balance.Balance.Replace(".", ",")) >= buyUsdtAmount)
                                {
                                    break;
                                }
                                else
                                {
                                    //hata
                                }
                            }
                        }
                    }
                    else
                    {
                        //hata
                    }
                }
                else
                {
                    //hata//hata
                }
                List<CreateOrderResponseModel> res = new List<CreateOrderResponseModel>();
                if (cexParameter.gridLevel == null)
                {
                    cexParameter.gridLevel = new List<GridLevels>();
                }

                foreach (CreateOrderRequestModel point in buySellPoints)
                {
                    GridLevels gridLevel = new GridLevels();
                    double comissionBuySell = 0;
                    double pointAmount = 0;
                    if (point.side == SIDE.Buy)
                    {
                        comissionBuySell = (gridPerAmount / Convert.ToDouble(point.price.Replace(".", ","))) * cexParameter.comissionBuy;
                        pointAmount = (gridPerAmount / Convert.ToDouble(point.price.Replace(".", ","))) + comissionBuySell;
                    }
                    else
                    {
                        comissionBuySell = (gridPerAmount / Convert.ToDouble(point.price.Replace(".", ","))) * cexParameter.comissionSell;
                        pointAmount = (gridPerAmount / Convert.ToDouble(point.price.Replace(".", ","))) - comissionBuySell;
                    }

                    point.amount = (pointAmount).ToString(qtyFormat);
                    point.typeTrade = TYPE_TRADE.LIMIT;
                    point.symbol = cexParameter.symbol;
                    CreateOrderResponseModel response = cEXAPIController.CreateSpotOrder(point);
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
                    Thread.Sleep(500);
                    if (response.OrderId != null && response.OrderId != "0")
                    {
                        gridLevel.orderId = response.OrderId;
                    }
                    gridLevel.price = point.price;
                    gridLevel.amount = point.amount;
                    gridLevel.symbol = point.symbol;
                    gridLevel.side = point.side;
                    gridLevel.typeTrade = point.typeTrade;
                    cexParameter.gridLevel.Add(gridLevel);
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return false;
            }
        }

        public override bool BotStoppingMethods(FacIStrategy facStrategyObserver)
        {
            try
            {
                CexGridStrategy _stopStrategy = (CexGridStrategy)facStrategyObserver;
                CEXEXCHANGE enumExchange = (CEXEXCHANGE)Enum.Parse(typeof(CEXEXCHANGE), _stopStrategy.exchange);
                ICEXAPIController cEXAPIController = CexExchangeSelect.CexSelect(enumExchange);
                List<CancelOrderResponseModel> cancelOrders = cEXAPIController.CancelOrders(_stopStrategy.parity);
                ComissionFeeResponseModel comission = cEXAPIController.ComissionFee(_stopStrategy.parity);
                Thread.Sleep(500);
                List<BalanceResponseModel> balances = cEXAPIController.GetBalance();
                string symbol1 = _stopStrategy.parity.Replace("USDT", "");
                CreateOrderResponseModel getCreateOrder = new CreateOrderResponseModel();
                PairList pair = cEXAPIController.GetPair(_stopStrategy.parity);
                string qtyFormat = "";
                if (pair != null)
                {
                    qtyFormat = pair.Pairs[0].qtyFormat;
                    string[] qtyFormatter = qtyFormat.Split('1');
                    qtyFormat = qtyFormatter[0] + "1";
                    qtyFormat = qtyFormat.Replace("1", "0");
                }
                foreach (BalanceResponseModel balance in balances)
                {
                    if (balance.Symbol.Contains(symbol1))
                    {
                        CreateOrderRequestModel order = new CreateOrderRequestModel();
                        double balanceDob = Convert.ToDouble(balance.Balance.Replace(".", ","));
                        double comissionSell = balanceDob * Convert.ToDouble(comission.takerCommission.Replace(".", ","));
                        order.typeTrade = TYPE_TRADE.MARKET;
                        order.side = SIDE.Sell;
                        order.amount = (balanceDob - comissionSell).ToString(qtyFormat);
                        order.symbol = _stopStrategy.parity;
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

                return true;
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return false;
            }
        }



        private List<decimal> CalculateGridLevels(decimal maxPrice, decimal minPrice, int gridCount)
        {
            List<decimal> gridLevels = new List<decimal>();
            try
            {
                decimal PriceRange = maxPrice - minPrice;
                decimal stepSize = PriceRange / (gridCount + 1);
                for (int i = 0; i <= gridCount; i++)
                {
                    decimal gridLevel = (stepSize * i) + minPrice;
                    gridLevels.Add(decimal.Parse(gridLevel.ToString("0.####")));
                }
                return gridLevels;
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return gridLevels;
            }

        }
        private List<CreateOrderRequestModel> CalculateBuySellPoints(List<decimal> gridLevels, decimal marketPrice)
        {
            List<CreateOrderRequestModel> buySellPoints = new List<CreateOrderRequestModel>();
            try
            {
                decimal closestLargerGridLevel = decimal.MaxValue;

                foreach (decimal gridLevel in gridLevels)
                {
                    if (gridLevel > marketPrice && gridLevel < closestLargerGridLevel)
                    {
                        closestLargerGridLevel = gridLevel;
                    }
                }

                foreach (decimal gridLevel in gridLevels)
                {
                    CreateOrderRequestModel buySellPoint = new CreateOrderRequestModel();
                    if (gridLevel == closestLargerGridLevel)
                    {
                        buySellPoint.price = gridLevel.ToString();
                        buySellPoint.side = null;
                    }
                    else if (gridLevel < marketPrice)
                    {
                        decimal buyPoint = gridLevel;
                        buySellPoint.price = buyPoint.ToString();
                        buySellPoint.side = SIDE.Buy;
                    }
                    else
                    {
                        decimal sellPoint = gridLevel;
                        buySellPoint.price = sellPoint.ToString();
                        buySellPoint.side = SIDE.Sell;
                    }
                    buySellPoints.Add(buySellPoint);
                }
                return buySellPoints;
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return buySellPoints;
            }

        }
        private decimal GetMarketPrice(CEXEXCHANGE exchange, string parity)
        {

            decimal marketPrice = 0;
            try
            {
                ICEXAPIController cEXAPIController = CexExchangeSelect.CexSelect(exchange);
                List<CandleStickStockChartDataModel> marketData = new List<CandleStickStockChartDataModel>();
                marketData = cEXAPIController.GetChartData(parity, Models.ChartsModels.Interval.hours4, Models.ChartsModels.CandlestickQuantity.Candle1);

                if (marketData.Count > 0)
                {
                    marketPrice = new decimal(marketData.First().Close);
                }
                return marketPrice;
            }
            catch (Exception ex)
            {

                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return marketPrice;
            }


        }
        private double CalculatePointPerAmount(double amountDeposit, int gridCount)
        {
            double pointPerAmount = 0.0;
            try
            {
                 pointPerAmount = amountDeposit / (gridCount + 1);
                return pointPerAmount;
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return pointPerAmount;
            }

        }

    }
}