using MMakerBotPanel.Business;
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
    public class PriceUnitController : Controller
    {
        private readonly ModelContext db = new ModelContext();
        // GET: ProductPrice
        [Authorize(Roles = "Admin")]
        public ActionResult ProductPriceUnitList()
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                List<PriceUnit> priceList = db.PriceUnits.Where(m => m.ShortName != "ETH").ToList();
                return View(priceList);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "BadRequest");
            }
        }

        [HttpPost]
        public JsonResult SavePriceModal(string PriceName, string PriceShortName, string ContractAdress)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                PriceUnit createPrice = new PriceUnit();
                PriceUnit priceUnitId = db.PriceUnits.FirstOrDefault(m => m.ShortName == PriceShortName);
                if (priceUnitId != null)
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    createPrice.Name = PriceName;
                    createPrice.ShortName = PriceShortName;
                    createPrice.Contract = ContractAdress;
                    _ = db.PriceUnits.Add(createPrice);
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

        [HttpGet]
        public JsonResult EditContractModal(int id)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                PriceUnit priceUnit = db.PriceUnits.FirstOrDefault(m => m.PriceUnitID == id);

                if (priceUnit != null)
                {
                    EditContractModel price = new EditContractModel
                    {
                        Name = priceUnit.Name,
                        ShortName = priceUnit.ShortName,
                        Contract = priceUnit.Contract
                    };
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

        private struct EditContractModel
        {
            public string Name { get; set; }

            public string ShortName { get; set; }

            public string Contract { get; set; }
        }

        [HttpPost]
        public JsonResult SaveEditContract(int id, string contract)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                PriceUnit priceUnit = db.PriceUnits.FirstOrDefault(m => m.PriceUnitID == id);
                if (priceUnit != null)
                {
                    priceUnit.Contract = contract;
                    _ = db.SaveChanges();
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

        [HttpGet]
        public JsonResult DeleteContract(int id)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                PriceUnit priceUnit = db.PriceUnits.FirstOrDefault(m => m.PriceUnitID == id);
                if (priceUnit != null)
                {
                    List<Product> products = db.Products.ToList();
                    foreach (Product item in products)
                    {
                        foreach (ProductPrice price in item.ProductPrices)
                        {
                        if(price.PriceUnitID == id)
                            {
                                return Json("Please check the price lists of the products first, as the payment method is entered as a price.", JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    List<PurchaseHistory> histories = db.PurchaseHistories.Where(m => m.PriceUnitID == id).ToList();
                if(histories.Count > 0)
                    {
                        return Json("As payment has previously been processed using this method, it cannot be removed", JsonRequestBehavior.AllowGet);
                    }
                    db.PriceUnits.Remove(priceUnit);
                    db.SaveChanges();
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Something went wrong", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }



        }


        [Authorize(Roles = "Admin")]
        public ActionResult ProductPriceUnitDetail(int id)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                ProductPrice productPrice = db.ProductPrices.Find(id);
                return View(productPrice);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "BadRequest");
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
