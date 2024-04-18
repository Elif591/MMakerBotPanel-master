using MMakerBotPanel.Business;
using MMakerBotPanel.Database.Context;
using MMakerBotPanel.Database.Model;
using MMakerBotPanel.Models;
using MMakerBotPanel.Models.ChatModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace MMakerBotPanel.Controllers
{
    [Authorize]
    public class TicketController : Controller
    {
        private readonly ModelContext db = new ModelContext();

        [HttpPost]
        public JsonResult CreateTicket(string TicketName, string TicketMessage)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                Ticket createTicket = new Ticket();
                int UserID = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                createTicket.UserID = UserID;
                createTicket.LastActivity = DateTime.Now;
                createTicket.CreateDate = DateTime.Now;
                createTicket.TicketName = TicketName;
                createTicket.TicketStatusEnumType = TICKETSTATUS.AwaitingAnswer;
                createTicket.TicketDetails = new List<TicketDetail>();
                TicketDetail createTicketDetail = new TicketDetail
                {
                    UserID = UserID,
                    Date = DateTime.Now,
                    Message = TicketMessage
                };
                createTicket.TicketDetails.Add(createTicketDetail);
                _ = db.Tickets.Add(createTicket);
                _ = db.SaveChanges();
                return Json(new { IsOk = true });
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult TicketList()
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                List<TicketList> ticketList = new List<TicketList>();
                List<Ticket> tickets = db.Tickets.ToList();
                foreach (Ticket item in tickets)
                {
                    TicketList ticket = new TicketList
                    {
                        TicketID = item.TicketID,
                        TicketName = item.TicketName,
                        TicketStatus = item.TicketStatusEnumType,
                        LastActivity = item.LastActivity.ToString("HH:mm:ss tt"),
                        Name = db.Users.FirstOrDefault(m => m.UserID == item.UserID).Name,
                        LastName = db.Users.FirstOrDefault(m => m.UserID == item.UserID).SurName,
                        Image = db.Users.FirstOrDefault(m => m.UserID == item.UserID).Image,
                        Email = db.Users.FirstOrDefault(m => m.UserID == item.UserID).Email
                    };
                    ticketList.Add(ticket);
                }
                ticketList = ticketList.OrderBy(m => m.TicketStatus == TICKETSTATUS.AwaitingAnswer).ToList();
                return View(ticketList);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "BadRequest");
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult TicketChatAdmin(int? id)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                Ticket ticket = db.Tickets.FirstOrDefault(m => m.TicketID == id);
                if(ticket == null)
                {
                    return View(new Ticket());
                }
                else
                {
                    return View(ticket);
                }
             
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "BadRequest");
            }
        }

        [HttpGet]
        public JsonResult TicketChatAdminn()
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                List<Ticket> tickets = db.Tickets.Where(m => m.TicketStatusEnumType != TICKETSTATUS.Closed).ToList();
                foreach (Ticket item in tickets)
                {
                    item.TicketDetails = item.TicketDetails.OrderBy(m => m.Date).ToList();
                }
                tickets = tickets.OrderBy(m => m.TicketStatusEnumType == TICKETSTATUS.Answered).ToList();

                return Json(tickets.Select(x => new
                {
                    x.TicketID,
                    x.TicketName,
                    x.CreateDate,
                    LastActivity = x.LastActivity.ToString("HH:mm:ss tt"),
                    x.TicketStatusEnumType,
                    TicketDetail = x.TicketDetails.Select(y => new
                    {
                        y.Message,
                        y.Date,
                        UserRole = y.User.Role,
                    })
                }), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);

            }
        
        }

        [Authorize(Roles = "Member")]
        public ActionResult TicketChatMember()
        {
            return View();
        }

        [HttpGet]
        public JsonResult TicketChatMemmber()
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                int UserID = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                List<Ticket> tickets = db.Tickets.Where(m => m.UserID == UserID).ToList();
                foreach (Ticket item in tickets)
                {
                    item.TicketDetails = item.TicketDetails.OrderBy(m => m.Date).ToList();
                }
                tickets = tickets.OrderBy(m => m.TicketStatusEnumType == TICKETSTATUS.Closed)
                .ThenBy(m => m.TicketStatusEnumType == TICKETSTATUS.Answered).ToList();
                return Json(tickets.Select(x => new
                {
                    x.TicketID,
                    x.TicketName,
                    x.CreateDate,
                    LastActivity = x.LastActivity.ToString("HH:mm:ss tt"),
                    x.TicketStatusEnumType,
                    TicketDetail = x.TicketDetails.Select(y => new
                    {
                        y.Message,
                        y.Date,
                        UserRole = y.User.Role,
                    })
                }), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }
        
        }

        [HttpGet]
        public JsonResult TicketChat(int? id)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                List<Ticket> ticketChat = db.Tickets.Where(m => m.TicketID == id).ToList();
                string userIdenttyRole = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).Role;
                return Json(ticketChat.Select(x => new
                {
                    userIdenttyRole,
                    LastActivity = x.LastActivity.ToString("HH:mm:ss tt"),
                    x.TicketStatusEnumType,
                    TicketDetail = x.TicketDetails.Select(y => new
                    {
                        TicketDetailID = y.TicketDetayID,
                        y.Message,
                        y.Date,
                        UserRole = y.User.Role
                    })
                }), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }
     
        }

        [HttpPost]
        public JsonResult MessageButton(int TicketID, string Message)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                User user = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name);
                Ticket ticket = db.Tickets.FirstOrDefault(m => m.TicketID == TicketID);
                if (ticket.TicketStatusEnumType != TICKETSTATUS.Closed)
                {
                    ticket.TicketStatusEnumType = user.Role == "Admin" ? TICKETSTATUS.Answered : TICKETSTATUS.AwaitingAnswer;
                    TicketDetail ticketDetail = new TicketDetail
                    {
                        UserID = user.UserID,
                        Message = Message,
                        Date = DateTime.Now,
                        TicketID = ticket.TicketID
                    };
                    ticket.TicketDetails.Add(ticketDetail);
                    _ = db.SaveChanges();
                    MessageButonModel messageButonModel = new MessageButonModel
                    {
                        TicketDetayID = ticketDetail.TicketDetayID,
                        TicketStatusEnumType = (int)ticket.TicketStatusEnumType
                    };
                    return Json(messageButonModel, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("", JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);

            }
           
        }

        [HttpGet]
        public JsonResult CloseTicket(int TicketID)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                Ticket ticket = db.Tickets.FirstOrDefault(m => m.TicketID == TicketID);
                ticket.TicketStatusEnumType = TICKETSTATUS.Closed;
                _ = db.SaveChanges();
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public JsonResult NewMessageCheck(int id)
        {
            TicketDetail lastMessage = db.TicketDetails.Find(id);
            List<ChatMessageModel> newMessageListJsonModel = new List<ChatMessageModel>();
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                if (lastMessage != null)
                {
                    List<TicketDetail> newMessageList = db.TicketDetails.Where(x => x.TicketID == lastMessage.TicketID && x.TicketDetayID > lastMessage.TicketDetayID).ToList();

                    foreach (TicketDetail item in newMessageList)
                    {
                        ChatMessageModel newMessage = new ChatMessageModel
                        {
                            TicketDetailID = item.TicketDetayID,
                            Message = item.Message,
                            UserRole = item.User.Role,
                            TicketID = item.TicketID
                        };
                        newMessageListJsonModel.Add(newMessage);
                    }
                }
                return Json(newMessageListJsonModel, JsonRequestBehavior.AllowGet);
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
        private struct MessageButonModel
        {
            public int TicketDetayID { get; set; }
            public int TicketStatusEnumType { get; set; }
        }
    }
}