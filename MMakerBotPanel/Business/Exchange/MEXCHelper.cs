namespace MMakerBotPanel.Business.Exchange
{
    using MMakerBotPanel.Models;

    public class MEXCHelper : ExchangeHelperAbstract
    {

        private static MEXCHelper _MEXCHelper;

        private static readonly object _Lock = new object();

        private MEXCHelper()
        {
            _cexExchangeEnum = CEXEXCHANGE.Mexc;
        }
        public static MEXCHelper GetMEXCHelper()
        {
            lock (_Lock)
            {
                if (_MEXCHelper == null)
                {
                    _MEXCHelper = new MEXCHelper();
                }
                return _MEXCHelper;
            }
        }

        //public static Tuple<string, string> GetUserAPIKeys()
        //{
        //    string APIKey = "mx0vglxTlpRcWMk9w8";
        //    string SecretKey = "412256d94c014dd68e6420d8869a6425";
        //    Tuple<string, string> ret = new Tuple<string, string>(APIKey, SecretKey);
        //    return ret;
        //}
    }
}