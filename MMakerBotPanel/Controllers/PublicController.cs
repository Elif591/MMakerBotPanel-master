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

    public class PublicController : Controller
    {

        private readonly ModelContext db = new ModelContext();

        // GET: Index
        public ActionResult Index()
        {
            try
            {

                Logger.GetLogger().LogUser(LOGTYPE.INFO, 0);
                List<Faq> homeFaqs = db.Faqs.Where(m => m.Home == true).ToList();
                if (homeFaqs != null)
                {
                    return View(homeFaqs);
                }
                else
                {
                    List<Faq> faq = new List<Faq>();
                    return View(faq);
                }
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "BadRequest");
            }
         
        }

        // GET: About
        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Contact(Contact contactUser)
        {
            try
            {
                Logger.GetLogger().LogUser(LOGTYPE.INFO, 0);
                string encodedResponse = Request.Form["g-Recaptcha-Response"];
                bool isCaptchaValid = ReCaptcha.Validate(encodedResponse);

                if (isCaptchaValid)
                {
                    if (contactUser != null)
                    {
                        Random random = new Random();
                        string result = string.Empty;
                        for (int i = 0; i < 11; i++)
                        {
                            result += random.Next(0, 10);
                        }
                        contactUser.ContactNumber = result;
                        contactUser.StatusEnumType = (int)REPLYSTATUS.Reply;
                        _ = db.Contacts.Add(contactUser);
                        _ = db.SaveChanges();
                    }
                    return RedirectToAction("Contact", "Public");
                }
                else
                {
                    ViewBag.error = "Wrong Captcha";
                    return RedirectToAction("Contact", "Public");
                }
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "BadRequest");
            }
           
        }

        // GET: Features
        public ActionResult Features()
        {
            return View();
        }

        // GET: HelpCenter
        public ActionResult HelpCenter()
        {
            try
            {
                Logger.GetLogger().LogUser(LOGTYPE.INFO, 0);
                List<Faq> helpFaqs = db.Faqs.Where(m => m.HelpDesk == true).ToList();
                if (helpFaqs != null)
                {
                    return View(helpFaqs);
                }
                else
                {
                    List<Faq> faq = new List<Faq>();
                    return View(faq);
                }
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "BadRequest");
            }
    
        }

        // GET: Policy
        public ActionResult Policy()
        {
            return View();
        }

        // GET: Terms
        public ActionResult Terms()
        {
            return View();
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