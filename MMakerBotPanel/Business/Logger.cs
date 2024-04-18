using MMakerBotPanel.Models;
using System;
using MMakerBotPanel.Database.Model;
using System.Web;
using System.Net;
using System.Runtime.CompilerServices;
using MMakerBotPanel.Database.Context;

namespace MMakerBotPanel.Business
{
    public class Logger
    {
        private static Logger Instance = new Logger();

        private static readonly object Locker = new object();
        private Logger() { }

        public static Logger GetLogger()
        {
            lock (Locker)
            {
                if (Instance == null)
                {
                    Instance = new Logger();
                }
                return Instance;
            }
        }
        public void LogTick()
        {
            //try
            //{
            using (ModelContext db = new ModelContext())
            {
                TickLog tickLog = new TickLog();
                tickLog.Timestamp = Convert.ToString((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
                db.TickLogs.Add(tickLog);
                db.SaveChanges();
            }
            //}
            //catch (Exception ex)
            //{
            //Console.WriteLine($"Log Error: {ex.Message}");
            //}
        }


        public void LogError(LOGTYPE logType, string data, string exception, string methodName)
        {
            //try
            //{
            using (ModelContext db = new ModelContext())
            {
                ErrorLog errorLog = new ErrorLog();
                errorLog.LogType = logType;
                errorLog.Data = data;
                errorLog.TimeStamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                errorLog.MethodName = methodName;
                errorLog.Exception = exception;
                db.ErrorLogs.Add(errorLog);
                db.SaveChanges();
            }
            //}
            //catch (Exception ex)
            //{
            //Console.WriteLine($"Log Error: {ex.Message}");
            //}
        }

        public void LogGridStrategy(LOGTYPE logType, string data, string methodName)
        {
            //try
            //{
            using (ModelContext db = new ModelContext())
            {
                GridStrategyLog log = new GridStrategyLog();
                log.LogType = logType;
                log.Data = data;
                log.TimeStamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                log.MethodName = methodName;
                db.GridStrategyLogs.Add(log);
                db.SaveChanges();
            }
            //}
            //catch (Exception ex)
            //{
            //Console.WriteLine($"Log Error: {ex.Message}");
            //}
        }


        public void LogUser(LOGTYPE logType, int userId, [CallerMemberName] string methodName = "", [CallerFilePath] string callerFilePath = "")
        {
            //try
            //{
            using (ModelContext db = new ModelContext())
            {
                UserLog userLog = new UserLog();
                userLog.LogType = logType;
                userLog.MethodName = methodName;
                userLog.BrowserInfo = HttpContext.Current.Request.UserAgent;
                userLog.OSInfo = Environment.OSVersion.ToString();
                userLog.IPNumber = GetIPAddress();
                userLog.TimeStamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                userLog.UserID = userId;
                db.UserLogs.Add(userLog);
                db.SaveChanges();
            }
            //}
            //catch (Exception ex)
            //{
            //Console.WriteLine("Hata oluştu: " + ex.Message);
            //}
        }

        public void LogApi(LOGAPITYPE logType, string requestData, string responseData, string Url)
        {
            //try
            //{
            using (ModelContext db = new ModelContext())
            {
                ApiLog apiLog = new ApiLog();
                apiLog.LogType = logType;
                apiLog.RequestData = requestData;
                apiLog.ResponseData = responseData;
                apiLog.Url = Url;
                apiLog.TimeStamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                db.ApiLogs.Add(apiLog);
                db.SaveChanges();
            }
            //}
            //catch (Exception ex)
            //{
            //Console.WriteLine("Hata oluştu: " + ex.Message);
            //}
        }

        public void LogPayment(string walletID, string adminWalletID, string productID, string workerID, string price, string priceUnitID, string gasFee, int userID)
        {
            //try
            //{
            using (ModelContext db = new ModelContext())
            {
                PaymentLog paymentLog = new PaymentLog();
                paymentLog.WalletID = walletID;
                paymentLog.AdminWalletID = adminWalletID;
                paymentLog.BrowserInfo = HttpContext.Current.Request.UserAgent;
                paymentLog.IPNumber = GetIPAddress();
                paymentLog.ProductID = productID;
                paymentLog.WorkerID = workerID;
                paymentLog.Price = price;
                paymentLog.PriceUnitID = priceUnitID;
                paymentLog.GasFee = gasFee;
                paymentLog.Timestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                paymentLog.UserID = userID;
                db.PaymentLogs.Add(paymentLog);
                db.SaveChanges();
            }
            //}
            //catch (Exception ex)
            //{
            //Console.WriteLine("Hata oluştu: " + ex.Message);
            //}
        }

        private string GetIPAddress()
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    string ipAddress = client.DownloadString("https://api.ipify.org");
                    return ipAddress;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Public IP adresi alınamadı: " + ex.Message);
                return string.Empty;
            }
        }
    }
}