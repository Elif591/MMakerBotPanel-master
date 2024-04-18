namespace MMakerBotPanel.Business.Exchange
{
    using MMakerBotPanel.Database.Context;
    using MMakerBotPanel.Models;
    using System;
    using System.Linq;

    public abstract class ExchangeHelperAbstract
    {
        private protected CEXEXCHANGE _cexExchangeEnum;

        public Tuple<string, string , string> GetUserAPIKeys()
        {
            string apiKey = "";
            string secretKey = "";
            string passPhrase = "";
            using (ModelContext db = new ModelContext())
            {
                try
                {
                    apiKey = db.ExchangeApis.FirstOrDefault(m => m.Exchange == _cexExchangeEnum && m.ExchangeApiKey == EXCHANGEAPIKEY.ApiKey).Value;
                    secretKey = db.ExchangeApis.FirstOrDefault(m => m.Exchange == _cexExchangeEnum && m.ExchangeApiKey == EXCHANGEAPIKEY.SecretKey).Value;
                    if (apiKey == null || secretKey == null)
                    {
                        apiKey = "";
                        secretKey = "";
                    }
                    passPhrase = db.ExchangeApis.FirstOrDefault(m => m.Exchange == _cexExchangeEnum && m.ExchangeApiKey == EXCHANGEAPIKEY.ApiKey).PassPhrase;
                    if(passPhrase == "" || passPhrase == null)
                    {
                        passPhrase = db.ExchangeApis.FirstOrDefault(m => m.Exchange == _cexExchangeEnum && m.ExchangeApiKey == EXCHANGEAPIKEY.SecretKey).PassPhrase;
                    }
                  
                }
                catch
                {
                    passPhrase = "";
                }
            }
            return Tuple.Create(apiKey, secretKey , passPhrase);
        }


    }
}