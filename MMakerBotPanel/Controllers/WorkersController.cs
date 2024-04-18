namespace MMakerBotPanel.Controllers
{
    using MMakerBotPanel.Business;
    using MMakerBotPanel.Business.Strategy.CexGrid.Model;
    using MMakerBotPanel.Business.Strategy;
    using MMakerBotPanel.Database.Context;
    using MMakerBotPanel.Database.Model;
    using MMakerBotPanel.Models;
    using MMakerBotPanel.Models.CexBotModels;
    using MMakerBotPanel.Models.ChartsModel;
    using MMakerBotPanel.Models.ChartsModels;
    using MMakerBotPanel.Models.MakerMarketingModels;
    using MMakerBotPanel.WebServices.Interface;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    [Authorize]
    public class WorkersController : Controller
    {
        private readonly ModelContext db = new ModelContext();
        // GET: Workers
        [Authorize(Roles = "Member")]
        public ActionResult Index()
        {

            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                int userID = UserHelper.GetUserID(User.Identity.Name);
                if (userID > 0)
                {
                    DateTime filterDate = DateTime.Now.AddDays(-60);

                    List<Worker> workerList = db.Workers.Where(m => m.UserID == userID && m.EndingDate >= filterDate)
                                    .OrderByDescending(m => (int)m.WorkerState == (int)WORKERSTATE.Working)
                                    .ThenByDescending(m => (int)m.WorkerState == (int)WORKERSTATE.Paused)
                                    .ThenByDescending(m => (int)m.WorkerState == (int)WORKERSTATE.PendingTXID)
                                    .ThenByDescending(m => (int)m.WorkerState == (int)WORKERSTATE.Expired)
                                    .ToList();
                    foreach (Worker item in workerList)
                    {
                        item.DaysRemaining = Convert.ToInt32((item.EndingDate - DateTime.Now).TotalDays);
                    }
                    return View(workerList);
                }
                else
                {
                    return View();
                }
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
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                int id = UserHelper.GetUserID(User.Identity.Name);
                if (id > 0)
                {
                    List<Worker> workerList = db.Workers.ToList();
                    foreach (Worker item in workerList)
                    {
                        item.DaysRemaining = Convert.ToInt32((item.EndingDate - DateTime.Now).TotalDays);
                    }
                    return View(workerList);
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "BadRequest");
            }

        }

        // GET: Workers/Create
        public ActionResult Create()
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                Worker worker = new Worker
                {
                    // worker.WorkerKey = WorkerHelper.CreateWorkerKey();
                    EndingDate = DateTime.Now.AddDays(30).Date
                };
                return View(worker);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "BadRequest");
            }

        }

        [Authorize(Roles = "Member")]
        public ActionResult WorkerPricing()
        {
            return View();
        }

        [HttpPost]
        public JsonResult PurchaseHistory(string transection, string accountWallet, string receiverWallet, string priceShortName, string price, bool yearlyCheck, string productId, string workerId, bool success , string gasfee)
        {
            try
            {
       
                PurchaseHistory purchaseHistory = new PurchaseHistory();
                PriceUnit priceUnit = db.PriceUnits.FirstOrDefault(m => m.ShortName == priceShortName);
                User user = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name);

                purchaseHistory.Amount = price;
                purchaseHistory.AccountWallet = accountWallet;
                purchaseHistory.ReceiverWallet = receiverWallet;
                purchaseHistory.PriceUnitID = priceUnit.PriceUnitID;
                purchaseHistory.PriceUnit = priceUnit;
                purchaseHistory.ShortNmae = priceShortName;
                purchaseHistory.Contract = priceUnit.Contract;
                purchaseHistory.Timestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                if (workerId == "0")
                {
                    Logger.GetLogger().LogUser(LOGTYPE.INFO, user.UserID);
                    Logger.GetLogger().LogPayment(accountWallet, receiverWallet, productId, workerId, price, priceUnit.PriceUnitID.ToString(), gasfee, user.UserID);
                    Product product = db.Products.FirstOrDefault(m => m.ProductID.ToString() == productId);
                    Worker newWorker = new Worker
                    {
                        WorkerName = product.ProductName,
                        StartingDate = DateTime.Now.AddDays(1),
                        ProductID = product.ProductID,
                        Product = product,
                        User = user,
                        UserID = user.UserID
                    };
                    if (success)
                    {
                        purchaseHistory.PurchaseStatusType = (PURCHASESTATUS)(int)PURCHASESTATUS.Pending;
                        purchaseHistory.TransectionId = transection;
                        newWorker.WorkerState = (WORKERSTATE)(int)WORKERSTATE.PendingTXID;
                        if (yearlyCheck)
                        {
                            newWorker.EndingDate = DateTime.Now.AddDays(365);
                            purchaseHistory.Yearly = true;
                            purchaseHistory.Monthly = false;
                        }
                        else
                        {
                            purchaseHistory.Monthly = true;
                            purchaseHistory.Yearly = false;
                            newWorker.EndingDate = DateTime.Now.AddDays(30);
                        }
                    }
                    else
                    {
                        purchaseHistory.PurchaseStatusType = (PURCHASESTATUS)(int)PURCHASESTATUS.Reject;
                        purchaseHistory.ErrorMessage = transection;
                        newWorker.WorkerState = (WORKERSTATE)(int)WORKERSTATE.Expired;


                        if (yearlyCheck)
                        {
                            newWorker.EndingDate = DateTime.Now.AddDays(1);
                            purchaseHistory.Yearly = true;
                            purchaseHistory.Monthly = false;
                        }
                        else
                        {
                            purchaseHistory.Monthly = true;
                            purchaseHistory.Yearly = false;
                            newWorker.EndingDate = DateTime.Now.AddDays(1);
                        }
                    }

                    newWorker.PurchaseHistories = new List<PurchaseHistory>
                    {
                        purchaseHistory
                    };

                    user.Workers.Add(newWorker);
                }
                else
                {
                    Logger.GetLogger().LogUser(LOGTYPE.INFO, user.UserID);
                    Logger.GetLogger().LogPayment(accountWallet, receiverWallet, "0", workerId, price, priceUnit.PriceUnitID.ToString(), gasfee, user.UserID);
                    Worker worker = db.Workers.FirstOrDefault(m => m.WorkerID.ToString() == workerId);
                    worker.WorkerName = worker.Product.ProductName;
                    worker.StartingDate = DateTime.Now.AddDays(1);
                    worker.ProductID = worker.ProductID;
                    if (success)
                    {
                        purchaseHistory.PurchaseStatusType = (PURCHASESTATUS)(int)PURCHASESTATUS.Pending;
                        purchaseHistory.TransectionId = transection;
                        worker.WorkerState = (WORKERSTATE)(int)WORKERSTATE.PendingTXID;


                        if (yearlyCheck)
                        {
                            worker.EndingDate = DateTime.Now.AddDays(365);
                            purchaseHistory.Yearly = true;
                            purchaseHistory.Monthly = false;
                        }
                        else
                        {
                            purchaseHistory.Monthly = true;
                            purchaseHistory.Yearly = false;
                            worker.EndingDate = DateTime.Now.AddDays(30);
                        }
                    }
                    else
                    {
                        purchaseHistory.PurchaseStatusType = (PURCHASESTATUS)(int)PURCHASESTATUS.Reject;
                        purchaseHistory.ErrorMessage = transection;
                        worker.WorkerState = (WORKERSTATE)(int)WORKERSTATE.Expired;


                        if (yearlyCheck)
                        {
                            worker.EndingDate = DateTime.Now.AddDays(1);
                            purchaseHistory.Yearly = true;
                            purchaseHistory.Monthly = false;
                        }
                        else
                        {
                            purchaseHistory.Monthly = true;
                            purchaseHistory.Yearly = false;
                            worker.EndingDate = DateTime.Now.AddDays(1);
                        }
                    }
                    worker.PurchaseHistories.Add(purchaseHistory);
                }
                _ = db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public JsonResult GetProductName()
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                List<Product> products = db.Products.ToList();
                return Json(products.Select(x => new
                {
                    productId = x.ProductID,
                    productName = x.ProductName,
                }), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult CexGridWorker()
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                List<ProductPrice> productPrices = db.ProductPrices.Where(m => m.Product.ProductName == "Cex Grid Worker").ToList();
                return productPrices.Count > 0
                    ? Json(productPrices.Select(x => new
                    {
                        x.PriceUnit.ShortName,
                        x.PriceUnit.Contract
                    }), JsonRequestBehavior.AllowGet)
                    : Json(false, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public JsonResult GetWallet()
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                Wallet wallet = db.Wallets.OrderByDescending(m => m.WalletID).ToList().FirstOrDefault();
                return wallet != null ? Json(wallet.WalletAddress, JsonRequestBehavior.AllowGet) : Json(false, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public JsonResult GetContractAddress(string shortName)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                PriceUnit price = db.PriceUnits.FirstOrDefault(m => m.ShortName == shortName);
                return price != null ? Json(price.Contract, JsonRequestBehavior.AllowGet) : Json(false, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public JsonResult GetPriceSelectedShortName(string shortName, string ProductId)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                List<ProductPrice> productPrices = db.ProductPrices.Where(m => m.Product.ProductID.ToString() == ProductId).ToList();
                ProductPrice selectedShortName = productPrices.FirstOrDefault(m => m.PriceUnit.ShortName == shortName);
                if (selectedShortName != null)
                {
                    SelectedSortNameModel selectedSortNameModel = new SelectedSortNameModel
                    {
                        YearlyPrice = selectedShortName.YearlyPrice,
                        MonthlyPrice = selectedShortName.MonthlyPrice
                    };
                    return Json(selectedSortNameModel, JsonRequestBehavior.AllowGet);
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

        [HttpGet]
        public JsonResult DexGridWorker()
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                List<ProductPrice> productPrices = db.ProductPrices.Where(m => m.Product.ProductName == "Dex Grid Worker").ToList();
                return productPrices.Count > 0
                    ? Json(productPrices.Select(x => new
                    {
                        x.PriceUnit.ShortName,
                        x.PriceUnit.Contract
                    }), JsonRequestBehavior.AllowGet)
                    : Json(false, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public JsonResult AdvancedMMakerWorker()
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                List<ProductPrice> productPrices = db.ProductPrices.Where(m => m.Product.ProductName == "Advanced MMaker Worker").ToList();
                return productPrices.Count > 0
                    ? Json(productPrices.Select(x => new
                    {
                        x.PriceUnit.ShortName,
                        x.PriceUnit.Contract
                    }), JsonRequestBehavior.AllowGet)
                    : Json(false, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }

        }

        private struct selectPickerModel
        {
            public string ShortName { get; set; }
        }

        private struct SelectedSortNameModel
        {
            public float MonthlyPrice { get; set; }
            public float YearlyPrice { get; set; }
        }

        [HttpGet]
        public JsonResult ExtendTimeInfo(int workerId)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                Worker extendWorker = db.Workers.FirstOrDefault(m => m.WorkerID == workerId);
                ExtendWorkerInfoModel extendWorkerInfo = new ExtendWorkerInfoModel
                {
                    ProductName = extendWorker.Product.ProductName,
                    ProductId = extendWorker.Product.ProductID,
                    WorkingState = extendWorker.WorkerState.ToString(),
                    StartDate = extendWorker.StartingDate.ToShortDateString(),
                    EndDate = extendWorker.EndingDate.ToShortDateString(),
                    DaysRemaining = Convert.ToInt32((extendWorker.EndingDate - DateTime.Now).TotalDays)
                };
                return Json(extendWorkerInfo, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }
        }

        private struct ExtendWorkerInfoModel
        {
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public int DaysRemaining { get; set; }
            public string StartDate { get; set; }
            public string EndDate { get; set; }
            public string WorkingState { get; set; }

        }

        [HttpGet]
        public JsonResult GetSelectProductPrice(int productId)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                List<ProductPrice> productPrices = db.ProductPrices.Where(m => m.Product.ProductID == productId).ToList();
                return productPrices.Count > 0
                    ? Json(productPrices.Select(x => new
                    {
                        PriceName = x.PriceUnit.ShortName,
                        Contract = x.PriceUnit.Contract,    
                    }), JsonRequestBehavior.AllowGet)
                    : Json(false, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetTotalPrice(int productId, int priceRadio , string priceName)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                List<ProductPrice> productPrices = db.ProductPrices.Where(m => m.Product.ProductID == productId).ToList();
                string price = "";
                if (productPrices != null)
                {
                    int priceUnitId = db.PriceUnits.FirstOrDefault(m => m.ShortName == priceName).PriceUnitID;
                    foreach (var item in productPrices)
                    {
                        if(item.PriceUnitID == priceUnitId)
                        {
                            price = priceRadio == 1 ? item.YearlyPrice.ToString() : item.MonthlyPrice.ToString();
                        }

                    }
                   
                    return Json(price, JsonRequestBehavior.AllowGet);
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
        // GET: Workers/Create
        public ActionResult AdminWorkerPricing()
        {
            return View();
        }

        // POST: Workers1/Create
        // Aşırı gönderim saldırılarından korunmak için bağlamak istediğiniz belirli özellikleri etkinleştirin. 
        // Daha fazla bilgi için bkz. https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WorkerID,WorkerName,WorkerKey,LicensePeriod")] Worker worker)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                if (ModelState.IsValid)
                {
                    _ = db.Workers.Add(worker);
                    _ = db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(worker);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "BadRequest");
            }
        }

        // GET: Workers1/Edit/5
        [Authorize(Roles = "Member")]
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
                Worker worker = db.Workers.Find(id);
                double totalHours = Convert.ToInt32((worker.EndingDate - DateTime.Now).TotalHours);
                if (totalHours < 0)
                {
                    worker.DaysRemaining = 0;
                    worker.HoursRemaining = 0;
                }
                else
                {
                    int days = (int)totalHours / 24;
                    int hours = (int)totalHours % 24;
                    worker.HoursRemaining = hours;
                    worker.DaysRemaining = days;
                }
                if (worker == null)
                {
                    return HttpNotFound();
                }

                worker.WorkerDetail = db.WorkerDetails.FirstOrDefault(x => x.Worker.WorkerID == worker.WorkerID);

                if (worker.WorkerDetail == null)
                {
                    worker.WorkerDetail = new WorkerDetail();
                }
                User user = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name);
                List<ExchangeApi> userExchange = db.ExchangeApis.Where(m => m.UserID == user.UserID).ToList();
                List<CEXEXCHANGE> cexExchangeList = new List<CEXEXCHANGE>();
                foreach (ExchangeApi item in userExchange)
                {
                    try
                    {
                        CEXEXCHANGE cex = item.Exchange;
                        string apiKey = userExchange.FirstOrDefault(m => m.Exchange == cex && m.ExchangeApiKey == EXCHANGEAPIKEY.ApiKey).Value;
                        string secretKey = userExchange.FirstOrDefault(m => m.Exchange == cex && m.ExchangeApiKey == EXCHANGEAPIKEY.SecretKey).Value;
                        if (apiKey != null && secretKey != null)
                        {
                            cexExchangeList.Add(cex);
                        }
                    }
                    catch (Exception)
                    {

                      
                    }
                   
                }
                ViewBag.UserExchange = cexExchangeList.Distinct();
                List<DEXEXCHANGE> dexExchangeList = new List<DEXEXCHANGE>()
                {
                    DEXEXCHANGE.Pancakeswap,
                    DEXEXCHANGE.UniswapV2,
                    DEXEXCHANGE.UniswapV3,
                    DEXEXCHANGE.TraderJoe
                };
                ViewBag.DexExchange = dexExchangeList.Distinct();
                return View(worker);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "BadRequest");
            }

        }


        [HttpGet]
        public JsonResult WorkerStart(int workerId)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                GridParameter lastGridStrategy = db.GridParameters.OrderByDescending(m => m.GridParameterID).FirstOrDefault(m => m.WorkerID == workerId);

                if (lastGridStrategy != null)
                {

                    GridParameter gridParameter = db.GridParameters.OrderByDescending(m => m.GridParameterID).FirstOrDefault(m => m.WorkerID == workerId);

                    ICEXAPIController cEXAPIController = CexExchangeSelect.CexSelect(gridParameter.Exchange);
                    List<BalanceResponseModel> userBalance = cEXAPIController.GetBalance();
                    foreach (BalanceResponseModel balance in userBalance)
                    {
                        if (balance.Symbol == "USDT")
                        {
                            if (Convert.ToDouble(balance.Balance.Replace(".", ",")) < gridParameter.Deposit)
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
                    parameters.userID = gridParameter.userID;
                    TradingBotCreator.GetTradingBotCreator().StartStrategy(parameters , workerId);
                }

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }

        }


        [HttpGet]
        public JsonResult WorkerStop(int workerId)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                GridParameter gridParameter = db.GridParameters.OrderByDescending(m => m.GridParameterID).FirstOrDefault(m => m.WorkerID == workerId);
                gridParameter.gridStatus = BOTSTATUS.stop;
                db.SaveChanges();
                TradingBotCreator.GetTradingBotCreator().StopStrategy(workerId);

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }

        }


        [HttpGet]
        public JsonResult WorkerInfo(int workerId)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                GridParameter lastGridStrategy = db.GridParameters.OrderByDescending(m => m.GridParameterID).FirstOrDefault(m => m.WorkerID == workerId);
                if(lastGridStrategy != null)
                {
                    object lastGridParameter = new
                    {
                        maxPrice = lastGridStrategy.MaxPrice,
                        minPrice = lastGridStrategy.MinPrice,
                        gridCount = lastGridStrategy.GridCount,
                        takeProfit = lastGridStrategy.TakeProfit,
                        stoploss = lastGridStrategy.StopLoss,
                        exchange = lastGridStrategy.Exchange.ToString(),
                        parity = lastGridStrategy.Parity,
                        minProfit = lastGridStrategy.MinProfitPerGrid,
                        maxProfit = lastGridStrategy.MaxProfitPerGrid,
                        deposit = lastGridStrategy.Deposit,
                    };

                    return Json(lastGridParameter, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    object lastGridParameter = new
                    {
                       
                    };

                    return Json(lastGridParameter, JsonRequestBehavior.AllowGet);
                }
   
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }
          
        }



        [HttpGet]
        public JsonResult CexSave(int workerId, string selectedParity, string selectedCex)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                Worker worker = db.Workers.FirstOrDefault(m => m.WorkerID == workerId);
                WorkerDetail workerDetail = new WorkerDetail();
                workerDetail.Parity = selectedParity;
                workerDetail.ExchangeType = EXCHANGETYPE.CEX;
                if (Enum.TryParse(selectedCex, true, out CEXEXCHANGE parsedEnum))
                {
                    workerDetail.CexExchange = parsedEnum;
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                worker.WorkerDetail = workerDetail;
                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }

        }

        [Authorize(Roles = "Member")]
        [HttpPost]
        public ActionResult Edit(Worker worker)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                if (ModelState.IsValid)
                {
                    Worker editWorker = db.Workers.Find(worker.WorkerID);
                    editWorker.WorkerName = worker.WorkerName;
                    _ = db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(worker);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "BadRequest");
            }

        }
        [HttpPost]
        public JsonResult GetParity(CEXEXCHANGE CexExchange)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                PairList pairList = CexExchangeSelect.CexSelect(CexExchange).GetPairList();
                return Json(pairList);
            }
            catch (Exception ex)
            {

                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult GetChart(string symbol, CEXEXCHANGE CexExchange)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                List<CandleStickStockChartDataModel> ChartData = CexExchangeSelect.CexSelect(CexExchange).GetChartData(symbol, Interval.hours4 , CandlestickQuantity.Candle1000);

                return Json(ChartData, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult GetChartDate(string symbol, CEXEXCHANGE CexExchange)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                List<CandleStickStockChartDataModel> ChartData = CexExchangeSelect.CexSelect(CexExchange).GetChartDataDate(symbol, Interval.hours4 , CandlestickQuantity.Candle1);
                return Json(ChartData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }

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