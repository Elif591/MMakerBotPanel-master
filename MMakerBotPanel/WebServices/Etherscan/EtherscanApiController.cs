namespace MMakerBotPanel.WebServices.Etherscan
{
    using MMakerBotPanel.Business;
    using MMakerBotPanel.Database.Context;
    using MMakerBotPanel.Models;
    using MMakerBotPanel.WebServices.Etherscan.Model;
    using System;
    using System.Linq;

    public class EtherscanApiController
    {
        private readonly EtherscanWebServices services = new EtherscanWebServices();
        private EtherscanStatusModel etherscanStatus = new EtherscanStatusModel();
        public EtherscanStatusModel GetEtherscanApiStatus(string txhash)
        {
            string apikey = CacheHelper.Get<string>(CACHEKEYENUMTYPE.Etherscan_APIKey.ToString());
            using (ModelContext db = new ModelContext())
            {
                if (apikey == null || apikey == "" || apikey == string.Empty)
                {
                    apikey = db.Wallets.OrderByDescending(x => x.WalletID).ToList().FirstOrDefault().EtherScanApiKey;
                    CacheHelper.Add<string>(CACHEKEYENUMTYPE.Etherscan_APIKey.ToString(), apikey, DateTimeOffset.Now.AddHours(24));
                }

                etherscanStatus = services.getEtherscanStatus(txhash, apikey);
            }


            return etherscanStatus;
        }
    }
}