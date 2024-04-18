using MMakerBotPanel.Business;
using MMakerBotPanel.Business.Services;
using MMakerBotPanel.Database.Context;
using MMakerBotPanel.Database.Model;
using MMakerBotPanel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace MMakerBotPanel.Controllers
{
    public class CexExchangeApiController : Controller
    {
        private readonly ModelContext db = new ModelContext();

        [HttpGet]
        public JsonResult GetCexExchange()
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                List<KeyValuePair<int, string>> translationKeyValuePairs = Enum.GetValues(typeof(CEXEXCHANGE))
                       .Cast<int>()
                       .Select(x => new KeyValuePair<int, string>(key: x, value: Enum.GetName(typeof(CEXEXCHANGE), x)))
                       .ToList();
                return Json(translationKeyValuePairs.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetKeys()
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                List<KeyValuePair<int, string>> keyList = Enum.GetValues(typeof(EXCHANGEAPIKEY))
                       .Cast<int>()
                       .Select(x => new KeyValuePair<int, string>(key: x, value: Enum.GetName(typeof(EXCHANGEAPIKEY), x)))
                       .ToList();
                return Json(keyList.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public JsonResult SaveNewKey(string cexEx, string keyEx, string value , string passphrase)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                ExchangeApi exchangeUser = db.ExchangeApis.FirstOrDefault(m => m.User.Email == User.Identity.Name);
                if (exchangeUser != null)
                {
                    ExchangeApi exchangeApi = new ExchangeApi();
                    if (exchangeUser.Exchange == (CEXEXCHANGE)Convert.ToInt32(cexEx) && exchangeUser.ExchangeApiKey == (EXCHANGEAPIKEY)Convert.ToInt32(keyEx))
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        exchangeApi.Value = value;
                        exchangeApi.PassPhrase = passphrase;
                        exchangeApi.UserID = exchangeUser.UserID;
                        exchangeApi.Exchange = (CEXEXCHANGE)Convert.ToInt32(cexEx);
                        exchangeApi.ExchangeApiKey = (EXCHANGEAPIKEY)Convert.ToInt32(keyEx);
                        _ = db.ExchangeApis.Add(exchangeApi);
                        _ = db.SaveChanges();
                    }

                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ExchangeApi exchangeApiNew = new ExchangeApi
                    {
                        Value = value,
                        PassPhrase = passphrase,    
                        UserID = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID,
                        User = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name),
                        Exchange = (CEXEXCHANGE)Convert.ToInt32(cexEx),
                        ExchangeApiKey = (EXCHANGEAPIKEY)Convert.ToInt32(keyEx)
                    };
                    _ = db.ExchangeApis.Add(exchangeApiNew);
                    _ = db.SaveChanges();

                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult EditModal(int Id)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                ExchangeApi exchange = db.ExchangeApis.FirstOrDefault(m => m.ExchangeApiID == Id);
                EditModel model = new EditModel
                {
                    Exchange = (int)exchange.Exchange,
                    ExchangeApiKey = (int)exchange.ExchangeApiKey,
                    Value = exchange.Value,
                    PassPhrase = exchange.PassPhrase,
                };
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }
 
        }

        [HttpGet]
        public JsonResult SaveEditModal(int Id, string value , string passphrase)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                ExchangeApi exchange = db.ExchangeApis.FirstOrDefault(m => m.ExchangeApiID == Id);
                exchange.Value = value;
                exchange.PassPhrase = passphrase;
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
        public JsonResult Delete(int Id)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                _ = db.ExchangeApis.Remove(db.ExchangeApis.FirstOrDefault(m => m.ExchangeApiID == Id));
                _ = db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }

        }
 
        public ActionResult Services()
        {
            return View();
        }

        [HttpGet]
        public JsonResult PaymentControlServiceStatus()
        {
            try
            {
                if(User.Identity.Name != "")
                {
                    int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                    Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                }
                else
                {
                    Logger.GetLogger().LogUser(LOGTYPE.INFO, 0);
                }

                long TimeStamp = CacheHelper.Get<long>(CACHEKEYENUMTYPE.PaymentControlService_LastActivity.ToString());
                return TimeStamp > 0 ? Json(true, JsonRequestBehavior.AllowGet) : Json(false, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public JsonResult RepairPaymentControlService()
        {
            try
            {
                if (User.Identity.Name != "")
                {
                    int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                    Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                }
                else
                {
                    Logger.GetLogger().LogUser(LOGTYPE.INFO, 0);
                }
                PaymentControlService.GetPaymentControlService().Unsubscribe();
                PaymentControlService.GetPaymentControlService().Subscribe(TaskSchedulerService.GetTaskSchedulerService());
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult WorkerSubscriptionServiceStatus()
        {
            try
            {
                if (User.Identity.Name != "")
                {
                    int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                    Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                }
                else
                {
                    Logger.GetLogger().LogUser(LOGTYPE.INFO, 0);
                }
                long TimeStamp = CacheHelper.Get<long>(CACHEKEYENUMTYPE.WorkerSubscriptionService_LastActivity.ToString());
                return TimeStamp > 0 ? Json(true, JsonRequestBehavior.AllowGet) : Json(false, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public JsonResult RepairWorkerSubscriptionService()
        {
            try
            {
                if (User.Identity.Name != "")
                {
                    int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                    Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                }
                else
                {
                    Logger.GetLogger().LogUser(LOGTYPE.INFO, 0);
                }
                WorkerSubscriptionService.GetWorkerSubscriptionService().Unsubscribe();
                WorkerSubscriptionService.GetWorkerSubscriptionService().Subscribe(TaskSchedulerService.GetTaskSchedulerService());
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult WorkerWorkingControlServiceStatus()
        {
            try
            {
                if (User.Identity.Name != "")
                {
                    int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                    Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                }
                else
                {
                    Logger.GetLogger().LogUser(LOGTYPE.INFO, 0);
                }
                long TimeStamp = CacheHelper.Get<long>(CACHEKEYENUMTYPE.WorkerWorkingControlService_LastActivity.ToString());
                return TimeStamp > 0 ? Json(true, JsonRequestBehavior.AllowGet) : Json(false, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult RepairWorkerWorkingControlService()
        {
            try
            {
                if (User.Identity.Name != "")
                {
                    int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                    Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                }
                else
                {
                    Logger.GetLogger().LogUser(LOGTYPE.INFO, 0);
                }
                WorkerWorkingControlService.GetWorkerWorkingControlService().Unsubscribe();
                WorkerWorkingControlService.GetWorkerWorkingControlService().Subscribe(TaskSchedulerService.GetTaskSchedulerService());
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult TaskSchedulerServiceStatus()
        {
            try
            {
                if (User.Identity.Name != "")
                {
                    int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                    Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                }
                else
                {
                    Logger.GetLogger().LogUser(LOGTYPE.INFO, 0);
                }
                long TimeStamp = CacheHelper.Get<long>(CACHEKEYENUMTYPE.TaskSchedulerService_LastActivity.ToString());
                return TimeStamp > 0 ? Json(true, JsonRequestBehavior.AllowGet) : Json(false, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult RepairTaskSchedulerService()
        {
            try
            {
                if (User.Identity.Name != "")
                {
                    int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                    Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                }
                else
                {
                    Logger.GetLogger().LogUser(LOGTYPE.INFO, 0);
                }
                TaskSchedulerService.GetTaskSchedulerService().SetTimer(25000, 1000);
                return Json(true, JsonRequestBehavior.AllowGet);
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
        private struct EditModel
        {
            public int Exchange { get; set; }
            public int ExchangeApiKey { get; set; }
            public string Value { get; set; }
            public string PassPhrase { get; set; }
        }

    }
}
