namespace MMakerBotPanel.Business.Services
{
    using MMakerBotPanel.Business.Services.Models;
    using MMakerBotPanel.Database.Context;
    using MMakerBotPanel.Database.Model;
    using MMakerBotPanel.Models;
    using MMakerBotPanel.Models.MakerMarketingModels;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ProfitService : ServicesInterfaces
    {
        private static ProfitService _ProfitService;

        private static readonly object _lock = new object();

        private int count = 0;
        private ProfitService()
        {

        }
        public static ProfitService GetProfitService()
        {
            lock (_lock)
            {
                if (_ProfitService == null)
                {
                    _ProfitService = new ProfitService();
                }
                return _ProfitService;
            }
        }

        public override void Tick()
        {
            lock (_lock)
            {
                Int64 cacheTmp = CacheHelper.Get<Int64>("usdtBalance");
                if (cacheTmp == 0)
                {
                    CacheHelper.Add("usdtBalance", Convert.ToInt64((DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds), DateTimeOffset.Now.AddMinutes(20));
                    using (ModelContext db = new ModelContext())
                    {
                        List<User> users = db.Users.ToList();
                        double usdtBalance = 0;
                        List<BalanceResponseModel> balances = new List<BalanceResponseModel>();
                        foreach (User user in users)
                        {
                            List<ExchangeApi> exchanges = db.ExchangeApis.Where(m => m.UserID == user.UserID).ToList();
                            foreach (ExchangeApi exchangeApi in exchanges)
                            {
                                if (exchangeApi.ExchangeApiKey == EXCHANGEAPIKEY.ApiKey && exchangeApi.Value != null)
                                {
                                    if(db.ExchangeApis.Any(m => m.Exchange == exchangeApi.Exchange && m.UserID ==  exchangeApi.UserID && m.ExchangeApiKey == EXCHANGEAPIKEY.SecretKey && m.Value != null))
                                    {
                                        CEXEXCHANGE cEXEXCHANGE = exchangeApi.Exchange;
                                        balances = CexExchangeSelect.CexSelect(cEXEXCHANGE).GetBalance();
                                        foreach (BalanceResponseModel balance in balances)
                                        {
                                            if (balance.Symbol.Contains("USDT"))
                                            {
                                                usdtBalance += Convert.ToDouble(balance.Balance.Replace(".", ","));
                                            }
                                        }
                                    }
                                }
                            }
                            ProfitUSDT profit = new ProfitUSDT();
                            profit.UsdtBalance = usdtBalance;
                            profit.Date = DateTime.Now;
                            profit.UserID = user.UserID;
                            db.ProfitUSDTs.Add(profit);
                            db.SaveChanges();
                        }
                    }
                }
            }

        }
        public override void OnNext(ObserverMessage value)
        {
            CacheHelper.Add<Int64>(CACHEKEYENUMTYPE.ProfitService_LastActivity.ToString(), Convert.ToInt64((DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds), DateTimeOffset.Now.AddSeconds(10));
            DateTime dateTime = DateTime.Now;
            DateTime moorningHour1 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 11, 40, 0);
            DateTime moorningHout2 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 11, 40, 10);

            if (dateTime > moorningHour1 && dateTime < moorningHout2)
            {
                Tick();
            }
        }
    }
}