namespace MMakerBotPanel.Controllers
{
    using MMakerBotPanel.Business;
    using MMakerBotPanel.Database.Context;
    using MMakerBotPanel.Database.Model;
    using MMakerBotPanel.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    public class ProductController : Controller
    {
        private readonly ModelContext db = new ModelContext();
        [Authorize(Roles = "Admin")]
        public ActionResult ProductList()
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                List<Product> productList = db.Products.ToList();
                return View(productList);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }

        }

        [Authorize(Roles = "Admin")]
        public ActionResult ProductDetail(int id)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                Product productPrice = db.Products.FirstOrDefault(m => m.ProductID == id);
                return View(productPrice);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ProductEdit(int ProductID, bool Demo)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                ProductPrice editProduct = db.ProductPrices.FirstOrDefault(m => m.ProductID == ProductID);
                editProduct.Product.Demo = Demo;
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
        public JsonResult PriceDelete(int id)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                ProductPrice productPrice = db.ProductPrices.FirstOrDefault(m => m.ProductPriceID == id);
                _ = db.ProductPrices.Remove(productPrice);
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
        public JsonResult PriceModal(int id)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                ProductPrice productPrice = db.ProductPrices.FirstOrDefault(m => m.ProductPriceID == id);
                PriceModalModel modal = new PriceModalModel
                {
                    ProductID = productPrice.ProductID,
                    MonthlyPrice = productPrice.MonthlyPrice,
                    YearlyPrice = productPrice.YearlyPrice,
                    PriceName = productPrice.PriceUnit.Name
                };

                return Json(modal, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }
        }

        private struct PriceModalModel
        {
            public int ProductID { get; set; }
            public float MonthlyPrice { get; set; }
            public float YearlyPrice { get; set; }
            public string PriceName { get; set; }
        }

        [HttpGet]
        public JsonResult GetPriceName(string name)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                List<PriceUnit> price = db.PriceUnits.OrderByDescending(m => m.Name == name).ToList();
                List<PriceUnitModel> priceUnitModels = new List<PriceUnitModel>();
                foreach (PriceUnit item in price)
                {
                    PriceUnitModel model = new PriceUnitModel
                    {
                        ShortName = item.ShortName
                    };
                    priceUnitModels.Add(model);
                }
                return Json(priceUnitModels, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }
        }

        private struct PriceUnitModel
        {
            public string ShortName { get; set; }
        }

        [HttpPost]
        public JsonResult AddPrrice(int ProductID, string PriceName, float MonthlyPrice, float YearlyPrice)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                ProductPrice createPrice = new ProductPrice();
                List<ProductPrice> productPrice = db.ProductPrices.Where(m => m.ProductID == ProductID).ToList();
                bool recordPrice = false;
                if (productPrice.Count > 0)
                {
                    foreach (ProductPrice item in productPrice)
                    {
                        if (item.PriceUnit.ShortName == PriceName)
                        {
                            recordPrice = true;
                            break;
                        }
                        else
                        {
                            recordPrice = false;
                        }
                    }

                }

                if (recordPrice)
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    int priceUnitId = db.PriceUnits.FirstOrDefault(m => m.ShortName == PriceName).PriceUnitID;
                    createPrice.PriceUnitID = priceUnitId;
                    createPrice.MonthlyPrice = MonthlyPrice;
                    createPrice.YearlyPrice = YearlyPrice;
                    createPrice.ProductID = ProductID;
                    _ = db.ProductPrices.Add(createPrice);
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
        public JsonResult ProductPriceEdit(int ProductID, string PriceName, float MonthlyPrice, float YearlyPrice)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);

                ProductPrice editPrice = db.ProductPrices.FirstOrDefault(m => m.ProductID == ProductID && m.PriceUnit.ShortName == PriceName);
                //int priceUnitId = db.PriceUnits.FirstOrDefault(m => m.ShortName == PriceName).PriceUnitID;

                //editPrice.PriceUnitID = priceUnitId;
                editPrice.MonthlyPrice = MonthlyPrice;
                editPrice.YearlyPrice = YearlyPrice;
                _ = db.SaveChanges();
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
    }
}
