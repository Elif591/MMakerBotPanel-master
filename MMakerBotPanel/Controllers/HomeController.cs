namespace MMakerBotPanel.Controllers
{
    using MMakerBotPanel.Business;
    using MMakerBotPanel.Business.Services;
    using MMakerBotPanel.Database.Context;
    using MMakerBotPanel.Database.Model;
    using MMakerBotPanel.Models;
    using MMakerBotPanel.Models.ChartsModel;
    using MMakerBotPanel.Models.MakerMarketingModels;
    using MMakerBotPanel.WebServices.Bitget.Model;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using System.Web.Mvc;

    [Authorize]
    public class HomeController : Controller
    {
        ModelContext db = new ModelContext();


        [Authorize(Roles = "Member")]
        public ActionResult Index()
        {
            try
            {
                Logger.GetLogger().LogUser(LOGTYPE.INFO, 0);
                return View();
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "BadRequest");
            }

        }

        [Authorize(Roles = "Admin")]
        public ActionResult AdminIndex()
        {
            try
            {
                Logger.GetLogger().LogUser(LOGTYPE.INFO, 0);
                return View();
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "BadRequest");
            }

        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public JsonResult GetWorkerInfo()
        {
            try
            {
                int userId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, userId);
                List<Worker> workers = db.Workers.Where(m => m.UserID == userId).ToList();

                return Json(workers.Select(x => new
                {
                    x.WorkerID,
                    x.WorkerName,
                    EndingDate = x.EndingDate.ToString("dd:MM:yyyy"),
                    x.WorkerState,
                }), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetTotalOrders()
        {
            try
            {
                int userId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, userId);
                Dashboard userDash = db.Dashboards.FirstOrDefault(m => m.UserID == userId);
                if (userDash != null)
                {
                    return Json(userDash.TotalOrders, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }


            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public JsonResult GetActiveOrders()
        {
            try
            {
                int userId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, userId);
                List<Worker> userWorkers = db.Workers.Where(m => m.UserID == userId).ToList();
                Dashboard dashboard = db.Dashboards.FirstOrDefault(m => m.UserID == userId);
                Dashboard dash = new Dashboard();
                foreach (Worker worker in userWorkers)
                {
                    if (worker.WorkerState == WORKERSTATE.Working)
                    {
                        GridParameter grid = db.GridParameters.Where(m => m.WorkerID == worker.WorkerID).OrderByDescending(m => m.GridParameterID).FirstOrDefault();
                        List<ActiveOrderResponseModel> activeOrders = CexExchangeSelect.CexSelect(grid.Exchange).GetActiveOrder(grid.Parity);
                        if (dashboard != null)
                        {
                            dashboard.ActiveOrders += activeOrders.Count();

                        }
                        else
                        {
                            dash.UserID = userId;
                            dash.ActiveOrders += activeOrders.Count();
                            db.Dashboards.Add(dashboard);
                        }
                    }
                }
                db.SaveChanges();
                if (dashboard != null)
                {
                    return Json(dashboard.ActiveOrders, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(dash.ActiveOrders, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetUsdtBalaance()
        {
            try
            {
                int userId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, userId);

                List<ExchangeApi> exchangeApis = db.ExchangeApis.Where(m => m.UserID == userId).ToList();
                List<ExchangeApi> userExchanges = new List<ExchangeApi>();
                foreach (ExchangeApi exchangeApi in exchangeApis)
                {
                    if (exchangeApi.ExchangeApiKey == EXCHANGEAPIKEY.ApiKey && exchangeApi.Value != null)
                    {
                        if (exchangeApis.Any(e => e.ExchangeApiKey == EXCHANGEAPIKEY.SecretKey && e.Value != null))
                        {
                            userExchanges.Add(exchangeApi);
                        }
                    }
                }
                List<BalanceResponseModel> balance = new List<BalanceResponseModel>();
                decimal totalBalance = 0;
                List<GetBalance> balances = new List<GetBalance>();
                foreach (ExchangeApi exchangeApi in userExchanges)
                {
                    balance = CexExchangeSelect.CexSelect(exchangeApi.Exchange).GetBalance();
                    foreach (BalanceResponseModel item in balance)
                    {
                        if (item.Symbol.Contains("USDT"))
                        {
                            GetBalance exchangeBalance = new GetBalance();
                            totalBalance += decimal.Parse(item.Balance.Replace(".", ","));
                            exchangeBalance.Exchange = exchangeApi.Exchange.ToString();
                            decimal formatted = decimal.Parse(item.Balance.Replace(".", ","));
                            exchangeBalance.Balance = formatted.ToString("0.000");
                            balances.Add(exchangeBalance);
                        }
                    }
                }
                string total = totalBalance.ToString("0.000");
                return Json(balances.Select(x => new
                {
                    x.Exchange,
                    x.Balance,
                    total
                }), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }
        }
        public class GetBalance
        {
            public string Exchange { get; set; }
            public string Balance { get; set; }
        }

        [HttpGet]
        public JsonResult AdminNewMessage()
        {
            try
            {
                int userId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, userId);
                List<Ticket> tickets = db.Tickets.Where(m => m.UserID == userId && m.TicketStatusEnumType == TICKETSTATUS.Answered).ToList();
                List<NewMessageModel> messages = new List<NewMessageModel>();
                foreach (Ticket ticket in tickets)
                {
                    var userMessages = new Dictionary<int, NewMessageModel>();

                    foreach (var item in ticket.TicketDetails)
                    {
                        if (userId != item.UserID)
                        {
                            NewMessageModel message = new NewMessageModel();
                            message.Date = item.Date.ToString("MM/dd/yyyy HH:mm:ss");
                            message.Message = item.Message;
                            if (userMessages.TryGetValue(item.UserID, out var existingMessage))
                            {
                                if (string.Compare(message.Date, existingMessage.Date) > 0)
                                {
                                    userMessages[item.UserID] = message;
                                }
                            }
                            else
                            {
                                userMessages[item.UserID] = message;
                            }
                        }
                    }
                    messages.AddRange(userMessages.Values);
                }
                return Json(messages, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }
        }

        public class NewMessageModel
        {
            public string Date { get; set; }
            public string Message { get; set; }
        }

        [HttpGet]
        public JsonResult GetServicesControl()
        {
            try
            {
                int userId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, userId);
                List<ServicesControlModel> serviceControl = new List<ServicesControlModel>();

                long paymentControlService = CacheHelper.Get<long>(CACHEKEYENUMTYPE.PaymentControlService_LastActivity.ToString());

                ServicesControlModel service = new ServicesControlModel()
                {
                    ServiceName = "Payment Control Service",
                    Status = paymentControlService > 0
                };
                serviceControl.Add(service);

                long workerSubscription = CacheHelper.Get<long>(CACHEKEYENUMTYPE.WorkerSubscriptionService_LastActivity.ToString());
                ServicesControlModel serviceWorker = new ServicesControlModel()
                {
                    ServiceName = "Worker Subscription Service",
                    Status = workerSubscription > 0
                };
                serviceControl.Add(serviceWorker);

                long workerWorking = CacheHelper.Get<long>(CACHEKEYENUMTYPE.WorkerWorkingControlService_LastActivity.ToString());
                ServicesControlModel serviceWorking = new ServicesControlModel()
                {
                    ServiceName = "Worker Working Service",
                    Status = workerWorking > 0
                };
                serviceControl.Add(serviceWorking);
                long taskSchaduler = CacheHelper.Get<long>(CACHEKEYENUMTYPE.TaskSchedulerService_LastActivity.ToString());
                ServicesControlModel serviceTask = new ServicesControlModel()
                {
                    ServiceName = "Task Schaduler Service",
                    Status = taskSchaduler > 0
                };
                serviceControl.Add(serviceTask);
                return Json(serviceControl, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }
        }

        public class ServicesControlModel
        {
            public string ServiceName { get; set; }
            public bool Status { get; set; }
        }

        [HttpGet]

        public JsonResult GetWorkerStartStopInfo()
        {
            try
            {
                int userId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, userId);
                List<Worker> workers = db.Workers.Where(m => m.UserID == userId).ToList();
                List<WorkerInfo> workerInfos = new List<WorkerInfo>();
                foreach (Worker worker in workers)
                {
                    WorkerInfo workerInfo = new WorkerInfo();
                    workerInfo.WorkerName = worker.WorkerName;
                    if (worker.WorkerState == WORKERSTATE.Working)
                    {
                        workerInfo.WorkStatus = true;
                    }
                    else
                    {
                        workerInfo.WorkStatus = false;
                    }
                    workerInfos.Add(workerInfo);
                }
                return Json(workerInfos, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }
        }

        public class WorkerInfo
        {
            public string WorkerName { get; set; }
            public bool WorkStatus { get; set; }

        }

        [HttpGet]

        public JsonResult GetTakeProfitAndStoplossInfo()
        {
            try
            {
                int userId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, userId);

                List<Worker> workers = db.Workers.Where(m => m.UserID == userId).ToList();
                List<WorkerTakeProfitAndStopLossInfo> infos = new List<WorkerTakeProfitAndStopLossInfo>();
                foreach (Worker worker in workers)
                {
                    WorkerTakeProfitAndStopLossInfo info = new WorkerTakeProfitAndStopLossInfo();
                    GridParameter gridParameter = db.GridParameters.OrderByDescending(m => m.GridParameterID).FirstOrDefault(m => m.WorkerID == worker.WorkerID);
                    if (gridParameter != null)
                    {
                        if (gridParameter.gridStatus == BOTSTATUS.takeProfit)
                        {
                            info.WorkerName = worker.WorkerName;
                            info.WorkStatus = "Take Profit";
                            infos.Add(info);
                        }
                        if (gridParameter.gridStatus == BOTSTATUS.stoploss)
                        {
                            info.WorkerName = worker.WorkerName;
                            info.WorkStatus = "Stop Loss";
                            infos.Add(info);
                        }
                    }
                }
                return Json(infos, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }
        }
        public class WorkerTakeProfitAndStopLossInfo
        {
            public string WorkerName { get; set; }
            public string WorkStatus { get; set; }

        }
        [HttpGet]
        public JsonResult UnrealizedOrdersUsdt()
        {
            try
            {
                int userId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, userId);
                List<Worker> workers = db.Workers.Where(m => m.UserID == userId).ToList();
                double usdt = 0;
                foreach (Worker worker in workers)
                {
                    GridParameter gridParameter = db.GridParameters.OrderByDescending(m => m.GridParameterID).FirstOrDefault(m => m.WorkerID == worker.WorkerID);
                    string symbol = "";
                    CEXEXCHANGE exchange = CEXEXCHANGE.Binance;
                    if (gridParameter != null)
                    {
                         symbol = gridParameter.Parity;
                         exchange = gridParameter.Exchange;
                    }
                    else
                    {
                        return Json(0, JsonRequestBehavior.AllowGet);
                    }
                 
                    
                    List<ActiveOrderResponseModel> activeOrders = CexExchangeSelect.CexSelect(exchange).GetActiveOrder(symbol);
                    List<CandleStickStockChartDataModel> marketData = CexExchangeSelect.CexSelect(exchange).GetChartDataDate(symbol, Models.ChartsModels.Interval.hours4, Models.ChartsModels.CandlestickQuantity.Candle1);
                    if (activeOrders.Count > 0)
                    {
                        foreach (ActiveOrderResponseModel item in activeOrders)
                        {
                            if(item.Status != "FILLED")
                            {
                                usdt += Convert.ToDouble(item.Volume.Replace("." , ",")) * marketData[0].Close;
                            }
                        }
                    }
                }

                return Json(usdt, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]

        public JsonResult GetChartData()
        {
            try
            {
                DateTime startDate = DateTime.Now.AddDays(-6);

                int userId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, userId);
                List<ProfitUSDT> usdtBalances = db.ProfitUSDTs.Where(m => m.UserID == userId).ToList();
                Dictionary<DateTime, ProfitUSDT> existingBalances = usdtBalances.ToDictionary(x => x.Date, x => x);
                List<ProfitUSDT> paddedBalances = new List<ProfitUSDT>();

                for (int i = 0; i < 7; i++)
                {
                    DateTime currentDate = startDate.AddDays(i);
                    if (existingBalances.TryGetValue(currentDate, out ProfitUSDT balance))
                    {
                        paddedBalances.Add(balance);
                    }
                    else
                    {
                        paddedBalances.Add(new ProfitUSDT { Date = currentDate, UsdtBalance = 0 });
                    }
                }
                foreach (var item in paddedBalances)
                {
                    foreach (var item2 in existingBalances)
                    {
                        if(item.Date.Day == item2.Key.Day)
                        {
                            item.UsdtBalance = item2.Value.UsdtBalance; 
                        }
                    }
                }
                var last7DaysData = paddedBalances.OrderBy(x => x.Date.DayOfWeek == DayOfWeek.Monday ? 0 : (int)x.Date.DayOfWeek)
                                  .Select((x, index) => new
                                  {
                                      Date = x.Date.ToString("ddd", CultureInfo.GetCultureInfo("en-US")),
                                      x.UsdtBalance,
                                      profit = (index > 0 && index < paddedBalances.Count) ? x.UsdtBalance - paddedBalances[index].UsdtBalance : 0,
                                  })
                                  .ToList();

                return Json(last7DaysData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetWorkerExpiredInfo()
        {
            try
            {
                int userId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, userId);

                List<Worker> workers = db.Workers.Where(m => m.UserID ==  userId).ToList();
                Dictionary<string, WORKERSTATE> workerStatuses = new Dictionary<string, WORKERSTATE>();
                foreach (var item in workers)
                {
                   if(item.WorkerState == WORKERSTATE.Expired)
                    {
                        workerStatuses[item.WorkerName] = item.WorkerState;
                    }
                }
                foreach (var kvp in workerStatuses)
                {
                    string workerName = kvp.Key;
                    WORKERSTATE workerState = kvp.Value;
                }

                return Json(workerStatuses, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }
        }


    }

}