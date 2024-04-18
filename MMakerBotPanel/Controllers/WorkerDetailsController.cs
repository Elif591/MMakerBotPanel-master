namespace MMakerBotPanel.Controllers
{
    using MMakerBotPanel.Business;
    using MMakerBotPanel.Database.Context;
    using MMakerBotPanel.Database.Model;
    using MMakerBotPanel.Models;
    using MMakerBotPanel.Models.ChartsModel;
    using MMakerBotPanel.WebServices.Interface;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using System.Reflection;
    using System.ComponentModel;
    using MMakerBotPanel.Models.MakerMarketingModels;
    using MMakerBotPanel.Business.Strategy;
    using MMakerBotPanel.Business.Strategy.CexGrid.Model;
    using System.Globalization;
    using MMakerBotPanel.Business.Strategy.CexMaker.Model;
    using MMakerBotPanel.Models.ApiModels;
    using MMakerBotPanel.Models.CexBotModels;

    public class WorkerDetailsController : Controller
    {
        private readonly ModelContext db = new ModelContext();
        public ActionResult Index()
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                IQueryable<WorkerDetail> workerDetails = db.WorkerDetails.Include(w => w.Worker);
                return View(workerDetails.ToList());
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "BadRequest");
            }

        }
        public ActionResult Details(int? id)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                WorkerDetail workerDetail = db.WorkerDetails.Find(id);
                return workerDetail == null ? HttpNotFound() : (ActionResult)View(workerDetail);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "BadRequest");
            }

        }

        public ActionResult Create()
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                ViewBag.WorkerID = new SelectList(db.Workers, "WorkerID", "WorkerName");
                return View();
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "BadRequest");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WorkerDetayID,ExchangeType,CexExchange,DexExchange,Primary,Secondary,StopLossPrice,TakeProfitPrice,MaxPrice,MinPrice,GridCount,WorkerID")] WorkerDetail workerDetail)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                if (ModelState.IsValid)
                {
                    _ = db.WorkerDetails.Add(workerDetail);
                    _ = db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.WorkerID = new SelectList(db.Workers, "WorkerID", "WorkerName", workerDetail.Worker.WorkerID);
                return View(workerDetail);

            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "BadRequest");
            }

        }

        // GET: WorkerDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                WorkerDetail workerDetail = db.WorkerDetails.Find(id);
                if (workerDetail == null)
                {
                    return HttpNotFound();
                }
                ViewBag.WorkerID = new SelectList(db.Workers, "WorkerID", "WorkerName", workerDetail.Worker.WorkerID);
                return View(workerDetail);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "BadRequest");
            }

        }

        // POST: WorkerDetails/Edit/5
        // Aşırı gönderim saldırılarından korunmak için bağlamak istediğiniz belirli özellikleri etkinleştirin. 
        // Daha fazla bilgi için bkz. https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "WorkerDetayID,ExchangeType,CexExchange,DexExchange,Primary,Secondary,StopLossPrice,TakeProfitPrice,MaxPrice,MinPrice,GridCount,WorkerID")] WorkerDetail workerDetail)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                if (ModelState.IsValid)
                {
                    db.Entry(workerDetail).State = EntityState.Modified;
                    _ = db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.WorkerID = new SelectList(db.Workers, "WorkerID", "WorkerName", workerDetail.Worker.WorkerID);
                return View(workerDetail);

            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "BadRequest");
            }

        }

        // GET: WorkerDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                WorkerDetail workerDetail = db.WorkerDetails.Find(id);
                return workerDetail == null ? HttpNotFound() : (ActionResult)View(workerDetail);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "BadRequest");
            }

        }

        // POST: WorkerDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                WorkerDetail workerDetail = db.WorkerDetails.Find(id);
                _ = db.WorkerDetails.Remove(workerDetail);
                _ = db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "BadRequest");
            }

        }

        [HttpGet]
        public JsonResult GetEnumTimeInterval()
        {
            try
            {
                User user = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name);
                Logger.GetLogger().LogUser(LOGTYPE.INFO, user.UserID);
                var timeIntervalValues = Enum.GetValues(typeof(TimeInterval)).Cast<TimeInterval>()
                                                                                 .Select(x => new
                                                                                 {
                                                                                     Key = x,
                                                                                     Value = GetEnumDescription(x)
                                                                                 })
                                                                                 .ToList();

                return Json(timeIntervalValues, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }
        }

        static string GetEnumDescription(Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            if (field != null)
            {
                DescriptionAttribute attribute = field.GetCustomAttribute<DescriptionAttribute>();
                if (attribute != null)
                {
                    return attribute.Description;
                }
            }

            return value.ToString();
        }

        [HttpGet]
        public JsonResult GetRiskLevel()
        {
            try
            {
                User user = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name);
                Logger.GetLogger().LogUser(LOGTYPE.INFO, user.UserID);
                USERRISKTYPE riskLevel = db.Users.FirstOrDefault(m => m.UserID == user.UserID).riskType;
                return Json((RiskLevel)riskLevel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult SaveMakerParameter(int workerID, string takeProfit, string stoplass, string mintether, string exchange, string parity)
        {
            try
            {
                User user = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name);
                Logger.GetLogger().LogUser(LOGTYPE.INFO, user.UserID);
                CEXEXCHANGE enumExchange = (CEXEXCHANGE)Enum.Parse(typeof(CEXEXCHANGE), exchange);
                RiskLevel riskLevel = (RiskLevel)user.riskType;
                if (riskLevel == 0)
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                MakerParameter makerParameter = new MakerParameter();
                makerParameter.WorkerID = workerID;
                makerParameter.TakeProfit = Convert.ToDouble(takeProfit, CultureInfo.InvariantCulture);
                makerParameter.StopLoss = Convert.ToDouble(stoplass, CultureInfo.InvariantCulture);
                makerParameter.Deposit = Convert.ToDouble(mintether, CultureInfo.InvariantCulture);
                makerParameter.Exchange = enumExchange;
                makerParameter.Parity = parity;
                makerParameter.userID = user.UserID;
                makerParameter.ParameterType = ParameterTYPE.manual;
                makerParameter.riskLevel = riskLevel;
                makerParameter.makerStatus = BOTSTATUS.start;
                db.MakerParameters.Add(makerParameter);
                db.SaveChanges();
                return Json(makerParameter, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);

            }
        }
        [HttpPost]
        public JsonResult StartMakerStrategy(string deposit, int workerID, string exchange, string parity)
        {
            try
            {
                User user = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name);
                Logger.GetLogger().LogUser(LOGTYPE.INFO, user.UserID);
                CEXEXCHANGE enumExchange = (CEXEXCHANGE)Enum.Parse(typeof(CEXEXCHANGE), exchange);
                MakerParameter makerParameter = db.MakerParameters.OrderByDescending(m => m.MakerParameterID).FirstOrDefault(m => m.WorkerID == workerID);
                ICEXAPIController cEXAPIController = CexExchangeSelect.CexSelect(makerParameter.Exchange);
                List<BalanceResponseModel> userBalance = cEXAPIController.GetBalance();
                foreach (BalanceResponseModel balance in userBalance)
                {
                    if (balance.Symbol == "USDT")
                    {
                        if (Convert.ToDouble(balance.Balance, CultureInfo.InvariantCulture) < Convert.ToDouble(deposit, CultureInfo.InvariantCulture))
                        {
                            return Json(false, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                ComissionFeeResponseModel comissionFee = CexExchangeSelect.CexSelect(enumExchange).ComissionFee(parity);
                PairList pair = CexExchangeSelect.CexSelect(enumExchange).GetPair(parity);
                CexMakerParameterModel parameters = new CexMakerParameterModel();
                parameters.takeProfit = makerParameter.TakeProfit;
                parameters.stopLass = makerParameter.StopLoss;
                parameters.workerID = workerID;
                parameters.deposit = Convert.ToDouble(deposit, CultureInfo.InvariantCulture);
                parameters.comissionBuy = Convert.ToDouble(comissionFee.takerCommission, CultureInfo.InvariantCulture);
                parameters.comissionSell = Convert.ToDouble(comissionFee.makerCommission, CultureInfo.InvariantCulture);
                string[] qtyFormatter = pair.Pairs[0].qtyFormat.Split('1');
                pair.Pairs[0].qtyFormat = qtyFormatter[0] + "1";
                string format = pair.Pairs[0].qtyFormat.Replace("1", "0");
                parameters.qtyFormat = format;
                parameters.exchange = exchange;
                parameters.userID = user.UserID;
                parameters.symbol = parity;
                TradingBotCreator.GetTradingBotCreator().StartStrategy(parameters, workerID);

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult StopMakerStrategy(int workerID)
        {
            try
            {
                User user = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name);
                Logger.GetLogger().LogUser(LOGTYPE.INFO, user.UserID);
                TradingBotCreator.GetTradingBotCreator().StopStrategy(workerID);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult SaveGridParameter(string maxPrice, string minPrice, int gridCount, int workerID, string type, string minProfitPerGrid, string maxProfitPerGrid, string takeProfit, string stoplass, string mintether, string exchange, string parity)
        {
            try
            {
                User user = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name);
                Logger.GetLogger().LogUser(LOGTYPE.INFO, user.UserID);
                GridParameter gridParameter = new GridParameter();
                CEXEXCHANGE enumExchange = (CEXEXCHANGE)Enum.Parse(typeof(CEXEXCHANGE), exchange);
                double maxPriceDob = Convert.ToDouble(maxPrice, CultureInfo.InvariantCulture);
                double minPriceDob = Convert.ToDouble(minPrice, CultureInfo.InvariantCulture);
                double takeProfitDob = Convert.ToDouble(takeProfit, CultureInfo.InvariantCulture);
                double stopLossDob = Convert.ToDouble(stoplass, CultureInfo.InvariantCulture);
                gridParameter.MaxPrice = Math.Round(maxPriceDob, 4);
                gridParameter.MinPrice = Math.Round(minPriceDob, 4);
                gridParameter.GridCount = gridCount;
                gridParameter.MinProfitPerGrid = minProfitPerGrid;
                gridParameter.MaxProfitPerGrid = maxProfitPerGrid;
                gridParameter.TakeProfit = Math.Round(takeProfitDob, 4);
                gridParameter.StopLoss = Math.Round(stopLossDob, 4);
                gridParameter.Exchange = enumExchange;
                gridParameter.Parity = parity;
                gridParameter.userID = user.UserID;
                gridParameter.Deposit = Convert.ToDouble(mintether, CultureInfo.InvariantCulture);
                gridParameter.gridStatus = BOTSTATUS.start;
                if (type == "manual")
                {
                    gridParameter.ParameterType = ParameterTYPE.manual;
                }
                else
                {
                    gridParameter.ParameterType = ParameterTYPE.auto;
                }

                gridParameter.WorkerID = workerID;
                db.GridParameters.Add(gridParameter);
                db.SaveChanges();

                return Json(gridParameter, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult DexWorkerStateStart(int workerID)
        {
            try
            {
                User user = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name);
                Logger.GetLogger().LogUser(LOGTYPE.INFO, user.UserID);

                Worker worker = db.Workers.FirstOrDefault(m => m.WorkerID == workerID);
                worker.WorkerState = WORKERSTATE.Working;
                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult DexWorkerStateStop(int workerID)
        {
            try
            {
                User user = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name);
                Logger.GetLogger().LogUser(LOGTYPE.INFO, user.UserID);

                Worker worker = db.Workers.FirstOrDefault(m => m.WorkerID == workerID);
                if (worker.WorkerState == WORKERSTATE.Working)
                {
                    worker.WorkerState = WORKERSTATE.Paused;
                    db.SaveChanges();
                    return Json(true, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
               

            }
            catch (Exception ex)
            {

                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult StartGridStrategy(string deposit, int workerID, string type)
        {
            try
            {
                User user = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name);
                Logger.GetLogger().LogUser(LOGTYPE.INFO, user.UserID);

                GridParameter gridParameter = db.GridParameters.OrderByDescending(m => m.GridParameterID).FirstOrDefault(m => m.WorkerID == workerID && type == m.ParameterType.ToString());

                ICEXAPIController cEXAPIController = CexExchangeSelect.CexSelect(gridParameter.Exchange);
                List<BalanceResponseModel> userBalance = cEXAPIController.GetBalance();
                foreach (BalanceResponseModel balance in userBalance)
                {
                    if (balance.Symbol == "USDT")
                    {
                        if (Convert.ToDouble(balance.Balance, CultureInfo.InvariantCulture) < Convert.ToDouble(deposit, CultureInfo.InvariantCulture))
                        {
                            return Json(false, JsonRequestBehavior.AllowGet);
                        }
                    }
                }

                CexGridParameterModel parameters = new CexGridParameterModel();
                parameters.maxPrice = new decimal(gridParameter.MaxPrice);
                parameters.minPrice = new decimal(gridParameter.MinPrice);
                parameters.gridCount = gridParameter.GridCount;
                parameters.exchange = gridParameter.Exchange.ToString();
                parameters.deposit = gridParameter.Deposit;
                parameters.symbol = gridParameter.Parity;
                parameters.workerID = gridParameter.WorkerID;
                parameters.takeProfit = gridParameter.TakeProfit;
                parameters.stopLass = gridParameter.StopLoss;
                TradingBotCreator.GetTradingBotCreator().StartStrategy(parameters, workerID);

                return Json(true, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult StopGridStrategy(int workerID)
        {
            try
            {
                User user = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name);
                Logger.GetLogger().LogUser(LOGTYPE.INFO, user.UserID);
                TradingBotCreator.GetTradingBotCreator().StopStrategy(workerID);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetManualGridParameter(string maxPrice, string minPrice, int gridCount, int workerID)
        {
            try
            {
                User user = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name);
                Logger.GetLogger().LogUser(LOGTYPE.INFO, user.UserID);

                int riskType = (int)user.riskType;
                RiskLevel riskLevel = (RiskLevel)riskType;
                if (riskType == 0)
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                double minPriceDob = Convert.ToDouble(Decimal.Parse(minPrice, CultureInfo.InvariantCulture));
                double maxPriceDob = Convert.ToDouble(Decimal.Parse(maxPrice, CultureInfo.InvariantCulture));

                (double minProfitPerGrid, double maxProfitPerGrid) profitPerGrid = CalculateProfitPerGrid(gridCount, minPriceDob, maxPriceDob);
                (decimal takeProfit, decimal stopLoss) takeProfitAndStopLoss = CalculateTakeProfitAndStopLoss(riskLevel, new decimal(maxPriceDob), new decimal(minPriceDob));
                double minTetherAmount = GetMinimumTetherAmount(minPriceDob, maxPriceDob, gridCount);

                GeridParameter gridParameter = new GeridParameter();
                gridParameter.TakeProfit = takeProfitAndStopLoss.takeProfit;
                gridParameter.StopLoss = takeProfitAndStopLoss.stopLoss;
                gridParameter.MinTetherAmount = minTetherAmount;
                gridParameter.MaxProfitPerGrid = profitPerGrid.maxProfitPerGrid.ToString("0.00");
                gridParameter.MinProfitPerGrid = profitPerGrid.minProfitPerGrid.ToString("0.00");

                return Json(gridParameter, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult GetGridParameter(string exchange, string timeInterval, string symbol, int workerID)
        {
            try
            {
                User user = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name);
                Logger.GetLogger().LogUser(LOGTYPE.INFO, user.UserID);

                int riskType = (int)user.riskType;
                RiskLevel riskLevel = (RiskLevel)riskType;
                if (riskType == 0)
                {
                    riskLevel = RiskLevel.Low;
                }

                CEXEXCHANGE enumExchange = (CEXEXCHANGE)Enum.Parse(typeof(CEXEXCHANGE), exchange);
                TimeInterval enumTımeInterval = (TimeInterval)Enum.Parse(typeof(TimeInterval), timeInterval);

                ICEXAPIController cEXAPIController = CexExchangeSelect.CexSelect(enumExchange);
                List<CandleStickStockChartDataModel> marketData = new List<CandleStickStockChartDataModel>();

                if (enumTımeInterval == TimeInterval.SevenDays)
                {
                    marketData = cEXAPIController.GetChartData(symbol, Models.ChartsModels.Interval.hours4, Models.ChartsModels.CandlestickQuantity.Candle168);
                }
                else if (enumTımeInterval == TimeInterval.ThirtyDays)
                {
                    marketData = cEXAPIController.GetChartData(symbol, Models.ChartsModels.Interval.hours4, Models.ChartsModels.CandlestickQuantity.Candle360);
                }
                else
                {
                    marketData = cEXAPIController.GetChartData(symbol, Models.ChartsModels.Interval.hours4, Models.ChartsModels.CandlestickQuantity.Candle1080);
                }

                (decimal minPrice, decimal maxPrice) priceRange = CalculateGridPriceRange(marketData, enumTımeInterval);

                int gridCount = CalculateGridCount(symbol, enumExchange, Convert.ToDouble(priceRange.minPrice), Convert.ToDouble(priceRange.maxPrice), enumTımeInterval, riskLevel);

                (double minProfitPerGrid, double maxProfitPerGrid) profitPerGrid = CalculateProfitPerGrid(gridCount, Convert.ToDouble(priceRange.minPrice), Convert.ToDouble(priceRange.maxPrice));

                (decimal takeProfit, decimal stopLoss) takeProfitAndStopLoss = CalculateTakeProfitAndStopLoss(riskLevel, priceRange.maxPrice, priceRange.minPrice);

                double minTetherAmount = GetMinimumTetherAmount(Convert.ToDouble(priceRange.minPrice), Convert.ToDouble(priceRange.maxPrice), gridCount);
                GeridParameter gridParameter = new GeridParameter();
                gridParameter.MinPrice = priceRange.minPrice;
                gridParameter.MaxPrice = priceRange.maxPrice;
                gridParameter.MaxProfitPerGrid = profitPerGrid.maxProfitPerGrid.ToString("0.00");
                gridParameter.MinProfitPerGrid = profitPerGrid.minProfitPerGrid.ToString("0.00");
                gridParameter.GridCount = gridCount;
                gridParameter.TakeProfit = takeProfitAndStopLoss.takeProfit;
                gridParameter.StopLoss = takeProfitAndStopLoss.stopLoss;
                gridParameter.MinTetherAmount = minTetherAmount;

                return Json(gridParameter, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }
        }

        public class GeridParameter
        {
            public decimal MinPrice { get; set; }
            public decimal MaxPrice { get; set; }
            public decimal TakeProfit { get; set; }
            public decimal StopLoss { get; set; }
            public double GridCount { get; set; }
            public double MinTetherAmount { get; set; }
            public string MinProfitPerGrid { get; set; }
            public string MaxProfitPerGrid { get; set; }
        }

        [HttpGet]
        public JsonResult GetCurrentUSDT(string exchange)
        {
            try
            {
                User user = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name);
                Logger.GetLogger().LogUser(LOGTYPE.INFO, user.UserID);
                CEXEXCHANGE enumExchange = (CEXEXCHANGE)Enum.Parse(typeof(CEXEXCHANGE), exchange);

                string baseCurrencyPrice = GetBaseCurrencyPrice(enumExchange);

                double value = Convert.ToDouble(baseCurrencyPrice, CultureInfo.InvariantCulture);

                double roundedValue = Math.Round(value, 3);

                string currentPrice = roundedValue.ToString("0.000");

                return Json(currentPrice, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }

        }



        public static double GetMinimumTetherAmount(double minPrice, double maxPrice, int gridCount)
        {
            double minimumUsdt = gridCount * 15.228;
            return Math.Round(minimumUsdt, 2);
        }

        public static string GetBaseCurrencyPrice(CEXEXCHANGE exchange)
        {

            ICEXAPIController cEXAPIController = CexExchangeSelect.CexSelect(exchange);
            List<BalanceResponseModel> balance = cEXAPIController.GetBalance();
            string baseCurrencyPrice = "";
            foreach (var item in balance)
            {
                if (item.Symbol == "USDT")
                {
                    baseCurrencyPrice = item.Balance;
                }
            }


            return baseCurrencyPrice;

        }

        public static int CalculateGridCount(string symbol, CEXEXCHANGE exchange, double minPrice, double maxPrice, TimeInterval enumTimeInterval, RiskLevel riskLevel)
        {

            List<CandleStickStockChartDataModel> marketDataGrid = new List<CandleStickStockChartDataModel>();
            ICEXAPIController cEXAPIController = CexExchangeSelect.CexSelect(exchange);
            if (enumTimeInterval == TimeInterval.SevenDays)
            {
                marketDataGrid = cEXAPIController.GetChartData(symbol, Models.ChartsModels.Interval.minutes30, Models.ChartsModels.CandlestickQuantity.Candle168);
            }
            else if (enumTimeInterval == TimeInterval.ThirtyDays)
            {
                marketDataGrid = cEXAPIController.GetChartData(symbol, Models.ChartsModels.Interval.hours1, Models.ChartsModels.CandlestickQuantity.Candle360);
            }
            else
            {
                marketDataGrid = cEXAPIController.GetChartData(symbol, Models.ChartsModels.Interval.hours2, Models.ChartsModels.CandlestickQuantity.Candle1080);
            }

            double atr = 0;
            double sumTR = 0;
            int periods = marketDataGrid.Count - 1;

            for (int i = 1; i < marketDataGrid.Count; i++)
            {
                double tr1 = marketDataGrid[i].High - marketDataGrid[i].Low;
                double tr2 = marketDataGrid[i].High - Math.Abs(marketDataGrid[i - 1].Close);
                double tr3 = Math.Abs(marketDataGrid[i].Low) - marketDataGrid[i - 1].Close;

                double maxTR = Math.Max(Math.Abs(tr1), Math.Max(Math.Abs(tr2), Math.Abs(tr3)));
                sumTR += maxTR;
            }

            atr = sumTR / periods;

            double bufferPercentage = 20.0;
            double buffer = 1 + (bufferPercentage / 100);
            double gridNumber = (buffer * (maxPrice - minPrice)) / atr;

            int gridCount = (int)Math.Floor(gridNumber);
            double riskAdjustmentFactor;
            switch (riskLevel)
            {
                case RiskLevel.Low:
                    riskAdjustmentFactor = 0.7;
                    break;
                case RiskLevel.Medium:
                    riskAdjustmentFactor = 1.0;
                    break;
                case RiskLevel.High:
                    riskAdjustmentFactor = 1.3;
                    break;
                default:
                    riskAdjustmentFactor = 1.0;
                    break;
            }
            int adjustedGridCount = (int)Math.Floor(gridCount * riskAdjustmentFactor);

            return adjustedGridCount;

        }

        public static (decimal minPrice, decimal maxPrice) CalculateGridPriceRange(List<CandleStickStockChartDataModel> historicalMarketData, TimeInterval timeInterval)
        {
            DateTime endDate = DateTime.Now;
            long endDateTimeStamp = DateTimeToTimeStamp(DateTime.Now);
            long startDate = DateTimeToTimeStamp(endDate.AddDays(-(int)timeInterval));

            List<CandleStickStockChartDataModel> filteredMarketData = historicalMarketData.Where(price => price.Date >= startDate && price.Date <= endDateTimeStamp).ToList();

            if (filteredMarketData.Count == 0)
            {
                return (0, 0);
            }


            if (historicalMarketData == null || historicalMarketData.Count == 0)
            {
                return (0, 0);
            }
            double sum = 0;
            for (int i = 0; i < filteredMarketData.Count - 1; i++)
            {
                sum += filteredMarketData[i].Close;
            }
            double movingAverage = sum / filteredMarketData.Count;

            double sumOfSquaredDifferences = 0;
            for (int i = 0; i < filteredMarketData.Count - 1; i++)
            {
                double difference = filteredMarketData[i].Close - movingAverage;
                sumOfSquaredDifferences += difference * difference;
            }
            double variance = sumOfSquaredDifferences / (filteredMarketData.Count - 1);
            double standardDeviation = Math.Sqrt(variance);

            double bbm = 2;

            decimal upperBand = (decimal)(movingAverage + (bbm * standardDeviation));
            decimal lowerBand = (decimal)(movingAverage - (bbm * standardDeviation));

            decimal upperBandRoundedNumber = Math.Round(upperBand, 2);
            string upperBandroundedNumberFormatter = upperBandRoundedNumber.ToString("0.00");
            decimal lowerBandRoundedNumber = Math.Round(lowerBand, 2);
            string lowerBandFormattedNumber = lowerBandRoundedNumber.ToString("0.00");
            decimal maxPrice = decimal.Parse(upperBandroundedNumberFormatter);
            decimal minPrice = decimal.Parse(lowerBandFormattedNumber);

            return (minPrice, maxPrice);
        }

        public static (double minProfitPerGrid, double maxProfitPerGrid) CalculateProfitPerGrid(int gridCount, double minPrice, double maxPrice)
        {
            double commissionRate = 0; // Komisyon oranı %0.1 olarak belirlendi

            // Fiyat Farkı (d) = (Grid Üst Limiti - Grid Alt Limiti) / Grid Sayısı
            double priceDifference = (maxPrice - minPrice) / gridCount;

            // Maximum Profit/Grid = (1 - 0.1%)*10/400 - 2*0.1% = 2.29%
            double maxProfitPerGrid = (1 - commissionRate) * priceDifference / minPrice - 2 * commissionRate;

            // Minimum Profit/Grid = (450*(1 - 0.1%)) / (450 - 10) - 1 - 0.1% = 2.07%
            double minProfitPerGrid = (maxPrice * (1 - commissionRate)) / (maxPrice - priceDifference) - 1 - commissionRate;

            maxProfitPerGrid = maxProfitPerGrid * 100;
            minProfitPerGrid = minProfitPerGrid * 100;

            return (Math.Abs(maxProfitPerGrid), Math.Abs(minProfitPerGrid));
        }

        public static (decimal takeProfit, decimal stopLoss) CalculateTakeProfitAndStopLoss(RiskLevel riskLevel, decimal MaxGridPriceRange, decimal MinGridPriceRange)
        {
            decimal takeProfit = 0;
            decimal stopLoss = 0;

            switch (riskLevel)
            {
                case RiskLevel.Low:
                    takeProfit = MaxGridPriceRange * 1.05m;
                    stopLoss = MinGridPriceRange * 0.95m;
                    break;
                case RiskLevel.Medium:
                    takeProfit = MaxGridPriceRange * 1.1m;
                    stopLoss = MinGridPriceRange * 0.90m;
                    break;
                case RiskLevel.High:
                    takeProfit = MaxGridPriceRange * 1.15m;
                    stopLoss = MinGridPriceRange * 0.85m;
                    break;
            }

            decimal takeProfitRoundedNumber = Math.Round(takeProfit, 2);
            string takeProfitroundedNumberFormatter = takeProfitRoundedNumber.ToString("0.00");
            decimal stopLossRoundedNumber = Math.Round(stopLoss, 2);
            string stopLossFormattedNumber = stopLossRoundedNumber.ToString("0.00");
            decimal takeProfitDec = decimal.Parse(takeProfitroundedNumberFormatter);
            decimal stopLossDec = decimal.Parse(stopLossFormattedNumber);

            return (takeProfitDec, stopLossDec);
        }

        public static long DateTimeToTimeStamp(DateTime time)
        {
            DateTime utcDateTime = time.ToUniversalTime();
            TimeSpan timeSpan = utcDateTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            long Timestamp = (long)timeSpan.TotalMilliseconds;
            return Timestamp;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
