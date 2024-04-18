namespace MMakerBotPanel.Controllers
{
    using MMakerBotPanel.Business;
    using MMakerBotPanel.Database.Context;
    using MMakerBotPanel.Database.Model;
    using MMakerBotPanel.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    public class FaqsController : Controller
    {

        private readonly ModelContext db = new ModelContext();

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                List<Faq> faqs = db.Faqs.ToList();

                return View(faqs);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "BadRequest");
            }

        }
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                Faq faq = db.Faqs.Find(id);
                return View(faq);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "BadRequest");
            }

        }
        [HttpPost]
        public ActionResult Edit([Bind(Include = "FaqID,Question,Answer,Panel,HelpDesk,Home")] Faq faq)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                if (ModelState.IsValid)
                {
                    Faq editFaq = db.Faqs.Find(faq.FaqID);
                    int helpDesk = db.Faqs.Where(m => m.HelpDesk == true).Count();
                    int home = db.Faqs.Where(m => m.Home == true).Count();

                    if (editFaq.HelpDesk != faq.HelpDesk)
                    {
                        if (helpDesk >= 10 && faq.HelpDesk)
                        {
                            ViewBag.HelpError = "Helpdesk area nerves up to 4 rounds";
                            return View(faq);
                        }
                    }
                    if (editFaq.Home != faq.Home)
                    {
                        if (home >= 4 && faq.Home)
                        {
                            ViewBag.HomeError = "Home area nerves up to 10 rounds";
                            return View(faq);
                        }
                    }
                    editFaq.Home = faq.Home;
                    editFaq.HelpDesk = faq.HelpDesk;
                    editFaq.Question = faq.Question;
                    editFaq.Answer = faq.Answer;
                    editFaq.Panel = faq.Panel;

                    db.Entry(editFaq).State = EntityState.Modified;
                    _ = db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.modelStateError = "Question area or answer area must be full";
                    return View(faq);
                }
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "BadRequest");
            }
            
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                Faq faq = new Faq();
                return View(faq);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "BadRequest");
            }
 
        }
        [HttpPost]
        public ActionResult Create([Bind(Include = "FaqID,Question,Answer,Panel,HelpDesk,Home")] Faq faq)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                List<Faq> faqs = db.Faqs.ToList();
                if (ModelState.IsValid)
                {
                    if (faq.HelpDesk)
                    {
                        int helpDesk = faqs.Where(m => m.HelpDesk == true).Count();
                        if (helpDesk >= 4)
                        {
                            ViewBag.HelpError = "Helpdesk area nerves up to 4 rounds";
                            return View(faq);
                        }

                    }
                    if (faq.Home)
                    {
                        int home = faqs.Where(m => m.Home == true).Count();
                        if (home >= 10)
                        {
                            ViewBag.HomeError = "Home area nerves up to 10 rounds";
                            return View(faq);
                        }

                    }

                    _ = db.Faqs.Add(faq);
                    _ = db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.modelStateError = "Question area or answer area must be full";
                    return View(faq);
                }

            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "BadRequest");
            }
          
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                Faq faq = db.Faqs.Find(id);
                _ = db.Faqs.Remove(faq);
                _ = db.SaveChanges();

                return RedirectToAction("Index", "Faqs");
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "BadRequest");
            }
        }


        [Authorize(Roles = "Member")]
        public ActionResult GetUserFaqs()
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                List<Faq> faqs = db.Faqs.Where(x => x.Panel).ToList();
                return View(faqs);
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
