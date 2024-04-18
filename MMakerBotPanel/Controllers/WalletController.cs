using MMakerBotPanel.Business;
using MMakerBotPanel.Database.Context;
using MMakerBotPanel.Database.Model;
using MMakerBotPanel.Models;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace MMakerBotPanel.Controllers
{
    public class WalletController : Controller
    {
        private readonly ModelContext db = new ModelContext();

        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult OrderSettings()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetSelectCheckMinute()
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                System.Collections.Generic.IEnumerable<SelectListItem> selectCheckMinute = Enum.GetValues(typeof(CHECKDATESTATUS))
                        .Cast<CHECKDATESTATUS>()
                        .Select(e => new SelectListItem() { Value = ((int)e).ToString(), Text = e.GetDisplayName() });

                return Json(selectCheckMinute.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }
 
        }


        [HttpGet]
        public JsonResult saveWalletAddress(string address, string apikey, int ttl, int controlDate)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                Wallet wallet = db.Wallets.OrderByDescending(x => x.WalletID).ToList().FirstOrDefault();
                if (wallet != null)
                {
                    wallet.WalletAddress = address;
                    wallet.EtherScanApiKey = apikey;
                    wallet.TtlCount = ttl;
                    wallet.CheckPeriod = (CHECKDATESTATUS)controlDate;
                    _ = db.SaveChanges();

                }
                else
                {
                    Wallet newWallet = new Wallet
                    {
                        WalletAddress = address,
                        EtherScanApiKey = apikey,
                        TtlCount = ttl,
                        CheckPeriod = (CHECKDATESTATUS)controlDate
                    };
                    _ = db.Wallets.Add(newWallet);
                    _ = db.SaveChanges();
                }
                CacheHelper.Remove(CACHEKEYENUMTYPE.OrderSettings_CheckPeriyod.ToString());
                CacheHelper.Remove(CACHEKEYENUMTYPE.OrderSetting_TLL.ToString());
                CacheHelper.Remove(CACHEKEYENUMTYPE.Etherscan_APIKey.ToString());

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public JsonResult GetWalletAddress()
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                Wallet wallet = db.Wallets.OrderByDescending(x => x.WalletID).ToList().First();
                GetWalletModel getWallet = new GetWalletModel
                {
                    WalletAddress = wallet.WalletAddress,
                    EtherscanApiKey = wallet.EtherScanApiKey,
                    TTEL = wallet.TtlCount,
                    CheckControlDate = wallet.CheckPeriod
                };
                return Json(getWallet, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }

        }

        private struct GetWalletModel
        {
            public string WalletAddress { get; set; }
            public string EtherscanApiKey { get; set; }
            public int TTEL { get; set; }
            public CHECKDATESTATUS CheckControlDate { get; set; }

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
