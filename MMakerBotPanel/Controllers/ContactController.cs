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
    [Authorize(Roles = "Admin")]
    public class ContactController : Controller
    {
        private readonly ModelContext db = new ModelContext();

        [HttpGet]
        public JsonResult ReplyMessageModal(int id)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                Contact userContact = db.Contacts.Find(id);
                return userContact != null ? Json(userContact, JsonRequestBehavior.AllowGet) : Json(JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ReplyMessage(string ContactNumber, string ReplyMessage)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                Contact replyContact = db.Contacts.FirstOrDefault(m => m.ContactNumber == ContactNumber);
                if (replyContact != null)
                {
                    if (ReplyMessage != null)
                    {
                        replyContact.ContactUserReply = ReplyMessage;
                        replyContact.StatusEnumType = (int)REPLYSTATUS.Replied;
                        _ = db.SaveChanges();
                        _ = Mailler.GetMailler().SendMail("Trading Manitou reply message!", replyContact.ContactUserReply, replyContact.ContactUserEmail, replyContact.ContactUserName);
                    }
                }
                return Json(true , JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ContactList()
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                List<Contact> contacts = db.Contacts.OrderBy(m => m.StatusEnumType).ToList();
                return View(contacts);
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
