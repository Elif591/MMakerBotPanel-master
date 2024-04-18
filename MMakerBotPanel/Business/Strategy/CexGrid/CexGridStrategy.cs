using MMakerBotPanel.Business.Services.Models;
using MMakerBotPanel.Business.Strategy.CexGrid.Model;
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


namespace MMakerBotPanel.Business.Strategy.CexGrid
{
    public class CexGridStrategy : FacStrategyObserver
    {
        public string exchange;
        public string parity;
        private List<GridLevels> gridLevels;
        private CexGridParameterModel cexParameter;
        public double comissionBuy;
        public double comissionSell;
        public string qtyFormat;
        public int userID;

        private static readonly object _lock = new object();

        public CexGridStrategy(object parameters)
        {
            cexParameter = (CexGridParameterModel)parameters;
            workerID = cexParameter.workerID;
            type = WORKERTYPE.CexGridWorker;
            exchange = cexParameter.exchange;
            parity = cexParameter.symbol;
            gridLevels = cexParameter.gridLevel;
            comissionBuy = cexParameter.comissionBuy;
            comissionSell = cexParameter.comissionSell;
            qtyFormat = cexParameter.qtyFormat;
            userID = cexParameter.userID;
        }

        public override void Tick()
        {
            lock (_lock)
            {
                try
                {
                    CacheHelper.Add<Int64>((WorkerID.ToString() + "_Tick"), Convert.ToInt64((DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds), DateTimeOffset.Now.AddMinutes(10));
                    CEXEXCHANGE enumExchange = (CEXEXCHANGE)Enum.Parse(typeof(CEXEXCHANGE), exchange);
                    ICEXAPIController cEXAPIController = CexExchangeSelect.CexSelect(enumExchange);
                    List<CandleStickStockChartDataModel> marketData = cEXAPIController.GetChartData(parity, Models.ChartsModels.Interval.hours4, Models.ChartsModels.CandlestickQuantity.Candle1);
                    double marketPrice = 0.0;
                    foreach (var item in marketData)
                    {
                        marketPrice = item.Close;
                    }
                    if (marketPrice >= cexParameter.takeProfit )
                    {
                        using(ModelContext db = new ModelContext())
                        {
                            GridParameter gridParameter = db.GridParameters.OrderByDescending(m => m.GridParameterID).FirstOrDefault(m => m.WorkerID == workerID);
                            gridParameter.gridStatus = BOTSTATUS.takeProfit;
                            db.SaveChanges();
                        }
                        TradingBotCreator.GetTradingBotCreator().StopStrategy(workerID);
                    }
                    if(marketPrice <= cexParameter.stopLass)
                    {
                        using (ModelContext db = new ModelContext())
                        {
                            GridParameter gridParameter = db.GridParameters.OrderByDescending(m => m.GridParameterID).FirstOrDefault(m => m.WorkerID == workerID);
                            gridParameter.gridStatus = BOTSTATUS.stoploss;
                            db.SaveChanges();
                        }
                        TradingBotCreator.GetTradingBotCreator().StopStrategy(workerID);
                    }

                    gridLevels = gridLevels.OrderByDescending(m => m.price).ToList();

                    for (int i = 0; i < gridLevels.Count(); i++)
                    {
                        if (gridLevels[i].orderId != null)
                        {

                            QueryOrderResponseModel order = cEXAPIController.QueryOrders(parity, gridLevels[i].orderId);
                            if(order == null)
                            {
                                break;
                            }

                            Logger.GetLogger().LogApi(LOGAPITYPE.Binance, "queryOrder", order.orderId, "line:70");
                            if (order.status == "FILLED")
                            {
                                CreateOrderRequestModel newOrder = new CreateOrderRequestModel();
                                if (qtyFormat != null)
                                {
                                    string[] qtyFormatter = qtyFormat.Split('1');
                                    qtyFormat = qtyFormatter[0] + "1";
                                    qtyFormat = qtyFormat.Replace("1", "0");
                                }

                                if (order.side == "SELL")
                                {
                                    int prevIndex = i + 1;
                                    if (prevIndex > gridLevels.Count())
                                    {
                                        break;
                                    }
                                    double qtyDob = Convert.ToDouble(gridLevels[prevIndex].amount.Replace(".", ","));
                                    double comission2 = qtyDob * comissionBuy;
                                    newOrder.amount = (qtyDob - comission2).ToString(qtyFormat);
                                    newOrder.price = gridLevels[prevIndex].price;
                                    newOrder.side = SIDE.Buy;
                                    newOrder.symbol = parity;
                                    newOrder.typeTrade = TYPE_TRADE.LIMIT;
                                    CreateOrderResponseModel createOrder = cEXAPIController.CreateSpotOrder(newOrder);

                                    if (createOrder.OrderId != "0")
                                    {
                                        Logger.GetLogger().LogApi(LOGAPITYPE.Binance, "createOder", createOrder.OrderId, "line:98");
                                        using(ModelContext db = new ModelContext())
                                        {
                                            Dashboard dash = db.Dashboards.FirstOrDefault(m => m.UserID == userID);
                                            if( dash == null )
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
                                        gridLevels[prevIndex].orderId = createOrder.OrderId;
                                        gridLevels[i].orderId = null;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    int nextIndex = i - 1;
                                    if (nextIndex < 0)
                                    {
                                        break;
                                    }
                                    double qtyDob = Convert.ToDouble(gridLevels[nextIndex].amount.Replace(".", ","));
                                    double comission2 = qtyDob * comissionSell;
                                    newOrder.amount = (qtyDob - comission2).ToString(qtyFormat);
                                    newOrder.price = gridLevels[nextIndex].price;
                                    newOrder.side = SIDE.Sell;
                                    newOrder.symbol = parity;
                                    newOrder.typeTrade = TYPE_TRADE.LIMIT;
                                    CreateOrderResponseModel createOrder = cEXAPIController.CreateSpotOrder(newOrder);

                                    if (createOrder.OrderId != "0")
                                    {
                                        Logger.GetLogger().LogApi(LOGAPITYPE.Binance, "createOder", createOrder.OrderId, "line:125");
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
                                        gridLevels[nextIndex].orderId = createOrder.OrderId;
                                        gridLevels[i].orderId = null;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    CacheHelper.Remove((WorkerID.ToString() + "_Tick"));
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
                Int64 cacheTick = CacheHelper.Get<Int64>((WorkerID.ToString() + "_Tick"));
                if (cache == 0 && cacheTick == 0)
                {
                    Logger.GetLogger().LogTick();
                    Tick();
                    CacheHelper.Add<Int64>(WorkerID.ToString(), Convert.ToInt64((DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds), DateTimeOffset.Now.AddSeconds(5));
                }
              
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
            }
        }
    }
}