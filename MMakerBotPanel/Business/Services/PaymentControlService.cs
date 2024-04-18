namespace MMakerBotPanel.Business.Services
{
    using MMakerBotPanel.Business.Services.Models;
    using MMakerBotPanel.Database.Context;
    using MMakerBotPanel.Database.Model;
    using MMakerBotPanel.Models;
    using MMakerBotPanel.WebServices.Etherscan;
    using MMakerBotPanel.WebServices.Etherscan.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PaymentControlService : ServicesInterfaces
    {
        private readonly EtherscanApiController controller = new EtherscanApiController();

        private static PaymentControlService _PaymentControlService;

        private static readonly object _lock = new object();

        private int sayac = 0;

        private PaymentControlService()
        {

        }

        public static PaymentControlService GetPaymentControlService()
        {
            lock (_lock)
            {
                if (_PaymentControlService == null)
                {
                    _PaymentControlService = new PaymentControlService();
                }
                return _PaymentControlService;
            }
        }

        public override void Tick()
        {
            using (ModelContext db = new ModelContext())
            {
                int? CheckPeriyod = CacheHelper.Get<int?>(CACHEKEYENUMTYPE.OrderSettings_CheckPeriyod.ToString());
                int? TTL = CacheHelper.Get<int?>(CACHEKEYENUMTYPE.OrderSetting_TLL.ToString());

                if (CheckPeriyod == null || TTL == null)
                {
                    Wallet wallet = db.Wallets.ToList().FirstOrDefault();
                    if (wallet == null)
                    {
                        return;
                    }
                    TTL = wallet.TtlCount;
                    CheckPeriyod = (int)wallet.CheckPeriod;
                    CacheHelper.Add<int>(CACHEKEYENUMTYPE.OrderSettings_CheckPeriyod.ToString(), (int)CheckPeriyod, DateTimeOffset.Now.AddHours(24));
                    CacheHelper.Add<int>(CACHEKEYENUMTYPE.OrderSetting_TLL.ToString(), (int)TTL, DateTimeOffset.Now.AddHours(24));
                }

                int nowTimeSpan = (int)DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                int maxTimeSpan = nowTimeSpan - (int)CheckPeriyod;
                List<PurchaseHistory> purchases = db.PurchaseHistories.Where(m => m.PurchaseStatusType == PURCHASESTATUS.Pending && m.TtlCount < TTL && m.LastCheckDate < maxTimeSpan).Take(5).ToList();

                foreach (PurchaseHistory item in purchases)
                {
                    item.TtlCount++;
                    item.LastCheckDate = nowTimeSpan;
                    Worker worker = db.Workers.FirstOrDefault(m => m.WorkerID == item.Worker.WorkerID);
                    if (item.TransectionId != null)
                    {
                        EtherscanStatusModel responseStatus = controller.GetEtherscanApiStatus(item.TransectionId);
                        if (responseStatus != null)
                        {

                            if (responseStatus.result.status == "1")
                            {
                                item.PurchaseStatusType = PURCHASESTATUS.Confirm;
                                worker.WorkerState = WORKERSTATE.Working;

                            }
                            else if (responseStatus.result.status == "0")
                            {
                                item.PurchaseStatusType = PURCHASESTATUS.Reject;
                                worker.WorkerState = WORKERSTATE.Expired;

                            }
                            else
                            {
                                item.PurchaseStatusType = PURCHASESTATUS.Pending;
                                worker.WorkerState = WORKERSTATE.PendingTXID;

                            }
                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        item.PurchaseStatusType = PURCHASESTATUS.Reject;
                        worker.WorkerState = WORKERSTATE.Expired;
                    }
                    _ = db.SaveChanges();
                }
            }
        }

        public override void OnNext(ObserverMessage value)
        {
            sayac++;
            CacheHelper.Add<Int64>(CACHEKEYENUMTYPE.PaymentControlService_LastActivity.ToString(), Convert.ToInt64((DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds), DateTimeOffset.Now.AddSeconds(10));
            if (sayac >= 5)
            {
                sayac = 0;
                Tick();
            }
        }

    }
}
