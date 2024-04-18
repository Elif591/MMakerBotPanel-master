using MMakerBotPanel.Business;
using MMakerBotPanel.Database.Context;
using MMakerBotPanel.Database.Model;
using MMakerBotPanel.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace MMakerBotPanel.Controllers
{
    public class UsersController : Controller
    {
        private readonly ModelContext db = new ModelContext();

        public ActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Login(User user)
        {
            try
            {
                string encodedResponse = Request.Form["g-Recaptcha-Response"];
                bool isCaptchaValid = ReCaptcha.Validate(encodedResponse);

                if (isCaptchaValid)
                {
                    if (user.Email == null || user.Password == null)
                    {
                        ViewBag.error = "Username or password is wrong";
                        return View();
                    }

                    if (UserHelper.Login(user))
                    {

                        user = db.Users.FirstOrDefault(x => x.Email == user.Email);

                        if (user.Disabled == true)
                        {
                            ViewBag.error = "Suspened";
                            return View();

                        }
                        user.LastLogin = DateTime.Now;
                        _ = db.SaveChanges();
                        int UserId = db.Users.FirstOrDefault(m => m.Email == user.Email).UserID;
                        Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                        return user.Role == "Admin" ? RedirectToAction("MemberList", "Users") : (ActionResult)RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.error = "Username or password is wrong";
                        return View();
                    }
                }
                else
                {
                    ViewBag.error = "Wrong Captcha";
                    return View();
                }
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "BadRequest");
            }
        }

        [HttpPost]
        public JsonResult CreateAdmin(string FisrtName, string LastName, string email, string password)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);

                User user = new User
                {
                    Name = FisrtName,
                    SurName = LastName,
                    Email = email
                };
                User dbUser = db.Users.FirstOrDefault(x => x.Email == user.Email);
                if (dbUser != null)
                {
                    ViewBag.error = "The e-mail address is already in use.";
                    return Json("The e-mail address is already in use.", JsonRequestBehavior.AllowGet);
                }
                user.Password = UserHelper.GetPassHash(password);
                user.PassaportAgain = "";
                user.Disabled = false;
                user.LastLogin = DateTime.Now;
                user.Role = "Admin";
                user.SuperAdmin = false;
                user.Image = getImageName(FisrtName);
                _ = db.Users.Add(user);
                _ = db.SaveChanges();

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetUserName()
        {
            try
            {
                User user = db.Users.First(x => x.Email == User.Identity.Name);
                Logger.GetLogger().LogUser(LOGTYPE.INFO, user.UserID);
                return Json(new { Name = user.Name + " " + user.SurName, user.Image, user.ImageType, role = user.SuperAdmin });

            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult GetMemberInfo(int memberId)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                User user = db.Users.FirstOrDefault(m => m.UserID == memberId);
                MemberInfoForAdminModel memberInfo = new MemberInfoForAdminModel
                {
                    Name = user.Name,
                    Email = user.Email,
                    SurName = user.SurName,
                    Image = user.Image,
                    ImageType = user.ImageType
                };
                return Json(memberInfo, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult GetMemberApiKeys(int memberId)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                User user = db.Users.FirstOrDefault(m => m.UserID == memberId);
                List<ExchangeApi> exchangeApiUser = db.ExchangeApis.Where(m => m.User.Email == user.Email).ToList();
                List<MemberApiKeysModel> memberApi = new List<MemberApiKeysModel>();
                foreach (ExchangeApi item in exchangeApiUser)
                {
                    MemberApiKeysModel memberEx = new MemberApiKeysModel
                    {
                        //  memberEx.Value = item.Value;
                        Exchange = item.Exchange.ToString(),
                        ExchangeApiKey = item.ExchangeApiKey.ToString()
                    };
                    memberApi.Add(memberEx);
                }
                return Json(memberApi, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult AdminExtendTimeForWorker(int workerId, string process)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                Worker worker = db.Workers.FirstOrDefault(m => m.WorkerID == workerId);

                if (process == "1")
                {
                    worker.EndingDate = worker.EndingDate.AddDays(365);
                    worker.WorkerState = WORKERSTATE.Expired;

                }
                else if (process == "2")
                {
                    worker.EndingDate = worker.EndingDate.AddDays(30);
                    worker.WorkerState = WORKERSTATE.Expired;

                }
                else if (process == "3")
                {
                    worker.WorkerState = WORKERSTATE.Paused;
                }
                else if (process == "4")
                {
                    worker.WorkerState = WORKERSTATE.Working;
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

        [HttpPost]
        public JsonResult CreateTicketMember(int memberId, string ticketHeader, string ticketMessage)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                if (ticketHeader != "" && ticketMessage != "")
                {
                    User admin = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name);
                    User member = db.Users.FirstOrDefault(m => m.UserID == memberId);
                    Ticket createTicket = new Ticket
                    {
                        UserID = member.UserID,
                        User = member,
                        LastActivity = DateTime.Now,
                        CreateDate = DateTime.Now,
                        TicketName = ticketHeader,
                        TicketStatusEnumType = TICKETSTATUS.AwaitingAnswer
                    };
                    TicketDetail createTicketDetail = new TicketDetail
                    {
                        UserID = admin.UserID,
                        User = admin,
                        Date = DateTime.Now,
                        Message = ticketMessage,
                        Ticket = createTicket
                    };
                    _ = db.Tickets.Add(createTicket);
                    _ = db.TicketDetails.Add(createTicketDetail);

                    _ = db.SaveChanges();
                    return Json(new { IsOk = true });
                }
                else
                {
                    return Json(new { IsOk = false, Message = "Something went wrong. Try again later." });
                }

            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }
        }

        private struct MemberApiKeysModel
        {
            public string ExchangeApiKey { get; set; }
            public string Exchange { get; set; }
            public string Value { get; set; }

        }

        private struct MemberInfoForAdminModel
        {
            public string Name { get; set; }
            public string SurName { get; set; }
            public string Email { get; set; }
            public string Image { get; set; }
            public string ImageType { get; set; }

        }

        [Authorize(Roles = "Admin")]
        public ActionResult AdminList()
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                List<User> adminList = db.Users.Where(m => m.Role == "Admin").ToList();
                return View(adminList.OrderBy(m => m.SuperAdmin == true));
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "BadRequest");
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult MemberList()
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                List<User> memberList = db.Users.Where(m => m.Role == "Member").ToList();
                return View(memberList);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "BadRequest");
            }

        }

        [Authorize(Roles = "Admin")]
        public ActionResult MemberDetail(int id)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                User member = db.Users.FirstOrDefault(m => m.UserID == id);
                int ticketId = 0;
                if (member != null)
                {
                    if (member.Tickets.Count < 1)
                    {
                        ticketId = 0;
                    }
                    else
                    {
                        ticketId = member.Tickets.ToList().First().TicketID;
                      
                    }
                    ViewBag.id = ticketId;
                    return View(member);
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


        [HttpGet]
        public JsonResult MemberWorkers(int memberId)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                List<Worker> userWorker = db.Workers.Where(m => m.UserID == memberId).ToList();
                foreach (Worker worker in userWorker)
                {
                    worker.DaysRemaining = Convert.ToInt32((worker.EndingDate - DateTime.Now).TotalDays);
                }

                List<MemberWorkersModel> memberWorkers = new List<MemberWorkersModel>();
                foreach (Worker item in userWorker)
                {
                    MemberWorkersModel memberWorker = new MemberWorkersModel
                    {
                        WorkerId = item.WorkerID,
                        ProductName = item.Product.ProductName,
                        StartingDate = item.StartingDate.ToShortDateString(),
                        EndingDate = item.EndingDate.ToShortDateString(),
                        DaysRemaining = item.DaysRemaining,
                        WorkerState = item.WorkerState
                    };
                    memberWorkers.Add(memberWorker);
                }

                return Json(memberWorkers, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }

        }

        private struct MemberWorkersModel
        {
            public int WorkerId { get; set; }
            public string ProductName { get; set; }
            public string StartingDate { get; set; }
            public string EndingDate { get; set; }
            public int DaysRemaining { get; set; }
            public WORKERSTATE WorkerState { get; set; }
        }

        [HttpGet]
        public JsonResult PaymentHistory(int userId)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                List<Worker> userWorker = db.Workers.Where(m => m.UserID == userId).ToList();
                List<PurchaseHistory> paymentHistory = new List<PurchaseHistory>();

                foreach (Worker item in userWorker)
                {
                    paymentHistory = db.PurchaseHistories.Where(m => m.WorkerID == item.WorkerID).ToList();

                }


                List<PaymentModel> paymentModel = new List<PaymentModel>();
                foreach (PurchaseHistory item in paymentHistory)
                {
                    Worker worker = db.Workers.FirstOrDefault(m => m.WorkerID == item.WorkerID);
                    PaymentModel payment = new PaymentModel
                    {
                        ProductName = worker.Product.ProductName,
                        Timestamp = item.Timestamp,
                        Amount = item.Amount,
                        ShortName = item.ShortNmae,
                        Contract = item.Contract,
                        AccountWallet = item.AccountWallet,
                        ReceiverWallet = item.ReceiverWallet,
                        TxOrError = item.TransectionId ?? item.ErrorMessage,
                        MonthOrYear = item.Yearly ? "Yearly" : "Monthly",
                        PurchaseStatusType = item.PurchaseStatusType
                    };

                    paymentModel.Add(payment);
                }


                return Json(paymentModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }

        }

        private struct PaymentModel
        {
            public int Timestamp { get; set; }
            public string Amount { get; set; }
            public string Contract { get; set; }
            public string ShortName { get; set; }
            public PURCHASESTATUS PurchaseStatusType { get; set; }
            public string TxOrError { get; set; }
            public string MonthOrYear { get; set; }
            public string AccountWallet { get; set; }
            public string ReceiverWallet { get; set; }
            public string ProductName { get; set; }
        }

        [HttpGet]
        public JsonResult Status(int id)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                User user = db.Users.FirstOrDefault(m => m.UserID == id);
                return Json(user.Disabled, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public JsonResult Suspended(int id)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                User user = db.Users.FirstOrDefault(m => m.UserID == id);
                user.Disabled = true;
                _ = db.SaveChanges();
                return Json(user.Disabled, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public JsonResult Active(int id)
        {
            User user = new User();
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                user = db.Users.FirstOrDefault(m => m.UserID == id);
                user.Disabled = false;
                _ = db.SaveChanges();
                return Json(user.Disabled, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }

        }

        [Authorize]
        public JsonResult Logout()
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                if (UserHelper.LogOut())
                {
                    Session.Abandon();
                }
                return Json(new { IsOk = true });
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }

        }

        [Authorize]
        public JsonResult MakeAdmin(User user)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                User user1 = db.Users.FirstOrDefault(x => x.UserID == user.UserID);
                if (!user1.SuperAdmin)
                {
                    user1.Role = "Admin";
                    _ = db.SaveChanges();
                }
                return Json(new { IsOk = true });
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }

        }

        [Authorize]
        public JsonResult DiscardAdmin(User user)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                User user1 = db.Users.FirstOrDefault(x => x.UserID == user.UserID);
                if (!user1.SuperAdmin)
                {
                    user1.Role = "Member";
                    _ = db.SaveChanges();
                }
                return Json(new { IsOk = true });
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);

            }

        }

        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                User user = db.Users.Find(id);
                if (user == null)
                {
                    return HttpNotFound();
                }

                string userName = User.Identity.Name;
                User loginUser = db.Users.First(x => x.Email == userName);
                ViewBag.Role = loginUser.SuperAdmin ? "SuperAdmin" : "Admin";

                return View(user);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }

        }

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                return View(db.Users.ToList());
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "BadRequest");
            }

        }


        [Authorize(Roles = "Member")]
        public ActionResult GetProfile()
        {
            try
            {
                User user = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name);
                Logger.GetLogger().LogUser(LOGTYPE.INFO, user.UserID);
                ViewBag.name = user.Name;
                ViewBag.email = user.Email;
                ViewBag.surname = user.SurName;
                List<ExchangeApi> exchangeApiUser = db.ExchangeApis.Where(m => m.User.Email == User.Identity.Name).ToList();
                return View(exchangeApiUser);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "BadRequest");
            }

        }

        [Authorize(Roles = "Admin")]
        public ActionResult GetProfileAdmin()
        {
            try
            {
                string userName = User.Identity.Name;
                User user = db.Users.First(x => x.Email == userName);
                Logger.GetLogger().LogUser(LOGTYPE.INFO, user.UserID);
                return user == null ? HttpNotFound() : (ActionResult)View(user);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "BadRequest");
            }

        }


        [Authorize(Roles = "Member")]
        public ActionResult MyProfile()
        {
            try
            {
                string userName = User.Identity.Name;
                User user = db.Users.First(x => x.Email == userName);
                Logger.GetLogger().LogUser(LOGTYPE.INFO, user.UserID);
                if (user == null)
                {
                    return HttpNotFound();
                }
                user.Password = "";
                return View(user);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "BadRequest");
            }

        }

        [Authorize(Roles = "Admin")]
        public ActionResult MyProfileAdmin()
        {
            try
            {
                string userName = User.Identity.Name;
                User user = db.Users.First(x => x.Email == userName);
                Logger.GetLogger().LogUser(LOGTYPE.INFO, user.UserID);
                if (user == null)
                {
                    return HttpNotFound();
                }
                user.Password = "";
                return View(user);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "BadRequest");
            }

        }

        [HttpPost]
        public JsonResult MyProfileMember([Bind(Exclude = "Image")] string Image, string Name, string SurName, string Password)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                string imageType = Image.Substring(5, 9);

                Image = Image.Substring(22);

                User updateUser = db.Users.First(x => x.Email == User.Identity.Name);

                if (imageType == "image/png")
                {
                    updateUser.ImageType = "PNG";
                    _ = db.SaveChanges();
                }
                else
                {
                    updateUser.ImageType = "JPG";
                    _ = db.SaveChanges();
                }

                if (string.IsNullOrEmpty(Name))
                {
                    ViewBag.error = "Name cannot be empty.";
                    return Json(false, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(SurName))
                {
                    ViewBag.error = "Last name cannot be empty.";
                    return Json(false, JsonRequestBehavior.AllowGet);
                }

                updateUser.Name = Name;
                updateUser.SurName = SurName;
                updateUser.Image = Image;
                updateUser.Password = UserHelper.GetPassHash(Password);
                _ = db.SaveChanges();

                ViewBag.success = "The settings are saved.";
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult MyProfileAdmin([Bind(Exclude = "Image")] string Image, string Name, string SurName, string Password)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                string imageType = Image.Substring(5, 9);

                Image = Image.Substring(22);

                User updateUser = db.Users.First(x => x.Email == User.Identity.Name);

                if (imageType == "image/png")
                {
                    updateUser.ImageType = "PNG";
                    _ = db.SaveChanges();
                }
                else
                {
                    updateUser.ImageType = "JPG";
                    _ = db.SaveChanges();
                }

                if (string.IsNullOrEmpty(Name))
                {
                    ViewBag.error = "Name cannot be empty.";
                    return Json(false, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(SurName))
                {
                    ViewBag.error = "Last name cannot be empty.";
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(Password))
                {
                    updateUser.Password = UserHelper.GetPassHash(Password);
                }
                updateUser.Name = Name;
                updateUser.SurName = SurName;
                updateUser.Image = Image;
                _ = db.SaveChanges();

                ViewBag.success = "The settings are saved.";
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize]
        public JsonResult ChangePassword(ChangePasswordModel changePasswordModel)
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                string userName = User.Identity.Name;
                User user = db.Users.First(x => x.Email == userName);

                if (string.IsNullOrEmpty(changePasswordModel.OldPassword))
                {
                    string message = "Old Password cannot be empty.";
                    return Json(new { IsOk = false, Message = message });
                }

                if (string.IsNullOrEmpty(changePasswordModel.NewPassword))
                {
                    string message = "New Password cannot be empty.";
                    return Json(new { IsOk = false, Message = message });
                }

                if (string.IsNullOrEmpty(changePasswordModel.RepeatNewPassword))
                {
                    string message = "Repeat New Password cannot be empty.";
                    return Json(new { IsOk = false, Message = message });
                }

                if (!UserHelper.PasswordCheck(changePasswordModel.OldPassword, user.UserID))
                {
                    string message = "The old password is incorrect.";
                    return Json(new { IsOk = false, Message = message });
                }

                if (!changePasswordModel.NewPassword.Equals(changePasswordModel.RepeatNewPassword))
                {
                    string message = "New Password and Repeat New Password are not the same.";
                    return Json(new { IsOk = false, Message = message });
                }

                if (!UserHelper.PasswordSave(changePasswordModel.NewPassword, user.UserID))
                {
                    string message = "Something went wrong. Try again later.";
                    return Json(new { IsOk = false, Message = message });
                }

                return Json(new { IsOk = true });
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult Register(User user)
        {
            try
            {
              
                Logger.GetLogger().LogUser(LOGTYPE.INFO, 0);
                string encodedResponse = Request.Form["g-Recaptcha-Response"];
                bool isCaptchaValid = ReCaptcha.Validate(encodedResponse);

                if (isCaptchaValid)
                {
                    if (string.IsNullOrEmpty(user.Name))
                    {
                        ViewBag.error = "Name cannot be empty.";
                        return View();
                    }

                    if (string.IsNullOrEmpty(user.SurName))
                    {
                        ViewBag.error = "Last name cannot be empty.";
                        return View();
                    }

                    if (string.IsNullOrEmpty(user.Email))
                    {
                        ViewBag.error = "Email cannot be empty.";
                        return View();
                    }

                    if (string.IsNullOrEmpty(user.Password))
                    {
                        ViewBag.error = "Password cannot be empty.";
                        return View();
                    }

                    if (string.IsNullOrEmpty(user.PassaportAgain))
                    {
                        ViewBag.error = "Repeat Password cannot be empty.";
                        return View();
                    }

                    User dbUser = db.Users.FirstOrDefault(x => x.Email == user.Email);
                    if (dbUser != null)
                    {
                        ViewBag.error = "The e-mail address is already in use.";
                        return View();
                    }

                    if (!user.Password.Equals(user.PassaportAgain))
                    {
                        ViewBag.error = "Password and Repeat Password are not the same.";
                        return View();
                    }

                    if (string.IsNullOrEmpty(user.Image))
                    {
                        user.Image = getImageName(user.Name);
                        _ = db.SaveChanges();
                    }

                    if (UserHelper.Register(user))
                    {
                        User userProduct = db.Users.FirstOrDefault(m => m.Email == user.Email);
                        List<Product> products = db.Products.Where(m => m.Demo == true).ToList();
                        foreach (Product product in products)
                        {
                            foreach (var item in product.ProductPrices)
                            {
                                if (item.PriceUnit != null)
                                {
                                    Worker worker = new Worker();
                                    worker.Product = product;
                                    worker.WorkerName = product.ProductName;
                                    worker.WorkerState = WORKERSTATE.Paused;
                                    worker.StartingDate = DateTime.Now;
                                    worker.EndingDate = DateTime.Now.AddDays(7);
                                    userProduct.Workers.Add(worker);
                                    db.SaveChanges();
                                    break;
                                }
                            }
                        }
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.error = "Something went wrong. Try again later.";
                        return View();
                    }
                }
                else
                {
                    ViewBag.error = "Wrong Captcha.";
                    return View();
                }
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult ResetPass()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResetPass(User user)
        {
            try
            {
                Logger.GetLogger().LogUser(LOGTYPE.INFO, 0);
                string encodedResponse = Request.Form["g-Recaptcha-Response"];
                bool isCaptchaValid = ReCaptcha.Validate(encodedResponse);

                if (isCaptchaValid)
                {
                    if (string.IsNullOrEmpty(user.Email))
                    {
                        ViewBag.error = "Email cannot be empty.";
                        return View();
                    }

                    if (UserHelper.ResetPass(user.Email))
                    {
                        ViewBag.success = "If a match is achieved, your new password will be sent to your e-mail address.";
                        return View();
                    }
                    else
                    {
                        ViewBag.error = "Something went wrong. Try again later.";
                        return View();
                    }
                }
                else
                {
                    ViewBag.error = "Wrong Captcha.";
                    return View();
                }
            }
            catch (Exception ex)
            {
                Logger.GetLogger().LogError(LOGTYPE.ERROR, ex.Data.ToString(), ex.Message, ex.StackTrace);
                return Json(HttpStatusCode.BadRequest, JsonRequestBehavior.AllowGet);
            }
        }

        private string HttpPostedFileBaseToBase64(HttpPostedFileBase objectFiles)
        {
            WebImage image = new WebImage(objectFiles.InputStream);
            _ = image.Resize(200, 200);
            byte[] imageData = image.GetBytes(image.ImageFormat);
            return byteArrayToBase64(imageData);
        }

        private string getImageName(string userName)
        {
            string path = Path.Combine(Server.MapPath("~/Images/"));
            string[] list = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
            string fileImageName = "";
            foreach (string file in list)
            {
                fileImageName = Path.GetFileName(file);
                if (userName.ToUpper().Substring(0, 1) == fileImageName.Substring(0, 1))
                {
                    break;
                }
            }
            string imagePath = path + fileImageName;
            byte[] imageData = null;
            FileStream fileStream = System.IO.File.Open(imagePath, FileMode.Open);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                fileStream.CopyTo(memoryStream);
                imageData = memoryStream.ToArray();
            }
            return byteArrayToBase64(imageData);
        }

        private string byteArrayToBase64(byte[] imageData)
        {
            return Convert.ToBase64String(imageData, 0, imageData.Length);
        }

        public ActionResult RiskTest()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetUserTest()
        {
            try
            {
                int UserId = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name).UserID;
                Logger.GetLogger().LogUser(LOGTYPE.INFO, UserId);
                User user = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name);
                RiskTest userTest = db.RiskTests.FirstOrDefault(m => m.UserID == user.UserID);
                if (userTest != null)
                {
                    var test = new
                    {
                        answer1 = userTest.Answer1,
                        answer2 = userTest.Answer2,
                        answer3 = userTest.Answer3,
                        answer4 = userTest.Answer4,
                        answer5 = userTest.Answer5,
                        answer6 = userTest.Answer6,
                        answer7 = userTest.Answer7,
                        answer8 = userTest.Answer8,
                        answer9 = userTest.Answer9,
                        answer10 = userTest.Answer10,
                    };
                    return Json(test, JsonRequestBehavior.AllowGet);
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
        public JsonResult UserTestBadge()
        {
            try
            {
                User user = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name);
                Logger.GetLogger().LogUser(LOGTYPE.INFO, user.UserID);
                if (user != null)
                {
                    string badgeStr = "";
                    switch (user.riskType)
                    {
                        case USERRISKTYPE.LowRiskProfile:
                            badgeStr = "Conservative Investor";
                            break;
                        case USERRISKTYPE.MediumRiskProfile:
                            badgeStr = "Balanced Investor";
                            break;
                        case USERRISKTYPE.HighRiskProfile:
                            badgeStr = "Aggressive Investor";
                            break;
                        default:
                            return Json(false, JsonRequestBehavior.AllowGet);
                    }

                    return Json(badgeStr, JsonRequestBehavior.AllowGet);
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

        [HttpPost]
        public JsonResult SaveTest(int answer1, int answer2, int answer3, int answer4, int answer5, int answer6, int answer7, int answer8, int answer9, int answer10)
        {
            try
            {
                User user = db.Users.FirstOrDefault(m => m.Email == User.Identity.Name);
                Logger.GetLogger().LogUser(LOGTYPE.INFO, user.UserID);
                if (user != null)
                {
                    RiskTest userTest = db.RiskTests.FirstOrDefault(m => m.UserID == user.UserID);
                    if (userTest != null)
                    {
                        userTest.Answer1 = answer1;
                        userTest.Answer2 = answer2;
                        userTest.Answer3 = answer3;
                        userTest.Answer4 = answer4;
                        userTest.Answer5 = answer5;
                        userTest.Answer6 = answer6;
                        userTest.Answer7 = answer7;
                        userTest.Answer8 = answer8;
                        userTest.Answer9 = answer9;
                        userTest.Answer10 = answer10;
                        float total = answer1 + answer2 + answer3 + answer4 + answer5 + answer6 + answer7 + answer8 + answer9 + answer10;
                        float result = total / 10;
                        if (result >= 1 && result < 2)
                        {
                            user.riskType = USERRISKTYPE.LowRiskProfile;
                        }
                        else if (result >= 2 && result < 3)
                        {
                            user.riskType = USERRISKTYPE.MediumRiskProfile;
                        }
                        else if (result >= 3 && result < 4)
                        {
                            user.riskType = USERRISKTYPE.HighRiskProfile;
                        }
                        _ = db.SaveChanges();
                    }
                    else
                    {
                        RiskTest test = new RiskTest
                        {
                            Answer1 = answer1,
                            Answer2 = answer2,
                            Answer3 = answer3,
                            Answer4 = answer4,
                            Answer5 = answer5,
                            Answer6 = answer6,
                            Answer7 = answer7,
                            Answer8 = answer8,
                            Answer9 = answer9,
                            Answer10 = answer10
                        };

                        float total = answer1 + answer2 + answer3 + answer4 + answer5 + answer6 + answer7 + answer8 + answer9 + answer10;
                        float result = total / 10;

                        if (result >= 1 && result < 2)
                        {
                            user.riskType = USERRISKTYPE.LowRiskProfile;
                        }
                        else if (result >= 2 && result < 3)
                        {
                            user.riskType = USERRISKTYPE.MediumRiskProfile;
                        }
                        else if (result >= 3 && result < 4)
                        {
                            user.riskType = USERRISKTYPE.HighRiskProfile;
                        }
                        test.UserID = user.UserID;
                        _ = db.RiskTests.Add(test);
                        _ = db.SaveChanges();
                    }
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
