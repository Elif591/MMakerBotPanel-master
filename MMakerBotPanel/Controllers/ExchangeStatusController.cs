namespace MMakerBotPanel.Controllers
{
    using MMakerBotPanel.Business;
    using MMakerBotPanel.Database.Context;
    using MMakerBotPanel.Models;
    using MMakerBotPanel.Models.CexBotModels;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    [Authorize(Roles = "Admin")]
    public class ExchangeStatusController : Controller
    {
        private readonly ModelContext db = new ModelContext();
        // GET: ExchangeStatus
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult BinanceStatusTime()
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                List<StatusModel> statusModels = new List<StatusModel>
            {
                BinanceStatus(),
            };
                return Json(statusModels, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public JsonResult AlterdiceStatusTime()
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                List<StatusModel> statusModels = new List<StatusModel>
            {
                AlterStatus(),
            };
                return Json(statusModels, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public JsonResult DextradeStatusTime()
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                List<StatusModel> statusModels = new List<StatusModel>
            {
                DexTradeStatus(),
            };
                return Json(statusModels, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public JsonResult BitfinexStatusTime()
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                List<StatusModel> statusModels = new List<StatusModel>
            {
                BitfinexStatus()
            };

                return Json(statusModels, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult BybitStatusTime()
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                List<StatusModel> statusModels = new List<StatusModel>
            {
                 BybitStatus()
            };

                return Json(statusModels, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }

        }
 
        [HttpGet]
        public JsonResult GateioStatusTime()
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                List<StatusModel> statusModels = new List<StatusModel>
            {
               GateioStatus(),
            };

                return Json(statusModels, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }

        }
   
        [HttpGet]
        public JsonResult HuobiStatusTime()
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                List<StatusModel> statusModels = new List<StatusModel>
            {
               HuobiStatus(),
            };

                return Json(statusModels, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public JsonResult KrakenStatusTime()
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                List<StatusModel> statusModels = new List<StatusModel>
            {
              KrakenStatus(),
            };

                return Json(statusModels, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public JsonResult KucoinStatusTime()
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                List<StatusModel> statusModels = new List<StatusModel>
            {
             KucoinStatus(),
            };

                return Json(statusModels, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public JsonResult LbankStatusTime()
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                List<StatusModel> statusModels = new List<StatusModel>
            {
                LbankStatus(),
            };

                return Json(statusModels, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public JsonResult MexcStatusTime()
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                List<StatusModel> statusModels = new List<StatusModel>
            {
                MexcStatus(),
            };

                return Json(statusModels, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult OKXStatusTime()
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                List<StatusModel> statusModels = new List<StatusModel>
            {
                OkxStatus(),
            };

                return Json(statusModels, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }

        }
     

        [HttpGet]
        public JsonResult CoinsbitStatusTime()
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                List<StatusModel> statusModels = new List<StatusModel>
            {
                CoinsbitStatus(),
            };

                return Json(statusModels, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }
    
        }
        private StatusModel BinanceStatus()
        {
            try
            {
                StatusModel statusModel = new StatusModel();
                CexBotStatusModel status = CexExchangeSelect.CexSelect(CEXEXCHANGE.Binance).Status();
                statusModel.Exchange = Enum.GetName(typeof(CEXEXCHANGE), CEXEXCHANGE.Binance);
                statusModel.Value = status.CexBotStatusModels.status;
                statusModel.date = status.CexBotStatusModels.date.ToString("MM/dd/yyyy HH:mm:ss");
                return statusModel;
            }
            catch (Exception ex)
            {
                StatusModel statusModel = new StatusModel();
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return statusModel;
            }
       
        }
        private StatusModel AlterStatus()
        {
            try
            {
                StatusModel statusModel = new StatusModel();
                CexBotStatusModel status = CexExchangeSelect.CexSelect(CEXEXCHANGE.Alterdice).Status();
                statusModel.Exchange = Enum.GetName(typeof(CEXEXCHANGE), CEXEXCHANGE.Alterdice);
                statusModel.Value = status.CexBotStatusModels.status;
                statusModel.date = status.CexBotStatusModels.date.ToString("MM/dd/yyyy HH:mm:ss");
                return statusModel;
            }
            catch (Exception ex)
            {
                StatusModel statusModel = new StatusModel();
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return statusModel;
            }
          
        }
        private StatusModel BitfinexStatus()
        {
            try
            {
                StatusModel statusModel = new StatusModel();
                CexBotStatusModel status = CexExchangeSelect.CexSelect(CEXEXCHANGE.Bitfinex).Status();
                statusModel.Exchange = Enum.GetName(typeof(CEXEXCHANGE), CEXEXCHANGE.Bitfinex);
                statusModel.Value = status.CexBotStatusModels.status;
                statusModel.date = status.CexBotStatusModels.date.ToString("MM/dd/yyyy HH:mm:ss");
                return statusModel;
            }
            catch (Exception ex)
            {
                StatusModel statusModel = new StatusModel();
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return statusModel;
            }
            
        }
        private StatusModel BybitStatus()
        {
            try
            {
                StatusModel statusModel = new StatusModel();
                CexBotStatusModel status = CexExchangeSelect.CexSelect(CEXEXCHANGE.Bybit).Status();
                statusModel.Exchange = Enum.GetName(typeof(CEXEXCHANGE), CEXEXCHANGE.Bybit);
                statusModel.Value = status.CexBotStatusModels.status;
                statusModel.date = status.CexBotStatusModels.date.ToString("MM/dd/yyyy HH:mm:ss");
                return statusModel;

            }
            catch (Exception ex)
            {
                StatusModel statusModel = new StatusModel();
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return statusModel;
            }
       
        }
        private StatusModel DexTradeStatus()
        {
            try
            {
                StatusModel statusModel = new StatusModel();
                CexBotStatusModel status = CexExchangeSelect.CexSelect(CEXEXCHANGE.Dextrade).Status();
                statusModel.Exchange = Enum.GetName(typeof(CEXEXCHANGE), CEXEXCHANGE.Dextrade);
                statusModel.Value = status.CexBotStatusModels.status;
                statusModel.date = status.CexBotStatusModels.date.ToString("MM/dd/yyyy HH:mm:ss");
                return statusModel;
            }
            catch (Exception ex)
            {
                StatusModel statusModel = new StatusModel();
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return statusModel;
            }
      
        }
        private StatusModel GateioStatus()
        {
            try
            {
                StatusModel statusModel = new StatusModel();
                CexBotStatusModel status = CexExchangeSelect.CexSelect(CEXEXCHANGE.Gateio).Status();
                statusModel.Exchange = Enum.GetName(typeof(CEXEXCHANGE), CEXEXCHANGE.Gateio);
                statusModel.Value = status.CexBotStatusModels.status;
                statusModel.date = status.CexBotStatusModels.date.ToString("MM/dd/yyyy HH:mm:ss");
                return statusModel;
            }
            catch (Exception ex)
            {
                StatusModel statusModel = new StatusModel();
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return statusModel;
            }
            
        }
 
        private StatusModel HuobiStatus()
        {
            try
            {
                StatusModel statusModel = new StatusModel();
                CexBotStatusModel status = CexExchangeSelect.CexSelect(CEXEXCHANGE.Huobi).Status();
                statusModel.Exchange = Enum.GetName(typeof(CEXEXCHANGE), CEXEXCHANGE.Huobi);
                statusModel.Value = status.CexBotStatusModels.status;
                statusModel.date = status.CexBotStatusModels.date.ToString("MM/dd/yyyy HH:mm:ss");
                return statusModel;
            }
            catch (Exception ex)
            {
                StatusModel statusModel = new StatusModel();
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return statusModel;
            }
           
        }
        private StatusModel KrakenStatus()
        {
            try
            {
                StatusModel statusModel = new StatusModel();
                CexBotStatusModel status = CexExchangeSelect.CexSelect(CEXEXCHANGE.Kraken).Status();
                statusModel.Exchange = Enum.GetName(typeof(CEXEXCHANGE), CEXEXCHANGE.Kraken);
                statusModel.Value = status.CexBotStatusModels.status;
                statusModel.date = status.CexBotStatusModels.date.ToString("MM/dd/yyyy HH:mm:ss");
                return statusModel;
            }
            catch (Exception ex)
            {
                StatusModel statusModel = new StatusModel();
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return statusModel;
            }
           
        }
        private StatusModel KucoinStatus()
        {
            try
            {
                StatusModel statusModel = new StatusModel();
                CexBotStatusModel status = CexExchangeSelect.CexSelect(CEXEXCHANGE.Kucoin).Status();
                statusModel.Exchange = Enum.GetName(typeof(CEXEXCHANGE), CEXEXCHANGE.Kucoin);
                statusModel.Value = status.CexBotStatusModels.status;
                statusModel.date = status.CexBotStatusModels.date.ToString("MM/dd/yyyy HH:mm:ss");
                return statusModel;
            }
            catch (Exception ex)
            {
                StatusModel statusModel = new StatusModel();
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return statusModel;
            }
           
        }
        private StatusModel LbankStatus()
        {
            try
            {
                StatusModel statusModel = new StatusModel();
                CexBotStatusModel status = CexExchangeSelect.CexSelect(CEXEXCHANGE.Lbank).Status();
                statusModel.Exchange = Enum.GetName(typeof(CEXEXCHANGE), CEXEXCHANGE.Lbank);
                statusModel.Value = status.CexBotStatusModels.status;
                statusModel.date = status.CexBotStatusModels.date.ToString("MM/dd/yyyy HH:mm:ss");
                return statusModel;
            }
            catch (Exception ex)
            {
                StatusModel statusModel = new StatusModel();
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return statusModel;
            }
         
        }
        private StatusModel MexcStatus()
        {
            try
            {
                StatusModel statusModel = new StatusModel();
                CexBotStatusModel status = CexExchangeSelect.CexSelect(CEXEXCHANGE.Mexc).Status();
                statusModel.Exchange = Enum.GetName(typeof(CEXEXCHANGE), CEXEXCHANGE.Mexc);
                statusModel.Value = status.CexBotStatusModels.status;
                statusModel.date = status.CexBotStatusModels.date.ToString("MM/dd/yyyy HH:mm:ss");
                return statusModel;
            }
            catch (Exception ex)
            {
                StatusModel statusModel = new StatusModel();
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return statusModel;
            }
       
        }
        private StatusModel OkxStatus()
        {
            try
            {
                StatusModel statusModel = new StatusModel();
                CexBotStatusModel status = CexExchangeSelect.CexSelect(CEXEXCHANGE.OKX).Status();
                statusModel.Exchange = Enum.GetName(typeof(CEXEXCHANGE), CEXEXCHANGE.OKX);
                statusModel.Value = status.CexBotStatusModels.status;
                statusModel.date = status.CexBotStatusModels.date.ToString("MM/dd/yyyy HH:mm:ss");
                return statusModel;

            }
            catch (Exception ex)
            {
                StatusModel statusModel = new StatusModel();
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return statusModel;
            }
      
        }
 
        private StatusModel CoinsbitStatus()
        {
            try
            {
                StatusModel statusModel = new StatusModel();
                CexBotStatusModel status = CexExchangeSelect.CexSelect(CEXEXCHANGE.Coinsbit).Status();
                statusModel.Exchange = Enum.GetName(typeof(CEXEXCHANGE), CEXEXCHANGE.Coinsbit);
                statusModel.Value = status.CexBotStatusModels.status;
                statusModel.date = status.CexBotStatusModels.date.ToString("MM/dd/yyyy HH:mm:ss");
                return statusModel;
            }
            catch (Exception ex)
            {
                StatusModel statusModel = new StatusModel();
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return statusModel;
            }
          
        }
        public struct StatusModel
        {
            public string Exchange { get; set; }
            public bool Value { get; set; }
            public string date { get; set; }
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
